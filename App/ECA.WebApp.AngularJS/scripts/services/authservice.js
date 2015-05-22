'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('AuthService', function ($rootScope, $log, $http, $q, $window, DragonBreath, adalAuthenticationService, orderByFilter) {

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
          },

          updatePermission: function (isAllowed, principalId, foreignResourceId, resourceType, permissionId) {
              var path = '';
              var permissionModel = {
                  granteePrincipalId: principalId,
                  resourceType: resourceType,
                  foreignResourceId: foreignResourceId,
                  permissionId: permissionId
              };
              if (isAllowed) {
                  path = 'principals/grant/permission';
              }
              else {
                  path = 'principals/revoke/permission';
              }
              return DragonBreath.create(permissionModel, path);
          },

          removePermission: function(principalId, foreignResourceId, resourceType, permissionId){
              var permissionModel = {
                  granteePrincipalId: principalId,
                  resourceType: resourceType,
                  foreignResourceId: foreignResourceId,
                  permissionId: permissionId
              };
              return DragonBreath.create(permissionModel, 'principals/remove/permission');
          },

          groupResourceAuthorizationsByPrincipalId: function (flatPrincipalPermissionList) {
              var groupedPermissionsByPrincipalIds = [];
              var principalIdOrderedCollaborators = orderByFilter(flatPrincipalPermissionList, '+principalId');
              var currentPrincipalId = null;

              var currentGroupedPermissionByPrincipalId = null;
              for (var i = 0; i < principalIdOrderedCollaborators.length; i++) {
                  var principalIdOrderedCollaborator = principalIdOrderedCollaborators[i];
                  if (currentPrincipalId === null) {
                      currentPrincipalId = principalIdOrderedCollaborator.principalId;
                  }

                  if (i === 0 || currentPrincipalId !== principalIdOrderedCollaborator.principalId) {
                      currentPrincipalId = principalIdOrderedCollaborator.principalId;
                      groupedPermissionsByPrincipalIds.push({
                          principalId: principalIdOrderedCollaborator.principalId,
                          displayName: principalIdOrderedCollaborator.displayName,
                          emailAddress: principalIdOrderedCollaborator.emailAddress,
                          rolePermissions: [],
                          revokedPermissions: [],
                          grantedPermissions: []
                      });
                      currentGroupedPermissionByPrincipalId = groupedPermissionsByPrincipalIds[groupedPermissionsByPrincipalIds.length - 1];
                  }
                  var groupedPermission = service.getGroupedPermission(principalIdOrderedCollaborator);
                  if (groupedPermission.isGrantedByRole) {
                      currentGroupedPermissionByPrincipalId.rolePermissions.push(groupedPermission);
                  }
                  else {
                      if (groupedPermission.isAllowed) {
                          currentGroupedPermissionByPrincipalId.grantedPermissions.push(groupedPermission);
                      }
                      else {
                          currentGroupedPermissionByPrincipalId.revokedPermissions.push(groupedPermission);
                      }
                  }
              }
              return groupedPermissionsByPrincipalIds;
          },

          getGroupedPermission: function (principalPermission) {
              var permission = {};
              for (var key in principalPermission) {
                  if (key !== 'principalId'
                      && key !== 'displayName'
                      && key !== 'emailAddress') {
                      permission[key] = principalPermission[key];
                  }
              }
              return permission;
          }
      };
      return service;
  });