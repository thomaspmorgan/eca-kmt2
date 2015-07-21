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
        ConstantsService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseSocialMedias = true;
      var tempAddressId = 0;

      $scope.view.onAddSocialMediaClick = function (socialableType, entitySocialMedias, entityId) {
          console.assert(entitySocialMedias, 'The entity social medias is not defined.');
          console.assert(entitySocialMedias instanceof Array, 'The entity social medias is defined but must be an array.');
          var newSocialMedia = {
              id: entityId,
              //socialMediaId: --tempAddressId,
              //addressableType: addressableType,
              //addressDisplayName: 'NEW ADDRESS'
          };
          entitySocialMedias.splice(0, 0, newAddress);
          $scope.view.collapseSocialMedias = false;
      }

      $scope.$on(ConstantsService.removeNewAddressEventName, function (event, newAddress) {
          console.assert($scope.addressable, 'The scope addressable property must exist.  It should be set by the directive.');
          console.assert($scope.addressable.addresses instanceof Array, 'The entity address is defined but must be an array.');

          var addresses = $scope.addressable.addresses;
          var index = addresses.indexOf(newAddress);          
          var removedItems = addresses.splice(index, 1);
          $log.info('Removed one new address at index ' + index);
      });

  });
