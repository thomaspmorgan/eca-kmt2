'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:LocationsCtrl
 * @description The locations controller is used to control the list of locations.
 * # LocationsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SearchLocationsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $modalInstance,
        smoothScroll,
        LookupService,
        LocationService,
        ConstantsService,
        NotificationService,
        TableService,
        FilterService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.start = 0;
      $scope.view.limit = 4;
      $scope.view.total = 0;
      $scope.view.locations = [];
      $scope.view.locationTypes = [];
      $scope.view.countries = [];
      $scope.view.regions = [];
      $scope.view.selectedLocationTypes = [];
      $scope.view.selectedCountries = [];
      $scope.view.selectedRegions = [];
      $scope.view.isLoadingLocations = false;
      $scope.view.isLoadingRequiredData = false;

      $scope.view.onSelectClick = function () {
          var selectedLocations = [];
          angular.forEach($scope.view.locations, function (location, index) {
              if (location.isSelected) {
                  selectedLocations.push(location);
                  delete location.isSelected;
              }
          });
          $modalInstance.close(selectedLocations);
      }
      $scope.view.onCancelClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onAddClick = function () {
          var addLocationModalInstance = $modal.open({
              animation: false,
              templateUrl: 'views/locations/addlocationmodal.html',
              controller: 'AddLocationCtrl',
              size: 'lg',
              resolve: {}
          });
          addLocationModalInstance.result.then(function (addedLocation) {
              $log.info('Finished adding locations.');
              $modalInstance.close([addedLocation]);
              
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      };

      $scope.view.getLocations = function (tableState) {
          return loadLocations(tableState);
      };

      function loadLocations(tableState) {
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };
          $scope.view.isLoadingLocations = true;
          return LocationService.get(params, tableState)
          .then(function (response) {
              var data = response.results;
              var total = response.total;
              var limit = TableService.getLimit();
              var start = TableService.getStart();
              tableState.pagination.numberOfPages = Math.ceil(total / limit);
              $scope.view.start = start + 1;
              $scope.view.end = start + data.length;
              $scope.view.total = total;
              $scope.view.locations = data;
              $scope.view.isLoadingLocations = false;

          })
          .catch(function () {
              $scope.view.isLoadingLocations = false;
              var message = "Unable to load locations.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var countriesFilter = FilterService.add('countries');
      var countriesParams = countriesFilter.skip(0).take(300).equal('locationTypeId', ConstantsService.locationType.country.id).toParams();
      function loadContries() {
          return LocationService.get(countriesParams)
          .then(function (response) {
              $scope.view.countries = response.results;
          })
          .catch(function () {
              var message = "Unable to load countries.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var regionsFilter = FilterService.add('regions');
      var regionsParams = regionsFilter.skip(0).take(25).equal('locationTypeId', ConstantsService.locationType.region.id).toParams();
      function loadRegions() {
          return LocationService.get(regionsParams)
          .then(function (response) {
              $scope.view.regions = response.results;
          })
          .catch(function () {
              var message = "Unable to load regions.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function getLocationTypes() {
          var params = { limit: 300 };
          return LookupService.getLocationTypes(params)
          .then(function (response) {
              $scope.view.locationTypes = response.data.results;
          })
          .catch(function (response) {
              var message = "Unable to load location types.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      $scope.view.isLoadingRequiredData = true;
      $q.all([getLocationTypes(), loadContries(), loadRegions()])
        .then(function () {
            $scope.view.isLoadingRequiredData = false;
        })
        .catch(function () {
            $scope.view.isLoadingRequiredData = false;
        });
  });
