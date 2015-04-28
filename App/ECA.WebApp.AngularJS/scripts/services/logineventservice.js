'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('LoginEventService', function ($rootScope, AuthService, ConstantsService) {      
      var service = {};
      $rootScope.$on(ConstantsService.loginEventName, function (event, data) {
          AuthService.login();
      });
      return service;
  });
