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

    var params = {
        limit: 25
    };

    DragonBreath.get(params, 'programs')
        .success(function (data) {
            $scope.programs = data.results;
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
