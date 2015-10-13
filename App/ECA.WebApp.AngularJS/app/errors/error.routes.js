'use strict';

angular.module('staticApp')
  .config(function ($stateProvider) {

      $stateProvider.state('error', {
          url: '/error',
          templateUrl: 'app/errors/error.html',
          requireADLogin: false
      })
  });