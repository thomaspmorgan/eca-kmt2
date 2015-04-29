'use strict';

/**
 * @ngdoc service
 * @name staticApp.LoginEventService
 * @description
 * # LoginEventService
 * Factory for handling login events.
 */
angular.module('staticApp')
  .factory('LoginEventService', function ($rootScope, AuthService, ConstantsService) {      
      var service = {};
      $rootScope.$on(ConstantsService.loginEventName, function (event, data) {
          AuthService.login();
      });
      return service;
  });
