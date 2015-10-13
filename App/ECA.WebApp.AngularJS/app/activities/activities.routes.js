'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('activities', {
          url: '/activities',
          templateUrl: 'app/activities/list.html',
          requireADLogin: true
      })
      .state('activitiesCreate', {
          url: '/activities/create',
          templateUrl: 'app/activities/create.html',
          requireADLogin: true
      })
      .state('activitiesTag', {
          url: '/activities/tag',
          templateUrl: 'app/activities/tag.html',
          requireADLogin: true
      })
      .state('activitiesOverview', {
          url: '/activities/overview',
          templateUrl: 'app/activities/overview.html',
          requireADLogin: true
      })
  });