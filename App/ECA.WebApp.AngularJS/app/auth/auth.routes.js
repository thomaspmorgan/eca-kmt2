'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('forbidden', {
          url: '/forbidden',
          templateUrl: 'app/auth/forbidden.html',
          requireADLogin: false
      })
      .state('error', {
          url: '/error',
          templateUrl: 'app/auth/error.html',
          requireADLogin: false
      })
  });