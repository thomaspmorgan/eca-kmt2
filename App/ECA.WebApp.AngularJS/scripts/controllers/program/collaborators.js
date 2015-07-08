angular.module('staticApp')
  .controller('ProgramCollaboratorsCtrl', function ($scope, $stateParams, $q, ProgramService, AuthService, ConstantsService, NotificationService, orderByFilter, UserService) {

      $scope.listCount = {
          start: 0,
          total: 0
      }

      $scope.collaboratorsLoading = false;

      $scope.editingCollaborator = {};

      $scope.editCollaborator = function (collaborator) {
          $scope.editingCollaborator[collaborator.principalId] = true;
      }

      $scope.addCollaborator = function () {
          var permissionModel = {
              principalId: $scope.selectedCollaborator.principalId,
              programId: $stateParams.programId,
              permissionId: ConstantsService.permission.viewProgram.id
          };
          ProgramService.addPermission(permissionModel)
                .then(function () {
                    $scope.selectedCollaborator = undefined;
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error adding the collaborator.');
                });
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

      $scope.allowPermissionHandler = function (collaborator, permission) {
          $scope.collaboratorsLoading = true;
          var permissionModel = {
              principalId: collaborator.principalId,
              programId: $stateParams.programId,
              permissionId: permission.permissionId
          };
          if (permission.isAllowed === false) {
              permission.isAllowed = undefined;
              ProgramService.removePermission(permissionModel)
                .then(function () {
                    loadCollaborators({ start: 0, limit: 300 });
                }, function (error) {
                    NotificationService.showErrorMessage('There was an error removing the permission.');
                });
          } else {
              ProgramService.addPermission(permissionModel)
                .then(function () {
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error adding the permission.');
                });
          }
      }

      $scope.denyPermissionHandler = function (collaborator, permission) {
          $scope.collaboratorsLoading = true;
          var permissionModel = {
              principalId: collaborator.principalId,
              programId: $stateParams.programId,
              permissionId: permission.permissionId
          };
          if (permission.isAllowed === true) {
              permission.isAllowed = undefined;
              ProgramService.removePermission(permissionModel)
                .then(function () {
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error revoking the permission.');
                });
          } else {
              ProgramService.revokePermission(permissionModel)
                .then(function () {
                    loadCollaborators({ start: 0, limit: 300 });
                }, function () {
                    NotificationService.showErrorMessage('There was an error revoking the permission.');
                });
          }
      }

      function loadCollaborators(params) {
          var programId = $stateParams.programId;
          var url = '/programs/' + programId + '/collaborators';
          $scope.collaboratorsLoading = true;
          return $q.when(AuthService.getPrincipalResourceAuthorizations(ConstantsService.resourceType.program.value, programId, url, params))
          .then(updateCollaborators, showLoadCollaboratorsError)
          .then(function () {
              if ($scope.collaborators.length > 0) {
                  $scope.listCount.start = 1;
              }
              $scope.listCount.total = $scope.collaborators.length;
              $scope.collaboratorsLoading = false;
          });
      }

      function updateCollaborators(resourceAuthorizations) {

          for (var i = 0; i < resourceAuthorizations.length; i++) {
              var resourceAuthorization = resourceAuthorizations[i];
              resourceAuthorization.mergedPermissions = mergePermissions(resourceAuthorization.assignedPermissions, resourceAuthorization.availablePermissions);
          }

          $scope.collaborators = resourceAuthorizations;
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

      $q.all([loadCollaborators({start: 0, limit: 300})]);

  });