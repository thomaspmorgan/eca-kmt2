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
        $filter,
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
      $scope.Math = window.Math;
      $scope.view.fiscalYears = fiscalYears;
      $scope.view.isLoadingSourceMoneyFlows = false;
      $scope.view.entityNameMaxLength = 100;
      $scope.view.entityName = entity.entityName;
      $scope.view.searchLimit = 25;
      $scope.view.params = $stateParams;
      $scope.view.allowedRecipientMoneyFlowSourceRecipientTypes = [];
      $scope.view.allowedSourceMoneyFlowSourceRecipientTypes = [];
      $scope.view.peers = [];
      $scope.view.peerCount = 0;
      $scope.view.filterPeerEntityValue = null;
      $scope.view.thisEntityCanFilterPeerEntities = false;
      $scope.view.valuesThatCanFilterPeerEntities = [];
      $scope.view.valuesThatCanFilterPeerEntitiesCount = 0;
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
          $scope.view.moneyFlow.peerEntityTypeId = ($scope.view.moneyFlow.peerEntityTypeId == ConstantsService.fundingSourceType) ? ConstantsService.moneyFlowSourceRecipientType.organization.id : $scope.view.moneyFlow.peerEntityTypeId
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
              $log.error('Direction key [' + directionKey + '] is not recognized.');
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

      $scope.view.getPeers = function (search) {
          var peerEntityTypeId = $scope.view.moneyFlow.peerEntityTypeId;
          if (peerEntityTypeId && peerEntityTypeId !== ConstantsService.moneyFlowSourceRecipientType.expense.id) {
              var entityId = entity.entityId;
              var filterByValue = $scope.view.filterPeerEntityValue;
              var searchParams = getSearchParams(peerEntityTypeId, entityId, search, filterByValue);
              $scope.view.peers = [];
              $scope.view.isLoadingSources = true;
              var success = function (response) {
                  $scope.view.isLoadingSources = false;
                  var total = null;
                  if (response.data) {
                      total = response.data.total;
                  }
                  else {
                      total = response.total;
                  }

                  var peers = handleSearchResponse(peerEntityTypeId, response);
                  $scope.view.peerCount = total;
                  $scope.view.peers = peers;
                  var parentSourceType = getEntityParentSourceType(entity.entityTypeId);
                  if (parentSourceType == peerEntityTypeId) {
                      var parentPeer = findPeer(peers, entity.entityParentId);
                      if (parentPeer)
                          $scope.view.peerEntity = parentPeer;
                  }
                  return $scope.view.peers;
              };
              var failure = function () {
                  $scope.view.isLoadingSources = false;

                  var message = "Unable to load ";
                  if ($scope.view.moneyFlow.isOutgoing) {
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
          else {
              return [];
          }
      }

      function findPeer(peers, id)
      {
          for (var i = 0, iLen = peers.length; i < iLen; i++)
              if (peers[i].peerEntityId == id)
                  return peers[i];
      }

      function getEntityParentSourceType(id)
      {
          if (id = ConstantsService.moneyFlowSourceRecipientType.program.id)
              return ConstantsService.moneyFlowSourceRecipientType.office.id
          else if (id = ConstantsService.moneyFlowSourceRecipientType.project.id)
              return ConstantsService.moneyFlowSourceRecipientType.program.id
          else if (id = ConstantsService.moneyFlowSourceRecipientType.participant.id)
              return ConstantsService.moneyFlowSourceRecipientType.project.id
          else
              return null;
      }

      $scope.view.onFilterPeerEntityValueSelect = function ($item, $model) {
          $scope.view.moneyFlow.peerEntity = null;
          $scope.view.getPeers(null);
      }

      $scope.view.onSelectSourceType = function () {
          onModelChange($scope.view.moneyFlow);
          $scope.view.loadValuesThatCanFilterPeerEntities(null, $scope.view.moneyFlow);
          $scope.view.getPeers(null);
          // clear peer, year and parent money flow selected models to clear dropdowns
          $scope.view.moneyFlow.peerEntity = null;
          $scope.view.moneyFlow.fiscalYear = null;
          $scope.view.moneyFlow.parentMoneyFlowId = null;
      }

      $scope.view.onSelectPeer = function ($item, $model) {
          console.assert($model.peerEntityId, "The $model must have the peer entity id defined.");
          console.assert($scope.view.moneyFlow.peerEntityTypeId, "The money flow must have the peer entity type id defined.");
          $scope.view.moneyFlow.peerEntityId = $model.peerEntityId;
          // clear year and parent money flow selected models to clear dropdowns
          $scope.view.moneyFlow.fiscalYear = null;
          $scope.view.moneyFlow.parentMoneyFlowId = null;
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
              $scope.view.moneyFlow.isDirect = true;
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

      var filterOnThisPeerEntity = false;
      $scope.view.onFilterOnThisPeerEntity = function (moneyFlow) {
          filterOnThisPeerEntity = true;
          $scope.view.loadValuesThatCanFilterPeerEntities(null, moneyFlow)
          .then(function (valuesThatCanFilterPeerEntities) {
              if (filterOnThisPeerEntity) {
                  $scope.view.filterPeerEntityValue = valuesThatCanFilterPeerEntities[0]
              }
              filterOnThisPeerEntity = false;
              return $scope.view.getPeers(null);
          });

      }

      var valuesThatCanFilterPeerEntitiesFilter = FilterService.add('moneyflowcontroller_valuesThatCanFilterPeerEntities');
      $scope.view.loadValuesThatCanFilterPeerEntities = function (search, moneyFlow) {
          var loadFn = null;

          valuesThatCanFilterPeerEntitiesFilter.reset();
          valuesThatCanFilterPeerEntitiesFilter = valuesThatCanFilterPeerEntitiesFilter
              .skip(0)
              .take($scope.view.searchLimit);

          var propertyToFilter = null;
          var idToFilter = null;
          $scope.view.filterPeerEntityValue = null;
          var idMapFn = null;
          var valueMapFn = null;
          var peerEntityTypeId = moneyFlow.peerEntityTypeId;
          var entityTypeId = entity.entityTypeId;
          var filterEntityTypeId = 0;

          if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              filterEntityTypeId = ConstantsService.moneyFlowSourceRecipientType.office.id;
              loadFn = OfficeService.getAll;
              propertyToFilter = 'name';
              idToFilter = 'organizationId';
              idMapFn = function (office) { return office.organizationId; };
              valueMapFn = function (office) { return office.name; };
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              filterEntityTypeId = ConstantsService.moneyFlowSourceRecipientType.program.id;
              loadFn = ProgramService.getAllProgramsAlpha
              propertyToFilter = 'name';
              idToFilter = 'programId';
              idMapFn = function (prog) { return prog.programId; };
              valueMapFn = function (prog) { return prog.name; };
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              loadFn = LookupService.getOrganizationRoles;
              propertyToFilter = 'value';
              idMapFn = function (role) { return role.id; };
              valueMapFn = function (role) { return role.value; };
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              loadFn = LookupService.getParticipantStatii;
              propertyToFilter = 'name';
              idToFilter = 'programId';
              idMapFn = function (role) { return role.id; };
              valueMapFn = function (role) { return role.name; };
          }
          else if (peerEntityTypeId === ConstantsService.fundingSourceType) {
              loadFn = LookupService.getOrganizationalRoles;
              propertyToFilter = 'value'
              idMapFn = function (role) { return role.id; };
              valueMapFn = function (role) { return role.value; };
          }

          $scope.view.thisEntityCanFilterPeerEntities =
              filterEntityTypeId !== 0
              && entityTypeId === filterEntityTypeId
          if (filterOnThisPeerEntity) {
              var entityIdInt = parseInt(entity.entityId, 10);
              valuesThatCanFilterPeerEntitiesFilter = valuesThatCanFilterPeerEntitiesFilter.equal(idToFilter, entityIdInt);
          }

          if (loadFn) {
              if (search) {
                  valuesThatCanFilterPeerEntitiesFilter = valuesThatCanFilterPeerEntitiesFilter.like(propertyToFilter, search);
              }
              valuesThatCanFilterPeerEntitiesFilter = valuesThatCanFilterPeerEntitiesFilter.sortBy(propertyToFilter);
              return loadFn(valuesThatCanFilterPeerEntitiesFilter.toParams())
              .then(function (response) {
                  var data = null;
                  var total = -1;
                  if (response.data) {
                      data = response.data.results;
                      total = response.data.total;
                  }
                  else {
                      data = response.results;
                      total = response.total;
                  }
                  angular.forEach(data, function (item, index) {
                      item.id = idMapFn(item);
                      item.value = valueMapFn(item);
                  });
                  if ($scope.view.moneyFlow.peerEntityTypeId == ConstantsService.moneyFlowSourceRecipientType.organization.id)
                  {
                      //remove Funding Source from Roles
                      var filterValue = '!' + ConstantsService.organizationRole.fundingSource.value
                      data = $filter('filter')(data, { value: filterValue });
                      total = data.length;
                  }
                  $scope.view.valuesThatCanFilterPeerEntities = data;
                  $scope.view.valuesThatCanFilterPeerEntitiesCount = total;
                  return $scope.view.valuesThatCanFilterPeerEntities;
              })
              .catch(function (response) {
                  var message = "Unable to load filter values for the source/recipient.";
                  $log.error(message);
              });
          }
          else {
              var dfd = $q.defer();
              $scope.view.valuesThatCanFilterPeerEntities = [];
              $scope.view.valuesThatCanFilterPeerEntitiesCount = 0;
              dfd.resolve();
              return dfd.promise;
          }
      }

      $scope.view.isPeerEntitiesFilterEnabled = function (moneyFlow) {
          var peerEntityTypeId = moneyFlow.peerEntityTypeId;
          if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              return true;
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              return true;
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              return true;
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              return true;
          }
          else {
              return false;
          }
      }

      $scope.view.getPeerEntityFilterDisplayName = function (moneyFlow) {
          var peerEntityTypeId = moneyFlow.peerEntityTypeId;
          if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              return 'Office';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              return 'Program';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              return 'Role';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              return 'Status';
          }
          else {
              return '';
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
              peerEntityTypeId = (moneyFlow.peerEntityTypeId == ConstantsService.fundingSourceType) ? ConstantsService.moneyFlowSourceRecipientType.organization.id : moneyFlow.peerEntityTypeId
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
          else if (peerEntityTypeId === ConstantsService.fundingSourceType) {
              return handleOrganizationsSearchResponse(response);
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
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
              return OfficeService.getAll(searchParams).then(thenCallback).catch(catchCallback);
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              return ParticipantService.getParticipantsByProject(entity.entityId, searchParams).then(thenCallback).catch(catchCallback);
          }
          else if (peerEntityTypeId === ConstantsService.fundingSourceType) {
              return OrganizationService.getOrganizations(searchParams).then(thenCallback).catch(catchCallback);
          }
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }
      }

      var searchFilter = FilterService.add('moneyflow_searchpeerentityfilter');
      function getSearchParams(peerEntityTypeId, entityId, search, filterByValue) {
          searchFilter.reset();
          var entityIdInt = parseInt(entityId, 10);
          var namePropertyName = '';
          var idPropertyName = '';
          var filterByPropertyName = '';
          if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              namePropertyName = 'projectName';
              idPropertyName = 'projectId';
              if (filterByValue) {
                  console.assert(filterByValue.programId, 'The filter by value programId must have a value.  Its from the program to filter by.');
                  searchFilter = searchFilter.equal('programId', filterByValue.programId);
              }
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              namePropertyName = 'name'
              idPropertyName = 'programId';
              if (filterByValue) {
                  console.assert(filterByValue.organizationId, 'The filter by value organizationId must have a value.  Its from the offices to filter by.');
                  searchFilter = searchFilter.equal('owner_OrganizationId', filterByValue.organizationId);
              }
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              namePropertyName = 'name';
              idPropertyName = 'organizationId';
              if (filterByValue) {
                  console.assert(filterByValue.id, 'The filter by value id must have a value.  Its a look up for org roles to filter by.');
                  searchFilter = searchFilter.containsAny('organizationRoleIds', [filterByValue.id]);
              }
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
              namePropertyName = 'name';
              idPropertyName = 'organizationId';
          }
          else if (peerEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              namePropertyName = 'name';
              idPropertyName = 'participantId';
              searchFilter = searchFilter.isNotNull('personId');
              if (filterByValue) {
                  console.assert(filterByValue.id, 'The filter by value id must have a value.  Its a look up for participant status to filter by.');
                  searchFilter = searchFilter.equal('statusId', filterByValue.id);
              }
          }
          else if (peerEntityTypeId === ConstantsService.fundingSourceType) {
              namePropertyName = 'name';
              idPropertyName = 'organizationId';
              searchFilter = searchFilter.containsAny('organizationRoleIds', [ConstantsService.organizationRole.fundingSource.id]);
          }
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }
          searchFilter = searchFilter
             .sortBy(namePropertyName)
            .skip(0)
            .take($scope.view.searchLimit);
          if (search) {
              searchFilter = searchFilter.like(namePropertyName, search);
          }
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
                  entityTypeId: entity.entityTypeId,
                  isDirect: true
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
              addFundingSource();
          })
          .catch(function (response) {
              var message = "Unable to load money flow source recipient types.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function addFundingSource() {

          if ((entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) ||
             (entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) ||
             (entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) ||
             (entity.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id)) {
              $scope.view.allowedSourceMoneyFlowSourceRecipientTypes.push({ id: ConstantsService.fundingSourceType, name: "Funding Source" });
          }
      }

      //function getAllMoneyFlowTypes() {
      //    return LookupService.getAllMoneyFlowTypes(lookupParams)
      //    .then(function (response) {
      //        $scope.view.allowedSourceMoneyFlowSourceRecipientTypes = response.data;
      //    })
      //    .catch(function (response) {
      //        var message = "Unable to load money flow types.";
      //        $log.error(message);
      //        NotificationService.showErrorMessage(message);
      //    });
      //}

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
      $q.all([getAllowedRecipientMoneyFlowSourceRecipientTypes(), getAllowedSourceMoneyFlowSourceRecipientTypes(), getAllMoneyFlowStati()])
        .then(function () {
            $scope.view.isLoadingRequiredData = false;
            if (entity.isCopy) {
            }
        })
        .catch(function () {
            $scope.view.isLoadingRequiredData = false;
        });
  });
