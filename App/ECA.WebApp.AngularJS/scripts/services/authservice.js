'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('AuthService', function ($rootScope, $log, $http, $q, $window, DragonBreath, adalAuthenticationService) {
      
      var service = {
          getResourcePermissions: function (resourceType, resourceId, config) {
              var hasPermissionCallbackName = "hasPermission";
              var notAuthorizedCallbackName = "notAuthorized";
              $log.info('Requesting permissions for resource with id [' + resourceId + '] of type [' + resourceType + '].');
              var promise = DragonBreath.get({
                  type: resourceType,
                  id: resourceId
              }, 'auth/user/permissions');
              promise.then(function (responseData) {
                  if (config) {
                      for (var key in config) {
                          var userPermissions = responseData.data;
                          var permissionFound = false;
                          var hasPermissionCallback = config[key][hasPermissionCallbackName];
                          console.assert(hasPermissionCallback, "The config object for the permission named [" + key + '] must have a callback function named [' + hasPermissionCallbackName + '].');
                          if (userPermissions.length === 0) {
                              $log.warn('Granting user permission [' + key + '] for resource [' + resourceId + '] of type [' + resourceType + '] because zero permissions were returned from the server.');
                              hasPermissionCallback();                              
                          }
                          else {
                              for (var i = 0; i < userPermissions.length; i++) {
                                  var userPermission = userPermissions[i];
                                  console.assert(userPermission.permissionName, "The user permission object should have a permissionName property.");
                                  if (userPermission.permissionName === key) {
                                      permissionFound = true;
                                      hasPermissionCallback();
                                  }
                              }
                              if (!permissionFound) {
                                  var notAuthorizedCallback = config[key][notAuthorizedCallbackName];
                                  console.assert(notAuthorizedCallback, "The config object for the permission named [" + key + '] must have a callback function named [' + notAuthorizedCallbackName + '].');                                  
                                  notAuthorizedCallback();
                              }
                              $log.info('User has [' + key + '] permission = ' + permissionFound);
                          }
                      }
                  }
              });
              return promise;
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
              return DragonBreath.create({}, 'auth/user/logout')
                  .then(function () {
                      adalAuthenticationService.logOut();
                  });
          },

          isAuthenticated: function () {
              return $rootScope.userInfo.isAuthenticated;
          }
      };
      return service;
  });