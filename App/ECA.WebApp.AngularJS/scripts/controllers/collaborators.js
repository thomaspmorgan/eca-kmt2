'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AboutCtrl
 * @description
 * # CollaboratorsCont
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('CollaboratorCtrl', function ($scope, $q, $modalInstance, parameters, AuthService, UserService, OfficeService, ProgramService, ProjectService, ConstantsService, NotificationService, orderByFilter) {
    
      $scope.showPermissions = {};
      
      $scope.collaborators = [];

      $scope.collaboratorsLoading = false;

      $scope.toggleShowPermissions = function (principalId) {
          if ($scope.showPermissions[principalId]) {
              $scope.showPermissions[principalId] = false;
          } else {
              $scope.showPermissions[principalId] = true;
          }
      }

      $scope.getUsers = function ($viewValue) {
          var params = {
              start: 0,
              limit: 25,
              filter: [{
                  property: 'displayName',
                  comparison: ConstantsService.likeComparisonType,
                  value: $viewValue
              }]
          };
          return UserService.get(params)
              .then(function (response) {
                  return response.data.results;
              }, function (error) {
                  NotificationService.showErrorMessage('There was an error loading available users.');
              });
      }

      $scope.close = function () {
          $modalInstance.close();
      }

      $scope.allowPermissionHandler = function (collaborator, permission) {
          var permissionModel = {
              principalId: collaborator.principalId,
              permissionId: permission.permissionId
          }
          addForeignResourceIdToPermissionModel(permissionModel);
          if (permission.isAllowed === false) {
              permission.isAllowed = undefined;
              removePermission(permissionModel);
          } else {
              addPermission(permissionModel);
          }
      }

      $scope.denyPermissionHandler = function (collaborator, permission) {
          var permissionModel = {
              principalId: collaborator.principalId,
              permissionId: permission.permissionId
          }
          addForeignResourceIdToPermissionModel(permissionModel);
          if (permission.isAllowed === true) {
              permission.isAllowed = undefined;
              removePermission(permissionModel);
          } else {
              revokePermission(permissionModel);
          }
      }

      function addForeignResourceIdToPermissionModel(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              permissionModel.officeId = parameters.foreignResourceId;
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              permissionModel.programId = parameters.foreignResourceId;
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              permissionModel.projectId = parameters.foreignResourceId;
          }
      }

      function addPermission(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              OfficeService.addPermission(permissionModel)
                .then(function () {
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error adding the permission.');
                });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              ProgramService.addPermission(permissionModel)
                  .then(function () {
                      loadCollaborators({ start: 0, limit: 300 });
                  }, function () {
                      NotificationService.showErrorMessage('There was an error adding the permission.');
                  });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              ProjectService.addPermission(permissionModel)
                  .then(function () {
                      loadCollaborators({ start: 0, limit: 300 });
                  }, function () {
                      NotificationService.showErrorMessage('There was an error adding the permission.');
                  });
          }
      }

      function removePermission(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              OfficeService.removePermission(permissionModel)
                .then(function () {
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error removing the permission.');
                });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              ProgramService.removePermission(permissionModel)
                  .then(function () {
                      loadCollaborators({ start: 0, limit: 300 });
                  }, function () {
                      NotificationService.showErrorMessage('There was an error removing the permission.');
                  });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              ProjectService.removePermission(permissionModel)
                  .then(function () {
                      loadCollaborators({ start: 0, limit: 300 });
                  }, function () {
                      NotificationService.showErrorMessage('There was an error removing the permission.');
                  });
          }
      }

      function revokePermission(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              OfficeService.revokePermission(permissionModel)
                .then(function () {
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error revoking the permission.');
                });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              ProgramService.revokePermission(permissionModel)
                  .then(function () {
                      loadCollaborators({ start: 0, limit: 300 });
                  }, function () {
                      NotificationService.showErrorMessage('There was an error revoking the permission.');
                  });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              ProjectService.revokePermission(permissionModel)
                  .then(function () {
                      loadCollaborators({ start: 0, limit: 300 });
                  }, function () {
                      NotificationService.showErrorMessage('There was an error revoking the permission.');
                  });
          }
      }

      $scope.addCollaborator = function () {
          var permissionModel = {
              principalId: $scope.selectedCollaborator.principalId,
              permissionId: ConstantsService.permission.viewProgram.id
          };
          addForeignResourceIdToPermissionModel(permissionModel);
          ProgramService.addPermission(permissionModel)
                .then(function () {
                    $scope.selectedCollaborator = "";
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error adding the collaborator.');
                });
      }

     
      
      function loadCollaborators(params) {
          $scope.collaboratorsLoading = true;
          var url = getUrl();
          return $q.when(AuthService.getPrincipalResourceAuthorizations(parameters.resourceType.value, parameters.foreignResourceId, url, params))
          .then(updateCollaborators, showLoadCollaboratorsError)
          .then(function () {
              $scope.collaboratorsLoading = false;
          });
      }
      
      function getUrl() {
          var url = "";
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              url = "/offices/";
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              url = "/programs/"
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              url = "/projects/"
          }

          url = url + parameters.foreignResourceId + "/collaborators";

          return url;
      }

      function updateCollaborators(resourceAuthorizations) {

          for (var i = 0; i < resourceAuthorizations.length; i++) {
              var resourceAuthorization = resourceAuthorizations[i];
              resourceAuthorization.mergedPermissions = mergePermissions(resourceAuthorization.assignedPermissions, resourceAuthorization.availablePermissions);
          }

          $scope.collaborators = resourceAuthorizations;
          console.log(resourceAuthorizations);
      }

      function mergePermissions(assignedPermissions, availablePermissions) {

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
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

      $q.all([loadCollaborators({ start: 0, limit: 300 })]);
  });
