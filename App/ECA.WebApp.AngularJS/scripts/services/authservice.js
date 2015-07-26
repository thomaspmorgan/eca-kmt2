'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('AuthService', function ($rootScope, $log, $http, $q, $window, DragonBreath, ConstantsService, adalAuthenticationService, orderByFilter) {

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

          isCurrentUser: function (emailAddress) {
              if (!emailAddress) {
                  return false;
              }
              return emailAddress.trim().toLowerCase() === adalAuthenticationService.userInfo.userName.trim().toLowerCase();
          },

          isEcaUser: function (emailAddress) {
              if (!emailAddress) {
                  return false;
              }
              var email = emailAddress.trim().toLowerCase();
              return email.indexOf('state.gov') >= 0 || email.indexOf('statedept.us') >= 0;
          },

          /**
           * Returns an array of principals with granted permissions, revoked permissions, and role permissions.
           * Each object will also contain an array of available permissions that could be granted to the given resource.
           * Use this method for retrieving current users and user permissions.
           * 
           * ResourceType is the resourcetype you want resource authorization for e.g. Project.
           * ForeignResourceId is the primary key of the resource e.g. ProjectId.
           * The url is the url the resource authorizations will be retrieved from e.g. api/Projects/{projectId}/Collaborators
           * params is the object containing, filter, sort and order by information.
           * 
           */
          getPrincipalResourceAuthorizations: function (resourceType, foreignResourceId, url, params) {
              if (!resourceType) {
                  throw Error('The resource type must be given.');
              }
              if (!ConstantsService.resourceType[resourceType.toLowerCase().trim()]) {
                  throw Error('The resource type is not recognized.');
              }
              if (!foreignResourceId) {
                  throw Error('The foreign resource id must be given.');
              }
              if (!url) {
                  throw Error('The resource authorizations url must be given.');
              }

              var getResourceAuthorizationsRequest = DragonBreath.get(params, url);
              $log.info('Retrieving resource authorizations for resource of type [' + resourceType + '] with id of [' + foreignResourceId + '] from [' + url + '].');
              return $q.all([
                  service.getGrantableResourcePermissions(resourceType, foreignResourceId),
                  getResourceAuthorizationsRequest
              ])
              .then(function (resultsArray) {
                  var availablePermissions = resultsArray[0].data;
                  $log.info('Found [' + availablePermissions.length + '] available permissions for the resource type ' + resourceType);

                  var resourceAuthorizationsList = resultsArray[1].data.results;
                  $log.info('Retrieved [' + resourceAuthorizationsList.length + '] of [' + resultsArray[1].data.total + '] resource authorizations from the server.');

                  var groupedResourceAuthorizations = service.groupResourceAuthorizationsByPrincipalId(resourceAuthorizationsList);
                  groupedResourceAuthorizations = orderByFilter(groupedResourceAuthorizations, "+displayName");
                  for (var i = 0; i < groupedResourceAuthorizations.length; i++) {
                      var groupedResourceAuthorization = groupedResourceAuthorizations[i];
                      groupedResourceAuthorization.isCurrentUser = service.isCurrentUser(groupedResourceAuthorization.emailAddress);
                      groupedResourceAuthorization.isEcaUser = service.isEcaUser(groupedResourceAuthorization.emailAddress);
                      groupedResourceAuthorization.availablePermissions =
                          service.createAvailablePermissions(availablePermissions, groupedResourceAuthorization, foreignResourceId, resourceType);
                      groupedResourceAuthorization.mergedPermissions =
                          service.mergePermissions(groupedResourceAuthorization.assignedPermissions, groupedResourceAuthorization.availablePermissions);
                  }
                  $log.info('Returning [' + groupedResourceAuthorizations.length + '] principal resource authorizations.');
                  return groupedResourceAuthorizations;
              });
          },

          /**
           * Creates a principal's available permissions.
           * availablePermissions is the array of all permissions that can be assigned to the resource.
           * principal is the object that contains granted, revoked, and role permissions.
           * foreignResourceId the primary key of the resource.
           * resourceType the resource type
           */
          createAvailablePermissions: function (availablePermissions, principal, foreignResourceId, resourceType) {
              var permissions = [];
              for (var i = 0; i < availablePermissions.length; i++) {
                  var availablePermission = availablePermissions[i];
                  var isRolePermission = false;
                  if (principal.rolePermissions && principal.rolePermissions.length > 0) {
                      for (var j = 0; j < principal.rolePermissions.length; j++) {
                          var rolePermission = principal.rolePermissions[j];
                          if (availablePermission.permissionId === rolePermission.permissionId) {
                              isRolePermission = true;
                              break;
                          }
                      }
                  }
                  var isPermissionAlreadyAdded = false;
                  for (var k = 0; k < permissions.length; k++) {
                      var alreadyAddedPermission = permissions[k];
                      if (availablePermission.permissionId === alreadyAddedPermission.permissionId) {
                          isPermissionAlreadyAdded = true;
                      }
                  }
                  if (!isRolePermission && !isPermissionAlreadyAdded) {
                      permissions.push(service.createAvailablePermission(availablePermission, principal, foreignResourceId, resourceType));
                  }
              }
              return permissions;
          },

          mergePermissions: function (assignedPermissions, availablePermissions) {
              var temp = {};
              for (var i = 0; i < assignedPermissions.length; i++) {
                  temp[assignedPermissions[i].permissionId] = assignedPermissions[i];
              }
              for (var i = 0; i < availablePermissions.length; i++) {
                  if (!temp[availablePermissions[i].permissionId]) {
                      temp[availablePermissions[i].permissionId] = availablePermissions[i];
                  }
              }
              var mergedPermissions = Object.keys(temp).map(function (k) { return temp[k] });
              return orderByFilter(mergedPermissions, '+permissionName');
          },

          /**
           * Returns an object with enough details to be able to grant a permission later.
           */
          createAvailablePermission: function (availablePermission, principal, foreignResourceId, resourceType) {
              return {
                  principalId: principal.principalId,
                  permissionId: availablePermission.permissionId,
                  permissionName: availablePermission.permissionName,
                  permissionDescription: availablePermission.permissionDescription,
                  foreignResourceId: foreignResourceId,
                  resourceType: resourceType,
                  projectId: foreignResourceId
              };
          },

          /**
           * Returns all available permissions for the resource by type and id.
           * 
           */
          getGrantableResourcePermissions: function (resourceType, foreignResourceId) {
              if (!resourceType) {
                  throw Error('The resource type must be defined.');
              }
              if (foreignResourceId) {
                  return DragonBreath.get('resources/permissions/' + resourceType + '/' + foreignResourceId);
              }
              else {
                  return DragonBreath.get(params, 'resources/permissions/' + resourceType);
              }
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

          removePermission: function (principalId, foreignResourceId, resourceType, permissionId) {
              var permissionModel = {
                  granteePrincipalId: principalId,
                  resourceType: resourceType,
                  foreignResourceId: foreignResourceId,
                  permissionId: permissionId
              };
              return DragonBreath.create(permissionModel, 'principals/remove/permission');
          },

          /**
           * Groups the given resource authorizations by principal and organizes role and permissions.
           * flatPrincipalPermissionList The array of resource authorizations returned by the web api.
           */
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
                          assignedPermissions: [],
                          inheritedPermissions: []
                      });
                      currentGroupedPermissionByPrincipalId = groupedPermissionsByPrincipalIds[groupedPermissionsByPrincipalIds.length - 1];
                  }
                  var groupedPermission = service.getGroupedPermission(principalIdOrderedCollaborator);
                  if (groupedPermission.isGrantedByRole) {
                      currentGroupedPermissionByPrincipalId.rolePermissions.push(groupedPermission);
                  }
                  else if (groupedPermission.isGrantedByInheritance) {
                      currentGroupedPermissionByPrincipalId.inheritedPermissions.push(groupedPermission);
                  }
                  else {
                      currentGroupedPermissionByPrincipalId.assignedPermissions.push(groupedPermission);
                  }
              }
              for (var i = 0; i < groupedPermissionsByPrincipalIds.length; i++) {
                  var groupedPrincipal = groupedPermissionsByPrincipalIds[i];
                  groupedPrincipal.rolePermissions = orderByFilter(groupedPrincipal.rolePermissions, '[+roleName, +permissionName]');
                  groupedPrincipal.inheritedPermissions = orderByFilter(groupedPrincipal.inheritedPermissions, '+permissionName');
                  groupedPrincipal.assignedPermissions = orderByFilter(groupedPrincipal.assignedPermissions, '+permissionName');
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