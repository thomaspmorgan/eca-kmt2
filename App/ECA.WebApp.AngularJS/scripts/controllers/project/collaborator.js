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
      $scope.view.addedCollaborator = {};

      var limit = 300;
      var collaboratorsLoadParams = {
          start: 0,
          limit: limit
      };
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

      $scope.view.onAddCollaboratorSelect = function ($item, $model, $label) {
          var viewPermission = {
              principalId: $item.principalId,
              foreignResourceId: projectId,
              permissionId: ConstantsService.permission.viewProject.id
          };
          var editPermission = {
              principalId: $item.principalId,
              foreignResourceId: projectId,
              permissionId: ConstantsService.permission.editProject.id
          };
          var collaborator = {
              principalId: $item.principalId
          };
          var isAllowed = true;
          $scope.view.addedCollaborator = $item;
          isSaving(true);
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
              isSaving(false);
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
          var url = '/projects/' + projectId + '/collaborators';
          $q.when(AuthService.getPrincipalResourceAuthorizations(ConstantsService.resourceType.project.value, projectId, url, params))
          .then(updateCollaborators, showLoadCollaboratorsError);
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

      function updateCollaborators(resourceAuthorizations) {
          $scope.view.collaborators = resourceAuthorizations;
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

      isLoading(true);
      $q.when([loadCollaborators(collaboratorsLoadParams)])
          .then(function () {
              isLoading(false);
          });


  });
