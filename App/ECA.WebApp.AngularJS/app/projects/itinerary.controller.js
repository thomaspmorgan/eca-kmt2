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
      $compile,
      smoothScroll,
      filterFilter,
      FilterService,
      ProjectService,
      LocationService,
      StateService,
      NotificationService,
      DateTimeService,
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
      $scope.view.eventSources = [[]];
      $scope.view.travelStopParticipants = [];
      $scope.view.selectedTravelStopParticipant = null;
      $scope.view.isArrivalDateOpen = false;
      $scope.view.areAllItineraryStopsExpanded = false;
      $scope.view.isLoadingItineraries = false;
      $scope.view.currentTimezone = moment.tz.guess();
      $scope.view.timezoneNames = moment.tz.names();

      //http://angular-ui.github.io/ui-calendar/
      //http://fullcalendar.io/
      //http://fullcalendar.io/docs/
      $scope.view.calendarConfig = {
          calendar: {
              editable: false,
              header: {
                  right: 'month agendaWeek agendaDay',// basicWeek basicDay',
                  center: 'title',
                  left: 'today prevYear,prev,next,nextYear'
              },
              defaultDate: $scope.view.itinerary.startDate,
              eventLimit: false,
              eventClick: function (calEvent, jsEvent, view) {
                  onCalendarItemClick(calEvent, jsEvent, view);
              }
          }
      }

      var colorIndex = 0;
      var itineraryCopy = angular.copy($scope.view.itinerary);

      $scope.view.openArrivalDate = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isArrivalDateOpen = true;
      }

      $scope.view.onSelectTravelStopParticipant = function ($item, $model) {
          clearEvents();
          $scope.view.selectedCalendarItineraryStop = null;
          if ($item) {
              var itineraryStops = getItineraryStopsByParticipant($item);
              angular.forEach(itineraryStops, function (stop, index) {
                  addEvent(stop);
              });
          }
          else {
              addAllItineraryStopsAsEvents($scope.view.itineraryStops);
          }
      };

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

      $scope.view.onExpandAllClick = function (itinerary) {
          return $scope.view.onItineraryExpandClick(itinerary)
          .then(function (itineraryStops) {
              angular.forEach(itineraryStops, function (stop, index) {
                  stop.isExpanded = true;
              });
              $scope.view.areAllItineraryStopsExpanded = true;
          });
      }

      $scope.view.onCollapseAllClick = function (itinerary) {
          $scope.view.isItineraryExpanded = true;
          angular.forEach($scope.view.itineraryStops, function (stop, index) {
              stop.isExpanded = false;
          });
          $scope.view.areAllItineraryStopsExpanded = false;
      }

      $scope.view.onAddItineraryStopClick = function (itinerary) {
          var addItineraryStopModal = $modal.open({
              animation: true,
              templateUrl: 'app/projects/add-itinerary-stop-modal.html',
              controller: 'AddItineraryStopModalCtrl',
              windowClass: 'full-screen-modal',
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
              loadItineraryStops(itinerary)
              addEvent(addedItineraryStop);
              NotificationService.showSuccessMessage("Successfully added the city stop.");
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

      $scope.view.isItineraryNameUnique = function ($value) {
          var dfd = $q.defer();
          if ($value && $value.trim().length > 0) {
              $scope.view.isLoadingItineraries = true;
              ProjectService.getItineraries($scope.view.itinerary.projectId)
              .then(function (response) {
                  $scope.view.isLoadingItineraries = false;
                  angular.forEach(response.data, function (itinerary, index) {
                      itinerary.name = itinerary.name.toLowerCase().trim();
                  });
                  var itineraryIds = response.data.map(function (i) {
                      return i.itineraryId;
                  });
                  var index = itineraryIds.indexOf($scope.view.itinerary.itineraryId);
                  response.data.splice(index, 1);
                  var likeItineraries = filterFilter(response.data, { name: $value.trim().toLowerCase() }, true);
                  if (likeItineraries.length == 0) {
                      dfd.resolve();
                  }
                  else {
                      dfd.reject();
                  }
              })
              .catch(function (response) {
                  $scope.view.isLoadingItineraries = false;
                  var message = "Unable to load travel periods.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
                  dfd.reject();
              });
          }
          else {
              dfd.resolve();
          }
          return dfd.promise;
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

      $scope.$on(ConstantsService.itineraryStopExpandedEventName, function (event, itineraryStop) {
          var allExpanded = true;
          angular.forEach($scope.view.itineraryStops, function (stop, index) {
              if (!stop.isExpanded) {
                  allExpanded = false;
              }
          });
          $scope.view.areAllItineraryStopsExpanded = allExpanded;
      });

      $scope.view.onManageParticipantsClick = function (itinerary) {
          var manageParticipantsModal = $modal.open({
              animation: true,
              templateUrl: 'app/projects/manage-itinerary-participants-modal.html',
              controller: 'ManageItineraryParticipantsModalCtrl',
              windowClass: 'full-screen-modal',
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
          manageParticipantsModal.result.then(function (travelPeriodParticipants) {
              $log.info('Finished managing itinerary participants.');
              itinerary.participantsCount = travelPeriodParticipants.length;
              loadItineraryStops(itinerary);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      function loadItineraryStops(itinerary) {
          $scope.view.isLoadingItineraryStops = true;
          return ProjectService.getItineraryStops(itinerary.projectId, itinerary.id)
          .then(function (response) {
              angular.forEach(response.data, function (stop, index) {
                  stop.isExpanded = false;
              });
              
              $scope.view.travelStopParticipants = getAllParticipants(response.data);
              $scope.view.itineraryStops = response.data;
              $scope.view.isLoadingItineraryStops = false;
              return $scope.view.itineraryStops;
          })
          .catch(function (response) {
              $scope.view.isLoadingItineraryStops = false;
              var message = "Unable to load city stops.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function getAllParticipants(itineraryStops) {
          var participants = [];
          var isTravelStopParticipantAdded = function (participant) {
              for (var i = 0; i < participants.length; i++) {
                  var travelStopParticipant = participants[i];
                  if (travelStopParticipant.participantId === participant.participantId) {
                      return true;
                  }
              }
              return false;
          };

          angular.forEach(itineraryStops, function (stop, stopIndex) {
              angular.forEach(stop.participants, function (stopParticipant, stopParticipantIndex) {
                  if (!isTravelStopParticipantAdded(stopParticipant)) {
                      participants.push(stopParticipant)
                  }
              });
          });
          return participants;
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
          colorIndex = 0;
          clearEvents();
          angular.forEach(itineraryStops, function (stop, index) {
              addEvent(stop);
          });
      }

      function addEvent(itineraryStop) {
          $scope.view.eventSources[0].push(itineraryStop.getEvent());
      }

      function clearEvents() {
          for (var i = $scope.view.eventSources[0].length - 1; i >= 0; i--) {
              $scope.view.eventSources[0].splice(i, 1);
          }
      }

      function onCalendarItemClick(calEvent, jsEvent, view) {
          var itineraryStop = null;

          for (var i = 0; i < $scope.view.itineraryStops.length; i++) {
              var stop = $scope.view.itineraryStops[i];
              if (stop.itineraryStopId === calEvent.itineraryStopId) {
                  itineraryStop = stop;
                  break;
              }
          }
          $scope.view.selectedCalendarItineraryStop = itineraryStop;
          $scope.view.calendarConfig.calendar.defaultDate = calEvent.start;
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

      function getItineraryStopsByParticipant(participant) {
          var itineraryStops = [];
          angular.forEach($scope.view.itineraryStops, function (stop, eventSourceIndex) {
              var containsParticipant = false;
              angular.forEach(stop.groups, function (group, groupIndex) {
                  angular.forEach(group.participants, function (groupParticipant, groupParticipantIndex) {
                      if (groupParticipant.participantId === participant.participantId) {
                          containsParticipant = true;
                      }
                  });
              })
              angular.forEach(stop.participants, function (stopParticipant, stopParticipantIndex) {
                  if (stopParticipant.participantId === participant.participantId) {
                      containsParticipant = true;
                  }
              });
              if (containsParticipant) {
                  itineraryStops.push(stop);
              }
          });
          return itineraryStops;
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
