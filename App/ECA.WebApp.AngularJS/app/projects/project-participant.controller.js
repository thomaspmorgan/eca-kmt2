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
        MessageBox,
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
        ParticipantStudentVisitorService,
        ParticipantExchangeVisitorService
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
      $scope.view.tabStudentVisitor = false;
      $scope.view.tabExchangeVistor = false;

      $scope.view.sevisCommStatuses = null;

      $scope.sevisInfo = {};
      $scope.studentVisitorInfo = {};
      $scope.exchangeVisitorInfo = {};
      $scope.participantInfo = {};

      $scope.actions = {
          "Select Action": undefined,
          "Send To SEVIS": 1
      };

      $scope.permissions = {};
      $scope.permissions.isProjectOwner = false;
      $scope.permissions.editProject = false;
      var projectId = $stateParams.projectId;

      $scope.view.onDeleteParticipantClick = function (participant) {
          MessageBox.confirm({
              title: 'Confirm',
              message: 'Are you sure you wish to delete the participant named ' + participant.name + '.',
              okText: 'Yes',
              cancelText: 'No',
              okCallback: function () {
                  deleteParticipant(participant, projectId);
              }
          });
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

      function deleteParticipant(participant, projectId) {
          $scope.participantInfo[participant.participantId] = $scope.participantInfo[participant.participantId] || {};
          $scope.participantInfo[participant.participantId].isDeleting = true;
          return ParticipantService.deleteParticipant(participant.participantId, projectId)
          .then(function (response) {
              $scope.participantInfo[participant.participantId].isDeleting = false;
              delete $scope.participantInfo[participant.participantId];
              NotificationService.showSuccessMessage("Successfully deleted the participant " + participant.name + '.');
              reloadParticipantTable();
          })
          .catch(function (response) {
              $scope.participantInfo[participant.participantId].isDeleting = false;
              var message = "Unable to delete the participant.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          })
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
                  $log.info('User has project owner permission in project-participant.controller.js.');
              },
              notAuthorized: function () {
                  $scope.permissions.isProjectOwner = false;
                  $log.info('User not authorized to project ownership in project-participant.controller.js.');
              }
          };
          config[ConstantsService.permission.editProject.value] = {
              hasPermission: function () {
                  $scope.permissions.editProject = true;
                  $log.info('User has edit project permission in project-participant.controller.js.');
              },
              notAuthorized: function () {
                  $scope.permissions.editProject = false;
                  $log.info('User not authorized to edit project in project-participant.controller.js.');
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
              filter: tableState.filter ? tableState.filter : TableService.getFilter(),
              keyword: TableService.getKeywords()
          };

          // Get the total number or participants
          ParticipantService.getParticipantsByProject($stateParams.projectId, {start: 0, limit: 1})
            .then(function (data) {
                $scope.view.totalParticipants = data.total;
            }, function () {
                $log.error('Unable to load project participants.');
                NotificationService.showErrorMessage('Unable to load project participants.');
            });

          ParticipantService.getParticipantsByProject($stateParams.projectId, params)
            .then(function (data) {
                angular.forEach(data.results, function (result, index) {
                    if (result.personId) {
                        result.href = StateService.getPersonState(result.personId);
                    }
                    else if (result.organizationId) {
                        result.href = StateService.getOrganizationState(result.organizationId);
                    }
                    else {
                        var message = 'Unable to generate href for participant because it is neither an organization or a person.';
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    }
                });
                $scope.participants = data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
                $scope.participantsLoading = false;
            }, function (error) {
                $log.error('Unable to load project participants.');
                NotificationService.showErrorMessage('Unable to load project participants.');
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

      function loadExchangeVisitorInfo(participantId) {
          return ParticipantExchangeVisitorService.getParticipantExchangeVisitorById(participantId)
          .then(function (data) {
              $scope.exchangeVisitorInfo[participantId] = data.data;
              //$scope.exchangeVisitorInfo[participantId].show = true;
          }, function (error) {
              if (error.status === 404) {
                  $scope.exchangeVisitorInfo[participantId] = {};
                  //$scope.exchangeVisitorInfo[participantId].show = true;
              } else {
                  $log.error('Unable to load participant exchange visitor info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant exchange visitor info for ' + participantId + '.');
              }
          });
      };

      $scope.onInfoTabSelected = function (participantId) {
          $scope.view.tabInfo = true;
      }

      function saveSevisInfoById(participantId) {
          var sevisInfo = $scope.sevisInfo[participantId];
          return ParticipantPersonsSevisService.updateParticipantPersonsSevis(sevisInfo)
          .then(function (data) {
              NotificationService.showSuccessMessage('Participant SEVIS info saved successfully.');
              $scope.sevisInfo[participantId] = data.data;
              $scope.sevisInfo[participantId].show = true;
          }, function (error) {
              $log.error('Unable to save participant SEVIS info for participantId: ' + participantId);
              NotificationService.showErrorMessage('Unable to save participant SEVIS info for participant: ' + participantId + '.');
          });
      };

      $scope.saveSevisInfo = function (participantId) {
          saveSevisInfoById(participantId);
      };

      function saveExchangeVisitorById(participantId) {
          var exchangeVisitorInfo = $scope.exchangeVisitorInfo[participantId];
          return ParticipantExchangeVisitorService.updateParticipantExchangeVisitor(exchangeVisitorInfo)
          .then(function (data) {
              NotificationService.showSuccessMessage('Participant exchange visitor info saved successfully.');
              $scope.exchangeVisitorInfo[participantId] = data.data;
              //$scope.exchangeVisitorInfo[participantId].show = true;
          }, function (error) {
              $log.error('Unable to save participant exchange visitor info for participantId: ' + participantId);
              NotificationService.showErrorMessage('Unable to save participant exchange visitor info for participant: ' + participantId + '.');
          });
      };

      $scope.saveExchangeVisitorInfo = function (participantId) {
          saveExchangeVisitorById(participantId);
      };

      function saveStudentVisitorById(participantId) {
          var studentVisitorInfo = $scope.studentVisitorInfo[participantId];
          return ParticipantStudentVisitorService.updateParticipantStudentVisitor(studentVisitorInfo)
          .then(function (data) {
              NotificationService.showSuccessMessage('Participant student visitor info saved successfully.');
              $scope.studentVisitorInfo[participantId] = data.data;
              $scope.studentVisitorInfo[participantId].show = true;
          }, function (error) {
              $log.error('Unable to save participant studdent visitor info for participantId: ' + participantId);
              NotificationService.showErrorMessage('Unable to save participant student visitor info for participant: ' + participantId + '.');
          });
      };

      $scope.saveStudentVisitorInfo = function (participantId) {
          saveStudentVisitorById(participantId);
      };

      $scope.onSevisTabSelected = function (participantId) {
          $scope.view.tabSevis = true;
          loadSevisInfo(participantId);
      };

      $scope.onStudentVisitorTabSelected = function (participantId) {
          $scope.view.tabStudentVisitor = true;
          loadStudentVisitorInfo(participantId)
      }

      $scope.onExchangeVisitorTabSelected = function (participantId) {
          $scope.view.tabExchangeVisitor = true;
          loadExchangeVisitorInfo(participantId)
      }

      $scope.toggleParticipantInfo = function (participantId) {
          var defaultParticipantInfo = {show: false};
          $scope.participantInfo[participantId] = $scope.participantInfo[participantId] || defaultParticipantInfo;
          $scope.participantInfo[participantId].show = !$scope.participantInfo[participantId].show;
          if ($scope.participantInfo[participantId].show) {
                  $scope.view.tabInfo = true;
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

      $scope.selectAllChanged = function () {
          if ($scope.selectAll) {
              for (var i = 0; i < $scope.participants.length; i++) {
                  var participantId = $scope.participants[0].participantId;
                  $scope.selectedParticipants[participantId] = true;
              }
          } else {
              $scope.selectedParticipants = {};
          }
      }

      $scope.selectedActionChanged = function () {
          $scope.selectAll = false;
          $scope.selectedParticipants = {};
          var tableState = $scope.getParticipantsTableState();
          tableState.filter = [];
          if ($scope.selectedAction === 1) {
              tableState.filter = { property: 'sevisStatus', comparison: 'eq', value: 'Ready To Submit' };
          }
          $scope.getParticipants(tableState);
      }

      $scope.selectedParticipants = {};

      $scope.selectedParticipantsEmpty = function () {
          return Object.keys($scope.selectedParticipants).length === 0;
      }

      $scope.applyAction = function () {
          if ($scope.selectedAction === 1) {
              var participants = Object.keys($scope.selectedParticipants).map(Number);
              ParticipantPersonsSevisService.sendToSevis(participants)
              .then(function (results) {
                  $scope.selectAll = false;
                  $scope.selectedParticipants = {};
                  NotificationService.showSuccessMessage("Successfully queued " + results.data.length + " of " + participants.length  + " participants.");
                  reloadParticipantTable();
              }, function () {
                  NotificationService.showErrorMessage("Failed to queue participants.");
              });
          }
      }

      $scope.selectedParticipant = function (participant, checked) {
          if (!checked) {
              delete $scope.selectedParticipants[participant.participantId];
          }
      }

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
