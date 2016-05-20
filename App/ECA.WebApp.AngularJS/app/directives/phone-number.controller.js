'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressCtrl
 * @description The phone number control is use to control a single phone number.
 * # PhoneNumberCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('PhoneNumberCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        LookupService,
        PhoneNumberService,
        ConstantsService,
        PersonService,
        NotificationService) {

      //angular.element('#phoneNumberValue').intlTelInput({
      //    utilsScript: "bower_components/intl-tel-input/lib/libphonenumber/build/utils.js",
      //    validationScript: "bower_components/international-phone-number/releases/international-phone-number.min.js"
      //});

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.phoneNumberTypes = [];
      $scope.view.showEditPhoneNumber = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isLoadingRequiredData = false;
      var originalPhoneNumber = angular.copy($scope.phoneNumber);

      $scope.view.savePhoneNumberChanges = function () {
          $scope.view.isSavingChanges = true;
          console.assert($scope.modelType, 'The phoneNumberable type must be defined.');
          console.assert($scope.modelId, 'The entity model id must be defined.');
          var phoneNumberableType = $scope.modelType;
          var modelId = $scope.modelId;
          
          if (isNewPhoneNumber($scope.phoneNumber)) {
              var tempId = angular.copy($scope.phoneNumber.id);
              return PhoneNumberService.add($scope.phoneNumber, phoneNumberableType, modelId)
                .then(onSavePhoneNumberSuccess)
                .then(function () {
                    updatePhoneNumberFormDivId(tempId);
                    updatePhoneNumbers(tempId, $scope.phoneNumber)
                })
                .catch(onSavePhoneNumberError);
          }
          else {
              return PhoneNumberService.update($scope.phoneNumber, phoneNumberableType, modelId)
                  .then(onSavePhoneNumberSuccess)
                  .catch(onSavePhoneNumberError);
          }
      };

      function updatePhoneNumbers(tempId, phoneNumber) {
          var index = $scope.phoneNumberable.phoneNumbers.map(function (e) { return e.id }).indexOf(tempId);
          $scope.phoneNumberable.phoneNumbers[index] = phoneNumber;
      };

      //function getPhoneCountryFlags() {
      //    var found = false;
      //    angular.forEach($scope.phoneNumberable.phoneNumbers, function (phone, index) {
      //        if (!found) {
      //            angular.forEach($scope.data.countries, function (country) {
      //                if (country.dialCode.indexOf(phone.number.slice(0, 2)) == 0 || country.dialCode.indexOf(phone.number.slice(0, 3)) == 0) {
      //                    phone.flag = 'iti-flag ' + country.iso2;
      //                    phone.flagtitle = country.name;
      //                    found = true;
      //                }
      //            });
      //        }
      //    });
      //}
      
      $scope.view.cancelPhoneNumberChanges = function () {
          $scope.view.showEditPhoneNumber = false;
          if (isNewPhoneNumber($scope.phoneNumber)) {
              removePhoneNumberFromView($scope.phoneNumber);
          }
          else {
              $scope.phoneNumber = angular.copy(originalPhoneNumber);
          }
      };

      $scope.view.onDeletePhoneNumberClick = function () {
          if (isNewPhoneNumber($scope.phoneNumber)) {
              removePhoneNumberFromView($scope.phoneNumber);
          }
          else {
              $scope.view.isDeletingPhoneNumber = true;
              console.assert($scope.modelType, 'The phoneNumberable type must be defined.');
              console.assert($scope.modelId, 'The entity model id must be defined.');
              var phoneNumberableType = $scope.modelType;
              var modelId = $scope.modelId;
              return PhoneNumberService.delete($scope.phoneNumber, phoneNumberableType, modelId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted phone number.");
                  $scope.view.isDeletingPhoneNumber = false;
                  removePhoneNumberFromView($scope.phoneNumber);
              })
              .catch(function () {
                  var message = "Unable to delete phone number.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      };

      $scope.view.onIsPrimaryChange = function () {
          $scope.$emit(ConstantsService.primaryPhoneNumberChangedEventName, $scope.phoneNumber);
      }

      $scope.view.onEditPhoneNumberClick = function () {
          $scope.view.showEditPhoneNumber = true;
          var id = getPhoneNumberFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          };
          smoothScroll(getPhoneNumberFormDivElement(id), options);
      };

      $scope.view.onPhoneNumberTypeChange = function () {
          var phoneNumberTypeId = $scope.phoneNumber.phoneNumberTypeId;
          angular.forEach($scope.view.phoneNumberTypes, function (type, index) {
              if (type.id === phoneNumberTypeId) {
                  $scope.phoneNumber.value = "";
              }
          });
      };

      function removePhoneNumberFromView(phoneNumber) {
          $scope.$emit(ConstantsService.removeNewPhoneNumberEventName, phoneNumber);
      }

      function getPhoneNumberFormDivIdPrefix() {
          return 'phoneNumberForm';
      }

      function getPhoneNumberFormDivId() {
          return getPhoneNumberFormDivIdPrefix() + $scope.phoneNumber.id;
      }
      
      function updatePhoneNumberFormDivId(tempId) {
          var id = getPhoneNumberFormDivIdPrefix() + tempId;
          var e = getPhoneNumberFormDivElement(id);
          e.id = getPhoneNumberFormDivIdPrefix() + $scope.phoneNumber.id.toString();
      }

      function getPhoneNumberFormDivElement(id) {
          return document.getElementById(id);
      }

      function onSavePhoneNumberSuccess(response) {
          $scope.phoneNumber = response.data;
          originalPhoneNumber = angular.copy($scope.phoneNumber);
          NotificationService.showSuccessMessage("Successfully saved changes to phone number.");
          $scope.view.showEditPhoneNumber = false;
          $scope.view.isSavingChanges = false;
      }

      function onSavePhoneNumberError() {
          var message = "Failed to save phone number changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewPhoneNumber(phoneNumber) {
          return phoneNumber.isNew;
      }

      $scope.view.isLoadingRequiredData = true;
      $scope.$parent.data.loadPhoneNumberTypesPromise.promise.then(function (phoneNumberTypes) {
          $scope.view.phoneNumberTypes = phoneNumberTypes;
          $scope.view.isLoadingRequiredData = false;
      });

      //$q.all(getPhoneCountryFlags());

  });
