'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllProgramsCtrl', function ($scope, DragonBreath, $stateParams) {
    

    $scope.programs = [];
    $scope.currentpage = $stateParams.page || 1;
    $scope.limit = 25;

    var filterParams = {
        limit: $scope.limit,
        offset: (($scope.currentpage - 1) * 20)
    };



    DragonBreath.get(filterParams,'programs')
        .success(function (data) {
            if(angular.isArray(data)){
                $scope.programs = data;
            }
        });




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
