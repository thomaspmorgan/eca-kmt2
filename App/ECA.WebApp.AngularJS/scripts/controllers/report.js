'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportCtrl
 * @description
 * # ReportCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ReportCtrl', function ($scope) {
    $scope.staticImageSet = [];
    for (var i = 1; i < 21; i++) {
        $scope.staticImageSet.push('images/placeholders/report/report' + i + '.png');
    }
  });
