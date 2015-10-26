'use strict';

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
        $state,
        $q,
        $log,
        $modal,
        MessageBox,
        smoothScroll,
        AuthService,
        MoneyFlowService,
        LookupService,
        ConstantsService,
        NotificationService,
        TableService,
        StateService
        ) {

      console.assert($scope.stateParamName !== undefined, 'The stateParamName must be defined in the directive, i.e. the state parameter name that has the id of the entity showing money flows.');
      console.assert($scope.sourceEntityTypeId !== undefined, 'The sourceEntityTypeId i.e. the money flow source recipient type id of the object that is current showing funding must be set in the directive.');
      console.assert($scope.resourceTypeId !== undefined, 'The resourceTypeId i.e. the cam resource type id must be set in the directive..');
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
              entityTypeId: $scope.sourceEntityTypeId,
              entityName: $scope.entityName
          };
          showEditMoneyFlow(newMoneyFlow);
      };

      $scope.view.onEditClick = function (moneyFlow) {
          moneyFlow.original = angular.copy(moneyFlow);
          moneyFlow.currentlyEditing = true;
          moneyFlow.editableAmount = moneyFlow.editableAmount < 0 ? -moneyFlow.editableAmount : moneyFlow.editableAmount;
          if (moneyFlow.parentMoneyFlowId) {
              loadSourceMoneyFlow(moneyFlow);
          }

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

      $scope.view.onEditableAmountChange = function ($event, moneyFlow) {
          if (moneyFlow.amount !== moneyFlow.editableAmount) {
              var newValue = moneyFlow.editableAmount;
              var positiveAmount = newValue > 0 ? newValue : -newValue;
              var negativeAmount = newValue < 0 ? newValue : -newValue;
              moneyFlow.editableAmount = positiveAmount;
              if (moneyFlow.isOutgoing) {
                  moneyFlow.amount = negativeAmount;
              }
              else {
                  moneyFlow.amount = positiveAmount;
              }
          }
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
          MessageBox.confirm({
              title: 'Confirm',
              message: 'Are you sure you wish to delete the funding line item?',
              okText: 'Yes',
              cancelText: 'No',
              okCallback: function () {
                  $log.info('User confirmed delete of money flow...');
                  deleteMoneyFlow(moneyFlow);
              }
          });
      }

      $scope.view.onCopyClick = function (moneyFlow) {
          return getCopiedMoneyFlow(moneyFlow)
          .then(function (copiedMoneyFlow) {
              showEditMoneyFlow(copiedMoneyFlow);
          })
          .catch(function () {
              var message = "Unable to copy funding item.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      $scope.view.validateSourceRemainingAmount = function ($value, moneyFlow) {
          if (moneyFlow.parentMoneyFlow) {
              if ($value && $value > moneyFlow.parentMoneyFlow.moneyFlowLineItemMaximumAmount) {
                  return false;
              }
              else {
                  return true;
              }
          }
          else {
              return true;
          }
      }

      $scope.view.getFiscalYears = function (moneyFlow) {
          var currentYear = new Date().getFullYear();
          var startYear = currentYear - 10;
          var maxYear = currentYear + 2;
          var allYears = [];
          for (var i = startYear; i <= maxYear; i++) {
              allYears.push(i);
          }
          
          if (moneyFlow.hasOwnProperty('fiscalYear')) {
              var containsMoneyFlowFiscalYear = false;
              angular.forEach(allYears, function (year, index) {
                  if (year === moneyFlow.fiscalYear) {
                      containsMoneyFlowFiscalYear = true;
                  }
              });
              if (!containsMoneyFlowFiscalYear) {
                  allYears.splice(0, 0, moneyFlow.fiscalYear);
              }
          }          
          return allYears;
      }

      function loadSourceMoneyFlow(moneyFlow) {
          console.assert(moneyFlow.parentMoneyFlowId, "The given money flow should have a parent id.");
          moneyFlow.isLoadingSource = true;
          return MoneyFlowService.getSourceMoneyFlowById(moneyFlow.id)
          .then(function (response) {
              moneyFlow.isLoadingSource = false;
              var positiveAmount = moneyFlow.amount < 0 ? -moneyFlow.amount : moneyFlow.amount;
              moneyFlow.parentMoneyFlow = response.data;
              moneyFlow.parentMoneyFlow.moneyFlowLineItemMaximumAmount = positiveAmount + moneyFlow.parentMoneyFlow.remainingAmount;
          })
          .catch(function (response) {
              moneyFlow.isLoadingSource = false;
              var message = "Unable to load parent money flow.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function showEditMoneyFlow(moneyFlow) {
          var fiscalYears = $scope.view.getFiscalYears(moneyFlow);
          var modalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/directives/moneyflow.directive.html',
              controller: 'MoneyFlowCtrl',
              size: 'lg',
              resolve: {
                  entity: function () {
                      return moneyFlow;
                  },
                  fiscalYears: function () {
                      return fiscalYears;
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
          copiedMoneyFlow.entityName = $scope.entityName;

          if (moneyFlow.parentMoneyFlowId) {
              moneyFlow.isCopyingMoneyFlow = true;
              copiedMoneyFlow.parentMoneyFlowId = moneyFlow.parentMoneyFlowId;
              $log.info("Copied money flow has parent money flow.");
              return MoneyFlowService.getSourceMoneyFlowById(moneyFlow.id)
              .then(function (response) {
                  var parentMoneyFlow = response.data;
                  copiedMoneyFlow.parentMoneyFlow = parentMoneyFlow;
                  copiedMoneyFlow.isSourceMoneyFlowAmountExpended = parentMoneyFlow.remainingAmount === 0;
                  copiedMoneyFlow.copiedMoneyFlowExceedsSourceLimit = parentMoneyFlow.remainingAmount < copiedMoneyFlow.value;
                  moneyFlow.isCopyingMoneyFlow = false;
                  return copiedMoneyFlow;
              });
          }
          else {
              var dfd = $q.defer();
              dfd.resolve(copiedMoneyFlow);
              return dfd.promise;
          }
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

      function getMoneyFlowHref(moneyFlow) {
          var dfd = $q.defer();

          if (moneyFlow.sourceRecipientEntityTypeId !== ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              dfd.resolve(StateService.getStateByMoneyFlowSourceRecipientType(moneyFlow.sourceRecipientEntityId, moneyFlow.sourceRecipientEntityTypeId));
          }
          else {
              StateService.getStateByMoneyFlowSourceRecipientType(moneyFlow.sourceRecipientEntityId, moneyFlow.sourceRecipientEntityTypeId)
              .then(function (result) {
                  dfd.resolve(result);
              })
              .catch(function (response) {
                  var message = "Unable to get participant state.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              });
          }
          return dfd.promise;
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
                  moneyFlow.isLoadingSource = false;
                  moneyFlow.isDeleting = false;
                  moneyFlow.isCopyingMoneyFlow = false;
                  moneyFlow.editableAmount = moneyFlow.amount < 0 ? -moneyFlow.amount : moneyFlow.amount;
                  moneyFlow.isTransactionDatePickerOpen = false;
                  moneyFlow.loadingEntityState = false;
                  if (StateService.isStateAvailableByMoneyFlowSourceRecipientTypeId(moneyFlow.sourceRecipientEntityTypeId)) {
                      moneyFlow.loadingEntityState = true;
                      getMoneyFlowHref(moneyFlow)
                      .then(function (href) {
                          moneyFlow.loadingEntityState = false;
                          moneyFlow.href = href;
                      });
                  }
                  var transactionDate = new Date(moneyFlow.transactionDate);
                  if (!isNaN(transactionDate.getTime())) {
                      moneyFlow.transactionDate = transactionDate;
                  }

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
          else if (moneyFlowSourceRecipientTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
              return MoneyFlowService.getMoneyFlowsByOffice;
          }
          else if (moneyFlowSourceRecipientTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
              return MoneyFlowService.getMoneyFlowsByOrganization;
          }
          else if (moneyFlowSourceRecipientTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
              return MoneyFlowService.getMoneyFlowsByPersonId;
          }
          else {
              throw Error('A mapping to a money flow service function for the money flow source recipient type id [' + moneyFlowSourceRecipientTypeId + '] does not exist.');
          }
      }


      function loadPermissions() {
          var foreignResourceId = $scope.view.entityId;
          var resourceTypeId = $scope.resourceTypeId;
          var hasEditPermissionCallback = function () {
              $log.info('User has edit money flow permission moneyflow.js controller.');
              $scope.view.canEditMoneyFlows = true;
          };
          var notAuthorizedCallback = function () {
              $scope.view.canEditMoneyFlows = false;
          };
          if (resourceTypeId !== '') {
              var resourceType = AuthService.getResourceTypeNameById(resourceTypeId);
              var config = getPermissionsConfig(resourceTypeId, hasEditPermissionCallback, notAuthorizedCallback);
              return AuthService.getResourcePermissions(resourceType, foreignResourceId, config)
                .then(function (result) {

                }, function () {
                    $log.error('Unable to load user permissions.');
                });
          }
          else {
              $log.info('Moneyflow object is not a resource type used in permissions, therefore, edit permission is granted.');
              var dfd = $q.defer();
              if ($state.current.name === StateService.stateNames.moneyflow.person) {
                  notAuthorizedCallback();
              }
              else {
                  hasEditPermissionCallback();
              }

              dfd.resolve();
              return dfd.promise;
          }
      }

      function getPermissionsConfig(resourceTypeId, hasEditPermissionCallback, notAuthorizedCallback) {
          if (resourceTypeId === ConstantsService.resourceType.project.id) {
              return getProjectPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback);
          }
          else if (resourceTypeId === ConstantsService.resourceType.program.id) {
              return getProgramPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback);
          }
          else if (resourceTypeId === ConstantsService.resourceType.office.id) {
              return getOfficePermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback);
          }
          else {
              throw Error("The resource type id is not yet supported for money flows.");
          }
      }

      function getGenericPermissionsConfig(permissionName, hasEditPermissionCallback, notAuthorizedCallback) {
          var config = {};
          config[permissionName] = {
              hasPermission: hasEditPermissionCallback,
              notAuthorized: notAuthorizedCallback
          };
          return config;
      }

      function getProjectPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback) {
          return getGenericPermissionsConfig(ConstantsService.permission.editProject.value, hasEditPermissionCallback, notAuthorizedCallback);
      }

      function getProgramPermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback) {
          return getGenericPermissionsConfig(ConstantsService.permission.editProgram.value, hasEditPermissionCallback, notAuthorizedCallback);
      }

      function getOfficePermissionsConfig(hasEditPermissionCallback, notAuthorizedCallback) {
          return getGenericPermissionsConfig(ConstantsService.permission.editOffice.value, hasEditPermissionCallback, notAuthorizedCallback);
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
      });


  });
