'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

    $stateProvider.state('projects', {
        url: '/projects/:projectId',
        templateUrl: 'app/projects/project.html',
        controller: 'ProjectCtrl',
        requireADLogin: true
    })
    .state('projects.overview', {
        url: '/overview',
        templateUrl: 'app/projects/overview.html',
        controller: 'ProjectOverviewCtrl',
        requireADLogin: true
    })
    .state('projects.edit', {
        url: '/edit',
        templateUrl: 'app/projects/edit.html',
        controller: 'ProjectEditCtrl',
        requireADLogin: true
    })
    .state('projects.artifacts', {
        url: '/artifact',
        templateUrl: 'app/projects/artifact.html',
        requireADLogin: true
    })
    .state('projects.activity', {
        url: '/activity',
        templateUrl: 'app/projects/activity.html',
        requireADLogin: true
    })
    .state('projects.itineraries', {
        url: '/itinerary',
        templateUrl: 'app/projects/itineraries.html',
        requireADLogin: true
    })
    .state('projects.implementers', {
        url: '/implementers',
        templateUrl: 'app/projects/implementer.html',
        requireADLogin: true
    })
    .state('projects.partners', {
        url: '/partners',
        templateUrl: 'app/projects/partners.html',
        requireADLogin: true
    })
    .state('projects.participants', {
        url: '/participant?section&tab&participantId',
        params: {
            section: null,
            tab: null,
            participantId: null
        },
        templateUrl: 'app/projects/participant.html',
        controller: 'ProjectParticipantCtrl',
        requireADLogin: true
    })
    .state('projects.moneyflows', {
        url: '/moneyflows',
        templateUrl: 'app/projects/moneyflows.html',
        requireADLogin: true
    })
    .state('projects.impact', {
        url: '/impact',
        templateUrl: 'app/projects/impact.html',
        requireADLogin: true
    })
  });
