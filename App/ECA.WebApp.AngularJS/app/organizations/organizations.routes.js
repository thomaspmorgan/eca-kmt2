'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {
      $stateProvider.state('allorganizations', {
          url: '/allorganizations',
          templateUrl: 'app/organizations/all-organizations.html',
          controller: 'AllOrganizationsCtrl',
          requireADLogin: true
      })
        .state('organizations', {
            url: '/organizations/:organizationId',
            templateUrl: 'app/organizations/organization.html',
            controller: 'OrganizationCtrl',
            requireADLogin: true
        })
        .state('organizations.overview', {
            url: '/overview',
            templateUrl: 'app/organizations/overview.html',
            controller: 'OrganizationOverviewCtrl',
            requireADLogin: true
        })
        .state('organizations.artifacts', {
            url: '/artifacts',
            templateUrl: 'app/organizations/artifacts.html',
            controller: 'OrganizationArtifactsCtrl',
            requireADLogin: true
        })
        .state('organizations.impact', {
            url: '/impact',
            templateUrl: 'app/organizations/impact.html',
            controller: 'OrganizationImpactCtrl',
            requireADLogin: true
        })
        .state('organizations.activities', {
            url: '/activities',
            templateUrl: 'app/organizations/activities.html',
            controller: 'OrganizationActivitiesCtrl',
            requireADLogin: true
        })
        .state('organizations.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'app/organizations/moneyflows.html',
            requireADLogin: true
        })
  });
