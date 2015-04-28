'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # Get parameters for ProjectAwards and execute report
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectAwardsCtrl', function ($scope, $modalInstance) {

    $scope.programs = [];
    $scope.countries = [];
    
    $scope.run = function () {
        // get report
        $modalInstance.close();
    } 
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }

  });