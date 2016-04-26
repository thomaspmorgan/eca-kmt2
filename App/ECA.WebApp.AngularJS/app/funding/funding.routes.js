'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {
      $stateProvider.state('allfunding', {
          url: '/allfunding',
          templateUrl: 'app/funding/all-funding.html',
          controller: 'AllFundingCtrl',
          requireADLogin: true
      })
        .state('funding', {
            url: '/funding/:organizationId',
            templateUrl: 'app/funding/funding.html',
            controller: 'FundingCtrl',
            requireADLogin: true
        })
        .state('funding.overview', {
            url: '/overview',
            templateUrl: 'app/funding/overview.html',
            controller: 'FundingOverviewCtrl',
            requireADLogin: true
        })
        .state('funding.artifacts', {
            url: '/artifacts',
            templateUrl: 'app/funding/artifacts.html',
            controller: 'FundingArtifactsCtrl',
            requireADLogin: true
        })
        .state('funding.activities', {
            url: '/activities',
            templateUrl: 'app/funding/activities.html',
            controller: 'FundingActivitiesCtrl',
            requireADLogin: true
        })
        .state('funding.moneyflows', {
            url: '/moneyflows',
            templateUrl: 'app/funding/moneyflows.html',
            requireADLogin: true
        })
  });
