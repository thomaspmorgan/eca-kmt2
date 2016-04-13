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
        $location,
        $timeout,
        $filter,
        $anchorScroll,
        smoothScroll,
        MessageBox,
        StateService,
        OrganizationService,
        PersonService,
        ConstantsService,
        AuthService,
        BrowserService,
        ProjectService,
        NotificationService,
        TableService,
        ParticipantService,
        ParticipantPersonsService,
        ParticipantPersonsSevisService,
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
      $scope.view.tabInfo = true;
      $scope.view.tabExchangeVisitor = false;
      $scope.view.tabStudentVisitor = false;
      $scope.view.sevisCommStatuses = null;

      $scope.view.hasRealActualParticipants = false;
      $scope.view.editingEstParticipants = false;
      $scope.view.editingActualParticipants = false;

      $scope.sevisInfo = {};
      $scope.exchangeVisitorInfo = {};
      $scope.participantInfo = {};

      $scope.actions = {
          "Select Action": undefined
      };

      $scope.selectedGridView = 'Default';
      $scope.test = true;

      $scope.permissions = {};
      $scope.permissions.isProjectOwner = false;
      $scope.permissions.editProject = false;
      $scope.permissions.hasEditSevisPermission = false;
      $scope.permissions.hasSendToSevisPermission = false;
      var projectId = $stateParams.projectId;

      var notifyStatuses = ConstantsService.sevisStatusIds.split(',');

      var origNonUsParticipantsEst;
      var origUsParticipantsEst;
      var origNonUsParticipantsActual;
      var origUsParticipantsActual;
      var kmtId = ConstantsService.kmtApplicationResourceId;

      $scope.view.saveEstParticipants = function () {
          $scope.view.editingEstParticipants = false;
          saveProject();
      }

      $scope.view.saveActualParticipants = function () {
          $scope.view.editingActualParticipants = false;
          saveProject();
      }

      $scope.view.cancelEstParticipants = function () {
          $scope.view.editingEstParticipants = false;
          restoreOriginalEstParticipantValues();
      }

      $scope.view.cancelActualParticipants = function () {
          $scope.view.editingActualParticipants = false;
          restoreOriginalActualParticipantValues();
      }

      function updateRelationshipIds(idsPropertyName, realPropertyName) {
          console.assert($scope.$parent.project.hasOwnProperty(idsPropertyName), "The project must have the property named " + idsPropertyName);
          console.assert($scope.$parent.project.hasOwnProperty(realPropertyName), "The project must have the property named " + realPropertyName);
          $scope.$parent.project[idsPropertyName] = [];
          $scope.$parent.project[idsPropertyName] = $scope.$parent.project[realPropertyName].map(function (c) {
              return c.id;
          });
      }

      function updatePointsOfContactIds() {
          var propertyName = "pointsOfContactIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'contacts');
      }

      function updateThemes() {
          var propertyName = "themeIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'themes');
      }

      function updateCategories() {
          var propertyName = "categoryIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'categories');
      }

      function updateObjectives() {
          var propertyName = "objectiveIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'objectives');
      }

      function updateGoals() {
          var propertyName = "goalIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'goals');
      }

      function updateLocations() {
          var propertyName = "locationIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'locations');
      }

      function updateRegions() {
          var propertyName = "regionIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'regions');
      }

      function restoreOriginalEstParticipantValues() {
          $scope.$parent.project.nonUsParticipantsEst = origNonUsParticipantsEst;
          $scope.$parent.project.usParticipantsEst = origUsParticipantsEst;
      }

      function restoreOriginalActualParticipantValues() {
          $scope.$parent.project.nonUsParticipantsActual = origNonUsParticipantsActual;
          $scope.$parent.project.usParticipantsActual = origUsParticipantsActual;
      }

      function saveProject() {
          updatePointsOfContactIds();
          updateThemes();
          updateGoals();
          updateCategories();
          updateObjectives();
          updateLocations();
          updateRegions();
          ProjectService.update($scope.$parent.project, $stateParams.projectId)
           .then(function (response) {
               $scope.$parent.project = response.data;
               NotificationService.showSuccessMessage('Successfully saved number of participants.');
           }, function (error) {
               restoreOriginalEstParticipantValues();
               restoreOriginalActualParticipantValues();
               if (error.status === 400) {
                   if (error.data.message && error.data.modelState) {
                       for (var key in error.data.modelState) {
                           NotificationService.showErrorMessage(error.data.modelState[key][0]);
                       }
                   }
                   else if (error.data.Message && error.data.ValidationErrors) {
                       for (var key in error.data.ValidationErrors) {
                           NotificationService.showErrorMessage(error.data.ValidationErrors[key]);
                       }
                   } else {
                       NotificationService.showErrorMessage(error.data);
                   }
               }
           })
           .then(function () {
               //nothing
           });
      }

      function showDeleteConfirm(participant) {
          MessageBox.confirm({
              title: 'Confirm',
              message: 'Are you sure you wish to delete the participant named ' + participant.name + '?',
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

      function handleParticipantState() {
          var me = this;
          var s = $scope;
          if ($state.current.name === StateService.stateNames.participant.sevis) {
              $timeout(function () {
                  var participantId = $state.params.participantId;
                  console.assert(participantId, "The participant id must be defined in this state.");
                  scrollToParticipant(participantId,
                      function (element) {
                      },
                      function (element) {
                          $scope.toggleParticipantInfo(participantId);
                          $scope.onSevisTabSelected(participantId);
                      });
              });
          }
      }

      function scrollToParticipant(participantId, callbackBefore, callbackAfter) {
          var el = document.getElementById($scope.view.getParticipantTableRowDivId(participantId));
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 175,
              callbackBefore: callbackBefore,
              callbackAfter: callbackAfter
          }
          smoothScroll(el, options);
      }

      function getParticipantTableRowDivIdPrefix() {
          return 'participant';
      }

      $scope.view.getParticipantTableRowDivId = function (participantId) {
          return getParticipantTableRowDivIdPrefix() + participantId;
      }

      function deleteParticipant(participant, projectId) {
          $scope.participantInfo[participant.participantId] = $scope.participantInfo[participant.participantId] || {};
          $scope.participantInfo[participant.participantId].isDeleting = true;
          return ParticipantService.deleteParticipant(participant.participantId, projectId)
          .then(function (response) {
              $scope.participantInfo[participant.participantId].isDeleting = false;
              delete $scope.participantInfo[participant.participantId];
              NotificationService.showSuccessMessage("Successfully deleted the participant " + participant.name + '.');
              getPage();
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
              getPage();
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

      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          BrowserService.setDocumentTitleByProject(project, 'Participants');
          origNonUsParticipantsEst = $scope.$parent.project.nonUsParticipantsEst;
          origUsParticipantsEst = $scope.$parent.project.usParticipantsEst;
          origNonUsParticipantsActual = $scope.$parent.project.nonUsParticipantsActual;
          origUsParticipantsActual = $scope.$parent.project.usParticipantsActual;

      });

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

      function loadProjectPermissions() {
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
                  addDeleteAction();
                  $scope.permissions.editProject = true;
                  $log.info('User has edit project permission in project-participant.controller.js.');
              },
              notAuthorized: function () {
                  $scope.permissions.editProject = false;
                  $log.info('User not authorized to edit project in project-participant.controller.js.');
              }
          };
          config[ConstantsService.permission.editSevis.value] = {
              hasPermission: function () {
                  $scope.permissions.hasEditSevisPermission = true;
                  $log.info('User has edit sevis permission in project-participant.controller.js.');
              },
              notAuthorized: function () {
                  $scope.permissions.hasEditSevisPermission = false;
                  $log.info('User not authorized to edit sevis in project-participant.controller.js.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      };

      function loadApplicationPermissions() {
          console.assert(ConstantsService.resourceType.application.value, 'The constants service must have the application resource type value.');
          var resourceType = ConstantsService.resourceType.application.value;
          var config = {};
          config[ConstantsService.permission.sendToSevis.value] = {
              hasPermission: function () {
                  $scope.permissions.hasSendToSevisPermission = true;
                  $log.info('User has send to sevis permission in project-participant.controller.js.');
              },
              notAuthorized: function () {
                  $scope.permissions.hasSendToSevisPermission = false;
                  $log.info('User not authorized to send to sevis in project-participant.controller.js.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, kmtId, config)
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
          ParticipantService.getParticipantsByProject(projectId, { start: 0, limit: 1 })
            .then(function (data) {
                $scope.view.totalParticipants = data.total;
            })
            .catch(function () {
                $log.error('Unable to load project participants.');
                NotificationService.showErrorMessage('Unable to load project participants.');
            });

          ParticipantService.getParticipantsByProject(projectId, params)
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
                $scope.view.hasRealActualParticipants = data.total > 0;
                $scope.participantsLoading = false;
                handleParticipantState();
            })
            .catch(function (error) {
                $log.error('Unable to load project participants.');
                NotificationService.showErrorMessage('Unable to load project participants.');
            });
      };

      function loadSevisInfo(participantId) {
          return ParticipantPersonsSevisService.getParticipantPersonsSevisById(projectId, participantId)
          .then(function (data) {
              $scope.onParticipantUpdated(data.data);
              $scope.sevisInfo[participantId] = data.data;
              $scope.sevisInfo[participantId].show = true;
          })
          .catch(function (error) {
              if (error.status === 404) {
                  $scope.sevisInfo[participantId] = {};
                  $scope.sevisInfo[participantId].show = true;
              } else {
                  $log.error('Unable to load participant SEVIS info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant SEVIS info for ' + participantId + '.');
              }
          });
      };

      function addSendToSevisAction() {
          if ($scope.permissions.hasEditSevisPermission && $scope.permissions.hasSendToSevisPermission) {
              $scope.actions["Send To SEVIS"] = 1;
          }
      }

      function addDeleteAction() {
          $scope.actions["Delete"] = 2;
      }

      function loadExchangeVisitorInfo(participantId) {
          return ParticipantExchangeVisitorService.getParticipantExchangeVisitorById(projectId, participantId)
          .then(function (data) {
              $scope.exchangeVisitorInfo[participantId] = data.data;
          })
          .catch(function (error) {
              if (error.status === 404) {
                  $scope.exchangeVisitorInfo[participantId] = {};
              } else {
                  $log.error('Unable to load participant exchange visitor info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant exchange visitor info for ' + participantId + '.');
              }
          });
      };

      $scope.onInfoTabSelected = function (participantId) {
          $scope.view.tabInfo = true;
          $scope.view.tabSevis = false;
      }

      function saveSevisInfoById(participantId) {
          var sevisInfo = $scope.sevisInfo[participantId];
          return ParticipantPersonsSevisService.updateParticipantPersonsSevis(projectId, sevisInfo)
          .then(function (data) {
              NotificationService.showSuccessMessage('Participant SEVIS info saved successfully.');
              return loadSevisInfo(participantId);
          })
          .catch(function (error) {
              $log.error('Unable to save participant SEVIS info for participantId: ' + participantId);
              NotificationService.showErrorMessage('Unable to save participant SEVIS info for participant: ' + participantId + '.');
          });
      };

      $scope.saveSevisInfo = function (participantId) {
          saveSevisInfoById(participantId);
      };

      function saveExchangeVisitorById(participantId) {
          var exchangeVisitorInfo = $scope.exchangeVisitorInfo[participantId];
          var sevisInfo = $scope.sevisInfo[participantId];
          return ParticipantExchangeVisitorService.updateParticipantExchangeVisitor(projectId, exchangeVisitorInfo)
          .then(function (data) {
              NotificationService.showSuccessMessage('Participant exchange visitor info saved successfully.');
              $scope.exchangeVisitorInfo[participantId] = data.data;
              return loadSevisInfo(participantId);
          })
          .catch(function (error) {
              $log.error('Unable to save participant exchange visitor info for participantId: ' + participantId);
              NotificationService.showErrorMessage('Unable to save participant exchange visitor info for participant: ' + participantId + '.');
          });
      };

      $scope.saveExchangeVisitorInfo = function (participantId) {
          saveExchangeVisitorById(participantId);
      };

      $scope.onSevisTabSelected = function (participantId) {
          $scope.view.tabSevis = true;
          $scope.view.tabInfo = false;
          loadSevisInfo(participantId);
          loadExchangeVisitorInfo(participantId);
      };

      $scope.toggleParticipantInfo = function (participantId) {
          var defaultParticipantInfo = { show: false };
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
              size: 'lg',
              resolve: {
              }
          });

          modalInstance.result.then(function (participant) {
              if (participant) {
                  getPage();
              }
          });
      };

      $scope.selectedActionChanged = function () {
          if ($scope.selectedAction === 1) {
              paginationOptions.filter = { property: 'sevisStatusId', comparison: 'eq', value: ConstantsService.sevisCommStatus.readyToSubmit.id };
              $scope.gridApi.selection.setMultiSelect(true);
          } else {
              $scope.gridApi.selection.setMultiSelect(false);
              paginationOptions.filter = null;
          }

          getPage();
      }

      $scope.selectedGridViewChanged = function () {
          if ($scope.selectedGridView === 'Sevis') {
              $scope.gridOptions.columnDefs = sevisColumnDefs;
          } else {
              $scope.gridOptions.columnDefs = defaultColumnDefs;
          }
          getPage();
      }

      $scope.getSelectedParticipants = function () {
          return $scope.gridApi.selection.getSelectedRows();
      }

      $scope.getSelectedParticipant = function () {
          return $scope.getSelectedParticipants()[0];
      }

      $scope.onParticipantUpdated = function (updatedParticipant) {
          var participantIds = $scope.gridOptions.data.map(function (p) { return p.participantId; });
          var index = participantIds.indexOf(parseInt(updatedParticipant.participantId, 10));
          if (index != -1) {
              var participantToUpdate = $scope.gridOptions.data[index];
              if (updatedParticipant.participantType) {
                  participantToUpdate.participantType = updatedParticipant.participantType;
              }
              if (updatedParticipant.participantTypeId) {
                  participantToUpdate.participantTypeId = updatedParticipant.participantTypeId;
              }
              if (updatedParticipant.participantStatus) {
                  participantToUpdate.participantStatus = updatedParticipant.participantStatus;
              }
              if (updatedParticipant.participantStatusId) {
                  participantToUpdate.participantStatusId = updatedParticipant.participantStatusId;
              }
              if (updatedParticipant.sevisStatus) {
                  participantToUpdate.sevisStatus = updatedParticipant.sevisStatus;
              }
              if (updatedParticipant.participantStatusId) {
                  participantToUpdate.sevisStatusId = updatedParticipant.sevisStatusId;
              }
          }
      }

      $scope.applyAction = function () {
          if ($scope.selectedAction === 1) {
              return AuthService.getUserInfo()
              .then(function (response) {
                  var userInfo = response.data;
                  var doSendParticipantsToSevis = function (sevisUserAccount) {
                      var selectedParticipants = $scope.getSelectedParticipants();
                      return sendParticipantsToSevis(selectedParticipants, sevisUserAccount.username, sevisUserAccount.orgId);
                  }
                  var sevisUserAccounts = userInfo.sevisUserAccounts;
                  var sevisUserAccount = sevisUserAccounts[0];
                  if (sevisUserAccounts.length > 1) {
                      promptUserForSevisUserAccount(userInfo, doSendParticipantsToSevis, function () { });
                  }
                  else {
                      doSendParticipantsToSevis(sevisUserAccount)
                  }
              })
              .catch(function (response) {
                  var message = "Unable to determine user info for the current user."
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              })
          } else if ($scope.selectedAction === 2) {
              var selectedParticipant = $scope.getSelectedParticipant();
              showDeleteConfirm(selectedParticipant);
          }
      }

      function sendParticipantsToSevis(participants, sevisUsername, sevisOrgId) {
          var participantIds = $scope.getSelectedParticipants().map(function (obj) { return obj.participantId });
          return ParticipantPersonsSevisService.sendToSevis(kmtId, projectId, participantIds, sevisUsername, sevisOrgId)
          .then(function (results) {
              NotificationService.showSuccessMessage("Successfully queued " + results.data.length + " of " + participantIds.length + " participants.");
              getPage();
          }, function () {
              NotificationService.showErrorMessage("Failed to queue participants.");
          });
      }

      function promptUserForSevisUserAccount(userInfo, okCallback, cancelCallback) {
          var modalInstance = $modal.open({
              templateUrl: '/app/projects/select-sevis-account-modal.html',
              controller: 'SelectSevisAccountCtrl',
              backdrop: 'static',
              resolve: {
                  userInfo: function () {
                      return userInfo;
                  }
              },
          });
          modalInstance.result.then(function (selectedAccount) {
              console.assert(selectedAccount, "The selected sevis account must be defined.");
              $log.info('Closing...');
              okCallback(selectedAccount);
          }, function () {
              $log.info('Dismiss select sevis account dialog...');
              cancelCallback();
          })
          .then(function () {
          });
      }

      $scope.view.isLoading = true;
      $q.all([loadProjectPermissions(), loadApplicationPermissions(), loadCollaboratorDetails()])
      .then(function (results) {
          addSendToSevisAction();
      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });

      $scope.showSevisTab = function (participantTypeId) {
          return (!($scope.project.visitorTypeId == ConstantsService.visitorType.notApplicable.id)
               && !(participantTypeId == ConstantsService.participantType.foreignNonTravelingParticipant.id ||
                   participantTypeId == ConstantsService.participantType.uSNonTravelingParticipant.id ||
                   participantTypeId == ConstantsService.participantType.uSTravelingParticipant.id ||
                   participantTypeId == ConstantsService.participantType.organizationalParticipant.id ||
                   participantTypeId == ConstantsService.participantType.otherOrganization.id));
      };

      $scope.visitorTypeExchangeVisitor = function (participantTypeId) {
          return ($scope.project.visitorTypeId == ConstantsService.visitorType.exchangeVisitor.id)
              && !(participantTypeId == ConstantsService.participantType.foreignNonTravelingParticipant.id ||
                   participantTypeId == ConstantsService.participantType.uSNonTravelingParticipant.id ||
                   participantTypeId == ConstantsService.participantType.uSTravelingParticipant.id ||
                   participantTypeId == ConstantsService.participantType.organizationalParticipant.id ||
                   participantTypeId == ConstantsService.participantType.otherOrganization.id);
      };

      var paginationOptions = {
          pageNumber: 1,
          pageSize: 25,
          sort: null,
          keyword: null,
          filter: null
      };

      var defaultColumnDefs = [
           { name: 'name', cellTemplate: '<a href="{{row.entity.href}}">{{row.entity.name}}</a>' },
           { name: 'participantType' },
           { name: 'participantStatus' },
           { name: 'sevisStatus' }
      ];
      var sevisColumnDefs = [
           { name: 'fullName', displayName: 'Name', cellTemplate: '<a href="{{row.entity.href}}">{{row.entity.fullName}}</a>' },
           { name: 'sevisStatus'},
           { name: 'sevisId'},
           { name: 'isCreatedViaBatch', displayName: 'Created via Batch', cellTemplate: '<input type="checkbox" ng-model="row.entity.isCreatedViaBatch" ng-disabled="true">'},
           { name: 'isSentToSevisViaRTI', displayName: 'Sent via RTI', cellTemplate: '<input type="checkbox" ng-model="row.entity.isSentToSevisViaRTI" ng-disabled="true">'},
           { name: 'isValidatedViaBatch', displayName: 'Validated via Batch', cellTemplate: '<input type="checkbox" ng-model="row.entity.isValidatedViaBatch" ng-disabled="true">'},
           { name: 'isValidatedViaRTI', displayName: 'Validated via RTI', cellTemplate: '<input type="checkbox" ng-model="row.entity.isValidatedViaRTI" ng-disabled="true">'},
           { name: 'isCancelled', displayName: 'Cancelled', cellTemplate: '<input type="checkbox" ng-model="row.entity.isCancelled" ng-disabled="true">'},
           { name: 'isDS2019Printed', displayName: 'Printed', cellTemplate: '<input type="checkbox" ng-model="row.entity.isDS2019Printed" ng-disabled="true">'}
      ];

      $scope.gridOptions = {
          paginationPageSizes: [25, 50, 75],
          paginationPageSize: 25,
          useExternalPagination: true,
          multiSelect: false,
          columnDefs: defaultColumnDefs,
          onRegisterApi: function (gridApi) {
              $scope.gridApi = gridApi;
              $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                  if (sortColumns.length == 0) {
                      paginationOptions.sort = null;
                  } else {
                      paginationOptions.sort = { property: sortColumns[0].name, direction: sortColumns[0].sort.direction };
                      $scope.gridOptions.paginationCurrentPage = 1;
                  }
                  getPage();
              });
              gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                  paginationOptions.pageNumber = newPage;
                  paginationOptions.pageSize = pageSize;
                  getPage();
              });
              gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                  if ($scope.view.tabSevis && $scope.getSelectedParticipants().length === 1) {
                      var participantId = row.entity.participantId;
                      loadSevisInfo(participantId);
                      loadExchangeVisitorInfo(participantId);
                  }
              });
          }
      };

      $scope.$watch('participantFilter', function (participantFilter) {
          if (participantFilter && participantFilter.length > 0) {
              paginationOptions.keyword = participantFilter;
          } else {
              paginationOptions.keyword = null;
          }
          getPage();
      });

      function getPage() {
          var params = {
              start: (paginationOptions.pageNumber - 1) * paginationOptions.pageSize,
              limit: ((paginationOptions.pageNumber - 1) * paginationOptions.pageSize) + paginationOptions.pageSize,
              sort: paginationOptions.sort,
              keyword: paginationOptions.keyword,
              filter: paginationOptions.filter
          };
          var promise;
          if ($scope.selectedGridView === 'Sevis') {
              promise = ParticipantPersonsSevisService.getSevisParticipantsByProjectId(projectId, params);
          } else {
              promise = ParticipantService.getParticipantsByProject(projectId, params);
          }
          promise.then(function (response) {
                   var data = response.data || response;
                   $scope.gridOptions.totalItems = data.total;
                   $scope.view.totalParticipants = data.total;
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
                   $scope.gridOptions.data = data.results;
                   $scope.view.hasRealActualParticipants = data.total > 0;
                   handleParticipantState();
               })
               .catch(function (error) {
                   $log.error('Unable to load project participants.');
                   NotificationService.showErrorMessage('Unable to load project participants.');
               });
      }

      getPage();
  });
