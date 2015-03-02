'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllProgramsCtrl', function ($scope, $stateParams, ProgramService, TableService) {

    $scope.programs = [];

    $scope.programsLoading = false;

    $scope.getPrograms = function (tableState) {

        $scope.programsLoading = true;

        TableService.setTableState(tableState);

        var params = {
            start: TableService.getStart(),
            limit: TableService.getLimit(),
            sort: TableService.getSort(),
            filter: TableService.getFilter()

        };

        ProgramService.getAllPrograms(params)
        .then(function (data) {
            $scope.programs = data.results;
            var limit = TableService.getLimit();
            tableState.pagination.numberOfPages = Math.floor(data.total / limit);
            $scope.programsLoading = false;
        });
    }

    /**
    DragonBreath.get(params, 'programs')
        .success(function (data) {
            $scope.programs = data.results;
        });
    */

    $scope.branches = [
    	{
    		name: 'Africa & Europe',
    		ticked: false
    	},
    	{
    		name: 'WHA, EAP, NEA, SCA',
    		ticked: false
    	}
    ];
  });
