'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddItineraryStopModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $modalInstance,
        project,
        itinerary,
        NotificationService,
        ConstantsService,
        LocationService,
        MessageBox,
        ProjectService,
        FilterService,
        DateTimeService,
        LookupService) {

      $scope.view = {};
      $scope.view.itinerary = itinerary;
      $scope.view.showConfirmCancel = false;
      $scope.view.maxNameLength = 100;
      $scope.view.project = project;
      $scope.view.isSavingItineraryStop = false;
      $scope.view.destinationLocations = [];
      $scope.view.destinationLocationsCount = 0;
      $scope.view.searchLimit = 30;
      $scope.view.isArrivalDateOpen = false;
      $scope.view.isDepartureDateOpen = false;
      $scope.view.currentTimezone = moment.tz.guess();
      $scope.view.timezoneNames = moment.tz.names();

      $scope.view.itineraryStop = {
          itineraryId: itinerary.id,
          projectId: project.id
      };
      ProjectService.initializeItineraryStopModel($scope.view.itineraryStop);

      $scope.view.onSaveClick = function () {
          saveItineraryStop();
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.itineraryStopForm.$dirty) {
              $scope.view.showConfirmCancel = true;
          }
          else {
              $modalInstance.dismiss('cancel');
          }
      }

      $scope.view.onYesCancelChangesClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onNoDoNotCancelChangesClick = function () {
          $scope.view.showConfirmCancel = false;
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

      var departureFilter = FilterService.add('additinerarystopmodal_destinationlocations');
      $scope.view.getDestinationLocations = function ($search) {
          var params = getSearchParams(departureFilter, $search, [
              ConstantsService.locationType.city.id
          ]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.destinationLocations = data.results;
              $scope.view.destinationLocationsCount = data.total;
              return data.results;
          });
      }

      $scope.view.onDestinationLocationSelect = function ($item, $model) {
          $scope.view.itineraryStop.destinationLocation = $model;
          $scope.view.itineraryStop.destinationLocationId = $model.id;
      }

      $scope.view.onAddNewDestinationLocationClick = function () {
          var setDestinationLocationCallback = function (addedLocation) {
              $scope.view.onDestinationLocationSelect(addedLocation[0], addedLocation[0]);
          };
          addNewLocation(setDestinationLocationCallback);
      }

      $scope.view.onClickCurrentTimezone = function (timezone) {
          $scope.view.itineraryStop.timezoneId = timezone;
      }

      function addNewLocation(callback) {
          var addLocationModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/locations/add-location-modal.html',
              controller: 'AddLocationCtrl',
              size: 'lg',
              resolve: {
                  allowedLocationTypeIds: function () {
                      return [
                          ConstantsService.locationType.city.id
                      ];
                  }
              }
          });
          addLocationModalInstance.result.then(function (addedLocation) {
              $log.info('Finished adding locations.');
              addLocationModalInstance.close([addedLocation]);
              callback([addedLocation]);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
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

      function loadLocations(params) {
          return LocationService.get(params)
          .then(function (response) {
              return response;
          })
          .catch(function (response) {
              $log.error('Unable to load locations.')
          })
      }

      function saveItineraryStop() {
          $scope.view.isSavingItineraryStop = true;
          $scope.view.itineraryStop.setArrivalDateFromDateAndTime($scope.view.itineraryStop.arrivalDate, $scope.view.itineraryStop.arrivalTime);
          $scope.view.itineraryStop.setDepartureDateFromDateAndTime($scope.view.itineraryStop.departureDate, $scope.view.itineraryStop.departureTime);

          return ProjectService.addItineraryStop($scope.view.itineraryStop, project.id, itinerary.id)
          .then(function (response) {
              $scope.view.isSavingItineraryStop = false;
              $modalInstance.close(response.data);
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
  });
