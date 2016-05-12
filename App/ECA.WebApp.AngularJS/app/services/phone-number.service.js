'use strict';

/**
 * @ngdoc service
 * @name staticApp.PhoneNumberService
 * @description
 * # PhoneNumberService
 * Factory for handling phone numbers.
 */
angular.module('staticApp')
  .factory('PhoneNumberService', function ($rootScope, $log, $http, DragonBreath) {

      var service = {

          phoneNumberableTypes: ['person','contact'],

          update: function (phoneNumber, phoneNumberableType, phoneNumberableId) {
              if (!service.isValidPhoneNumberableType(phoneNumberableType)) {
                  throw Error('The phoneNumberable type [' + phoneNumberableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(phoneNumberableType);
              return DragonBreath.save(phoneNumber, resourceApiPrefix + '/' + phoneNumberableId + '/phonenumber');
          },
          delete: function (phoneNumber, phoneNumberableType, phoneNumberableId) {
              if (!service.isValidPhoneNumberableType(phoneNumberableType)) {
                  throw Error('The phoneNumberable type [' + phoneNumberableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(phoneNumberableType);
              return DragonBreath.delete(phoneNumber, resourceApiPrefix + '/' + phoneNumberableId + '/phonenumber/' + phoneNumber.id);
          },

          add: function (phoneNumber, phoneNumberableType, phoneNumberableId) {
              if (!service.isValidPhoneNumberableType(phoneNumberableType)) {
                  throw Error('The phoneNumberable type [' + phoneNumberableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(phoneNumberableType);
              return DragonBreath.create(phoneNumber, resourceApiPrefix + '/' + phoneNumberableId + '/phonenumber');
          },
          getResourceApiPrefix: function (phoneNumberableType) {
              if (!service.isValidPhoneNumberableType(phoneNumberableType)) {
                  throw Error('The phoneNumberable type [' + phoneNumberableType + '] is not supported.');
              }
              if (phoneNumberableType === service.phoneNumberableTypes[0]) {
                  return 'people';
              } else if (phoneNumberableType === service.phoneNumberableTypes[1]) {
                  return 'contact';
              }
          },
          isValidPhoneNumberableType: function (phoneNumberableType) {
              var types = service.phoneNumberableTypes;
              return types.indexOf(phoneNumberableType) >= 0;
          }
      };
      return service;
  });