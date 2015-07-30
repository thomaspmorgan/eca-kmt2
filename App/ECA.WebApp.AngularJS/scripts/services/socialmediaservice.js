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
          update: function (socialMedia) {
              return DragonBreath.save(socialMedia, 'socialmedias');
          },
          delete: function (socialMedia) {
              return DragonBreath.delete(socialMedia, 'socialmedias/' + socialMedia.id);
          }
      };
      return service;
  });