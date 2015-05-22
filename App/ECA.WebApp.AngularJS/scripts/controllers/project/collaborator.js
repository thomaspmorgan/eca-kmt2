'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProjectCollaboratorCtrl
 * @description The AddProjectCollaboratorCtrl is used to manage collaborators on a project.
 * # AddProjectCollaboratorCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectCollaboratorCtrl', function ($scope, $stateParams, $q, $modalInstance, ConstantsService, ProjectService, NotificationService, TableService, AuthService, orderByFilter) {

      $scope.view = {};
      $scope.view.isLoading = false;
      $scope.view.isSaving = false;
      $scope.view.onAddClick = function () {
          $modalInstance.close();
      }

      $scope.view.onCancelClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.collaborators = [];
      $scope.view.collaboratorPermissions = {};
      $scope.view.availablePermissions = [];

      $scope.view.loadCollaborators = function (tableState) {
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()

          };
          loadCollaborators(params);
      }
      $scope.view.onPermissionCheckboxClick = function ($event, $permission, collaborator) {
          isSaving(true);
          ProjectService.updatePermission(
              $permission.isAllowed,
              collaborator.principalId,
              $permission.foreignResourceId,
              $permission.permissionId
              )
          .then(function () {
              NotificationService.showSuccessMessage('Successfully updated the user\'s permission.');
          }, function () {
              NotificationService.showErrorMessage('There was an error updating the user\'s permission.');
          })
          .then(function () {
              isSaving(false);
          });;
      }

      $scope.view.onRemovePermission = function ($item, collaborator) {
          isSaving(true);
          ProjectService.removePermission(
              collaborator.principalId,              
              $item.foreignResourceId,
              $item.permissionId)
          .then(function () {
              NotificationService.showSuccessMessage('Successfully removed the user\'s permission.');
          }, function () {
              NotificationService.showErrorMessage('There was an error removing the user\'s permission.');
          })
          .then(function() {
              isSaving(false);
          });
      }


      var projectId = $stateParams.projectId;
      function loadCollaborators(params) {
          isLoading(true);
          return ProjectService.getCollaborators(projectId, params)
          .then(updateCollaborators, showLoadCollaboratorsError)
          .then(function () {
              isLoading(false);
          });
      }

      function isLoading(isLoading) {
          $scope.view.isLoading = isLoading;
      }

      function isSaving(isSaving) {
          $scope.view.isSaving = isSaving;
      }

      function updateCollaborators(response) {
          var collaborators = response.data.results;
          var groupedResourceAuthorizations = AuthService.groupResourceAuthorizationsByPrincipalId(collaborators);
          groupedResourceAuthorizations = orderByFilter(groupedResourceAuthorizations, '+displayName');
          for (var i = 0; i < groupedResourceAuthorizations.length; i++) {
              var groupedResourceAuthorization = groupedResourceAuthorizations[i];
              groupedResourceAuthorization.allPermissions = [];
              for (var j = 0; j < groupedResourceAuthorization.grantedPermissions.length; j++) {
                  groupedResourceAuthorization.allPermissions.push(groupedResourceAuthorization.grantedPermissions[j]);
              }
              for (var k = 0; k < groupedResourceAuthorization.revokedPermissions.length; k++) {
                  groupedResourceAuthorization.allPermissions.push(groupedResourceAuthorization.revokedPermissions[k]);
              }
          }
          $scope.view.collaborators = groupedResourceAuthorizations;
      }

      function getGroupedPrincipalPermissions(collaborators) {
          var collaboratorPermissions = [];
          var principalIdOrderedCollaborators = orderByFilter(collaborators, '+principalId');
          var currentPrincipalId = null;

          for (var i = 0; i < principalIdOrderedCollaborators.length; i++) {
              var collaborator = principalIdOrderedCollaborators[i];
              if (currentPrincipalId === null) {
                  currentPrincipalId = collaborator.principalId;
              }

              if (i === 0 || currentPrincipalId !== collaborator.principalId) {
                  currentPrincipalId = collaborator.principalId;
                  collaboratorPermissions.push({
                      principalId: currentPrincipalId,
                      permissions: []
                  });
              }
              var permissionArray = collaboratorPermissions[collaboratorPermissions.length - 1].permissions;
              permissionArray.push(collaborator);
          }
          
          return collaboratorPermissions;
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

  });
