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

      var availablePermissions = [];

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
          .then(function () {
              isSaving(false);
          });
      }

      $scope.view.onSelectPermission = function ($item, collaborator) {
          $item.isAllowed = true;
          $item.foreignResourceId = projectId;
          isSaving(true);
          ProjectService.updatePermission(
              $item.isAllowed,
              collaborator.principalId,
              $item.foreignResourceId,
              $item.permissionId
              )
          .then(function () {
              NotificationService.showSuccessMessage('Successfully updated the user\'s permission.');
          }, function () {
              NotificationService.showErrorMessage('There was an error updating the user\'s permission.');
          })
          .then(function () {
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
              groupedResourceAuthorization.availablePermissions = createAvailablePermissions(availablePermissions, groupedResourceAuthorization, projectId, ConstantsService.resourceType.project.value);
          }
          $scope.view.collaborators = groupedResourceAuthorizations;
      }

      function createAvailablePermissions(availablePermissions, collaborator, foreignResourceId, resourceType) {
          var permissions = [];
          for (var i = 0; i < availablePermissions.length; i++) {
              permissions.push(createAvailablePermission(availablePermissions[i], collaborator, foreignResourceId, resourceType));
          }
          return permissions;

      }

      function createAvailablePermission(availablePermission, collaborator, foreignResourceId, resourceType) {
          return {
              principalId: collaborator.principalId,
              permissionId: availablePermission.permissionId,
              permissionName: availablePermission.permissionName,
              permissionDescription: availablePermission.permissionDescription,
              foreignResourceId: foreignResourceId,
              resourceType: resourceType,
              projectId: foreignResourceId
          };
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

      function loadAvailablePermissions() {
          AuthService.getGrantableResourcePermissions(ConstantsService.resourceType.project.value, projectId)
          .then(function (response) {
              availablePermissions = response.data;
          }, function () {
              NotificationService.showErrorMessage('Unable to load available permissions.');
          });
      }
      loadAvailablePermissions();


  });
