'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

    $stateProvider.state('allprograms', {
        url: '/allprograms',
        templateUrl: 'app/programs/all-programs.html',
        controller: 'AllProgramsCtrl',
        requireADLogin: true
    })
    .state('programs', {
        url: '/programs/:programId',
        templateUrl: 'app/programs/programs.html',
        controller: 'ProgramCtrl',
        requireADLogin: true
    })
    .state('programs.overview', {
        url: '/overview',
        controller: 'ProgramOverviewCtrl',
        templateUrl: 'app/programs/overview.html',
        requireADLogin: true
    })
    .state('programs.edit', {
        url: '/edit',
        controller: 'ProgramEditCtrl',
        templateUrl: 'app/programs/edit.html',
        requireADLogin: true
    })
    .state('programs.projects', {
        url: '/projects',
        templateUrl: 'app/programs/projects.html',
        controller: 'SubProgramsAndProjectsCtrl',
        requireADLogin: true
    })

    .state('programs.activity', {
        url: '/activity',
        templateUrl: 'app/programs/activity.html',
        requireADLogin: true
    })
    .state('programs.artifacts', {
        url: '/artifacts',
        templateUrl: 'app/programs/artifacts.html',
        requireADLogin: true
    })

    .state('programs.impact', {
        url: '/impact',
        templateUrl: 'app/programs/impact.html',
        requireADLogin: true
    })
    .state('programs.moneyflows', {
        url: '/moneyflows',
        templateUrl: 'app/programs/moneyflows.html',
        requireADLogin: true
    })
  });
