'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ItineraryStopCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        filterFilter,
        NotificationService,
        ConstantsService,
        LocationService,
        DateTimeService,
        ItineraryService,
        FilterService) {
      $scope.view = {};
      $scope.view.isInEditMode = false;
      $scope.view.itineraryStop = $scope.itineraryStop;
      $scope.view.itinerary = $scope.itinerary;
      $scope.view.isArrivalDateOpen = false;
      $scope.view.isDepartureDateOpen = false;
      $scope.view.isSavingItineraryStop = false;
      $scope.view.isLoadingTimezone = false;
      $scope.view.searchLimit = 30;
      $scope.view.maxNameLength = 100;
      $scope.view.timezoneNames = moment.tz.names();
      $scope.view.isLoadingItineraryStops = false;

      $scope.view.onSaveClick = function () {
          saveItineraryStop($scope.view.itineraryStop);
      }

      $scope.view.onCancelClick = function () {
          $scope.view.itineraryStop = angular.copy(itineraryStopCopy);
          ItineraryService.initializeItineraryStopModel($scope.view.itineraryStop);
          initialize($scope.view.itineraryStop);
          $scope.view.isInEditMode = false;
      }

      $scope.view.openArrivalDate = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isArrivalDateOpen = true;
      }

      $scope.view.openDepartureDate = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isDepartureDateOpen = true;
      }

      $scope.view.onExpandItineraryStopClick = function (itineraryStop) {
          itineraryStop.isExpanded = true;
          $scope.$emit(ConstantsService.itineraryStopExpandedEventName, itineraryStop);
          scrollToItineraryStop(itineraryStop);
      }

      $scope.view.onCollapseItineraryStopClick = function (itineraryStop) {
          itineraryStop.isExpanded = false;
          $scope.$emit(ConstantsService.itineraryStopExpandedEventName, itineraryStop);
          scrollToItineraryStop(itineraryStop);
      }

      $scope.view.onEditClick = function (itineraryStop) {
          $log.info('edit itinerary stop');
          $scope.view.isInEditMode = true;
      }

      $scope.view.onCommentClick = function (itineraryStop) {
          $log.info('comment itinerary stop');
      }

      $scope.view.onDeleteClick = function (itineraryStop) {
          $log.info('delete itinerary stop');
      }

      $scope.view.getDivId = function (itineraryStop) {
          return 'itineraryStop' + itineraryStop.itineraryStopId;
      }

      $scope.view.isItineraryStopNameUnique = function ($value) {
          var dfd = $q.defer();
          if ($value && $value.trim().length > 0) {
              $scope.view.isLoadingItineraryStops = true;
              ItineraryService.getItineraryStops($scope.view.itinerary.projectId, $scope.view.itinerary.id)
              .then(function (response) {
                  $scope.view.isLoadingItineraryStops = false;
                  angular.forEach(response.data, function (stop, index) {
                      stop.name = stop.name.toLowerCase().trim();
                  });
                  var itineraryStopIds = response.data.map(function (i) {
                      return i.itineraryStopId;
                  });
                  var index = itineraryStopIds.indexOf($scope.view.itineraryStop.itineraryStopId);
                  response.data.splice(index, 1);
                  var likeStops = filterFilter(response.data, { name: $value.trim().toLowerCase() }, true);
                  if (likeStops.length == 0) {
                      dfd.resolve();
                  }
                  else {
                      dfd.reject();
                  }
              })
              .catch(function (response) {
                  $scope.view.isLoadingItineraryStops = false;
                  var message = "Unable to load city stops.";
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

      var departureFilter = FilterService.add('additinerarystopmodal_destinationlocations');
      $scope.view.getDestinationLocations = function ($search) {
          var params = getSearchParams(departureFilter, $search, [
              ConstantsService.locationType.city.id,
              ConstantsService.locationType.division.id,
              ConstantsService.locationType.country.id,
              ConstantsService.locationType.region.id
          ]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.destinationLocations = data.results;
              $scope.view.destinationLocationsCount = data.total;
              return data.results;
          });
      }

      $scope.view.onDestinationSelect = function ($item, $model) {
          $scope.view.itineraryStop.destinationLocation = $model;
          $scope.view.itineraryStop.destinationLocationId = $model.id;
          $scope.view.itineraryStop.timezoneId = null;
          $scope.view.isLoadingTimezone = true;
          return LocationService.getLocationByTimezone($model)
          .then(function (timezoneId) {
              $scope.view.isLoadingTimezone = false;
              if (timezoneId) {
                  $scope.view.itineraryStop.timezoneId = timezoneId;
              }
          })
          .catch(function () {
              $scope.view.isLoadingTimezone = false;
          });
      }

      function scrollToItineraryStop(itineraryStop) {
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
          var id = $scope.view.getDivId(itineraryStop)
          var e = document.getElementById(id);
          smoothScroll(e, options);
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

      function saveItineraryStop(itineraryStop) {
          $scope.view.isSavingItineraryStop = true;
          
          itineraryStop.setArrivalDateFromDateAndTime($scope.view.itineraryStop.arrivalDate, $scope.view.itineraryStop.arrivalDate);
          itineraryStop.setDepartureDateFromDateAndTime($scope.view.itineraryStop.departureDate, $scope.view.itineraryStop.departureDate);
          
          return ItineraryService.updateItineraryStop(itineraryStop, itineraryStop.projectId, itineraryStop.itineraryId)
          .then(function (response) {
              initialize(response.data);
              $scope.view.itineraryStop = response.data;
              initialize($scope.view.itineraryStop);
              copyItineraryStop($scope.view.itineraryStop);

              $scope.view.isSavingItineraryStop = false;
              $scope.view.isInEditMode = false;
              NotificationService.showSuccessMessage("Successfully updated city stop.");
              return $scope.view.itineraryStop;
          })
          .catch(function (response) {
              $scope.view.isSavingItineraryStop = false;
              if (response.status === 400) {
                  if (response.data.message && response.data.modelState) {
                      for (var key in response.data.modelState) {
                          NotificationService.showErrorMessage(response.data.modelState[key][0]);
                      }
                  }
                  else if (response.data.Message && response.data.ValidationErrors) {
                      for (var key in response.data.ValidationErrors) {
                          NotificationService.showErrorMessage(response.data.ValidationErrors[key]);
                      }
                  }
              }
              else {
                  var message = 'Unable to save itinerary stop.';
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
          });
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

      function initialize(itineraryStop) {
          setItineraryStopArrivalAndDepartureDatesAsLocalDates(itineraryStop);
      }

      function setItineraryStopArrivalAndDepartureDatesAsLocalDates(itineraryStop) {
          $scope.view.itineraryStop.arrivalDate = DateTimeService.getDateAsLocalDisplayMoment(itineraryStop.destinationArrivalMoment).toDate();
          $scope.view.itineraryStop.departureDate = DateTimeService.getDateAsLocalDisplayMoment(itineraryStop.destinationDepartureMoment).toDate();
      }

      function getArrivalDate() {
          return DateTimeService.getDateAndTimeInTimezoneAsMoment($scope.view.arrivalDate, $scope.view.arrivalTime, $scope.view.itineraryStop.timeezoneId);
      }

      function getDepartureDate() {
          return DateTimeService.getDateAndTimeInTimezoneAsMoment($scope.view.departureDate, $scope.view.departureTime, $scope.view.itineraryStop.timeezoneId);
      }

      var itineraryStopCopy = null;
      function copyItineraryStop(itineraryStop) {
          itineraryStopCopy = angular.copy(itineraryStop);
          return itineraryStopCopy;
      }
      
      initialize($scope.view.itineraryStop);
      copyItineraryStop($scope.view.itineraryStop);
  });
