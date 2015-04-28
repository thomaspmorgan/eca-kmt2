'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('LogoutEventService', function ($rootScope, $q, AuthService, ConstantsService) {      
      var service = {};
      $rootScope.$on(ConstantsService.logoutEventName, function (event, data) {
          $q.when(AuthService.logOut()).then(function () {
          });
      });
      return service;
  });
