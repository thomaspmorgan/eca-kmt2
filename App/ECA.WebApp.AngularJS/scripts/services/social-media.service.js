'use strict';

/**
 * @ngdoc service
 * @name staticApp.SocialMediaService
 * @description
 * # SocialMediaService
 * Factory for handling social media.
 */
angular.module('staticApp')
  .factory('SocialMediaService', function ($rootScope, $log, $http, DragonBreath) {

      var service = {
          organizationSociableType: 'organization',
          personSociableType: 'person',

          update: function (socialMedia, sociableMediaType, sociableId) {
              if (!service.isValidSociableType(sociableMediaType)) {
                  throw Error('The socialable media type [' + sociableMediaType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(sociableMediaType);
              return DragonBreath.save(socialMedia, resourceApiPrefix + '/' + sociableId + '/socialmedia');
          },
          delete: function (socialMedia, sociableMediaType, socialableId) {
              if (!service.isValidSociableType(sociableMediaType)) {
                  throw Error('The socialable media type [' + sociableMediaType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(sociableMediaType);
              return DragonBreath.delete(socialMedia, resourceApiPrefix + '/' + socialableId + '/socialmedia/' + socialMedia.id);
          },

          add: function(socialMedia, sociableMediaType, socialableId){
              if (!service.isValidSociableType(sociableMediaType)) {
                  throw Error('The socialable media type [' + sociableMediaType + '] is not supported.');
              }
              var resourceApiPrefix = service.getResourceApiPrefix(sociableMediaType);
              return DragonBreath.create(socialMedia, resourceApiPrefix + '/' + socialableId + '/socialmedia');
          },
          getResourceApiPrefix: function(socialableMediaType){
              if (!service.isValidSociableType(socialableMediaType)) {
                  throw Error('The sociable media type [' + socialableMediaType + '] is not supported.');
              }
              if (socialableMediaType === service.organizationSociableType) {
                  return 'organizations';
              }
              if (socialableMediaType === service.personSociableType) {
                  return 'people';
              }
          },
          isValidSociableType: function (sociableType) {
              var types = [service.organizationSociableType, service.personSociableType];
              return types.indexOf(sociableType) >= 0;
          }
      };
      return service;
  });