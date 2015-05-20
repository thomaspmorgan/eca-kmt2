'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProjectCollaboratorCtrl
 * @description The AddProjectCollaboratorCtrl is used to manage collaborators on a project.
 * # AddProjectCollaboratorCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddProjectCollaboratorCtrl', function ($scope, $stateParams, $q, $modalInstance, ProjectService, NotificationService, TableService, orderByFilter) {

      $scope.view = {};
      $scope.view.isLoading = false;
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


      var projectId = $stateParams.projectId;
      function loadCollaborators(params) {
          isLoading(true);
          ProjectService.getCollaborators(projectId, params)
          .then(updateCollaborators, showLoadCollaboratorsError)
          .then(isLoading(false))
      }

      function isLoading(isLoading) {
          $scope.view.isLoading = isLoading;
      }

      function updateCollaborators(response) {
          var collaborators = response.data.results;
          $scope.view.collaborators = collaborators;
          var groupedPermissions = getGroupedPrincipalPermissions(collaborators);
          for (var i = 0; i < groupedPermissions.length; i++) {
              var collaboratorPermission = groupedPermissions[i];
              var orderedPermissions = orderByFilter(collaboratorPermission.permissions, '+permissionName');
              $scope.view.collaboratorPermissions[collaboratorPermission.principalId.toString()] = orderedPermissions;
          }
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
              permissionArray.push({
                  permissionId: collaborator.permissionId,
                  permissionName: collaborator.permissionName,
                  permissionDescription: collaborator.permissionDescription
              });
          }
          return collaboratorPermissions;
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

  });
