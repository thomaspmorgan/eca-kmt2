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
        $modal,
        $modalInstance,
        project,
        NotificationService,
        ConstantsService,
        LocationService,
        MessageBox,
        ProjectService,
        FilterService,
        LookupService) {

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
          return ProjectService.addItinerary($scope.view.itinerary, project.id)
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
