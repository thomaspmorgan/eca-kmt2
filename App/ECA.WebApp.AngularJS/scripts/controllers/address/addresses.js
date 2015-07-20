'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressesCtrl
 * @description The addresses controller is used to control the list of addresses.
 * # AddressesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddressesCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        ConstantsService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseAddresses = true;

      $scope.view.onAddAddressClick = function (addressableType, entityAddresses, entityId) {
          console.assert(entityAddresses, 'The entity addresses is not defined.');
          console.assert(entityAddresses instanceof Array, 'The entity address is defined but must be an array.');
          var newAddress = {
              Id: entityId,
              addressableType: addressableType,
              addressDisplayName: 'New Address'
          };
          entityAddresses.splice(0, 0, newAddress);
          $scope.view.collapseAddresses = false;
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
