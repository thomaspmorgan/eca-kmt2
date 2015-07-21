angular.module('staticApp')
  .controller('ProjectMoneyFlowsCtrl',
  function ($scope, $stateParams, $q, ProjectService,
      MoneyFlowService, ConstantsService, TableService, LookupService) {

      $scope.modalForm = {};
      $scope.currencyTypes = {};
      $scope.moneyFlowCreationType = "Create";
      $scope.moneyFlows = [];
      $scope.moneyFlowTypes = [];
      $scope.moneyFlowStati = [];
      $scope.moneyFlowSourceTypes = [];
      $scope.moneyFlowFromTo = [];

      $scope.currentMoneyFlowId = -1;

      $scope.moneyFlowEditColumnClass = "col-md-2";
      $scope.moneyFlowConfirmMessage = "saved";

      $scope.showFromToSelectControl = false;

      $scope.showFullMoneyFlowDescription = [];
      $scope.editingMoneyFlows = [];
      $scope.dateFormat = 'dd-MMMM-yyyy';

      $scope.fiscalYears = [];

      for (var i = new Date().getFullYear() ; i >= 2010 ; i--) {
          $scope.fiscalYears.push({ year: i, name: i });
      }
      $scope.listCount = {
          start: 0,
          total: 0
      }

      $scope.showConfirmClose = false;
      $scope.confirmCopy = false;
      $scope.confirmDelete = false;
      $scope.confirmSuccess = false;

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

      LookupService.getAllMoneyFlowStati({
          limit: 300 })
      .then(function (data) {
      $scope.moneyFlowStati = data.results;
        });

      LookupService.getAllMoneyFlowTypes({
      limit: 300 })
        .then(function (data) {
            $scope.moneyFlowTypes = data.results;
        });

      LookupService.getAllMoneyFlowSourceRecipientTypes({
        limit: 300 })
        .then(function (data) {
          $scope.moneyFlowSourceTypes = data.results;
          });

      $scope.moneyFlowsLoading = false;

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
                     $scope.showFullMoneyFlowDescription[value.id] = false;
                 });

                 $scope.moneyFlowsLoading = false;
             });
      };

      $scope.createMoneyFlowForm = function () {
          $scope.moneyFlowDirection = "incoming";
          $scope.incomingSelected = "directionSelected";
          $scope.outgoingSelected = "";
          $scope.sourceRecipientLabel = "Source";

          $scope.showCreateMoneyFlow = true;
      };

      $scope.editMoneyFlow = function (moneyFlowId) {

            $scope.moneyFlowEditColumnClass = "col-md-1 editButtons";
            $scope.editingMoneyFlows[moneyFlowId] = true;

            MoneyFlowService.get(moneyFlowId)
            .then(function (data) {
                $scope.draftMoneyFlow = data;
                //$scope.changeSourceRecipientType();
                $scope.currentlyEditing = true;
            });
        };
      

      $scope.copyMoneyFlow = function (moneyFlowId) {
          // take selected money flow; create object and put in new money flow dialog
          $scope.currentMoneyFlowId = moneyFlowId;
          $scope.moneyFlowCreationType = "Copy";

          MoneyFlowService.get(moneyFlowId)
            .then(function (data) {

                $scope.draftMoneyFlow = data;
                $scope.changeSourceRecipientType();
                $scope.currentlyEditing = true;

                $scope.showCreateMoneyFlow = true;
            });
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
          $("#changesSaved" + moneyFlowId).animate({
              opacity: 0,
          }, 3000, function () {
              $("#transactionDate" + moneyFlowId).css("opacity", "100");
              $("#changesSaved" + moneyFlowId).css({ "display": "none", "opacity": "100" });
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
                    $scope.modalClearMoneyFlow();
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
          if ($scope.moneyFlowDirection == 'incoming') {
              // set the source
              $scope.draftMoneyFlow.sourceTypeid = $scope.draftMoneyFlow.sourceRecipientTypeId;
              $scope.draftMoneyFlow.recipientProjectId = $scope.currentProjectId;
              $scope.draftMoneyFlow.recipientTypeId = 3;
              $scope.draftMoneyFlow.moneyFlowTypeId = 1;

              switch ($scope.draftMoneyFlow.sourceRecipientTypeId) {
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
          else {
              // set the recipient
              $scope.draftMoneyFlow.recipientTypeid = $scope.draftMoneyFlow.sourceRecipientTypeId;
              $scope.draftMoneyFlow.sourceProjectId = $scope.currentProjectId;
              $scope.draftMoneyFlow.sourceTypeId = 3;
              $scope.draftMoneyFlow.moneyFlowTypeId = 2;

              switch ($scope.draftMoneyFlow.sourceRecipientTypeId) {
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
          //MoneyFlowService.deleteMoneyFlow();
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
                    $scope.modalClearMoneyFlow();
                }
            });
      };

      $scope.expandMoneyFlowDescription = function (moneyFlowId, showFullDescription) {

          $scope.showFullMoneyFlowDescription[moneyFlowId] = showFullDescription;
          $('#expand' + moneyFlowId).toggle();
          $('#contract' + moneyFlowId).toggle();

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
          var lookupParams = { start: null, limit: 300, sort: null, filter: null };

          switch ($scope.draftMoneyFlow.sourceRecipientTypeId) {
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

          if (closeModal) {
              $scope.showCreateMoneyFlow = false;
              $scope.modalClear();
          }
      };

      $scope.confirmCloseSuccess = function () {
          $scope.confirmSuccess = false;
          $scope.currentlyEditing = false;
          $scope.getMoneyFlows();

      };


      $scope.modalClearMoneyFlow = function () {
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
          $scope.confirmDelete = false;
      };

      $scope.openTransactionDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.isTransactionDatePickerOpen = true;
      };

  });