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
        MoneyFlowService,
        LookupService,
        ConstantsService,
        ProjectService,
        ProgramService,
        OrganizationService,
        NotificationService
        ) {

      $scope.view = {};
      $scope.view.searchLimit = 10;
      $scope.view.params = $stateParams;
      $scope.view.moneyFlowSourceRecipientTypes = [];
      $scope.view.moneyFlowStatii = [];
      $scope.view.moneyFlowTypes = [];
      $scope.view.isLoadingRequiredData = false;
      $scope.view.maxDescriptionLength = 255;
      $scope.view.maxAmount = ConstantsService.maxNumericValue;
      $scope.view.incomingDirectionKey = "incoming";
      $scope.view.outgoingDirectionKey = "outgoing";
      $scope.view.isLoadingSources = false;
      $scope.view.isSaving = false;

      $scope.view.openTransactionDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isTransactionDatePickerOpen = true;
      };

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
          }
          else {
              $log.error('Direction key ]' + directionKey + '] is not recognized.');
          }
      }

      $scope.view.moneyFlow = getMoneyFlow(entity.entityId, entity.entityTypeId);

      $scope.view.getPeers = function ($viewValue) {
          var peerEntityTypeId = $scope.view.moneyFlow.peerEntityTypeId;
          var searchParams = getSearchParams(peerEntityTypeId, $viewValue);
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
          var peerEntityTypeId = $scope.view.moneyFlow.peerEntityTypeId;
          $scope.view.moneyFlow.peerEntityId = null;
      }

      $scope.view.onSelectPeer = function ($item, $model, $label) {
          console.assert($model.peerEntityId, "The $model must have the peer entity id defined.");
          $scope.view.moneyFlow.peerEntityId = $model.peerEntityId;
      }

      $scope.view.formatPeerEntity = function ($item, $model, $label) {
          if (!$model) {
              $log.info("return empty string");
              return '';
          }
          else {
              $log.info("return primary text");
              return $model.primaryText;
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
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }
      }

      function handleOrganizationsSearchResponse(response) {
          var orgs = response.results;
          angular.forEach(orgs, function (org, index) {
              setDataForResultTemplate(org, 'organizationId', org.name, org.location);
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
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }
      }

      function getSearchParams(peerEntityTypeId, search) {
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
          else {
              throw Error("The peer entity type id [" + peerEntityTypeId + "] is not yet supported.");
          }
          var params = {
              start: 0,
              limit: $scope.view.searchLimit,
              filter: [{
                  comparison: ConstantsService.likeComparisonType,
                  value: search,
                  property: namePropertyName
              }]
          };
          if (peerEntityTypeId === entity.entityTypeId) {
              $log.info('Including identical entity id not equal filter.');
              var id = entity.entityId;
              if (!angular.isNumber(id)) {
                  id = parseInt(entity.entityId, 10);
              }
              params.filter.push({
                  comparison: ConstantsService.notEqualComparisonType,
                  value: id,
                  property: idPropertyName
              });
          }
          return params;
      }

      function getMoneyFlow(entityId, entityTypeId) {
          var moneyFlow = {
              value: 0,
              isOutgoing: false,
              description: '',
              transactionDate: null,
              fiscalYear: null,
              peerEntityTypeId: null,
              moneyFlowStatusId: null,
              peerEntityId: null,
              peerEntity: {},
              entityTypeId: entityTypeId
          };
          if (entityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              moneyFlow.projectId = entityId;
          }
          else if (entityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              moneyFlow.programId = entityId;
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

      function getMoneyFlowSourceRecipientTypes() {
          var sourceRecipientTypesParams = {
              start: lookupParams.start,
              limit: lookupParams.limit,
              filter: [
                  {
                      comparison: ConstantsService.inComparisonType,
                      property: 'id',
                      value: [
                          ConstantsService.moneyFlowSourceRecipientType.project.id,
                          ConstantsService.moneyFlowSourceRecipientType.program.id,
                          ConstantsService.moneyFlowSourceRecipientType.organization.id
                      ]
                  }
              ]
          };
          $log.info("Artifically limiting source and recipient types for the type being.");

          return LookupService.getAllMoneyFlowSourceRecipientTypes(sourceRecipientTypesParams)
          .then(function (response) {
              $scope.view.moneyFlowSourceRecipientTypes = response.data.results;
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
      $q.all([getMoneyFlowSourceRecipientTypes(), getAllMoneyFlowTypes(), getAllMoneyFlowStati()])
        .then(function () {
            $scope.view.isLoadingRequiredData = false;
        })
        .catch(function () {
            $scope.view.isLoadingRequiredData = false;
        });


  });
