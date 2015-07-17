'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationOverviewCtrl
 * @description The overview controller is used on the overview tab of an organization.
 * # OrganizationOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddressCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        LookupService,
        AddressService,
        ConstantsService,
        LocationService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.addressTypes = [];
      $scope.view.showEditAddress = false;
      $scope.view.isLoadingCities = false;
      $scope.view.isLoadingDivisions = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.searchLimit = 10;
      var originalAddress = angular.copy($scope.address);

      $scope.view.getCities = function ($viewValue) {
          return getCities($viewValue);
      };

      $scope.view.getDivisions = function ($viewValue) {
          return getDivisions($viewValue);
      }

      $scope.view.saveAddressChanges = function () {
          $scope.view.isSavingChanges = true;
          return AddressService.update($scope.address)
          .then(function (response) {
              $scope.address = response.data;
              originalAddress = angular.copy($scope.address);
              NotificationService.showSuccessMessage("Successfully saved changes to address.");
              $scope.view.showEditAddress = false;
              $scope.view.isSavingChanges = false;
          })
          .catch(function(){
              var message = "Failed to save address changes.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          })
      };

      $scope.view.cancelAddressChanges = function () {
          $scope.view.showEditAddress = false;
          $scope.form.addressForm.$setPristine();
          $scope.form.addressForm.$setUntouched();
          $scope.address = angular.copy(originalAddress);
      };

      $scope.view.onSelectCity = function ($item, $model, $label) {
          $scope.address.city = $item.name;
          $scope.address.cityId = $item.id;
          if ($item.country) {
              $log.info('Auto populating country to address.');
              $scope.address.country = $item.country;
              $scope.address.countryId = $item.countryId;
          }
          if ($item.division) {
              $log.info('Auto populating division to address.');
              $scope.address.division = $item.division;
              $scope.address.divisionId = $item.divisionId;
          }
      }

      function getAddressTypes() {
          var params = {
              start: 0,
              limit: 300
          };
          return LookupService.getAddressTypes(params)
          .then(function (response) {
              if (response.data.total > params.limit) {
                  var message = "There are more address types than could be loaded.  Not all address types will be shown.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              }
              var addressTypes = response.data.results;
              $scope.view.addressTypes = addressTypes;
              return addressTypes;
          })
          .catch(function () {
              var message = 'Unable to load address types.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function getDivisions(search) {
          $scope.view.isLoadingDivisions = true;
          return getLocations(search, ConstantsService.locationType.state.id, $scope.view.isLoadingDivisions);
      }

      function getCities(search) {
          $scope.view.isLoadingCities = true;
          return getLocations(search, ConstantsService.locationType.city.id, $scope.view.isLoadingCities);
      }

      function getLocations (search, locationTypeId, loadingIndicator){
          var params = {
              start: 0,
              limit: $scope.view.searchLimit,
              filter: [
                  {
                      comparison: ConstantsService.equalComparisonType,
                      value: locationTypeId,
                      property: 'locationTypeId'
                  }
              ]
          };
          if (search) {
              params.filter.push({
                  comparison: ConstantsService.likeComparisonType,
                  value: search,
                  property: 'name'
              });
          }
          return LocationService.get(params)
          .then(function (response) {
              var data = response.results;
              var total = response.total;
              loadingIndicator = false;
              return data;
          })
          .catch(function () {
              var message = 'Unable to load available locations.';
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      $scope.view.isLoadingRequiredData = true;
      $q.all([getAddressTypes()])
      .then(function () {
          $log.info('Loaded all resources.');
          $scope.view.isLoadingRequiredData = false;
      })

  });
