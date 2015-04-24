'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('LogoutEventService', function ($rootScope, AuthService, ConstantsService) {      
      var service = {};
      $rootScope.$on(ConstantsService.logoutEventName, function (event, data) {
          AuthService.logOut();
      });
      return service;
  });
