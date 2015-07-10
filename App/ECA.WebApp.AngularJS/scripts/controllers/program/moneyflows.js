angular.module('staticApp')
  .controller('ProgramMoneyFlowsCtrl',
  function ($scope, $stateParams, $q, ProgramService,
      MoneyFlowService, ConstantsService, TableService) {

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

      $scope.moneyFlowsLoading = false;

      $scope.getMoneyFlows = function (tableState) {

          $scope.moneyFlowsLoading = true;
          $scope.showFullMoneyFlowDescription = [];
          $scope.editingMoneyFlows = [];

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          MoneyFlowService.getMoneyFlowsByProgram($stateParams.programId, params)
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

  });