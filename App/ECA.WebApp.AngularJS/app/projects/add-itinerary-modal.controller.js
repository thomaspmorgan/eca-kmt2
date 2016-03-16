'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddItineraryModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $timeout,
        $modal,
        $modalInstance,
        project,
        filterFilter,
        NotificationService,
        ConstantsService,
        LocationService,
        ItineraryService,
        FilterService) {

      $scope.view = {};
      $scope.view.showConfirmCancel = false;
      $scope.view.maxNameLength = 100;
      $scope.view.project = project;
      $scope.view.isSavingItinerary = false;
      $scope.view.arrivalLocations = [];
      $scope.view.arrivalLocationsCount = 0;
      $scope.view.departureLocations = [];
      $scope.view.departureLocationsCount = 0;
      $scope.view.searchLimit = 30;
      $scope.view.isStartDateOpen = false;
      $scope.view.isEndDateOpen = false;
      $scope.view.isLoadingItineraries = false;

      $scope.view.itinerary = {};

      $scope.view.onSaveClick = function () {
          saveItinerary();
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.itineraryForm.$dirty) {
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

      $scope.view.openStartDate = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isStartDateOpen = true;
      }

      $scope.view.openEndDate = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isEndDateOpen = true;
      }

      $scope.view.isItineraryNameUnique = function ($value) {
          var dfd = $q.defer();
          if ($value && $value.trim().length > 0) {
              $scope.view.isLoadingItineraries = true;
              ItineraryService.getItineraries(project.id)
              .then(function (response) {
                  $scope.view.isLoadingItineraries = false;
                  angular.forEach(response.data, function (itinerary, index) {
                      itinerary.name = itinerary.name.toLowerCase().trim();
                  });
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


      var arrivalFilter = FilterService.add('additinerarymodal_arrivallocations');
      $scope.view.getArrivalLocations = function ($search) {
          var params = getSearchParams(arrivalFilter, $search, [ConstantsService.locationType.city.id]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.arrivalLocations = data.results;
              $scope.view.arrivalLocationsCount = data.total;
              return data.results;
          });
      }

      var departureFilter = FilterService.add('additinerarymodal_departurelocations');
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

      $scope.view.onDepartureLocationSelect = function ($item, $model) {
          $scope.view.itinerary.departureLocation = $model;
          $scope.view.itinerary.departureLocationId = $model.id;
      }

      $scope.view.onArrivalLocationSelect = function ($item, $model) {
          $scope.view.itinerary.arrivalLocation = $model;
          $scope.view.itinerary.arrivalLocationId = $model.id;
      }

      $scope.view.onAddNewArrivalLocationClick = function ($event) {
          var setArrivalLocationCallback = function (addedLocation) {
              $scope.view.onArrivalLocationSelect(addedLocation[0], addedLocation[0]);
          };
          addNewLocation(setArrivalLocationCallback);
          
          $event.preventDefault();
          $event.stopPropagation();
      }

      $scope.view.onAddNewDepartureDestinationLocationClick = function () {
          var setDepartureDestinationLocationCallback = function (addedLocation) {
              $scope.view.onDepartureLocationSelect(addedLocation[0], addedLocation[0]);
          };
          addNewLocation(setDepartureDestinationLocationCallback);
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

      function saveItinerary() {
          $scope.view.isSavingItinerary = true;
          return ItineraryService.addItinerary($scope.view.itinerary, project.id)
          .then(function (response) {
              $scope.view.isSavingItinerary = false;
              $modalInstance.close(response.data);
          })
          .catch(function (response) {
              $scope.view.isSavingItinerary = false;
              var message = 'Unable to save itinerary.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
  });
