'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MoneyFlowsCtrl
 * @description The money flows controller is used to control the list of money flows.
 * # MoneyFlowsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('MoneyFlowCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modalInstance,
        entity,
        fiscalYears,
        FilterService,
        MoneyFlowService,
        LookupService,
        ConstantsService,
        OfficeService,
        ProjectService,
        ProgramService,
        OrganizationService,
        ParticipantService,
        NotificationService
        ) {

      console.assert(entity.entityName && entity.entityName.length > 0, "The entity.entityName value must be defined.");

      $scope.view = {};
      $scope.view.fiscalYears = fiscalYears;
      $scope.view.isLoadingSourceMoneyFlows = false;
      $scope.view.entityNameMaxLength = 100;
      $scope.view.entityName = entity.entityName;
      $scope.view.searchLimit = 10;
      $scope.view.params = $stateParams;
      $scope.view.allowedRecipientMoneyFlowSourceRecipientTypes = [];
      $scope.view.allowedSourceMoneyFlowSourceRecipientTypes = [];
      $scope.view.sourceMoneyFlows = [];
      $scope.view.selectedSourceMoneyFlow = null;
      $scope.view.moneyFlowStatii = [];
      $scope.view.moneyFlowTypes = [];
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isCopyingMoneyFlow = false;
      $scope.view.maxDescriptionLength = 255;
      $scope.view.maxAmount = ConstantsService.maxNumericValue;
      $scope.view.incomingDirectionKey = "incoming";
      $scope.view.outgoingDirectionKey = "outgoing";
      $scope.view.isLoadingSources = false;
      $scope.view.isSaving = false;
      $scope.view.isSourceRecipientFieldEnabled = false;
      $scope.view.isSourceRecipientFieldRequired = true;
      $scope.view.isSourceMoneyFlowAmountExpended = false;
      $scope.view.copiedMoneyFlowExceedsSourceLimit = false;

      $scope.view.openTransactionDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isTransactionDatePickerOpen = true;
      };

      $scope.view.onFiscalYearChange = function () {
          resetParentMoneyFlow();
      }

      $scope.view.save = function () {
          $scope.view.isSaving = true;
          return MoneyFlowService.create($scope.view.moneyFlow)
          .then(function (response) {
              NotificationService.showSuccessMessage("Successfully saved the new funding.");
              $scope.view.isSaving = false;
              $modalInstance.close($scope.view.moneyFlow);
          })
          .catch(function (response) {
              $scope.view.isSaving = false;
              var message = "Unable to save the funding item.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      };

      $scope.view.cancel = function () {
          $modalInstance.dismiss('cancel');
      };

      $scope.view.toggleIncomingOutgoing = function (directionKey) {
          if (directionKey === $scope.view.outgoingDirectionKey) {
              $scope.view.moneyFlow.isOutgoing = true;
          }
          else if (directionKey === $scope.view.incomingDirectionKey) {
              $scope.view.moneyFlow.isOutgoing = false;
              onModelChange($scope.view.moneyFlow);
          }
          else {
              $log.error('Direction key ]' + directionKey + '] is not recognized.');
          }
          resetParentMoneyFlow();
          loadSourceMoneyFlows($scope.view.moneyFlow);
      }

      function resetParentMoneyFlow() {
          $scope.view.moneyFlow.parentMoneyFlowId = null;
          $scope.view.selectedSourceMoneyFlow = null;
      }

      $scope.view.moneyFlow = toMoneyFlow(entity);
      onModelChange($scope.view.moneyFlow);

      $scope.view.validateSourceRemainingAmount = function ($value) {
          if ($value
              && $scope.view.selectedSourceMoneyFlow
              && $scope.view.selectedSourceMoneyFlow !== null
              && $scope.view.selectedSourceMoneyFlow.remainingAmount - $value < 0) {
              return false;
          }
          else {
              return true;
          }
      }

      $scope.view.getPeers = function ($viewValue) {
          var peerEntityTypeId = $scope.view.moneyFlow.peerEntityTypeId;
          var entityId = entity.entityId;
          var searchParams = getSearchParams(peerEntityTypeId, entityId, $viewValue);
          $scope.view.isLoadingSources = true;
          var success = function (response) {
              $scope.view.isLoadingSources = false;
              return handleSearchResponse(peerEntityTypeId, response);
          };
          var failure = function () {
              $scope.view.isLoadingSources = false;

              var message = "Unable to load ";
              if ($scope.view.isOutgoing) {
                  message += "recipients.";
              }
              else {
                  message += "sources.";
              }
              $log.error(message);
              NotificationService.showErrorMessage(message);
          };
          return getLoadPeerEntitiesFunction(peerEntityTypeId, searchParams, success, failure);
      }

      $scope.view.onSelectSourceType = function () {
          onModelChange($scope.view.moneyFlow);
      }

      $scope.view.onSelectPeer = function ($item, $model, $label) {
          console.assert($model.peerEntityId, "The $model must have the peer entity id defined.");
          console.assert($scope.view.moneyFlow.peerEntityTypeId, "The money flow must have the peer entity type id defined.");
          $scope.view.moneyFlow.peerEntityId = $model.peerEntityId;
          loadSourceMoneyFlows($scope.view.moneyFlow);
      }

      $scope.view.formatPeerEntity = function ($item, $model, $label) {
          if (!$model) {
              return '';
          }
          else {
              return $model.primaryText;
          }
      }

      $scope.view.onSelectSourceMoneyFlow = function ($item, $model) {
          if (!$item) {
              $scope.view.selectedSourceMoneyFlow = null;
          }
          else {
              $scope.view.selectedSourceMoneyFlow = $item;
          }
      }


      $scope.view.getAllowedMoneyFlowSourceRecipientTypes = function (mf) {
          if (mf.isOutgoing) {
              return $scope.view.allowedRecipientMoneyFlowSourceRecipientTypes;
          }
          else {
              return $scope.view.allowedSourceMoneyFlowSourceRecipientTypes;
          }
      }

      function loadSourceMoneyFlows(moneyFlow) {
          var loadFn = null;
          var peerEntityTypeId = 0;
          var entityId = 0;
          if (moneyFlow.isOutgoing) {
              peerEntityTypeId = moneyFlow.entityTypeId;
              entityId = entity.entityId;
          }
          else {
              peerEntityTypeId = moneyFlow.peerEntityTypeId;
              entityId = moneyFlow.peerEntityId;
          }
          if (peerEntityTypeId && entityId) {
              if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
                  loadFn = MoneyFlowService.getSourceMoneyFlowsByProgramId(entityId, {});
              }
              else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
                  loadFn = MoneyFlowService.getSourceMoneyFlowsByProjectId(entityId, {});
              }
              else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
                  loadFn = MoneyFlowService.getSourceMoneyFlowsByOrganizationsId(entityId, {});
              }
              else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
                  loadFn = MoneyFlowService.getSourceMoneyFlowsByOfficeId(entityId, {});
              }
              else {
                  throw Error("The peer entity type id [" + peerEntityTypeId + "] is not supported.");
              }
              $scope.view.sourceMoneyFlows = [];
              $scope.view.sourceMoneyFlows.push({
                  id: 0,
                  sourceName: ''
              });
              $scope.view.isLoadingSourceMoneyFlows = true;
              return loadFn.then(function (response) {
                  var sources = response.data;
                  $scope.view.isLoadingSourceMoneyFlows = false;
                  $scope.view.sourceMoneyFlows = sources;
                  return sources;
              })
              .catch(function (response) {
                  $scope.view.isLoadingSourceMoneyFlows = false;
                  var message = "Unable to load source money flows.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              })
          }
          else {
              var dfd = $q.defer();
              dfd.resolve();
              return dfd.promise;
          }

      }

      function onModelChange(moneyFlow) {
          var peerEntityTypeId = moneyFlow.peerEntityTypeId;
          var isExpense = peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.expense.id;
          if (isExpense) {
              $scope.view.isSourceRecipientFieldEnabled = false;
              $scope.view.isSourceRecipientFieldRequired = false;
              moneyFlow.peerEntityId = null;
              moneyFlow.isOutgoing = true;
              moneyFlow.isExpense = true;
              delete moneyFlow.peerEntity;
          }
          else {
              moneyFlow.isExpense = false;
              $scope.view.isSourceRecipientFieldEnabled = true;
              $scope.view.isSourceRecipientFieldRequired = true;
          }
          if (!moneyFlow.peerEntityTypeId) {
              //user hasn't selected a source/recipient type yet
              $scope.view.isSourceRecipientFieldEnabled = false;
          }
      }

      function handleSearchResponse(peerEntityTypeId, response) {
          if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              return handleProgramsSearchResponse(response);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              return handleProjectsSearchResponse(response);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              return handleOrganizationsSearchResponse(response);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
              return handleOfficesSearchResponse(response);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              return handleParticipantSearchResponse(response);
          }
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }
      }

      function handleParticipantSearchResponse(response) {
          var participants = response.results;
          angular.forEach(participants, function (participant, index) {
              setDataForResultTemplate(participant, 'participantId', participant.name, participant.participantStatus + " - " + participant.participantType);
          });
          return participants;
      }

      function handleOfficesSearchResponse(response) {
          var offices = response.data.results;
          angular.forEach(offices, function (office, index) {
              setDataForResultTemplate(office, 'organizationId', office.name, office.officeSymbol);
          });
          return offices;
      }

      function handleOrganizationsSearchResponse(response) {
          var orgs = response.results;
          angular.forEach(orgs, function (org, index) {
              var roleNames = '';
              angular.forEach(org.organizationRoleNames, function (roleName, index) {
                  roleNames += roleName + ', ';
              });
              if (roleNames.length > 0) {
                  roleNames = roleNames.substring(0, roleNames.length - 2);
              }
              setDataForResultTemplate(org, 'organizationId', org.name, roleNames);
          });
          return orgs;
      }

      function handleProgramsSearchResponse(response) {
          var programs = response.results;
          var maxLength = 20;
          angular.forEach(programs, function (program, index) {
              var owner = '';
              if (program.officeSymbol && program.officeSymbol.length > 0) {
                  owner += program.officeSymbol + " - ";
              }
              owner += program.orgName;
              setDataForResultTemplate(program, 'programId', program.name, owner);
          });
          return programs;
      }

      function handleProjectsSearchResponse(response) {
          var projects = response.data.results;
          angular.forEach(projects, function (project, index) {
              setDataForResultTemplate(project, 'projectId', project.projectName, 'Program:  ' + project.programName);
          });
          return projects;
      }

      function setDataForResultTemplate(entity, entityIdPropertyName, primaryText, secondaryText) {
          entity.peerEntityId = entity[entityIdPropertyName];
          entity.primaryText = primaryText;
          entity.secondaryText = secondaryText;
      }

      function getLoadPeerEntitiesFunction(peerEntityTypeId, searchParams, thenCallback, catchCallback) {
          if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              return ProgramService.getAllProgramsAlpha(searchParams).then(thenCallback, catchCallback);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              return ProjectService.get(searchParams).then(thenCallback).catch(catchCallback);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              return OrganizationService.getOrganizations(searchParams).then(thenCallback).catch(catchCallback);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              return OrganizationService.getOrganizations(searchParams).then(thenCallback).catch(catchCallback);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
              return OfficeService.getAll(searchParams).then(thenCallback).catch(catchCallback);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              return ParticipantService.getParticipantsByProject(entity.entityId, searchParams).then(thenCallback).catch(catchCallback);
          }
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }
      }

      var searchFilter = FilterService.add('moneyflow_searchpeerentityfilter');
      function getSearchParams(peerEntityTypeId, entityId, search) {
          searchFilter.reset();
          var entityIdInt = parseInt(entityId, 10);
          var namePropertyName = '';
          var idPropertyName = '';
          if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              namePropertyName = 'projectName';
              idPropertyName = 'projectId';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              namePropertyName = 'name'
              idPropertyName = 'programId';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              namePropertyName = 'name';
              idPropertyName = 'organizationId';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
              namePropertyName = 'name';
              idPropertyName = 'organizationId';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              namePropertyName = 'name';
              idPropertyName = 'participantId';
              searchFilter = searchFilter.isNotNull('personId');
          }
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }

          searchFilter = searchFilter
            .skip(0)
            .take($scope.view.searchLimit)
            .like(namePropertyName, search);
          if (peerEntityTypeId === entity.entityTypeId) {
              $log.info('Including identical entity id not equal filter.');
              var id = entity.entityId;
              if (!angular.isNumber(id)) {
                  id = parseInt(entity.entityId, 10);
              }

              searchFilter = searchFilter.notEqual(idPropertyName, id);
          }
          return searchFilter.toParams();
      }

      function toMoneyFlow(entity) {
          var moneyFlow = null;
          console.assert(entity.entityTypeId, 'The entity must have at the entityTypeId defined.');
          console.assert(entity.entityId, 'The entity must have at the entityId defined.');
          
          if (entity.isCopy) {
              moneyFlow = entity;
              $scope.view.isSourceMoneyFlowAmountExpended = moneyFlow.isSourceMoneyFlowAmountExpended;
              $scope.view.copiedMoneyFlowExceedsSourceLimit = moneyFlow.copiedMoneyFlowExceedsSourceLimit;
              $scope.view.selectedSourceMoneyFlow = moneyFlow.parentMoneyFlow ? moneyFlow.parentMoneyFlow : null;
              loadSourceMoneyFlows(moneyFlow);
          }
          else {
              moneyFlow = {
                  value: 0,
                  isOutgoing: false,
                  isExpense: false,
                  description: '',
                  transactionDate: null,
                  fiscalYear: null,
                  peerEntityTypeId: null,
                  moneyFlowStatusId: null,
                  peerEntityId: null,
                  peerEntity: {},
                  entityTypeId: entity.entityTypeId
              };
          }

          if (entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              moneyFlow.projectId = entity.entityId;
          }
          else if (entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              moneyFlow.programId = entity.entityId;
          }
          else if (entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
              moneyFlow.officeId = entity.entityId;
          }
          else if (entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              moneyFlow.organizationId = entity.entityId;
          }
          else {
              throw Error('The money flow source recipient type is not yet supported.');
          }
          return moneyFlow;
      }

      function getLookupValueById(values, id) {
          var value = '';
          angular.forEach(values, function (v, index) {
              if (v.id === id) {
                  value = v.name;
              }
          });
          return value;
      }

      var lookupParams = {
          start: 0,
          limit: 300
      };

      function getAllowedRecipientMoneyFlowSourceRecipientTypes() {
          return LookupService.getAllowedRecipientMoneyFlowSourceRecipientTypesBySourceTypeId(entity.entityTypeId)
          .then(function (response) {
              $scope.view.allowedRecipientMoneyFlowSourceRecipientTypes = response.data;
          })
          .catch(function (response) {
              var message = "Unable to load money flow source recipient types.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function getAllowedSourceMoneyFlowSourceRecipientTypes() {
          return LookupService.getAllowedSourceMoneyFlowSourceRecipientTypesByRecipientTypeId(entity.entityTypeId)
          .then(function (response) {
              $scope.view.allowedSourceMoneyFlowSourceRecipientTypes = response.data;
          })
          .catch(function (response) {
              var message = "Unable to load money flow source recipient types.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function getAllMoneyFlowTypes() {
          return LookupService.getAllMoneyFlowTypes(lookupParams)
          .then(function (response) {
              $scope.view.moneyFlowTypes = response.data.results;
          })
          .catch(function (response) {
              var message = "Unable to load money flow types.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function getAllMoneyFlowStati() {
          return LookupService.getAllMoneyFlowStati(lookupParams)
          .then(function (response) {
              $scope.view.moneyFlowStatii = response.data.results;
          })
          .catch(function (response) {
              var message = "Unable to load money flow types.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      $scope.view.isLoadingRequiredData = true;
      $q.all([getAllowedRecipientMoneyFlowSourceRecipientTypes(), getAllowedSourceMoneyFlowSourceRecipientTypes(), getAllMoneyFlowTypes(), getAllMoneyFlowStati()])
        .then(function () {
            $scope.view.isLoadingRequiredData = false;
            if (entity.isCopy) {                
            }
        })
        .catch(function () {
            $scope.view.isLoadingRequiredData = false;
        });
  });
