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
        OrganizationService,
        PersonService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.addressTypes = [];
      $scope.view.showEditAddress = false;
      $scope.view.isLoadingCities = false;
      $scope.view.isLoadingDivisions = false;
      $scope.view.isLoadingCountries = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.searchLimit = 10;
      $scope.view.autopopulateOnCitySelect = true;
      var originalAddress = angular.copy($scope.address);

      var addressableTypeToServiceMapping = {
          'organization': OrganizationService,
          'person': PersonService
      };

      $scope.view.getCities = function ($viewValue) {
          return getCities($viewValue);
      };

      $scope.view.getDivisions = function ($viewValue) {
          return getDivisions($viewValue);
      }

      $scope.view.getCountries = function ($viewValue) {
          return getCountries($viewValue);
      }

      $scope.view.saveAddressChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewAddress($scope.address)) {
              console.assert($scope.address.addressableType, 'The addressable type must be defined.');
              var addressableType = $scope.address.addressableType;
              console.assert(addressableTypeToServiceMapping[addressableType], 'The mapping must contain a value for the addressable type [' + addressableType + '].');
              var service = addressableTypeToServiceMapping[addressableType];
              console.assert(service.addAddress, 'The service must have an addAddress method defined.');
              console.assert(typeof service.addAddress === 'function', 'The service addAddress property must be a function.');
              return service.addAddress($scope.address)
                .then(onSaveAddressSuccess)
                .catch(onSaveAddressError);
          }
          else {
              return AddressService.update($scope.address)
                  .then(onSaveAddressSuccess)
                  .catch(onSaveAddressError);
          }
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
          if ($item.country && $scope.view.autopopulateOnCitySelect) {
              $log.info('Auto populating country to address.');
              $scope.address.country = $item.country;
              $scope.address.countryId = $item.countryId;
          }
          if ($item.division && $scope.view.autopopulateOnCitySelect) {
              $log.info('Auto populating division to address.');
              $scope.address.division = $item.division;
              $scope.address.divisionId = $item.divisionId;
          }
      }

      $scope.view.onSelectDivision = function ($item, $model, $label) {
          $scope.address.division = $item.name;
          $scope.address.divisionId = $item.id;          
      }

      $scope.view.onSelectCountry = function ($item, $model, $label) {
          $scope.address.country = $item.name;
          $scope.address.countryId = $item.id;
      }

      $scope.view.onAddressClick = function (addressableType, entityAddresses, entityId) {          
          console.assert(entityAddresses, 'The entity addresses is not defined.');
          console.assert(entityAddresses instanceof Array, 'The entity address is defined but must be an array.');
          var newAddress = {
              Id: entityId,
              addressableType: addressableType
          };
          entityAddresses.splice(0, 0, newAddress);          
      }

      $scope.view.onSelectCityBlur = function ($event) {
          if ($scope.address.city === '') {
              delete $scope.address.cityId;
              delete $scope.address.city;
          }
      };

      $scope.view.onSelectDivisionBlur = function ($event) {
          if ($scope.address.division === '') {
              delete $scope.address.divisionId;
              delete $scope.address.division;
          }
      };

      $scope.view.onSelectCountryBlur = function ($event) {
          if ($scope.address.country === '') {
              delete $scope.address.countryId;
              delete $scope.address.country;
          }
      };

      function onSaveAddressSuccess(response) {
          $scope.address = response.data;
          originalAddress = angular.copy($scope.address);
          NotificationService.showSuccessMessage("Successfully saved changes to address.");
          $scope.view.showEditAddress = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveAddressError() {
          var message = "Failed to save address changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewAddress(address) {
          return address.addressableType;
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

      function getCountries(search) {
          $scope.view.isLoadingCountries = true;
          return getLocations(search, ConstantsService.locationType.country.id, $scope.view.isLoadingCountries);
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
