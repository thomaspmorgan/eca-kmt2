'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectParticipantCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        ConstantsService,
        AuthService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isCollaboratorExpanded = true;

      $scope.permissions = {};
      $scope.permissions.isProjectOwner = false;
      var projectId = $stateParams.projectId;

      $scope.view.addCollaborator = function ($event) {
          var modalInstance = $modal.open({
              templateUrl: '/views/project/collaborators.html',
              controller: 'ProjectCollaboratorCtrl',
              backdrop: 'static',
              resolve: {},
              windowClass: 'modal-center-large'
          });
          modalInstance.result.then(function () {
              $log.info('Cancelling changes...');              
          }, function () {
              $log.info('Dismiss add collaborator dialog...');
          });
      };
      

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.projectOwner.value] = {
              hasPermission: function () {
                  $scope.permissions.isProjectOwner = true;
                  $log.info('User has project owner permission in collaborator.js controller.');
              },
              notAuthorized: function () {
                  $scope.permissions.isProjectOwner = false;
                  $log.info('User not authorized to manage project collaborators in collaborator.js controller.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      }
      

      $scope.view.isLoading = true;
      $q.all([loadPermissions()])
      .then(function (results) {
          //results is an array

      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });
  });
