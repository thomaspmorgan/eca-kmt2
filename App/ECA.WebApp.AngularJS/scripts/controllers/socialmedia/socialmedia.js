'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressCtrl
 * @description The address control is use to control a single address.
 * # AddressCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SocialMediaCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        LookupService,
        AddressService,
        ConstantsService,
        LocationService,
        OrganizationService,
        PersonService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.socialMediaTypes = [];
      $scope.view.showEditSocialMedia = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.searchLimit = 10;
      $scope.view.autopopulateOnCitySelect = true;
      var originalSocialMedia = angular.copy($scope.socialMedia);

      var socialableTypeToServiceMapping = {
          'organization': OrganizationService,
          'person': PersonService
      };

      $scope.view.getSocialMediaTypes = function ($viewValue) {
          return getCities($viewValue);
      };



      $scope.view.saveSocialMediaChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewAddress($scope.address)) {
              console.assert($scope.address.addressableType, 'The addressable type must be defined.');
              var addressableType = $scope.address.addressableType;
              console.assert(addressableTypeToServiceMapping[addressableType], 'The mapping must contain a value for the addressable type [' + addressableType + '].');
              var service = addressableTypeToServiceMapping[addressableType];
              console.assert(service.addAddress, 'The service for the addressable type [' + $scope.address.addressableType + '] must have an addAddress method defined.');
              console.assert(typeof service.addAddress === 'function', 'The service addAddress property must be a function.');

              var tempId = angular.copy($scope.address.addressId);
              return service.addAddress($scope.address)
                .then(onSaveAddressSuccess)
                .then(function () {
                    updateAddressFormDivId(tempId);
                })
                .catch(onSaveAddressError);
          }
          else {
              return AddressService.update($scope.address)
                  .then(onSaveAddressSuccess)
                  .catch(onSaveAddressError);
          }
      };

      $scope.view.cancelSocialMediaChanges = function () {
          $scope.view.showEditSocialMedia = false;
          $scope.form.addressForm.$setPristine();
          $scope.form.addressForm.$setUntouched();
          if (isNewAddress($scope.address)) {
              $scope.$emit(ConstantsService.removeNewAddressEventName, $scope.address);
          }
          else {
              $scope.socialMedia = angular.copy(originalSocialMedia);
          }
      };

      $scope.view.onEditSocialMediaClick = function () {
          $scope.view.showEditSocialMedia = true;
          var id = getSocialMediaFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 150,
              callbackBefore: function (element) {},
              callbackAfter: function (element) { }
          }
          smoothScroll(getSocialMediaFormDivElement(id), options);
      };     

      function getSocialMediaFormDivIdPrefix(){
          return 'socialMediaForm';
      }

      function getSocialMediaFormDivId() {
          return getSocialMediaFormDivIdPrefix() + $scope.socialMedia.id;
      }
      
      function updateSocialMediaFormDivId(tempId) {          
          var id = getSocialMediaFormDivIdPrefix() + tempId;
          var e = getSocialMediaFormDivElement(id);
          e.id = getSocialMediaFormDivIdPrefix() + $scope.socialMedia.id.toString();
      }

      function getSocialMediaFormDivElement(id) {
          return document.getElementById(id)
      }

      function onSaveAddressSuccess(response) {
          $scope.socialMedia = response.data;
          originalSocialMedia = angular.copy($scope.socialMedia);
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


      $scope.view.isLoadingRequiredData = true;
      $q.all([getAddressTypes()])
      .then(function () {
          $log.info('Loaded all resources.');
          $scope.view.isLoadingRequiredData = false;
      })

  });
