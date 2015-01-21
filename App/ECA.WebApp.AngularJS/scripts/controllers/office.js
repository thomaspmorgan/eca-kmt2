'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OfficeCtrl', function ($scope, DragonBreath, $stateParams) {
 
    $scope.office = {};

    DragonBreath.get('organizations', $stateParams.officeId)
        .success(function (data) {
            if(angular.isArray(data.results)){
                $scope.office = data.results[0];
            }
            
        });

    $scope.tabs = {
        overview: {
            title: 'Overview',
            path: 'overview',
            active: true,
            order: 1
        },
        partners: {
            title: 'Branches & Programs',
            path: 'branches',
            active: true,
            order: 2
        },
        participants: {
            title: 'Activity',
            path: 'activity',
            active: true,
            order: 3
        },
        artifacts: {
            title: 'Artifacts',
            path: 'artifacts',
            active: true,
            order: 4
        },
        moneyflows: {
            title: 'Money Flows',
            path: 'moneyflows',
            active: true,
            order: 5
        }
    };


  });
