'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressesCtrl
 * @description The addresses controller is used to control the list of addresses.
 * # AddressesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SocialMediasCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        ConstantsService,
        NotificationService,
        LookupService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseSocialMedias = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadSocialMediaTypesPromise = $q.defer();
      $scope.data.socialMediaTypes = [];

      $scope.view.onAddSocialMediaClick = function (socialableType, entitySocialMedias, socialableId) {
          console.assert(entitySocialMedias, 'The entity social medias is not defined.');
          console.assert(entitySocialMedias instanceof Array, 'The entity social medias is defined but must be an array.');
          var url = '';
          angular.forEach($scope.data.socialMediaTypes, function (type, index) {
              if (type.id === ConstantsService.socialMediaType.facebook.id) {
                  url = type.url;
              }
          });
          var newSocialMedia = {
              id: --tempId,
              socialableId: socialableId,
              socialableType: socialableType,
              socialMediaType: ConstantsService.socialMediaType.facebook.value,
              socialMediaTypeId: ConstantsService.socialMediaType.facebook.id,
              value: url
          };
          entitySocialMedias.splice(0, 0, newSocialMedia);
          $scope.view.collapseSocialMedias = false;
      };

      $scope.$on(ConstantsService.removeNewSocialMediaEventName, function (event, newSocialMedia) {
          console.assert($scope.socialable, 'The scope socialable property must exist.  It should be set by the directive.');
          console.assert($scope.socialable.socialMedias instanceof Array, 'The entity social medias is defined but must be an array.');

          var socialMedias = $scope.socialable.socialMedias;
          var index = socialMedias.indexOf(newSocialMedia);
          var removedItems = socialMedias.splice(index, 1);
          $log.info('Removed one new social media at index ' + index);
      });

      function getSocialMediaTypes() {
          var params = {
              start: 0,
              limit: 300
          };
          return LookupService.getSocialMediaTypes(params)
          .then(function (response) {
              if (response.data.total > params.limit) {
                  var message = "There are more social media types than could be loaded.  Not all social media types will be shown.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              }
              $log.info('Loaded all social media types.');
              var socialMediaTypes = response.data.results;
              $scope.data.loadSocialMediaTypesPromise.resolve(socialMediaTypes);
              $scope.data.socialMediaTypes = socialMediaTypes;
              return socialMediaTypes;
          })
          .catch(function () {
              var message = 'Unable to load social media types.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
      getSocialMediaTypes();
  });
