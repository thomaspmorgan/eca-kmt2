'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('RegisterUserEventService', function ($q, $log, $rootScope, AuthService, ConstantsService, adalAuthenticationService) {
      var service = {
          doRegistration: function () {
              $log.info("User successfully authenticated with Azure.");
              var user = $rootScope.userInfo;
              if (user.isAuthenticated) {
                  $log.info('Check if user registered...');
                  $q.when(AuthService.getUserInfo())
                  .then(function (userInfoResponse) {
                      var userInfo = userInfoResponse.data;
                      $log.info('User [' + userInfo.userName + '] authenticated.');
                      if (!userInfo.isRegistered) {
                          $rootScope.$broadcast(ConstantsService.registeringUserEventName);
                          AuthService.register()
                              .then(function (registerResponse) {
                                  $rootScope.$broadcast(ConstantsService.registerUserSuccessEventName);
                                  $log.info('Successfully registered user.');
                              }, function (registerErrorResponse) {
                                  $rootScope.$broadcast(ConstantsService.registerUserFailureEventName);
                                  $log.error('Unable to register user.');
                              });
                      }
                      else {
                          $log.info('User is registered.');
                          
                      }
                  }, function (errorResponse) {
                      $log.error('Unable to check user registration.');
                  });
              }
              else {
                  $log.warn("User is not authenticated.");
              }
          }
      };
      $rootScope.$on(ConstantsService.adalLoginSuccessEventName, function () {
          service.doRegistration();
      });

      return service;
  });
