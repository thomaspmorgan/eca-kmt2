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
        ProjectService,
        NotificationService,
        TableService,
        ParticipantService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isCollaboratorExpanded = false;
      $scope.view.numberOfCollaborators = -1;
      $scope.view.collaboratorsLastUpdated = null;
      $scope.view.isCollaboratorsModalOpen = false;

      $scope.permissions = {};
      $scope.permissions.isProjectOwner = false;
      var projectId = $stateParams.projectId;

      $scope.view.addCollaborator = function ($event) {
          $scope.view.isCollaboratorsModalOpen = true;
          var modalInstance = $modal.open({
              templateUrl: '/views/project/collaborators.html',
              controller: 'ProjectCollaboratorCtrl',
              backdrop: 'static',
              resolve: {},
              windowClass: 'modal-center-large'
          });
          modalInstance.result.then(function () {
              $log.info('Closing...');
          }, function () {
              $log.info('Dismiss add collaborator dialog...');
          })
          .then(function () {
              $scope.view.isCollaboratorsModalOpen = false;
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
      
      function loadCollaboratorDetails() {
          return ProjectService.getCollaboratorInfo(projectId)
          .then(function (response) {
              if (response.data !== null) {
                  $scope.view.numberOfCollaborators = response.data.allowedPrincipalsCount;
                  var lastRevisedDate = new Date(response.data.lastRevisedOn);
                  if (!isNaN(lastRevisedDate.getTime())) {
                      $scope.view.collaboratorsLastUpdated = lastRevisedDate;
                  }
              }
              else {
                  NotificationService.showWarningMessage('Unable to load collaborator details.');
              }
          }, function (error) {
              $log.error('Unable to load project collaborator details.');
              NotificationService.showErrorMessage('Unable to load project collaborator details.');
          })
          .catch(function () {
              $log.error('Unable to load project collaborator details.');
              NotificationService.showErrorMessage('Unable to load project collaborator details.');
          });
      }

      $scope.participantsLoading = false;
      $scope.getParticipants = function (tableState) {

          $scope.showParticipantInfo = [];
          $scope.participantsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          ParticipantService.getParticipantsByProject($stateParams.projectId, params)
            .then(function (data) {
                $scope.project.participants = data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
                $scope.participantsLoading = false;
            }, function (error) {
                $log.error('Unable to load project participants.');
                NotificationService.showErrorMessage('Unable to load project participants.');
            });
      };

      $scope.showParticipantInfo = [];
      $scope.toggleParticipantInfo = function (participantId) {
          if ($scope.showParticipantInfo[participantId] === true) {
              $scope.showParticipantInfo[participantId] = false;
          } else {
              $scope.showParticipantInfo[participantId] = true;
          }
      };

      $scope.view.isLoading = true;
      $q.all([loadPermissions(), loadCollaboratorDetails()])
      .then(function (results) {

      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });
  });
