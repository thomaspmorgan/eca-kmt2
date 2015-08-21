'use strict';

/**
 * @ngdoc service
 * @name staticApp.AddressService
 * @description
 * # authService
 * Factory for handling addresses.
 */
angular.module('staticApp')
  .factory('AddressService', function ($rootScope, $log, $http, DragonBreath) {

      var service = {
          organizationAddressableType: 'organization',
          personAddressableType: 'person',

          update: function (address, addressableType, addressableId) {
              if (!service.isValidAddressableType(addressableType)) {
                  throw Error('The addressable type [' + addressableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(addressableType);
              return DragonBreath.save(address, resourceApiPrefix + '/' + addressableId + '/address');
          },
          delete: function (address, addressableType, addressableId) {
              if (!service.isValidAddressableType(addressableType)) {
                  throw Error('The addressable type [' + addressableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(addressableType);
              return DragonBreath.delete(address, resourceApiPrefix + '/' + addressableId + '/address/' + address.addressId);
          },
          add: function(address, addressableType, addressableId){
              if (!service.isValidAddressableType(addressableType)) {
                  throw Error('The addressable type [' + addressableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(addressableType);
              return DragonBreath.create(address, resourceApiPrefix + '/' + addressableId + '/address');
          },
          isValidAddressableType: function(addressableType){
              var types = [service.organizationAddressableType, service.personAddressableType];
              return types.indexOf(addressableType) >= 0;
          },
          getResourceApiPrefix: function(addressableType){
              if (!service.isValidAddressableType(addressableType)) {
                  throw Error('The addressable type [' + addressableType + '] is not supported.');
              }
              if (addressableType === service.organizationAddressableType) {
                  return 'organizations';
              }
              if (addressableType === service.personAddressableType) {
                  return 'people';
              }
          }
      };
      return service;
  });