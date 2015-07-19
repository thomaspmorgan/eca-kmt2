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
        LookupService,
        AddressService,
        ConstantsService,
        LocationService,
        OrganizationService,
        PersonService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseAddresses = false;

      $scope.view.onAddAddressClick = function (addressableType, entityAddresses, entityId) {
          console.assert(entityAddresses, 'The entity addresses is not defined.');
          console.assert(entityAddresses instanceof Array, 'The entity address is defined but must be an array.');
          var newAddress = {
              Id: entityId,
              addressableType: addressableType
          };
          entityAddresses.splice(0, 0, newAddress);
          $scope.view.collapseAddresses = false;
      }

  });
