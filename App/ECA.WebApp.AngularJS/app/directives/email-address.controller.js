'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressCtrl
 * @description The email address control is use to control a single email address.
 * # EmailAddressCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('EmailAddressCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        LookupService,
        EmailAddressService,
        ConstantsService,
        PersonService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.emailAddressTypes = [];
      $scope.view.isSavingChanges = false;
      $scope.view.isLoadingRequiredData = false;
      var originalEmailAddress = angular.copy($scope.emailAddress);


      $scope.view.saveEmailAddressChanges = function () {
          $scope.view.isSavingChanges = true;
          console.assert($scope.modelType, 'The emailAddressable type must be defined.');
          console.assert($scope.modelId, 'The entity model id must be defined.');
          var emailAddressableType = $scope.modelType;
          var modelId = $scope.modelId;

          if (isNewEmailAddress($scope.emailAddress)) {
              var tempId = angular.copy($scope.emailAddress.id);
              return EmailAddressService.add($scope.emailAddress, emailAddressableType, modelId)
                .then(onSaveEmailAddressSuccess)
                .then(function () {
                    updateEmailAddressFormDivId(tempId);
                    updateEmailAddresses(tempId, $scope.emailAddress);
                })
                .catch(onSaveEmailAddressError);
          }
          else {
              return EmailAddressService.update($scope.emailAddress, emailAddressableType, modelId)
                  .then(onSaveEmailAddressSuccess)
                  .catch(onSaveEmailAddressError);
          }
      };

      function updateEmailAddresses(tempId, emailAddress) {
          var index = $scope.emailAddressable.emailAddresses.map(function (e) { return e.id; }).indexOf(tempId);
          $scope.emailAddressable.emailAddresses[index] = emailAddress;
      };

      $scope.view.cancelEmailAddressChanges = function () {
          $scope.view.showEditEmailAddress = false;
          if (isNewEmailAddress($scope.emailAddress)) {
              removeEmailAddressFromView($scope.emailAddress);
          }
          else {
              $scope.emailAddress = angular.copy(originalEmailAddress);
          }
      };

      $scope.view.onDeleteEmailAddressClick = function () {
          if (isNewEmailAddress($scope.emailAddress)) {
              removeEmailAddressFromView($scope.emailAddress);
          }
          else {
              $scope.view.isDeletingEmailAddress = true;
              console.assert($scope.modelType, 'The emailAddressable type must be defined.');
              console.assert($scope.modelId, 'The entity model id must be defined.');
              var emailAddressableType = $scope.modelType;
              var modelId = $scope.modelId;
              return EmailAddressService.delete($scope.emailAddress, emailAddressableType, modelId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted email address.");
                  $scope.view.isDeletingEmailAddress = false;
                  removeEmailAddressFromView($scope.emailAddress);
              })
              .catch(function () {
                  var message = "Unable to delete email address.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      };

      $scope.view.onIsPrimaryChange = function () {
          $scope.$emit(ConstantsService.primaryEmailAddressChangedEventName, $scope.emailAddress);
      }

      $scope.view.onEditEmailAddressClick = function () {
          $scope.view.showEditEmailAddress = true;
          var id = getEmailAddressFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          };
          smoothScroll(getEmailAddressFormDivElement(id), options);
      };

      $scope.view.onEmailAddressTypeChange = function () {
          var emailAddressTypeId = $scope.emailAddress.emailAddressTypeId;
          angular.forEach($scope.view.emailAddressTypes, function (type, index) {
              if (type.id === emailAddressTypeId) {
                  $scope.emailAddress.value = "";
              }
          });
      };

      function removeEmailAddressFromView(emailAddress) {
          $scope.$emit(ConstantsService.removeNewEmailAddressEventName, emailAddress);
      }

      function getEmailAddressFormDivIdPrefix() {
          return 'emailAddressForm';
      }

      function getEmailAddressFormDivId() {
          return getEmailAddressFormDivIdPrefix() + $scope.emailAddress.id;
      }
      
      function updateEmailAddressFormDivId(tempId) {
          var id = getEmailAddressFormDivIdPrefix() + tempId;
          var e = getEmailAddressFormDivElement(id);
          e.id = getEmailAddressFormDivIdPrefix() + $scope.emailAddress.id.toString();
      }

      function getEmailAddressFormDivElement(id) {
          return document.getElementById(id);
      }

      function onSaveEmailAddressSuccess(response) {
          $scope.emailAddress = response.data;
          originalEmailAddress = angular.copy($scope.emailAddress);
          NotificationService.showSuccessMessage("Successfully saved changes to email address.");
          $scope.view.showEditEmailAddress = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEmailAddressError() {
          var message = "Failed to save email address changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewEmailAddress(emailAddress) {
          return emailAddress.isNew;
      }

      $scope.view.isLoadingRequiredData = true;
      $scope.$parent.data.loadEmailAddressTypesPromise.promise.then(function (emailAddressTypes) {
          $scope.view.emailAddressTypes = emailAddressTypes;
          $scope.view.isLoadingRequiredData = false;
      });
  });
