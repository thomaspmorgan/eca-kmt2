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
          update: function (address) {
              return DragonBreath.save(address, 'addresses');
          },
          delete: function (address) {
              return DragonBreath.delete(address, 'addresses/' + address.addressId);
          }
      };
      return service;
  });