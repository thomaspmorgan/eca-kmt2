'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('forbidden', {
          url: '/forbidden',
          templateUrl: 'app/auth/forbidden.html',
          requireADLogin: false
      })
      .state('consent', {
          url: '/consent',
          controller: 'ConsentCtrl',
          templateUrl: 'app/auth/consent.html',
          requireADLogin: false
      })

  });