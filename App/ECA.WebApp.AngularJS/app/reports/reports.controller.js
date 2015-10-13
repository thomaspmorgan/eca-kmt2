'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportCtrl
 * @description
 * # ReportCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ReportsCtrl', function ($scope, $stateParams) {
      $scope.tabs = {
          archive: {
              title: 'Archive',
              path: 'archive',
              controller: 'ReportsArchiveCtrl',
              active: true,
              order: 1
          },
          custom: {
              title: 'Custom',
              path: 'custom',
              controller: 'ReportsCustomCtrl',
              active: true,
              order: 2
          }
      };
  });
