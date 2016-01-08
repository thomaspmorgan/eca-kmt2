'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ItineraryCtrl
 * @description
 * # ItineraryCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ItineraryCtrl', function (
      $scope,
      $state,
      $stateParams,
      $log,
      $q,
      $modal,
      smoothScroll,
      FilterService,
      ProjectService,
      LocationService,
      StateService,
      NotificationService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.itinerary = $scope.$parent.itinerary;
      $scope.view.project = null;
      $scope.view.isInEditMode = false;
      $scope.view.arrivalLocations = [];
      $scope.view.arrivalLocationsCount = 0;
      $scope.view.departureLocations = [];
      $scope.view.departureLocationsCount = 0;
      $scope.view.searchLimit = 30;
      $scope.view.isStartDateDatePickerOpen = false;
      $scope.view.isEndDateDatePickerOpen = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isLoadingItineraryStops = false;
      $scope.view.isSaving = false;
      $scope.view.isItineraryExpanded = false;
      $scope.view.calendarKey = 'cal';
      $scope.view.listKey = 'list';
      $scope.view.viewType = $scope.view.listKey;
      $scope.view.selectedCalendarItineraryStop = null;
      $scope.view.itineraryStops = [];
      $scope.view.eventSources = [];

      $scope.view.calendarConfig = {
          calendar: {
              //height: 300,
              editable: false,
              header: {
                  left: 'month basicWeek basicDay agendaWeek agendaDay',
                  center: 'title',
                  right: 'today prev,next'
              },
              eventDrop: function (event, delta, revertFunc, jsEvent, ui, view) {
                  var itineraryStop = getItineraryStop(event);
                  updateItineraryStop(itineraryStop, event, delta);
              },
              eventClick: function (calEvent, jsEvent, view) {
                  onCalendarItemClick(calEvent, jsEvent, view);
              },
              eventResize: function (event, delta, revertFunc, jsEvent, ui, view) {
                  var itineraryStop = getItineraryStop(event);
                  updateItineraryStop(itineraryStop, event, delta);
              }
          }
      }

      var itineraryCopy = angular.copy($scope.view.itinerary);


      $scope.view.onEditClick = function (itinerary) {
          $log.info('edit');
          $scope.view.isInEditMode = true;
      }

      $scope.view.onCommentClick = function (itinerary) {
          $log.info('comment');
      }

      $scope.view.onDeleteClick = function (itinerary) {
          $log.info('delete');
      }

      $scope.view.onItineraryExpandClick = function (itinerary) {
          $scope.view.isItineraryExpanded = true;
          scrollToItinerary(itinerary);
          return loadItineraryStops(itinerary);
      }

      $scope.view.onItineraryCollapseClick = function (itinerary) {
          $scope.view.isItineraryExpanded = false;
      }

      $scope.view.onAddItineraryStopClick = function (itinerary) {
          var addItineraryStopModal = $modal.open({
              animation: true,
              templateUrl: 'app/projects/add-itinerary-stop-modal.html',
              controller: 'AddItineraryStopModalCtrl',
              size: 'lg',
              backdrop: 'static',
              resolve: {
                  project: function () {
                      return $scope.view.project;
                  },
                  itinerary: function () {
                      return itinerary;
                  }
              }
          });
          addItineraryStopModal.result.then(function (addedItineraryStop) {
              $log.info('Finished adding itinerary stop.');
              loadItineraryStops(itinerary);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      };

      $scope.view.isLoadingRequiredData = true;
      $scope.$parent.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.project = project;
          $scope.view.isLoadingRequiredData = false;
      });

      $scope.view.onCancelClick = function (itinerary) {
          $log.info('cancel');
          $scope.view.itinerary = itineraryCopy;
          $scope.view.isInEditMode = false;
      }

      $scope.view.onSaveClick = function (itinerary) {
          $scope.view.isInEditMode = false;
          $scope.view.isSaving = true;

          return ProjectService.updateItinerary(itinerary, $scope.view.project.id)
          .then(function (response) {
              $scope.view.isSaving = false;
              NotificationService.showSuccessMessage("Successfully updated the traveling period.");
              var updateditinerary = response.data;
              initializeItinerary(updateditinerary);
              itineraryCopy = angular.copy(updateditinerary);
              $scope.view.itinerary = updateditinerary;
          })
          .catch(function (error) {
              $scope.view.isSaving = false;
              if (error.status === 400) {
                  if (error.data.message && error.data.modelState) {
                      for (var key in error.data.modelState) {
                          NotificationService.showErrorMessage(error.data.modelState[key][0]);
                      }
                  }
                  else if (error.data.Message && error.data.ValidationErrors) {
                      for (var key in error.data.ValidationErrors) {
                          NotificationService.showErrorMessage(error.data.ValidationErrors[key]);
                      }
                  }
              }
              else {
                  var message = "Unable to save updated itinerary.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
          });
      }

      $scope.view.onDepartureLocationSelect = function ($item, $model) {
          $scope.view.itinerary.departureLocation = $model;
          $scope.view.itinerary.departureLocationId = $model.id;
      }

      $scope.view.onArrivalLocationSelect = function ($item, $model) {
          $scope.view.itinerary.arrivalLocation = $model;
          $scope.view.itinerary.arrivalLocationId = $model.id;
      }

      $scope.view.openStartDateDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isStartDateDatePickerOpen = true;
      }

      $scope.view.openEndDateDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isEndDateDatePickerOpen = true;
      }

      $scope.view.onCalendarKeySelect = function () {
          addAllItineraryStopsAsEvents($scope.view.itineraryStops);
      }

      var arrivalFilter = FilterService.add('itinerary_arrivallocations');
      $scope.view.getArrivalLocations = function ($search) {
          var params = getSearchParams(arrivalFilter, $search, [ConstantsService.locationType.city.id]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.arrivalLocations = data.results;
              $scope.view.arrivalLocationsCount = data.total;
              return data.results;
          });
      }

      var departureFilter = FilterService.add('itinerary_departurelocations');
      $scope.view.getDepartureLocations = function ($search) {
          var params = getSearchParams(arrivalFilter, $search, [
              ConstantsService.locationType.city.id,
              ConstantsService.locationType.division.id,
              ConstantsService.locationType.country.id,
              ConstantsService.locationType.region.id
          ]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.departureLocations = data.results;
              $scope.view.departureLocationsCount = data.total;
              return data.results;
          });
      }

      $scope.view.getDivId = function (itinerary) {
          return 'itinerary' + itinerary.id;
      }

      function loadItineraryStops(itinerary) {
          $scope.view.isLoadingItineraryStops = true;
          return ProjectService.getItineraryStops(itinerary.projectId, itinerary.id)
          .then(function (response) {
              angular.forEach(response.data, function (stop, index) {
                  var arrivalDate = new Date(stop.arrivalDate);
                  if (!isNaN(arrivalDate.getTime())) {
                      stop.arrivalDate = arrivalDate;
                  }
                  var departureDate = new Date(stop.departureDate);
                  if (!isNaN(departureDate.getTime())) {
                      stop.departureDate = departureDate;
                  }
                  setItineraryStopColor(stop);
                  //$scope.$watch(function () {
                  //    return stop.arrivalDate;
                  //}, function (newValue, oldValue) {
                  //    console.log('change');
                  //    console.log(newValue);
                  //    console.log(oldValue);
                  //});
              });

              

              $scope.view.itineraryStops = response.data;
              $scope.view.isLoadingItineraryStops = false;
              return response.data;
          })
          .catch(function (response) {
              $scope.view.isLoadingItineraryStops = false;
              var message = "Unable to load city stops.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function scrollToItinerary(itinerary) {
          var toolbarDivs = document.getElementsByClassName('toolbar');
          var additionalOffset = 0;
          if (toolbarDivs.length > 0) {
              angular.forEach(toolbarDivs, function (toolbarDiv, index) {
                  var angularElement = angular.element(toolbarDiv)[0];
                  additionalOffset += angularElement.offsetHeight;
              });
          }
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 70 + additionalOffset,
              callbackBefore: function (element) {
              },
              callbackAfter: function (element) { }
          }
          var id = $scope.view.getDivId(itinerary)
          var e = document.getElementById(id);
          smoothScroll(e, options);
      }

      function getSearchParams(filter, search, locationTypesById) {
          if (!angular.isArray(locationTypesById)) {
              throw Error('locationTypesById must be an array.');
          }
          filter.reset();
          filter = filter
              .skip(0)
              .take($scope.view.searchLimit);

          if (search) {
              filter = filter.like('name', search);
          }
          if (locationTypesById.length > 1) {
              filter = filter.in('locationTypeId', locationTypesById);
          }
          else if (locationTypesById.length === 1) {
              filter = filter.equal('locationTypeId', locationTypesById[0]);
          }
          return filter.toParams();
      }

      function loadLocations(params) {
          return LocationService.get(params)
          .then(function (response) {
              return response;
          })
          .catch(function (response) {
              $log.error('Unable to load locations.')
          })
      }


      function addAllItineraryStopsAsEvents(itineraryStops) {
          clearEvents();
          var itineraryStopEvents = [];
          angular.forEach(itineraryStops, function (stop, index) {
              itineraryStopEvents.push(getEvent(stop));
          });
          $scope.view.eventSources.push(itineraryStopEvents);
      }

      function addEvent(itineraryStop) {
          $scope.view.eventSources[0].push(getEvent(itineraryStop));
      }

      function clearEvents() {
          $scope.view.eventSources = [];
      }

      function getEvent(itineraryStop) {
          return {
              title: itineraryStop.name,
              start: itineraryStop.arrivalDate,
              end: itineraryStop.departureDate,
              allDay: true,
              itineraryStopId: itineraryStop.itineraryStopId,
              //textColor: 'lightgray'
              color: itineraryStop.color

          };
      }

      function onCalendarItemClick(calEvent, jsEvent, view) {
          var itineraryStop = null;

          for (var i = 0; i < $scope.view.itineraryStops.length; i++) {
              var stop = $scope.view.itineraryStops[i];
              if (stop.itineraryStopId === calEvent.itineraryStopId) {
                  itineraryStop = stop;
              }
          }
          $scope.view.selectedCalendarItineraryStop = itineraryStop;
      }

      function getItineraryStop(calendarEvent) {
          var index = -1;
          for (var i = 0; i < $scope.view.itineraryStops.length; i++) {
              var stop = $scope.view.itineraryStops[i];
              if (stop.itineraryStopId === calendarEvent.itineraryStopId) {
                  index = i;
                  break;
              }
          }
          if (index !== -1) {
              return $scope.view.itineraryStops[index];
          }
          else {
              return null;
          }
      }

      function updateItineraryStop(itineraryStop, calendarEvent, delta) {          
          var copy = angular.copy(itineraryStop);
          itineraryStop.arrivalDate = calendarEvent.start.toDate();
          itineraryStop.departureDate = calendarEvent.end.toDate();
          
          //itineraryStop.arrivalDate = calendarEvent.start.hours(0).minutes(0).seconds(0).milliseconds(0).toDate();
          //itineraryStop.arrivalDate = calendarEvent.start.add(delta).toDate();
          //if (calendarEvent.end === null) {
          //    itineraryStop.departureDate = itineraryStop.arrivalDate;
          //}
          //else {
          //    itineraryStop.departureDate = calendarEvent.end.add(delta).toDate();
          //}
          return ProjectService.updateItineraryStop(itineraryStop, itineraryStop.projectId, itineraryStop.itineraryId)
          .then(function (response) {
              //initializeItineraryStop(response.data);
              //$scope.view.itineraryStop = response.data;
              //copyItineraryStop($scope.view.itineraryStop);

              //$scope.view.isSavingItineraryStop = false;
              //$scope.view.isInEditMode = false;
              NotificationService.showSuccessMessage("Successfully updated city stop.");
              return response.data;
          })
          .catch(function (response) {
              $scope.view.isSavingItineraryStop = false;
              var message = 'Unable to save itinerary stop.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              itineraryCopy = copy;
              addAllItineraryStopsAsEvents($scope.view.itineraryStops);
          });
      }

      
      //var colors = randomColor({
      //    count: 64,
      //    //hue: 'blue',
      //    luminosity: 'dark'
      //});
      var colorIndex = 0;
      var colors = ["#d35304", "#8e9900", "#689e0c", "#640096", "#dd067d", "#ef09ef", "#ea007d", "#028287", "#024c60", "#0da514", "#e216af", "#9e0910", "#0b6293", "#51a508", "#1c9e0e", "#dbb702", "#078435", "#8c0c21", "#460f9e", "#319e03", "#064260", "#bc0b6f", "#0da591", "#0f0666", "#2c930d", "#054175", "#c405a1", "#9e0803", "#02426b", "#6e9601", "#576d00", "#099620", "#0c7287", "#89a309", "#e05d06", "#98a004", "#027f5c", "#0e8c5d", "#2a7a00", "#071f8c", "#e50d80", "#017756", "#cc6a14", "#007a78", "#056482", "#007267", "#af0e08", "#076677", "#9b1d07", "#350d84", "#87011e", "#0b5570", "#e56814", "#8c0c35", "#69960f", "#646d02", "#4a820a", "#af001d", "#ad1608", "#068934", "#023e70", "#efbc02", "#ce0ca1", "#e008ae"];
      function setItineraryStopColor(itineraryStop) {
          itineraryStop.color = colors[colorIndex++ % colors.length];
      }

      function initializeItinerary(itinerary) {
          toDate(itinerary, 'startDate');
          toDate(itinerary, 'endDate');
          toDate(itinerary, 'lastRevisedOn');
          if (itinerary.arrivalLocation) {
              itinerary.arrivalLocationId = itinerary.arrivalLocation.id;
          }
          if (itinerary.departureLocation) {
              itinerary.departureLocationId = itinerary.departureLocation.id;
          }
      }

      function toDate(itinerary, datePropertyName) {
          var date = new Date(itinerary[datePropertyName]);
          if (!isNaN(date.getTime())) {
              itinerary[datePropertyName] = date;
          }
      }
      initializeItinerary($scope.view.itinerary);
  });
