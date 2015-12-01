'use strict';

/**
 * @ngdoc service
 * @name staticApp.LogoutEventService
 * @description
 * # LogoutEventService
 * Factory for handling logout events.
 */
angular.module('staticApp')
  .factory('LogoutEventService', function ($rootScope, $q, AuthService, ConstantsService) {      
      var service = {};
      $rootScope.$on(ConstantsService.logoutEventName, function (event, data) {
          return AuthService.logOut();
      });
      return service;
  });
