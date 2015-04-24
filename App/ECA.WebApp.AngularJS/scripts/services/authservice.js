'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('AuthService', function ($rootScope, $http, $q, $window, DragonBreath, adalAuthenticationService) {
      
      var service = {
          getResourcePermissions: function (resourceType, id) {
              return DragonBreath.get({
                  type: resourceType,
                  id: id
              }, 'auth/user/permissions');
          },

          getUserInfo: function () {
              return DragonBreath.get('auth/user');
          },

          register: function () {
              return DragonBreath.create({}, 'auth/user/register');
          },

          login: function () {
              adalAuthenticationService.login();
          },

          logOut: function () {
              adalAuthenticationService.logOut();
          }
      };
      return service;
  });
