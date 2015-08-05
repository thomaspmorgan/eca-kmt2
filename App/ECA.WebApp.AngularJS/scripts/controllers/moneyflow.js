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
        ConstantsService
        ) {

      $scope.view = {};
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

      $scope.view.save = function () {
          $modalInstance.close($scope.view.moneyFlow);
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

      $scope.view.getSources = function ($viewValue) {

      }

      $scope.view.onSelectSource = function ($item, $model, $label) {

      }

      function getMoneyFlow(entityId, entityTypeId) {
          var moneyFlow = {
              value: 0,
              isOutgoing: false,
              description: 'The ',
              transactionDate: new Date(),
              fiscalYear: new Date().getYear(),
              peerEntityTypeId: 0,
              moneyFlowStatusId: 0,
              peerEntityId: null
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


      $scope.view.isLoadingRequiredData = true;
      $q.all([getMoneyFlowSourceRecipientTypes(), getAllMoneyFlowTypes(), getAllMoneyFlowStati()])
        .then(function () {
            $scope.view.isLoadingRequiredData = false;
        })
        .catch(function () {
            $scope.view.isLoadingRequiredData = false;
        });


  });
