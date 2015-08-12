﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MoneyFlowsCtrl
 * @description The money flows controller is used to control the list of money flows.
 * # MoneyFlowsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('MoneyFlowsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        smoothScroll,
        AuthService,
        MoneyFlowService,
        LookupService,
        ConstantsService,
        NotificationService,
        TableService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.moneyFlows = [];
      $scope.view.moneyFlowSourceRecipientTypes = [];
      $scope.view.moneyFlowStatii = [];
      $scope.view.moneyFlowTypes = [];
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isLoadingMoneyFlows = false;
      $scope.view.start = 0;
      $scope.view.end = 0;
      $scope.view.total = 0;
      $scope.view.limit = 10;
      $scope.view.canEditMoneyFlows = false;
      $scope.view.maxAmount = ConstantsService.maxNumericValue;
      $scope.view.maxDescriptionLength = 255;

      $scope.view.selectedFilterMoneyFlowStatii = [];
      $scope.view.selectedFilterSourceRecipientTypes = [];

      //the program id, project id, etc...
      $scope.view.entityId = $stateParams[$scope.$parent.stateParamName];

      $scope.view.getMoneyFlows = function (tableState) {
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };
          return loadMoneyFlows(params, tableState);
      };

      $scope.view.onToggleExpandClick = function (moneyFlow) {
          moneyFlow.showDescription = !moneyFlow.showDescription;
      }

      $scope.view.onAddFundingItemClick = function () {
          var newMoneyFlow = {
              entityId: $scope.view.entityId,
              entityTypeId: $scope.sourceEntityTypeId
          };
          showEditMoneyFlow(newMoneyFlow);
      };

      $scope.view.onEditClick = function (moneyFlow) {
          moneyFlow.original = angular.copy(moneyFlow);
          moneyFlow.currentlyEditing = true;
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 225,
              callbackBefore: function (element) {
              },
              callbackAfter: function (element) { }
          }
          var id = $scope.view.getMoneyFlowDivId(moneyFlow)
          var e = document.getElementById(id);
          smoothScroll(e, options);
      }

      $scope.view.saveMoneyFlowChanges = function (moneyFlow) {
          moneyFlow.isSavingUpdate = true;
          moneyFlow.moneyFlowStatus = getLookupValueById($scope.view.moneyFlowStatii, moneyFlow.moneyFlowStatusId);
          moneyFlow.amount = moneyFlow.amount < 0 ? -moneyFlow.amount : moneyFlow.amount;

          return MoneyFlowService.update(moneyFlow, $scope.view.entityId)
          .then(function (response) {
              NotificationService.showSuccessMessage("Successfully updated funding line item.");
              if (moneyFlow.isOutgoing) {
                  moneyFlow.amount = -moneyFlow.amount;
              }
              moneyFlow.isSavingUpdate = false;
              moneyFlow.currentlyEditing = false;
          })
          .catch(function () {
              var message = "Unable to update the funding line item.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
              moneyFlow.isSavingUpdate = false;
          });
      };

      $scope.view.cancelMoneyFlowChanges = function (moneyFlow) {
          moneyFlow.moneyFlowStatusId = moneyFlow.original.moneyFlowStatusId;
          moneyFlow.moneyFlowStatus = getLookupValueById($scope.view.moneyFlowStatii, moneyFlow.moneyFlowStatusId);
          moneyFlow.description = moneyFlow.original.description;
          moneyFlow.amount = moneyFlow.original.amount;
          moneyFlow.editableAmount = moneyFlow.original.amount;
          moneyFlow.fiscalYear = moneyFlow.original.fiscalYear;
          delete moneyFlow.original;
          moneyFlow.currentlyEditing = false;
      };

      $scope.view.getMoneyFlowDivId = function (moneyFlow) {
          return 'editMoneyFlow' + moneyFlow.id;
      };

      $scope.view.openTransactionDatePicker = function ($event, moneyFlow, form) {
          $event.preventDefault();
          $event.stopPropagation();
          moneyFlow.isTransactionDatePickerOpen = true;
      }

      $scope.view.onDeleteClick = function (moneyFlow) {
          var modalInstance = $modal.open({
              animation: true,
              templateUrl: 'views/directives/confirmdialog.html',
              controller: 'ConfirmCtrl',
              resolve: {
                  options: function () {
                      return {
                          title: 'Confirm',
                          message: 'Are you sure you wish to delete the funding line item?',
                          okText: 'Yes',
                          cancelText: 'No'
                      };
                  }
              }
          });
          modalInstance.result.then(function () {
              $log.info('User confirmed delete of money flow...');
              deleteMoneyFlow(moneyFlow);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      $scope.view.onCopyClick = function (moneyFlow) {
          var copiedMoneyFlow = getCopiedMoneyFlow(moneyFlow);
          showEditMoneyFlow(copiedMoneyFlow);
      }

      function showEditMoneyFlow(moneyFlow) {
          var modalInstance = $modal.open({
              animation: true,
              templateUrl: 'views/directives/moneyflow.html',
              controller: 'MoneyFlowCtrl',
              size: 'lg',
              resolve: {
                  entity: function () {
                      return moneyFlow;
                  }
              }
          });

          modalInstance.result.then(function (newMoneyFlow) {
              $log.info('Finished adding new money flow.');
              reloadMoneyFlowTable();
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      function getCopiedMoneyFlow(moneyFlow) {
          var copiedMoneyFlow = angular.copy(moneyFlow);
          delete copiedMoneyFlow.id;

          copiedMoneyFlow.description = 'COPY:  ' + copiedMoneyFlow.description;
          copiedMoneyFlow.isExpense = moneyFlow.sourceRecipientEntityTypeId === ConstantsService.moneyFlowSourceRecipientType.expense.id;
          copiedMoneyFlow.isOutoing = copiedMoneyFlow.amount < 0;
          copiedMoneyFlow.entityId = $scope.view.entityId,
          copiedMoneyFlow.entityTypeId = $scope.sourceEntityTypeId;
          copiedMoneyFlow.isCopy = true;
          copiedMoneyFlow.peerEntityTypeId = moneyFlow.sourceRecipientEntityTypeId;
          copiedMoneyFlow.peerEntityId = moneyFlow.sourceRecipientEntityId;
          copiedMoneyFlow.value = moneyFlow.amount < 0 ? -moneyFlow.amount : moneyFlow.amount;
          copiedMoneyFlow.peerEntity = {
              primaryText: moneyFlow.sourceRecipientName
          };
          return copiedMoneyFlow;
      }

      function deleteMoneyFlow(moneyFlow) {
          moneyFlow.isDeleting = true;
          return MoneyFlowService.remove(moneyFlow, $scope.view.entityId)
          .then(function (response) {
              moneyFlow.isDeleting = false;
              var index = $scope.view.moneyFlows.indexOf(moneyFlow);
              $scope.view.moneyFlows.splice(index, 1);
              NotificationService.showSuccessMessage('Successfully deleted the funding line item.');
          })
          .catch(function (response) {
              moneyFlow.isDeleting = false;
              var message = 'Unable to remove the money flow.';
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function reloadMoneyFlowTable() {
          console.assert($scope.getMoneyFlowsTableState, "The table state function must exist.");
          $scope.view.getMoneyFlows($scope.getMoneyFlowsTableState());
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
          return LookupService.getAllMoneyFlowSourceRecipientTypes(lookupParams)
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

      function loadMoneyFlows(params, tableState) {
          $scope.view.isLoadingMoneyFlows = true;
          var entityId = $scope.view.entityId;
          var fn = getMoneyFlowServiceFunctionByTypeId($scope.sourceEntityTypeId);
          return fn(entityId, params)
          .then(function (response) {
              $log.info('Loading money flows...');
              var pagedMoneyFlows = response.data.results;
              angular.forEach(pagedMoneyFlows, function (moneyFlow, index) {
                  moneyFlow.showDescription = false;
                  moneyFlow.currentlyEditing = false;
                  moneyFlow.isOutgoing = moneyFlow.amount < 0;
                  moneyFlow.isSavingUpdate = false;
                  moneyFlow.isDeleting = false;
                  moneyFlow.editableAmount = moneyFlow.amount < 0 ? -moneyFlow.amount : moneyFlow.amount;
                  moneyFlow.isTransactionDatePickerOpen = true;
                  $scope.$watch(function () {
                      return moneyFlow.editableAmount;
                  },
                  function (newValue, oldValue) {
                      if (newValue !== oldValue) {
                          console.assert(newValue >= 0, 'The amount value should never be negative.');
                          moneyFlow.amount = newValue;
                      }
                  });
              });
              var total = response.data.total;
              var limit = TableService.getLimit();
              var start = TableService.getStart();
              tableState.pagination.numberOfPages = Math.ceil(total / limit);
              $scope.view.start = start + 1;
              $scope.view.end = start + pagedMoneyFlows.length;
              $scope.view.total = total;
              $scope.view.isLoadingMoneyFlows = false;
              $scope.view.moneyFlows = pagedMoneyFlows;

          })
          .catch(function (response) {
              var message = "Unable to load money flows for source entity with id " + entityId;
              $log.error(message);
              NotificationService.showErrorMessage(message);
              $scope.view.isLoadingMoneyFlows = false;
          });
      }

      function getMoneyFlowServiceFunctionByTypeId(moneyFlowSourceRecipientTypeId) {
          if (moneyFlowSourceRecipientTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
              return MoneyFlowService.getMoneyFlowsByProgram;
          }
          else if (moneyFlowSourceRecipientTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
              return MoneyFlowService.getMoneyFlowsByProject;
          }
          else {
              throw Error('A mapping to a money flow service function for the money flow source recipient type id [' + moneyFlowSourceRecipientTypeId + '] does not exist.');
          }
      }


      function loadPermissions() {
          var resourceTypeId = $scope.resourceTypeId;
          var resourceType = AuthService.getResourceTypeNameById(resourceTypeId);
          var foreignResourceId = $scope.view.entityId;
          var hasEditPermissionCallback = function () {
              $log.info('User has edit money flow permission moneyflow.js controller.');
              $scope.view.canEditMoneyFlows = true;
          };
          var notAuthorizedCallback = function () {
              $scope.view.canEditMoneyFlows = false;
          };
          var config = getPermissionsConfig(resourceTypeId, hasEditPermissionCallback, notAuthorizedCallback);

          return AuthService.getResourcePermissions(resourceType, foreignResourceId, config)
            .then(function (result) {

            }, function () {
                $log.error('Unable to load user permissions.');
            });
      }

      function getPermissionsConfig(resourceTypeId, hasEditPermissionCallback, notAuthorizedCallback) {
          if (resourceTypeId === ConstantsService.resourceType.project.id) {
              return getProjectPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback);
          }
          else if (resourceTypeId === ConstantsService.resourceType.program.id) {
              return getProgramPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback);
          }
          else {
              throw Error("The resource type id is not yet supported for money flows.");
          }
      }

      function getProjectPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback) {
          var config = {};
          config[ConstantsService.permission.editProject.value] = {
              hasPermission: hasEditPermissionCallback,
              notAuthorized: notAuthorizedCallback
          };
          return config;
      }

      function getProgramPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback) {
          var config = {
          };
          config[ConstantsService.permission.editProgram.value] = {
              hasPermission: hasEditPermissionCallback,
              notAuthorized: notAuthorizedCallback
          };
          return config;
      }



      $scope.view.isLoadingRequiredData = true;
      loadPermissions()
        .then($q.all([getMoneyFlowSourceRecipientTypes(), getAllMoneyFlowTypes(), getAllMoneyFlowStati()])
          .then(function () {
              $scope.view.isLoadingRequiredData = false;
          })
          .catch(function () {
              $scope.view.isLoadingRequiredData = false;
          })
      )
      .catch(function () {
          var message = "Unable to load user permissions.";
          $log.error(message);
          NotificationService.showErrorMessage(message);
      })


  });
