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
        SocialMediaService,
        ConstantsService,
        OrganizationService,
        PersonService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.socialMediaTypes = [];
      $scope.view.showEditSocialMedia = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isLoadingRequiredData = false;
      var originalSocialMedia = angular.copy($scope.socialMedia);

      $scope.view.saveSocialMediaChanges = function () {
          $scope.view.isSavingChanges = true;
          console.assert($scope.modelType, 'The socialable type must be defined.');
          console.assert($scope.modelId, 'The entity model id must be defined.');
          var sociableType = $scope.modelType;
          var modelId = $scope.modelId;

          if (isNewSocialMedia($scope.socialMedia)) {
              var tempId = angular.copy($scope.socialMedia.id);
              return SocialMediaService.add($scope.socialMedia, sociableType, modelId)
                .then(onSaveSocialMediaSuccess)
                .then(function () {
                    updateSocialMediaFormDivId(tempId);
                    updateSocialMedias(tempId, $scope.socialMedia);
                })
                .catch(onSaveSocialMediaError);
          }
          else {
              return SocialMediaService.update($scope.socialMedia, sociableType, modelId)
                  .then(onSaveSocialMediaSuccess)
                  .catch(onSaveSocialMediaError);
          }
      };

      function updateSocialMedias(tempId, socialMedia) {
          var index = $scope.socialable.socialMedias.map(function (e) { return e.id }).indexOf(tempId);
          $scope.socialable.socialMedias[index] = socialMedia;
      };

      $scope.view.cancelSocialMediaChanges = function () {
          $scope.view.showEditSocialMedia = false;
          if (isNewSocialMedia($scope.socialMedia)) {
              removeSocialMediaFromView($scope.socialMedia);
          }
          else {
              $scope.socialMedia = angular.copy(originalSocialMedia);
          }
          $scope.form.socialMediaForm.$setPristine();
          $scope.form.socialMediaForm.$setUntouched();
      };

      $scope.view.onDeleteSocialMediaClick = function () {
          if (isNewSocialMedia($scope.socialMedia)) {
              removeSocialMediaFromView($scope.socialMedia);
          }
          else {
              $scope.view.isDeletingSocialMedia = true;
              console.assert($scope.modelType, 'The socialable type must be defined.');
              console.assert($scope.modelId, 'The entity model id must be defined.');
              var sociableType = $scope.modelType;
              var modelId = $scope.modelId;
              var test = SocialMediaService.delete($scope.socialMedia, sociableType, modelId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted address.");
                  $scope.view.isDeletingSocialMedia = false;
                  removeSocialMediaFromView($scope.socialMedia);
              })
              .catch(function () {
                  var message = "Unable to delete address.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }

      };

      $scope.view.onEditSocialMediaClick = function () {
          $scope.view.showEditSocialMedia = true;
          var id = getSocialMediaFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          };
          smoothScroll(getSocialMediaFormDivElement(id), options);
      };

      $scope.view.onSocialMediaTypeChange = function () {
          var socialMediaTypeId = $scope.socialMedia.socialMediaTypeId;
          angular.forEach($scope.view.socialMediaTypes, function (type, index) {
              if (type.id === socialMediaTypeId) {
                  $scope.socialMedia.value = type.url;
              }
          });
      };

      function removeSocialMediaFromView(socialMedia) {
          $scope.$emit(ConstantsService.removeNewSocialMediaEventName, socialMedia);
      }

      function getSocialMediaFormDivIdPrefix() {
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
          return document.getElementById(id);
      }

      function onSaveSocialMediaSuccess(response) {
          $scope.socialMedia = response.data;
          originalSocialMedia = angular.copy($scope.socialMedia);
          NotificationService.showSuccessMessage("Successfully saved changes to social media.");
          $scope.view.showEditSocialMedia = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveSocialMediaError() {
          var message = "Failed to save social media changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewSocialMedia(socialMedia) {
          return socialMedia.isNew;
      }

      $scope.view.isLoadingRequiredData = true;
      $scope.$parent.data.loadSocialMediaTypesPromise.promise.then(function (socialMediaTypes) {
          $scope.view.socialMediaTypes = socialMediaTypes;
          $scope.view.isLoadingRequiredData = false;
      });
  });
