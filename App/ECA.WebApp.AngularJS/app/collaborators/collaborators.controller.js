'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AboutCtrl
 * @description
 * # CollaboratorsCont
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('CollaboratorCtrl', function ($scope, $q, $modalInstance, parameters, AuthService, UserService, OfficeService, ProgramService, ProjectService, ConstantsService, NotificationService) {

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
              return removePermission(permissionModel)
              .then(updateRolePermissionOnPermissionChange(collaborator, permission));
          } else {
              return addPermission(permissionModel)
              .then(updateRolePermissionOnPermissionChange(collaborator, permission));
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
              return removePermission(permissionModel)
              .then(updateRolePermissionOnPermissionChange(collaborator, permission));
          } else {
              return revokePermission(permissionModel)
              .then(updateRolePermissionOnPermissionChange(collaborator, permission));
          }
      }

      function updateRolePermissionOnPermissionChange(collaborator, permission) {
          angular.forEach(collaborator.rolePermissions, function (rolePermission, index) {
              if (rolePermission.permissionId === permission.permissionId
                  && rolePermission.foreignResourceId === permission.foreignResourceId) {
                  var isAllowed = true;
                  if (permission.isAllowed !== undefined) {
                      isAllowed = permission.isAllowed;
                  }
                  rolePermission.isAllowed = isAllowed;
              }
          });
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


      var params = { start: 0, limit: 300 };

      function addPermission(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              return OfficeService.addPermission(permissionModel)
                .then(function () {
                    NotificationService.showSuccessMessage('The permission was successfully added.');
                }, function () {
                    NotificationService.showErrorMessage('There was an error adding the permission.');
                    loadCollaborators(params);
                });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              return ProgramService.addPermission(permissionModel)
                  .then(function () {
                      NotificationService.showSuccessMessage('The permission was successfully added.');
                  }, function () {
                      NotificationService.showErrorMessage('There was an error adding the permission.');
                      loadCollaborators(params);
                  });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              return ProjectService.addPermission(permissionModel)
                  .then(function () {
                      NotificationService.showSuccessMessage('The permission was successfully added.');
                  }, function () {
                      NotificationService.showErrorMessage('There was an error adding the permission.');
                      loadCollaborators(params);
                  });
          }
      }

      function removePermission(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              return OfficeService.removePermission(permissionModel)
                .then(function () {
                    NotificationService.showSuccessMessage('The permission was successfully removed.');
                }, function () {
                    NotificationService.showErrorMessage('There was an error removing the permission.');
                    loadCollaborators(params);
                });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              return ProgramService.removePermission(permissionModel)
                  .then(function () {
                      NotificationService.showSuccessMessage('The permission was successfully removed.');
                  }, function () {
                      NotificationService.showErrorMessage('There was an error removing the permission.');
                      loadCollaborators(params);
                  });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              return ProjectService.removePermission(permissionModel)
                  .then(function () {
                      NotificationService.showSuccessMessage('The permission was successfully removed.');
                  }, function () {
                      NotificationService.showErrorMessage('There was an error removing the permission.');
                      loadCollaborators(params);
                  });
          }
      }

      function revokePermission(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              return OfficeService.revokePermission(permissionModel)
                .then(function () {
                    NotificationService.showSuccessMessage('The permission was successfully revoked.');
                }, function () {
                    NotificationService.showErrorMessage('There was an error revoking the permission.');
                    loadCollaborators(params);
                });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              return ProgramService.revokePermission(permissionModel)
                  .then(function () {
                      NotificationService.showSuccessMessage('The permission was successfully revoked.');
                  }, function () {
                      NotificationService.showErrorMessage('There was an error revoking the permission.');
                      loadCollaborators(params);
                  });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              return ProjectService.revokePermission(permissionModel)
                  .then(function () {
                      NotificationService.showSuccessMessage('The permission was successfully revoked.');
                  }, function () {
                      NotificationService.showErrorMessage('There was an error revoking the permission.');
                      loadCollaborators(params);
                  });
          }
      }

      $scope.addCollaborator = function () {
          var permissionModel = {
              principalId: $scope.selectedCollaborator.principalId,
          };
          addPermissionIdToPermissionModel(permissionModel);
          addForeignResourceIdToPermissionModel(permissionModel);
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              return OfficeService.addPermission(permissionModel)
                .then(function () {
                    loadCollaborators(params);
                }, function () {
                    NotificationService.showErrorMessage('There was an error adding the collaborator.');
                });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              return ProgramService.addPermission(permissionModel)
                  .then(function () {
                      loadCollaborators(params);
                  }, function () {
                      NotificationService.showErrorMessage('There was an error adding the collaborator');
                  });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              return ProjectService.addPermission(permissionModel)
                  .then(function () {
                      loadCollaborators(params);
                  }, function () {
                      NotificationService.showErrorMessage('There was an error adding the collaborator');
                  });
          }
      }


      function addPermissionIdToPermissionModel(permissionModel) {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              permissionModel.permissionId = ConstantsService.permission.viewOffice.id;
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              permissionModel.permissionId = ConstantsService.permission.viewProgram.id;
          } else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              permissionModel.permissionId = ConstantsService.permission.viewProject.id;
          }
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
          $scope.collaborators = resourceAuthorizations;
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

      $q.all([loadCollaborators(params)]);
  });
