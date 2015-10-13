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

       
        $stateProvider.state('allorganizations', {
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