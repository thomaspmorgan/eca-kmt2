'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('reports', {
          url: '/report',
          templateUrl: 'app/reports/reports.html',
          controller: 'ReportsCtrl',
          requireADLogin: true
      })
        .state('reports.archive', {
            url: '/archive',
            templateUrl: 'app/reports/archive.html',
            controller: 'ReportsArchiveCtrl',
            requireADLogin: true
        })
        .state('reports.custom', {
            url: '/custom',
            templateUrl: 'app/reports/custom.html',
            controller: 'ReportsCustomCtrl',
            requireADLogin: true
        })

  });