'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProjectCollaboratorCtrl
 * @description The AddProjectCollaboratorCtrl is used to manage collaborators on a project.
 * # AddProjectCollaboratorCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectCollaboratorCtrl', function ($scope, $stateParams, $q, $modalInstance, ConstantsService, ProjectService, NotificationService, TableService, AuthService, UserService, orderByFilter) {

      var projectId = $stateParams.projectId;
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
      $scope.view.pageSize = 25;
      $scope.view.addCollaboratorLimit = 25;
      $scope.view.addedCollaborator = {};


      var collaboratorsLoadParams = {};
      $scope.view.loadCollaborators = function (tableState) {
          TableService.setTableState(tableState);
          collaboratorsLoadParams = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()

          };
          loadCollaborators(collaboratorsLoadParams);
      }

      var availablePermissions = [];

      $scope.view.onRemovePermission = function (permission, collaborator) {
          isSaving(true);
          ProjectService.removePermission(
              collaborator.principalId,
              permission.foreignResourceId,
              permission.permissionId)
          .then(function () {
              NotificationService.showSuccessMessage('Successfully removed ' + permission.permissionName + ' permission from ' + collaborator.displayName + '.');
          }, function () {
              NotificationService.showErrorMessage('There was an error removing the user\'s permission.');
          })
          .then(function () {
              isSaving(false);
          });
      }

      $scope.view.onSelectPermission = function (permission, collaborator) {
          doUpdatePermission(true, permission, collaborator)
          .then(function () {
              NotificationService.showSuccessMessage('Successfully granted ' + permission.permissionName + ' permission to ' + collaborator.displayName + '.');
          }, function () {
              NotificationService.showErrorMessage('There was an error updating the user\'s permission.');
          });
      }

      $scope.view.onPlusButtonClick = function (permission, collaborator) {
          doUpdatePermission(true, permission, collaborator)
          .then(function () {
              NotificationService.showSuccessMessage('Successfully granted ' + permission.permissionName + ' permission to ' + collaborator.displayName + '.');
          }, function () {
              NotificationService.showErrorMessage('There was an error updating the user\'s permission.');
          });
      }

      $scope.view.onMinusButtonClick = function (permission, collaborator) {
          doUpdatePermission(false, permission, collaborator)
          .then(function () {
              NotificationService.showSuccessMessage('Successfully revoked ' + permission.permissionName + ' permission from ' + collaborator.displayName + '.');
          }, function () {
              NotificationService.showErrorMessage('There was an error updating the user\'s permission.');
          });
      }

      $scope.view.getUsers = function ($viewValue) {
          var params = {
              start: 0,
              limit: $scope.view.addCollaboratorLimit,
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

      $scope.view.onAddCollaboratorSelect = function ($item, $model, $label) {
          var viewPermission = {
              principalId: $item.principalId,
              foreignResourceId: projectId,
              permissionId: ConstantsService.permission.viewproject.id
          };
          var editPermission = {
              principalId: $item.principalId,
              foreignResourceId: projectId,
              permissionId: ConstantsService.permission.editproject.id
          };
          var collaborator = {
              principalId: $item.principalId
          };
          var isAllowed = true;
          $scope.view.addedCollaborator = $item;
          $q.when([
              doUpdatePermission(true, viewPermission, collaborator),
              doUpdatePermission(true, editPermission, collaborator)
          ])
          .then(function () {
              NotificationService.showSuccessMessage('Successfully added the collaborator.');
          }, function () {
              NotificationService.showSuccessMessage('There was an error adding the collaborator.');
          })
          .then(function () {
              
          });
      }

      $scope.view.addedCollaboratorFormatter = function ($item) {
          if ($scope.view.addedCollaborator.displayName && $scope.view.addedCollaborator.email) {
              return $scope.view.addedCollaborator.displayName + ' (' + $scope.view.addedCollaborator.email + ')';
          }
          else {
              return null;
          }
      }

      
      function loadCollaborators(params) {
          isLoading(true);
          return ProjectService.getCollaborators(projectId, params)
          .then(updateCollaborators, showLoadCollaboratorsError)
          .then(function () {
              isLoading(false);
          });
      }

      function doUpdatePermission(isAllowed, permission, collaborator) {
          isSaving(true);
          return ProjectService.updatePermission(
              isAllowed,
              collaborator.principalId,
              permission.foreignResourceId,
              permission.permissionId
              )
          .then(function () {
              isSaving(false);
              loadCollaborators(collaboratorsLoadParams);
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
