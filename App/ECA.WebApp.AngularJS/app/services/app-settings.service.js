'use strict';

/**
 * @ngdoc service
 * @name staticApp.AppSettingsService
 * @description
 * # AppSettingsService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('AppSettingsService', function (DragonBreath) {
      return {
          get: function (params) {
              return DragonBreath.get(params, 'appSettings');
          }
      };
  });