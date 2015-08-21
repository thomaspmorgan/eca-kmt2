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
        NotificationService,
        ConstantsService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseAddresses = true;
      $scope.data = {};
      $scope.data.loadAddressTypesPromise = $q.defer();
      var tempAddressId = 0;

      $scope.view.onAddAddressClick = function (addressableType, entityAddresses, entityId) {
          console.assert(entityAddresses, 'The entity addresses is not defined.');
          console.assert(entityAddresses instanceof Array, 'The entity address is defined but must be an array.');
          var newAddress = {
              id: entityId,
              addressId: --tempAddressId,
              addressableType: addressableType,
              addressDisplayName: 'NEW ADDRESS'
          };
          entityAddresses.splice(0, 0, newAddress);
          $scope.view.collapseAddresses = false;
      };

      $scope.$on(ConstantsService.removeNewAddressEventName, function (event, newAddress) {
          console.assert($scope.addressable, 'The scope addressable property must exist.  It should be set by the directive.');
          console.assert($scope.addressable.addresses instanceof Array, 'The entity address is defined but must be an array.');

          var addresses = $scope.addressable.addresses;
          var index = addresses.indexOf(newAddress);          
          var removedItems = addresses.splice(index, 1);
          $log.info('Removed one new address at index ' + index);
      });

      $scope.$on(ConstantsService.primaryAddressChangedEventName, function (event, primaryAddress) {
          console.assert($scope.addressable, 'The scope addressable property must exist.  It should be set by the directive.');
          console.assert($scope.addressable.addresses instanceof Array, 'The entity address is defined but must be an array.');

          var addresses = $scope.addressable.addresses;
          var primaryAddressIndex = addresses.indexOf(primaryAddress);
          angular.forEach(addresses, function (address, index) {
              if (primaryAddressIndex !== index) {
                  address.isPrimary = false;
              }
          });
      });
      
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
              $log.info('Loaded all address types.');
              $scope.data.loadAddressTypesPromise.resolve(addressTypes);
              return addressTypes;
          })
          .catch(function () {
              var message = 'Unable to load address types.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
      getAddressTypes();
  });
