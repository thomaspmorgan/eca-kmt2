'use strict';

/**
 * @ngdoc service
 * @name staticApp.userService
 * @description
 * # authService
 * Factory for handling users.
 */
angular.module('staticApp')
  .factory('UserService', function ($rootScope, $log, $http, $q, $window, DragonBreath, adalAuthenticationService, orderByFilter) {

      var service = {
          get: function (params) {
              return DragonBreath.get(params, 'Users');
          }
      };
      return service;
  });