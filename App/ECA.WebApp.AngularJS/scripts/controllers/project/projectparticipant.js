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

      $scope.view.addPersonFilterValue = 'person';
      $scope.view.addOrganizationFilterValue = 'organization';
      $scope.view.addParticipantFilter = 'person';
      $scope.view.selectedExistingParticipant = null;
      $scope.view.addParticipantsLimit = 10;
      $scope.view.isLoadingAvailableParticipants = false;
      $scope.view.totalAvailableParticipants = 0;
      $scope.view.displayedAvailableParticipantsCount = 0;

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

      $scope.view.onAddParticipantSelect = function ($item, $model, $label) {
          $scope.view.isLoading = false;
          var clientModel = {
              projectId: projectId
          }
          var dfd = null;
          if ($item.personId) {
              clientModel.personId = $item.personId;
              dfd = ProjectService.addPersonParticipant(clientModel)
          }
          else {
              clientModel.organizationId = $item.organizationId;
              dfd = ProjectService.addOrganizationParticipant(clientModel);
          }
          dfd.then(function () {
              NotificationService.showSuccessMessage('Successfully added the project participant.');
          })
          .catch(function () {
              NotificationService.showErrorMessage('Unable to add project participant.');
          })
          .then(function () {
              $scope.view.isLoading = false;
          });
          return dfd;
      }
      
      $scope.view.getAvailableParticipants = function (search) {
          return loadAvailableParticipants(search, $scope.view.addParticipantFilter);
      }

      $scope.view.formatAddedParticipant = function (participant) {
          if (participant && participant.name.length > 0) {
              return participant.name;
          }
          else {
              return '';
          }
      }


      $scope.view.onRadioButtonChange = function (radioButtonValue) {
          $scope.view.selectedExistingParticipant = null;
          $scope.view.displayedAvailableParticipantsCount = 0;
          $scope.view.totalAvailableParticipants = 0;
      }

      function loadAvailableParticipants(search, participantType) {
          var participantTypeFilter = {
              comparison: ConstantsService.isNotNullComparisonType
          };
          if (participantType === $scope.view.addPersonFilterValue) {
              $log.info('Adding not null filter on person participant type.');
              participantTypeFilter.property = 'personId';
          }
          else if (participantType === $scope.view.addOrganizationFilterValue) {
              $log.info('Adding not null filter on organization participant type.');
              participantTypeFilter.property = 'organizationId';
          }
          else {
              $log.error('Unable to add participant type filter.');
          }

          var params = {
              start: 0,
              limit: $scope.view.addParticipantsLimit,
              filter: [participantTypeFilter]
          };
          if (search) {
              params.keyword = search
          }
          $scope.view.isLoadingAvailableParticipants = true;
          return ParticipantService.getParticipants(params)
          .then(function (response) {
              $scope.view.isLoadingAvailableParticipants = false;
              $scope.view.totalAvailableParticipants = response.total;
              $scope.view.displayedAvailableParticipantsCount = response.results.length;
              return response.results;
          })
          .catch(function () {
              $scope.view.isLoadingAvailableParticipants = false;
              $log.error('Unable to load available participants.');
              NotificationService.showErrorMessage('Unable to load available participants.');
          });
      }

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

          $scope.showParticipantInfo = {};
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

      $scope.showParticipantInfo = {};
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
