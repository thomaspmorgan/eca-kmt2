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
        $state,
        StateService,
        OrganizationService,
        PersonService,
        ConstantsService,
        AuthService,
        ProjectService,
        NotificationService,
        TableService,
        ParticipantService,
        ParticipantPersonsService,
        ParticipantPersonsSevisService,
        ParticipantStudentVisitorService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isCollaboratorExpanded = false;
      $scope.view.numberOfCollaborators = -1;
      $scope.view.collaboratorsLastUpdated = null;

      $scope.view.addPersonFilterValue = 'person';
      $scope.view.addOrganizationFilterValue = 'organization';
      $scope.view.addParticipantFilter = 'person';
      $scope.view.selectedExistingParticipant = null;
      $scope.view.addParticipantsLimit = 10;
      $scope.view.isLoadingAvailableParticipants = false;
      $scope.view.totalAvailableParticipants = 0;
      $scope.view.displayedAvailableParticipantsCount = 0;
      $scope.view.isAddingParticipant = false;
      $scope.view.isDobDatePickerOpen = false;
      $scope.view.dateFormat = 'dd-MMMM-yyyy';
      $scope.view.totalParticipants = 0;
      $scope.view.tabSevis = false;
      $scope.view.tabInfo = false;

      $scope.view.sevisCommStatuses = null;

      $scope.sevisInfo = {};
      $scope.studentVisitorInfo = {};
      $scope.participantInfo = {};

      $scope.permissions = {};
      $scope.permissions.isProjectOwner = false;
      var projectId = $stateParams.projectId;

      $scope.onParticipantClick = function (participant) {

          if (participant.personId) {
              $log.info('Navigating the individual state.');
              $state.go('people.personalinformation', { personId: participant.personId });
          }
          else if (participant.organizationId) {
              $log.info('Navigating to organization overview state.');
              $state.go('organizations.overview', { organizationId: participant.organizationId });
          }
          else {
              NotificationService.showErrorMessage('The participant is neither an organization nor a person.');
          }
      };

      $scope.view.onAddParticipantSelect = function ($item, $model, $label) {
          var clientModel = {
              projectId: projectId,
              name: $model.name || $model.fullName
          }
          if ($item.personId) {
              clientModel.personId = $item.personId;
          }
          else {
              clientModel.organizationId = $item.organizationId;
          }
          addParticipant(clientModel);
      }

      function addParticipant(clientModel) {
          var modalInstance = $modal.open({
              templateUrl: '/app/projects/select-participant-type.html',
              controller: 'SelectParticipantTypeCtrl',
              backdrop: 'static',
              resolve: {
                  clientModel: function () {
                      return clientModel;
                  }
              },
          });
          modalInstance.result.then(function (updatedClientModel) {
              console.assert(updatedClientModel.participantTypeId, "The participant type must be set.");
              doAddParticipant(updatedClientModel);
              $log.info('Closing...');
          }, function () {
              clearAddParticipantView();
              $log.info('Dismiss select participant type dialog...');
          })
          .then(function () {

          });
      };

      function doAddParticipant(clientModel) {
          $scope.view.isAddingParticipant = true;
          var dfd = null;
          if (clientModel.personId) {
              dfd = ProjectService.addPersonParticipant(clientModel);
          }
          else {
              dfd = ProjectService.addOrganizationParticipant(clientModel);
          }
          dfd.then(function () {
              NotificationService.showSuccessMessage('Successfully added ' + clientModel.name + ' as a project participant.');
          })
          .catch(function () {
              NotificationService.showErrorMessage('Unable to add project participant.');
          })
          .then(function () {
              $scope.view.isAddingParticipant = false;
              reloadParticipantTable();
          });
          return dfd;
      }

      $scope.view.getAvailableParticipants = function (search) {
          return loadAvailableParticipants(search, $scope.view.addParticipantFilter);
      }

      $scope.view.formatAddedParticipant = function (participant) {
          if (participant && participant.name && participant.name.length > 0) {
              return participant.name;
          }
          else if (participant && participant.fullName && participant.fullName.length > 0) {
              return participant.fullName;
          }
          else {
              return '';
          }
      }

      $scope.view.onRadioButtonChange = function (radioButtonValue) {
          clearAddParticipantView();
      }

      function reloadParticipantTable() {
          console.assert($scope.getParticipantsTableState, "The table state function must exist.");
          $scope.getParticipants($scope.getParticipantsTableState());
      }

      function clearAddParticipantView() {
          $scope.view.selectedExistingParticipant = null;
          $scope.view.displayedAvailableParticipantsCount = 0;
          $scope.view.totalAvailableParticipants = 0;
      }

      function loadAvailableParticipants(search, participantType) {
          var params = {
              start: 0,
              limit: $scope.view.addParticipantsLimit
          };
          if (search) {
              params.keyword = search;
          }
          $scope.view.isLoadingAvailableParticipants = true;
          var dfd = null;


          if (participantType === $scope.view.addPersonFilterValue) {
              dfd = PersonService.getPeople(params);
          }
          else if (participantType === $scope.view.addOrganizationFilterValue) {
              dfd = OrganizationService.getOrganizations(params);
          }
          else {
              throw error('Unable to add participant type filter.');
          }

          return dfd
          .then(function (response) {
              var data = null;
              var total = null;
              if (response.data) {
                  data = response.data.results;
                  total = response.data.total;
              }
              else {
                  data = response.results;
                  total = response.total;
              }
              $scope.view.isLoadingAvailableParticipants = false;
              $scope.view.totalAvailableParticipants = total;
              $scope.view.displayedAvailableParticipantsCount = data.length;
              return data;
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
      };

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
      };

      $scope.participantsLoading = false;
      $scope.getParticipants = function (tableState) {
          $scope.participantInfo = {};
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
                $scope.participants = data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
                $scope.view.totalParticipants = data.total;
                $scope.participantsLoading = false;
            }, function (error) {
                $log.error('Unable to load project participants.');
                NotificationService.showErrorMessage('Unable to load project participants.');
            });
      };

      function getPreferredAddress(institution, participantPersonInstitutionAddressId) {
          var address = null;
          if (institution && institution.addresses && institution.addresses.length > 0) {
              angular.forEach(institution.addresses, function (institutionAddress, index) {
                  if (institutionAddress.addressId === participantPersonInstitutionAddressId) {
                      address = institutionAddress;
                  }
              });
          }
          return address;
      }

      function loadParticipantInfo(participantId) {
          return ParticipantPersonsService.getParticipantPersonsById(participantId)
          .then(function (data) {
              if (data.data.homeInstitution) {
                  data.data.homeInstitutionId = data.data.homeInstitution.organizationId;
                  data.data.homeInstitution.href = StateService.getOrganizationState(data.data.homeInstitution.organizationId);
                  data.data.homeInstitutionAddress = getPreferredAddress(data.data.homeInstitution, data.data.homeInstitutionAddressId);
              }
              if (data.data.hostInstitution) {
                  data.data.hostInstitutionId = data.data.hostInstitution.organizationId;
                  data.data.hostInstitution.href = StateService.getOrganizationState(data.data.hostInstitution.organizationId);
                  data.data.hostInstitutionAddress = getPreferredAddress(data.data.hostInstitution, data.data.hostInstitutionAddressId);
              }
              $scope.participantInfo[participantId] = data.data;
              $scope.participantInfo[participantId].show = true;
          }, function (error) {
              if (error.status === 404) {
                  $scope.participantInfo[participantId] = {};
                  $scope.participantInfo[participantId].show = true;
              } else {
                  $log.error('Unable to load participant info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant info for ' + participantId + '.');
              }
          });
      };

      function loadSevisInfo(participantId) {
          return ParticipantPersonsSevisService.getParticipantPersonsSevisById(participantId)
          .then(function (data) {
              $scope.sevisInfo[participantId] = data.data;
              $scope.sevisInfo[participantId].show = true;
          }, function (error) {
              if (error.status === 404) {
                  $scope.sevisInfo[participantId] = {};
                  $scope.sevisInfo[participantId].show = true;
              } else {
                  $log.error('Unable to load participant SEVIS info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant SEVIS info for ' + participantId + '.');
              }
          });
      };

      function loadStudentVisitorInfo(participantId) {
          return ParticipantStudentVisitorService.getParticipantStudentVisitorById(participantId)
          .then(function (data) {
              $scope.studentVisitorInfo[participantId] = data.data;
              $scope.studentVisitorInfo[participantId].show = true;
          }, function (error) {
              if (error.status === 404) {
                  $scope.studentVisitorInfo[participantId] = {};
                  $scope.studentVisitorInfo[participantId].show = true;
              } else {
                  $log.error('Unable to load participant student visitor info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant student visitor info for ' + participantId + '.');
              }
          });
      };

      $scope.onInfoTabSelected = function (participantId) {
          $scope.view.tabInfo = true;
          //the participant info tab is selected by default so this check prevents all participants from being loaded
          //when the page is rendered
          if ($scope.participantInfo[participantId] && $scope.participantInfo[participantId].show) {
              return loadParticipantInfo(participantId);
          }
          
      }

      function saveSevisInfoById(participantId) {
          var sevisInfo = $scope.sevisInfo[participantId];
          return ParticipantPersonsSevisService.updateParticipantPersonsSevis(sevisInfo)
          .then(function (data) {
              NotificationService.showSuccessMessage('Participant SEVIS info saved successfully.');
              $scope.sevisInfo[participantId].show = true;
          }, function (error) {
              $log.error('Unable to save participant SEVIS info for participantId: ' + participantId);
              NotificationService.showErrorMessage('Unable to save participant SEVIS info for participant: ' + participantId + '.');
          });
      };

      $scope.saveSevisInfo = function (participantId) {
          saveSevisInfoById(participantId);
      };

      $scope.onSevisTabSelected = function (participantId) {
          $scope.view.tabSevis = true;
          loadSevisInfo(participantId);
      };

      $scope.onStudentVisitorTabSelected = function (participantId) {
          $scope.view.tabStudentVisitor = true;
          loadStudentVisitorInfo(participantId)
      }

      $scope.toggleParticipantInfo = function (participantId) {
          if ($scope.participantInfo[participantId]) {
              if ($scope.participantInfo[participantId].show === true) {
                  $scope.participantInfo[participantId].show = false;
              }
              else {
                  $scope.participantInfo[participantId].show = true;
                  $scope.view.tabInfo = true;
              }
              }
          else {
              loadParticipantInfo(participantId);
          }
      };

      $scope.openAddNewParticipant = function () {
          var modalInstance = $modal.open({
              templateUrl: '/app/projects/add-new-participant.html',
              controller: 'AddNewParticipantCtrl',
              backdrop: 'static',
              size: 'lg'
          });

          modalInstance.result.then(function (participant) {
              if (participant) {
                  reloadParticipantTable();
              }
          });
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
