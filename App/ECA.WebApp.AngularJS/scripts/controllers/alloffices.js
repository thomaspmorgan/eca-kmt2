'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOfficesCtrl', function ($scope, $stateParams, OfficeService, TableService) {
    

    $scope.offices = [];
    $scope.currentpage = $stateParams.page || 1;
    $scope.limit = 200;

    var filterParams = {
        limit: $scope.limit,
        offset: (($scope.currentpage - 1) * 20)
    };

    $scope.getOffices = function (tableState) {

        $scope.officesLoading = true;


        TableService.setTableState(tableState);

        var params = {
            start: TableService.getStart(),
            limit: TableService.getLimit(),
            sort: TableService.getSort(),
            filter: TableService.getFilter()

        };

        OfficeService.getAll(params)
        .then(function (results) {
            $scope.offices = results.data;
            var limit = TableService.getLimit();
            //tableState.pagination.limit = Math.ceil(data.length / limit);
            $scope.officesLoading = false;
        });
    };

  });
