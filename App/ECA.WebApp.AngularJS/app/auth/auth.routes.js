'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('forbidden', {
          url: '/forbidden',
          templateUrl: 'app/auth/forbidden.html',
          requireADLogin: false
      })
  });