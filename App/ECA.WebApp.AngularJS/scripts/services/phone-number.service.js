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

          personPhoneNumberableType: 'person',
          contactPhoneNumberableType: 'contact',

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

          add: function (emailAddress, phoneNumberableType, phoneNumberableId) {
              if (!service.isValidPhoneNumberableType(phoneNumberableType)) {
                  throw Error('The phoneNumberable type [' + phoneNumberableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(phoneNumberableType);
              return DragonBreath.create(emailAddress, resourceApiPrefix + '/' + phoneNumberableId + '/phonenumber');
          },
          getResourceApiPrefix: function (phoneNumberableType) {
              if (!service.isValidPhoneNumberableType(phoneNumberableType)) {
                  throw Error('The phoneNumberable type [' + phoneNumberableType + '] is not supported.');
              }
              if (phoneNumberableType === service.personPhoneNumberableType) {
                  return 'people';
              } else if (phoneNumberableType === service.contactPhoneNumberableType) {
                  return 'contact';
              }
          },
          isValidPhoneNumberableType: function (phoneNumberableType) {
              var types = [service.personPhoneNumberableType];
              return types.indexOf(phoneNumberableType) >= 0;
          }
      };
      return service;
  });