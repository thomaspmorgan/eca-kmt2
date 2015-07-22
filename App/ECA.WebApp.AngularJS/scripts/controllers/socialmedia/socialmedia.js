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
      var originalSocialMedia = angular.copy($scope.socialMedia);

      var socialableTypeToServiceMapping = {
          'organization': OrganizationService,
          'person': PersonService
      };

      $scope.view.saveSocialMediaChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewSocialMedia($scope.socialMedia)) {
              console.assert($scope.socialMedia.socialableType, 'The socialable type must be defined.');
              var socialableType = $scope.socialMedia.socialableType;
              console.assert(socialableTypeToServiceMapping[socialableType], 'The mapping must contain a value for the socialable type [' + socialableType + '].');
              var service = socialableTypeToServiceMapping[socialableType];
              console.assert(service.addSocialMedia, 'The service for the socialable type [' + socialableType + '] must have an addSocialMedia method defined.');
              console.assert(typeof service.addSocialMedia === 'function', 'The service addSocialMedia property must be a function.');

              var tempId = angular.copy($scope.socialMedia.id);
              return service.addSocialMedia($scope.socialMedia)
                .then(onSaveSocialMediaSuccess)
                .then(function () {
                    updateSocialMediaFormDivId(tempId);
                })
                .catch(onSaveSocialMediaError);
          }
          else {
              return SocialMediaService.update($scope.socialMedia)
                  .then(onSaveSocialMediaSuccess)
                  .catch(onSaveSocialMediaError);
          }
      };

      $scope.view.cancelSocialMediaChanges = function () {
          $scope.view.showEditSocialMedia = false;
          $scope.form.socialMediaForm.$setPristine();
          $scope.form.socialMediaForm.$setUntouched();
          if (isNewSocialMedia($scope.socialMedia)) {
              $scope.$emit(ConstantsService.removeNewSocialMediaEventName, $scope.socialMedia);
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
              offset: 200,
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
          return socialMedia.socialableId;
      }

      $scope.view.isLoadingRequiredData = true;
      $scope.$parent.data.loadSocialMediaTypesPromise.promise.then(function (socialMediaTypes) {
          $scope.view.socialMediaTypes = socialMediaTypes;
          $scope.view.isLoadingRequiredData = false;
      });
  });
