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
              var promise = DragonBreath.get({
                  type: resourceType,
                  id: resourceId
              }, 'auth/user/permissions');
              promise.then(function (responseData) {
                  if (config) {
                      for (var key in config) {
                          var userPermissions = responseData.data;
                          var permissionFound = false;
                          if (userPermissions.length === 0) {
                              permissionFound = true;
                              $log.warn('Granting user permissions for resource [' + resourceId + '] of type [' + resourceType + '] because zero permissions were returned from the server.');
                          }
                          else {
                              for (var i = 0; i < userPermissions.length; i++) {
                                  var userPermission = userPermissions[i];
                                  console.assert(userPermission.permissionName, "The user permission object should have a permissionName property.");
                                  if (userPermission.permissionName === key) {
                                      permissionFound = true;
                                      var hasPermissionCallback = config[key][hasPermissionCallbackName];
                                      console.assert(hasPermissionCallback, "The config object for the permission named [" + userPermission.permissionName + '] must have a callback function named [' + hasPermissionCallbackName + '].');
                                      hasPermissionCallback();
                                  }
                              }
                              if (!permissionFound) {
                                  var notAuthorizedCallback = config[key][notAuthorizedCallbackName];
                                  console.assert(notAuthorizedCallback, "The config object for the permission named [" + key + '] must have a callback function named [' + notAuthorizedCallbackName + '].');
                                  notAuthorizedCallback();
                              }
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
              adalAuthenticationService.logOut();
          }
      };
      return service;
  });


//var requiredEditPermissionId = ConstantsService.permission.editproject;
//function loadPermissions() {
//    var projectId = $stateParams.projectId;
//    return AuthService.getResourcePermissions('Project', projectId)
//      .then(function (result) {
//          var permissions = result.data;
//          for (var i = 0; i < permissions.length; i++) {
//              var permission = permissions[i];
//              console.assert(permission.permissionId, 'The permission should have a permission id property');
//              var permissionId = permission.permissionId;
//              if (permissionId === requiredEditPermissionId) {
//                  $scope.permissions.canEdit = true;
//              }
//          }
//      }, function() {
//          console.log('Unable to load user permissions.');
//      });
//}