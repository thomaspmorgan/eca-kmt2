'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectCtrl', function ($scope, $state, $stateParams, $log, $q, ProjectService, PersonService,
      ProgramService, ParticipantService, LocationService, MoneyFlowService, AuthService,OrganizationService,
      TableService, ConstantsService, LookupService, orderByFilter) {

      $scope.project = {};

      $scope.newParticipant = {};

      $scope.modalForm = {};

      $scope.showConfirmCopy = false;

      $scope.genders = {};
      $scope.currencyTypes = {};

      /* Money Flow */

      $scope.currentProjectId = $stateParams.projectId;

      $scope.moneyFlows = [];
      $scope.moneyFlowTypes = [];
      $scope.moneyFlowStati = [];
      $scope.moneyFlowSourceTypes = [];
      $scope.moneyFlowFromTo = [];

      $scope.currentMoneyFlowId = -1;

      $scope.moneyFlowEditColumnClass = "col-md-2";
      $scope.sourceRecipientFreeText = false;
      $scope.moneyFlowConfirmMessage = "saved";
      
      $scope.showFromToSelectControl = false;
      
      $scope.editingMoneyFlows = [];
      $scope.dateFormat = 'dd-MMMM-yyyy';

// initialize new money flow record to be used for creating and editing
      $scope.draftMoneyFlow = {
          description: '',
          moneyFlowTypeId: null,
          moneyFlowStatusId: null,
          value: 0,
          transactionDate: new Date(),
          fiscalYear: new Date().getFullYear(),
          sourceProjectId: null,
          recipientProjectId: null,
          sourceRecipientId: null,
          sourceTypeId: 0,
          recipientTypeId: 0,
          sourceRecipientTypeId: null
      };

      /* END money flows*/
      $scope.currentlyEditing = false;

      $scope.isProjectEditCancelButtonVisible = false;
      $scope.showProjectEditCancelButton = false;
      $scope.isProjectStatusButtonInEditMode = false;
      $scope.isInEditViewState = false;
      $scope.projectStatusButtonText = "...";
      $scope.isTransactionDatePickerOpen = false;

      $scope.tabs = {
          overview: {title: 'Overview', path: 'overview', active: true, order: 1 },
          partners: {title: 'Partners', path: 'partners', active: false,order: 3 },
          participants: {title: 'Participants',path: 'participants',active: true,order: 2 },
          artifacts: {title: 'Attachments',path: 'artifacts',active: true,order: 4 },
          moneyflows: {title: 'Funding',path: 'moneyflows',active: true,order: 5},
          impact: {title: 'Impact',path: 'impact',active: false,order: 6},
          activity: {title: 'Timeline',path: 'activity',active: true,order: 7},
          itinerary: {title: 'Travel',path: 'itineraries',active: true,order: 8}
      };

      function enabledProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = true;
      }

      function disableProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = false;
      }

      ProjectService.get($stateParams.projectId)
        .then(function (data) {
            $scope.project = data.data;
            if (angular.isArray($scope.project.participants)) {
                $scope.tabs.participants.active = true;
            }
            if (angular.isArray($scope.project.agreements)) {
                $scope.tabs.partners.active = true;
            }
            if (angular.isArray($scope.project.moneyFlows)) {
                $scope.tabs.moneyflows.active = true;
            }
        });

      var editStateName = 'projects.edit';

      $scope.showProjectEditCancelButton = function() {
          $scope.isProjectEditCancelButtonVisible = true;
      }

      $scope.hideProjectEditCancelButton = function() {
          $scope.isProjectEditCancelButtonVisible = false;
      }
      $scope.isInEditViewState = $state.current.name === editStateName;
      if ($scope.isInEditViewState) {
          $scope.showProjectEditCancelButton();
      }
      else {
          $scope.hideProjectEditCancelButton();
      }
      $scope.onProjectStatusButtonClick = function ($event) {
          if ($state.current.name === editStateName) {
              $scope.$broadcast(ConstantsService.saveProjectEventName);
          }
          else {              
              $state.go(editStateName);
              $scope.showProjectEditCancelButton();
          }
      };

      $scope.onCancelButtonClick = function ($event) {
          $scope.$broadcast(ConstantsService.cancelProjectEventName);
      }

      $scope.params = $stateParams;

      ProgramService.get($stateParams.programId)
        .then(function (data) {
            $scope.program = data;
        });

      $scope.updateProject = function () {
          saveProject();
      };

      $scope.addTab = function () {
          if ($scope.tabs.participants.active && !$scope.project.participants) {
              $scope.project.participants = [];
          }
          if ($scope.tabs.partners.active && !$scope.project.agreements) {
              $scope.project.agreements = [];
          }
          if ($scope.tabs.artifacts.active && !$scope.project.artifactReferences) {
              $scope.project.artifactReferences = [];
          }
          if ($scope.tabs.moneyflows.active && !$scope.project.moneyFlows) {
              $scope.project.moneyFlows = [];
          }
          if ($scope.tabs.impact.active && !$scope.project.impacts) {
              $scope.project.impacts = [];
          }
          console.log($scope.tabs);
          saveProject();
      }

      LookupService.getAllGenders({ limit: 300 })
        .then(function (data) {
            $scope.genders = data.results;
        });

      LookupService.getAllMoneyFlowStati({ limit: 300 })
        .then(function (data) {
            $scope.moneyFlowStati = data.results;
        });

      LookupService.getAllMoneyFlowTypes({ limit: 300 })
        .then(function (data) {
            $scope.moneyFlowTypes = data.results;
        });

      LookupService.getAllMoneyFlowSourceRecipientTypes({ limit: 300 })
      .then(function (data) {
          $scope.moneyFlowSourceTypes = data.results;
      });

      LocationService.get({ limit: 300, filter: {property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id}})
        .then(function (data) {
            $scope.countries = data.results;
        });

      $scope.birthCountrySelected = function (data) {
          LocationService.get({
              limit: 300,
              filter: [{ property: 'countryId', comparison: 'eq', value: data.id },
                       { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id}]
          }).then(function (data) {
                $scope.cities = data.results;
            });
      }

      $scope.addParticipant = function () {

          setupNewParticipant();

          console.log($scope.newParticipant);

          PersonService.create($scope.newParticipant)
            .then(function () {
                $scope.participantsLoading = true;
                var params = {
                    start: TableService.getStart(),
                    limit: TableService.getLimit(),
                };
                ParticipantService.getParticipantsByProject($stateParams.projectId, params)
                    .then(function (data) {
                        $scope.project.participants = data.results;
                        $scope.participantsLoading = false;
                    });
                displaySuccess();
            }, function (error) {
                if (error.status == 400) {
                    displayError(error.data);
                }
            });
          $scope.modalClose();
      };
      
      function setupNewParticipant() {
          delete $scope.newParticipant.countryOfBirth;
          $scope.newParticipant.projectId = $scope.project.id;
          $scope.newParticipant.gender = $scope.newParticipant.gender[0].id;
          $scope.newParticipant.cityOfBirth = $scope.newParticipant.cityOfBirth[0].id;
          $scope.newParticipant.countriesOfCitizenship =
               $scope.newParticipant.countriesOfCitizenship.map(function (obj) {
                   return obj.id;
               });
      };

      function displaySuccess() {
          $scope.modal.addParticipantResult = true;
          $scope.result = {};
          $scope.result.title = "Person Created";
          $scope.result.subtitle = "The person was created successfully!";
      };

      function displayError(error) {
          $scope.modal.addParticipantResult = true;
          $scope.result = {};
          $scope.result.title = "Error Creating Person";
          $scope.result.subtitle = "There was an error creating the new person.";
          $scope.result.error = error;
      };

      $scope.saveProject = function () {
          var project = {
              id: Date.now().toString(),
              name: $scope.newProject.title,
              description: $scope.newProject.description,
              branch: $scope.newProject.branch[0].name,
              parentProgram: {
                  name: $scope.program.name,
                  id: $scope.program.id
              },
              parentOffice: {
                  name: $scope.program.owner.longName,
                  id: $scope.program.owner.organizationId
              }
          };

          ProjectService.create(project)
              .then(function (project) {
                  console.log($scope.program.id);
                  $state.go('projects.overview', { officeId: $scope.program.owner.organizationId, projectId: project.id, programId: $scope.program.id });
              });

          if (!$scope.program.projectReferences) {
              $scope.program.projectReferences = [];
          }
          $scope.program.projectReferences.push({ name: project.name, id: project.id });
          saveProgram();
      };


      function saveProject() {
          ProjectService.update($scope.project, $stateParams.projectId)
            .then(function (project) {
                $scope.project = project;
            });
      }

      $scope.modalClose = function () {
          $scope.modal.addParticipant = false;
      };

      $scope.modalClear = function () {
          $scope.modal.addParticipant = false;

          angular.forEach($scope.newParticipant, function (value, key) {
              $scope.newParticipant[key] = '';
          });

          angular.forEach($scope.genders, function (value, key) {
              if ($scope.genders[key].ticked === undefined) {
                  $scope.genders[key].ticked = false;
              } else {
                  delete $scope.genders[key].ticked;
              }
          });
          angular.forEach($scope.countries, function (value, key) {
              if ($scope.countries[key].ticked === undefined) {
                  $scope.countries[key].ticked = false;
              } else {
                  delete $scope.countries[key].ticked;
              }
          });
          $scope.cities = [];
      };
      
      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var projectId = $stateParams.projectId;
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.editProject.value] = {
              hasPermission: function () {
                  enabledProjectStatusButton();
                  $log.info('User has edit project permission in project.js controller.');
              },
              notAuthorized: function () {
                  disableProjectStatusButton();
                  $log.info('User not authorized to edit project  in project.js controller.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      }

      $scope.moneyFlowsLoading = false;

      /* money flow manipulation */
      $scope.getMoneyFlows = function (tableState) {

          $scope.moneyFlowsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          MoneyFlowService.getMoneyFlowsByProject($stateParams.projectId, params)
             .then(function (data) {
                 $scope.moneyFlows = data.results;
                 var limit = TableService.getLimit();
                 tableState.pagination.numberOfPages = Math.ceil(data.total / limit);

                 angular.forEach($scope.moneyFlows, function (value) {
                     $scope.editingMoneyFlows[value.id] = false;
                 });

                 $scope.moneyFlowsLoading = false;
             });
      };
      
      $scope.createMoneyFlowForm = function () {
          $scope.moneyFlowDirection = "incoming";
          $scope.incomingSelected = "directionSelected";
          $scope.outgoingSelected = "";
          $scope.sourceRecipientLabel = "Source";

          $scope.fiscalYears = [];
          for (var i = new Date().getFullYear() ; i >= 2010 ; i--) {
              $scope.fiscalYears.push({ year: i });
          }

          $scope.showCreateMoneyFlow = true;
      };

      $scope.editMoneyFlow = function (moneyFlowId) {
          $scope.moneyFlowEditColumnClass = "col-md-1 editButtons";
          $scope.editingMoneyFlows[moneyFlowId] = true;

          MoneyFlowService.get(moneyFlowId)
            .then(function (data) {
                $scope.draftMoneyFlow = data;
                $scope.changeSourceRecipientType();
                $scope.currentlyEditing = true;
            });
      };
      
      $scope.copyMoneyFlow = function (moneyFlowId) {
          // show dialog then create new money flow based on existing
          $scope.currentMoneyFlowId = moneyFlowId;
          $scope.confirmCopy = true;
      };

      $scope.deleteMoneyFlow = function (moneyFlowId) {
          // show confirmation dialog then delete
          $scope.currentMoneyFlowId = moneyFlowId;
          $scope.confirmDelete = true;

      };

      $scope.changeMoneyFlowDirection = function (direction) {
          $scope.moneyFlowDirection = direction;
          $scope.incomingSelected = direction == 'incoming' ? 'directionSelected' : '';
          $scope.outgoingSelected = direction == 'outgoing' ? 'directionSelected' : '';

          $scope.sourceRecipientLabel = direction == 'incoming' ? 'Source' : 'Recipient';
      };

      // DIALOG BOX FUNCTIONS
      $scope.confirmCopyYes = function () {
          executeCopyMoneyFlow();
          $scope.confirmCopy = false;
          $scope.moneyFlowConfirmMessage = "copied";
          $scope.confirmSuccess = true;

      }

      $scope.confirmDeleteYes = function () {
          executeDeleteMoneyFlow();
          $scope.confirmDelete = false;
          $scope.moneyFlowConfirmMessage = "deleted";
          $scope.confirmSuccess = true;

      }

      $scope.closeConfirm = function () {
          $scope.confirmSuccess = false;
      }
      
      $scope.saveMoneyFlow = function (moneyFlowId) {
          $("#transactionDate" + moneyFlowId).css("opacity", "0");
          $("#changesSaved" + moneyFlowId).css("display", "inline");

          $scope.editingMoneyFlows[moneyFlowId] = false;
          $scope.currentlyEditing = false;

          // animate the 'saved' label
          $("#changesSaved"+moneyFlowId).animate({
              opacity: 0,
          }, 3000, function () {
              $("#transactionDate" + moneyFlowId).css("opacity", "100");
              $("#changesSaved" + moneyFlowId).css({"display": "none", "opacity" : "100"});
          });
      };

      // calendar popup for startDate
      $scope.transactionCalendarOpen = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();

          $scope.transactionCalendarOpened = true;

      };

      $scope.saveCreatedMoneyFlow = function () {
          $scope.setSourceRecipient();

          MoneyFlowService.create($scope.draftMoneyFlow)
            .then(function (moneyFlow) {
                if (Array.isArray(moneyFlow)) {
                    $scope.errorMessage = "There were one or more errors:";
                    $scope.validations = moneyFlow;
                    $scope.confirmFail = true;
                }
                else if (moneyFlow.hasOwnProperty('Message')) {
                    $scope.errorMessage = moneyFlow.Message;
                    $scope.validations = moneyFlow.ValidationErrors;
                    $scope.confirmFail = true;
                }
                else if (moneyFlow.hasOwnProperty('ErrorMessage')) {
                    $scope.errorMessage = moneyFlow.ErrorMessage;
                    $scope.validations.push(moneyFlow.Property);
                    $scope.validations.confirmFail = true;
                }
                else if (Array.isArray(moneyFlow)) {
                    $scope.errorMessage = "There were one or more errors:";
                    $scope.validations = moneyFlow;
                    $scope.validations.confirmFail = true;
                }
                else {
                    $scope.draftMoneyFlow = moneyFlow; //perhaps not, this is to get the id
                    $scope.confirmSuccess = true;
                    $scope.modalClear();
                }
            })
              .then(function () {
                $scope.isFormBusy = false;
            })

              .catch(function () {
                  alert('Unable to save this funding item.');
              });
      };

      $scope.setSourceRecipient = function () {

          $scope.draftMoneyFlow.fiscalYear = $scope.draftMoneyFlow.moneyFlowFiscalYear.year;
          // using the direction and the selected org/project/participant, set the source or recipient
          if ($scope.moneyFlowDirection == 'incoming')
          {
              // set the source
              $scope.draftMoneyFlow.sourceTypeid = $scope.draftMoneyFlow.sourceRecipientTypeId;
              $scope.draftMoneyFlow.recipientProjectId = $scope.currentProjectId;
              $scope.draftMoneyFlow.recipientTypeId = 3;
              $scope.draftMoneyFlow.moneyFlowTypeId = 1;

              switch($scope.draftMoneyFlow.sourceRecipientTypeId)
              {
                  case 1:
                      $scope.draftMoneyFlow.sourceOrganizationId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;
                  case 2:
                      $scope.draftMoneyFlow.sourceProgramId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;                  
                  case 3:
                      $scope.draftMoneyFlow.sourceProjectId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;                  
                  case 4:
                      $scope.draftMoneyFlow.sourceParticipantId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;
                  case 5:
                      $scope.draftMoneyFlow.sourceItineraryStopId = $scope.draftMoneyFlow.sourceRecipientId;          
                      break;
                  default:
                      break;
              }
          }
          else
          {
              // set the recipient
              $scope.draftMoneyFlow.recipientTypeid = $scope.draftMoneyFlow.sourceRecipientTypeId;
              $scope.draftMoneyFlow.sourceProjectId = $scope.currentProjectId;
              $scope.draftMoneyFlow.sourceTypeId = 3;
              $scope.draftMoneyFlow.moneyFlowTypeId = 2;

              switch($scope.draftMoneyFlow.sourceRecipientTypeId)
              {
                  case 1:
                      $scope.draftMoneyFlow.recipientOrganizationId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;
                  case 2:
                      $scope.draftMoneyFlow.recipientProgramId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;                  
                  case 3:
                      $scope.draftMoneyFlow.recipientProjectId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;                  
                  case 4:
                      $scope.draftMoneyFlow.recipientParticipantId = $scope.draftMoneyFlow.sourceRecipientId;
                      break;
                  case 5:
                      $scope.draftMoneyFlow.recipientItineraryStopId = $scope.draftMoneyFlow.sourceRecipientId;          
                      break;
                  default:
                      break;
              }
          }
      };

      function executeDeleteMoneyFlow() {
          MoneyFlowService.deleteMoneyFlow
      };

      function executeCopyMoneyFlow() {

          MoneyFlowService.copy($scope.currentMoneyFlowId)
            .then(function (moneyFlow) {
                if (Array.isArray(moneyFlow)) {
                    $scope.errorMessage = "There were one or more errors:";
                    $scope.validations = moneyFlow;
                    $scope.confirmFail = true;
                }
                else if (moneyFlow.hasOwnProperty('Message')) {
                    $scope.errorMessage = moneyFlow.Message;
                    $scope.validations = moneyFlow.ValidationErrors;
                    $scope.confirmFail = true;
                }
                else if (moneyFlow.hasOwnProperty('ErrorMessage')) {
                    $scope.errorMessage = moneyFlow.ErrorMessage;
                    $scope.validations.push(moneyFlow.Property);
                    $scope.validations.confirmFail = true;
                }
                else if (Array.isArray(moneyFlow)) {
                    $scope.errorMessage = "There were one or more errors:";
                    $scope.validations = moneyFlows;
                    $scope.validations.confirmFail = true;
                }
                else {
                    $scope.moneyFlow = moneyFlow; //perhaps not, this is to get the id
                    $scope.confirmSuccess = true;
                    $scope.modalClear();
                }
            });
      };

      $scope.changeSourceRecipientType = function () {
          // change the from/to box or keep free-text
          $scope.showFromToSelectControl = false;

          if ($scope.draftMoneyFlow.sourceTypeId <= 7) {
                  getFromToChoices();
                  $scope.showFromToSelectControl = true;
          }
      };
      
      function getFromToChoices() {

          $scope.moneyFlowFromTo = [];
          var lookupParams = { start: null, limit: null,sort: null,filter: null };

          switch ($scope.draftMoneyFlow.sourceRecipientTypeId)
          {
              case 1: // organization service
                  return OrganizationService.getOrganizations(lookupParams)
                      .then(function (data) {
                          $scope.moneyFlowFromTo = data.results;
                      });
                  break;
              case 2: // programs
                  return ProgramService.getAllProgramsAlpha(lookupParams)
                      .then(function (data) {
                          $scope.moneyFlowFromTo = data.results;
                      });
                  break;
              case 3:  //projects
                  
                  break;
              case 4: // participants
                  return ParticipantService.getParticipantsByProject($scope.project.id, lookupParams)
                    .then(function (data) {
                        $scope.moneyFlowFromTo = data.results;
                    });
                  break;
              case 5: // itenerary stops
                  // stub
                  break;
              case 6: // accommodation
                  // stub
                  break;
              case 7: // transportation
                  // stub
                  break;
          }
      };

      $scope.createModalCancel = function () {
          $scope.checkFormStatus();
      };

      $scope.confirmClose = function (closeModal) {
          $scope.showConfirmClose = false;

          if (closeModal)
          {
              $scope.showCreateMoneyFlow = false;
              $scope.modalClear();
          }
      };

      $scope.confirmCloseSuccess = function () {
          $scope.confirmSuccess = false;
          $scope.getMoneyFlows();

      };


      $scope.modalClear = function () {
          angular.forEach($scope.draftMoneyFlow, function (value, key) {
              $scope.draftMoneyFlow[key] = ''
          });

          $scope.modalForm.moneyFlowForm.$setPristine();
          $scope.showCreateMoneyFlow = false;
      };

      $scope.checkFormStatus = function () {
          if ($scope.modalForm.moneyFlowForm.$dirty) {
              $scope.showConfirmClose = true;
          }
          else {
              $scope.showCreateMoneyFlow = false;
          }
      };


      $scope.cancelMoneyFlowEdit = function (moneyFlowId) {
          $scope.editingMoneyFlows[moneyFlowId] = false;
          $scope.currentlyEditing = false;
      };

      $scope.confirmCancel = function () {
          // simply close the confirmation modal dialog
          $scope.confirmCopy = false;
          $scope.confirmDelete = false;
      };

      $scope.openTransactionDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.isTransactionDatePickerOpen = true;
      };

      $q.all([loadPermissions()])
      .then(function () {

      }, function () {

      });
  });
