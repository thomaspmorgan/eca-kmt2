'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('AuthService', function ($http, $q, $window, DragonBreath) {
      return {
          getResourcePermissions: function (resourceType, id) {
              return DragonBreath.get({
                  type: resourceType,
                  id: id
              }, 'auth/user/permissions');
          }
      };

  });
