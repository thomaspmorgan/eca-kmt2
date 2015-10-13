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

        $stateProvider.state('partner', {
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