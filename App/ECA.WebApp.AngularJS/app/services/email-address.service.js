'use strict';

/**
 * @ngdoc service
 * @name staticApp.EmailAddressService
 * @description
 * # SocialMediaService
 * Factory for handling email addresses.
 */
angular.module('staticApp')
  .factory('EmailAddressService', function ($rootScope, $log, $http, DragonBreath) {

      var service = {

          emailAddressableTypes: ['person', 'contact'],

          update: function (emailAddress, emailAddressableType, emailAddressableId) {
              if (!service.isValidEmailAddressableType(emailAddressableType)) {
                  throw Error('The emailAddressable type [' + emailAddressableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(emailAddressableType);
              return DragonBreath.save(emailAddress, resourceApiPrefix + '/' + emailAddressableId + '/emailaddress');
          },
          delete: function (emailAddress, emailAddressableType, emailAddressableId) {
              if (!service.isValidEmailAddressableType(emailAddressableType)) {
                  throw Error('The emailAddressable type [' + emailAddressableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(emailAddressableType);
              return DragonBreath.delete(emailAddress, resourceApiPrefix + '/' + emailAddressableId + '/emailaddress/' + emailAddress.id);
          },

          add: function (emailAddress, emailAddressableType, emailAddressableId) {
              if (!service.isValidEmailAddressableType(emailAddressableType)) {
                  throw Error('The emailAddressable type [' + emailAddressableType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(emailAddressableType);
              return DragonBreath.create(emailAddress, resourceApiPrefix + '/' + emailAddressableId + '/emailaddress');
          },
          getResourceApiPrefix: function (emailAddressableType) {
              if (!service.isValidEmailAddressableType(emailAddressableType)) {
                  throw Error('The email addressable type [' + emailAddressableType + '] is not supported.');
              }
              if (emailAddressableType === service.emailAddressableTypes[0]) {
                  return 'people';
              } else if (emailAddressableType === service.emailAddressableTypes[1]) {
                  return 'contact';
              }
          },
          isValidEmailAddressableType: function (emailAddressableType) {
              var types = service.emailAddressableTypes;
              return types.indexOf(emailAddressableType) >= 0;
          }
      };
      return service;
  });