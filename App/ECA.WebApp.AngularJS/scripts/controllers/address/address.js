'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressCtrl
 * @description The address control is use to control a single address.
 * # AddressCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddressCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        FilterService,
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
      $scope.view.isLoadingCountries = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isDeletingAddress = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.searchLimit = 10;
      $scope.view.autopopulateOnCitySelect = true;
      $scope.view.collapseAddress = true;      

      var originalAddress = angular.copy($scope.address);
      

      if (!isNewAddress($scope.address) && $scope.address.isPrimary) {
          $scope.$parent.view.collapseAddresses = false;
          $scope.view.collapseAddress = false;
      }

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
          console.assert($scope.modelType, 'The addressable type must be defined.');
          console.assert($scope.modelId, 'The entity model id must be defined.');
          var addressableType = $scope.modelType;
          var modelId = $scope.modelId;
          if (isNewAddress($scope.address)) {
              var tempId = angular.copy($scope.address.addressId);
              return AddressService.add($scope.address, addressableType, modelId)
                .then(onSaveAddressSuccess)
                .then(function () {
                    updateAddressFormDivId(tempId);
                })
                .catch(onSaveAddressError);
          }
          else {
              return AddressService.update($scope.address, addressableType, modelId)
                  .then(onSaveAddressSuccess)
                  .catch(onSaveAddressError);
          }
      };

      $scope.view.cancelAddressChanges = function () {
          $scope.view.showEditAddress = false;
          $scope.view.collapseAddress = true;
          $scope.form.addressForm.$setPristine();
          $scope.form.addressForm.$setUntouched();
          if (isNewAddress($scope.address)) {
              removeAddressFromView($scope.address);
          }
          else {
              $scope.address = angular.copy(originalAddress);
          }
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

      $scope.view.onEditAddressClick = function () {
          $scope.view.showEditAddress = true;
          $scope.view.collapseAddress = false;
          var id = getAddressFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 150,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          }
          smoothScroll(getAddressFormDivElement(id), options);
      };

      $scope.view.onDeleteAddressClick = function () {
          console.assert($scope.modelType, 'The addressable type must be defined.');
          console.assert($scope.modelId, 'The entity model id must be defined.');
          var addressableType = $scope.modelType;
          var modelId = $scope.modelId;
          $scope.view.isDeletingAddress = true;
          return AddressService.delete($scope.address, addressableType, modelId)
          .then(function () {
              NotificationService.showSuccessMessage("Successfully deleted address.");
              $scope.view.isDeletingAddress = false;
              removeAddressFromView($scope.address);
          })
          .catch(function () {
              var message = "Unable to delete address.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      $scope.view.onIsPrimaryChange = function () {
          $scope.$emit(ConstantsService.primaryAddressChangedEventName, $scope.address);
      }

      function removeAddressFromView(address) {
          $scope.$emit(ConstantsService.removeNewAddressEventName, $scope.address);
      }

      function getAddressFormDivIdPrefix() {
          return 'addressForm';
      }

      function getAddressFormDivId() {
          return getAddressFormDivIdPrefix() + $scope.address.addressId;
      }

      function updateAddressFormDivId(tempId) {
          var id = getAddressFormDivIdPrefix() + tempId;
          var e = getAddressFormDivElement(id);
          e.id = getAddressFormDivIdPrefix() + $scope.address.addressId.toString();
      }

      function getAddressFormDivElement(id) {
          return document.getElementById(id)
      }

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
          return address.isNew;
      }

      function getDivisions(search) {
          return getLocations(search, ConstantsService.locationType.division.id, $scope.view.isLoadingDivisions);
      }

      function getCities(search) {
          return getLocations(search, ConstantsService.locationType.city.id, $scope.view.isLoadingCities);
      }

      function getCountries(search) {
          return getLocations(search, ConstantsService.locationType.country.id, $scope.view.isLoadingCountries);
      }

      var locationsFilter = FilterService.add('address_locationsfilter');
      function getLocations(search, locationTypeId, loadingIndicator) {
          loadingIndicator = true;
          locationsFilter.reset();
          locationsFilter = locationsFilter.skip(0).take($scope.view.searchLimit).equal('locationTypeId', locationTypeId);
          if (search) {
              locationsFilter = locationsFilter.like('name', search);
          }
          return LocationService.get(locationsFilter.toParams())
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
      $scope.$parent.data.loadAddressTypesPromise.promise.then(function (addressTypes) {
          $scope.view.addressTypes = addressTypes;
          $scope.view.isLoadingRequiredData = false;
      });
  });
