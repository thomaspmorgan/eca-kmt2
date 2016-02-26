'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('offices', {
          url: '/offices/:officeId',
          templateUrl: 'app/offices/offices.html',
          controller: 'OfficeCtrl',
          requireADLogin: true
      })
       .state('offices.overview', {
           url: '/overview',
           templateUrl: 'app/offices/overview.html',
           controller: 'OfficeOverviewCtrl',
           requireADLogin: true
       })
       .state('offices.edit', {
           url: '/edit',
           templateUrl: 'app/offices/edit.html',
           controller: 'OfficeEditCtrl',
           requireADLogin: true
       })
       .state('offices.branches', {
           url: '/branches',
           templateUrl: 'app/offices/branches.html',
           controller: 'BranchesAndProgramsCtrl',
           requireADLogin: true
       })
       .state('offices.activity', {
           url: '/activity',
           templateUrl: 'app/offices/activity.html',
           requireADLogin: true
       })
       .state('offices.artifacts', {
           url: '/artifacts',
           templateUrl: 'app/offices/artifacts.html',
           requireADLogin: true
       })
       .state('offices.moneyflows', {
           url: '/moneyflows',
           templateUrl: 'app/offices/moneyflows.html',
           requireADLogin: true
       })
       .state('alloffices', {
           url: '/alloffices',
           templateUrl: 'app/offices/all-offices.html',
           controller: 'AllOfficesCtrl',
           requireADLogin: true
       })
  });
