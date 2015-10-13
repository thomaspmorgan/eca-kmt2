'use strict';

angular.module('staticApp')
  .config(function ($stateProvider, $httpProvider, $urlRouterProvider, adalAuthenticationServiceProvider) {

      adalAuthenticationServiceProvider.init({
          base: '',
          tenant: 'statedept.us',
          clientId: 'e0356e55-e124-452c-837d-aeb7504185ff',
          resource: 'https://ecaserver.state.gov'
      }, $httpProvider
      );

      $urlRouterProvider.otherwise('/');

        $stateProvider.state('projects', {
            url: '/projects/:projectId',
            templateUrl: 'views/project.html',
            controller: 'ProjectCtrl',
            requireADLogin: true
        })
        .state('projects.overview', {
            url: '/overview',
            templateUrl: 'views/project/overview.html',
            controller: 'ProjectOverviewCtrl',
            requireADLogin: true
        })
        .state('projects.edit', {
            url: '/edit',
            templateUrl: 'views/project/edit.html',
            controller: 'ProjectEditCtrl',
            requireADLogin: true
        })
        .state('projects.participants', {
            url: '/participant',
            templateUrl: 'views/project/participant.html',
            controller: 'ProjectParticipantCtrl',
            requireADLogin: true
        })
        .state('projects.artifacts', {
            url: '/artifact',
            templateUrl: 'views/project/artifact.html',
            requireADLogin: true
        })
        .state('projects.activity', {
            url: '/activity',
            templateUrl: 'views/project/activity.html',
            requireADLogin: true
        })
        .state('projects.itineraries', {
            url: '/itinerary',
            templateUrl: 'views/project/itinerary.html',
            requireADLogin: true
        })
        .state('projects.implementers', {
            url: '/implementers',
            templateUrl: 'views/project/implementer.html',
            requireADLogin: true
        })
        .state('projects.partners', {
            url: '/partners',
            templateUrl: 'views/project/partners.html',
            requireADLogin: true
        })

        .state('projects.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/project/moneyflows.html',
            requireADLogin: true
        })

        .state('projects.impact', {
            url: '/impact',
            templateUrl: 'views/project/impact.html',
            requireADLogin: true
        })

        .state('offices', {
            url: '/offices/:officeId',
            templateUrl: 'views/offices.html',
            controller: 'OfficeCtrl',
            requireADLogin: true
        })
        .state('offices.overview', {
            url: '/overview',
            templateUrl: 'views/office/overview.html',
            controller: 'OfficeOverviewCtrl',
            requireADLogin: true
        })
        .state('offices.branches', {
            url: '/branches',
            templateUrl: 'views/office/branches.html',
            controller: 'BranchesAndProgramsCtrl',
            requireADLogin: true
        })
        .state('offices.activity', {
            url: '/activity',
            templateUrl: 'views/office/activity.html',
            requireADLogin: true
        })
        .state('offices.artifacts', {
            url: '/artifacts',
            templateUrl: 'views/office/artifacts.html',
            requireADLogin: true
        })
        .state('offices.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/office/moneyflows.html',
            requireADLogin: true
        })

        .state('alloffices', {
            url: '/alloffices',
            templateUrl: 'views/office/alloffices.html',
            controller: 'AllOfficesCtrl',
            requireADLogin: true
        })
        .state('allorganizations', {
            url: '/allorganizations',
            templateUrl: 'views/organizations/allorganizations.html',
            controller: 'AllOrganizationsCtrl',
            requireADLogin: true
        })
        .state('organizations', {
            url: '/organizations/:organizationId',
            templateUrl: 'views/organization.html',
            controller: 'OrganizationCtrl',
            requireADLogin: true
        })
        .state('organizations.overview', {
            url: '/overview',
            templateUrl: 'views/organizations/overview.html',
            controller: 'OrganizationOverviewCtrl',
            requireADLogin: true
        })
        .state('organizations.artifacts', {
            url: '/artifacts',
            templateUrl: 'views/organizations/artifacts.html',
            controller: 'OrganizationArtifactsCtrl',
            requireADLogin: true
        })
        .state('organizations.impact', {
            url: '/impact',
            templateUrl: 'views/organizations/impact.html',
            controller: 'OrganizationImpactCtrl',
            requireADLogin: true
        })
        .state('organizations.activities', {
            url: '/activities',
            templateUrl: 'views/organizations/activities.html',
            controller: 'OrganizationActivitiesCtrl',
            requireADLogin: true
        })
        .state('organizations.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'views/organizations/moneyflows.html',
            requireADLogin: true
        })
        .state('partner', {
            url: '/partner',
            templateUrl: 'views/partner.html',
            requireADLogin: true
        })

        .state('offshoot', {
            url: '/clearance',
            templateUrl: 'views/offshoot.html',
            requireADLogin: true
        })

        .state('lastupdated', {
            url: '/lastupdated',
            templateUrl: 'views/lastupdated.html',
            requireADLogin: true
        });
      $httpProvider.interceptors.push('ErrorInterceptor');
  });