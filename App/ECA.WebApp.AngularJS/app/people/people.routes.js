'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('people', {
          url: '/people/:personId',
          templateUrl: 'app/people/people.html',
          controller: 'PeopleCtrl',
          requireADLogin: true
      })
      .state('people.timeline', {
          url: '/timeline',
          templateUrl: 'app/people/timeline.html',
          controller: 'PersonTimelineCtrl',
          requireADLogin: true
      })
      .state('people.personalinformation', {
          url: '/personalinformation',
          templateUrl: 'app/people/personal-information.html',
          controller: 'PersonInformationCtrl',
          requireADLogin: true
      })
      .state('people.personalinformation.pii', {
          url: '/pii',
          templateUrl: 'app/people/personal-information.html',
          controller: 'PersonInformationCtrl',
          requireADLogin: true
      })
      .state('people.personalinformation.general', {
          url: '/general',
          templateUrl: 'app/people/personal-information.html',
          controller: 'PersonInformationCtrl',
          requireADLogin: true
      })
      .state('people.personalinformation.contact', {
          url: '/contact',
          templateUrl: 'app/people/personal-information.html',
          controller: 'PersonInformationCtrl',
          requireADLogin: true
      })
      .state('people.relatedreports', {
          url: '/relatedreports',
          templateUrl: 'app/people/related-reports.html',
          requireADLogin: true
      })
      .state('people.impact', {
          url: '/impact',
          templateUrl: 'app/people/impact.html',
          requireADLogin: true
      })
      .state('people.moneyflows', {
          url: '/moneyflows/:section/:tab',
          templateUrl: 'app/people/moneyflows.html',
          requireADLogin: true
      })
      .state('allpeople', {
          url: '/allpeople',
          templateUrl: 'app/people/all-people.html',
          controller: 'AllPeopleCtrl',
          requireADLogin: true
      })
  });