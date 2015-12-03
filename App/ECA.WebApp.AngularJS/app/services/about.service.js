'use strict';

/**
 * @ngdoc service
 * @name staticApp.AboutService
 * @description
 * # AboutService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('AboutService', function (DragonBreath) {
      return {
          get: function (params) {
              return DragonBreath.get(params, 'about');
          }
      };
  });