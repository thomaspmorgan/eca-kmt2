'use strict';

angular.module('staticApp')
  .config(function ($httpProvider, $urlRouterProvider, adalAuthenticationServiceProvider, insightsProvider, $locationProvider) {

      adalAuthenticationServiceProvider.init({
          base: '',
          tenant: 'statedept.us',
          clientId: 'e0356e55-e124-452c-837d-aeb7504185ff',
          resource: 'https://ecaserver.state.gov'
      }, $httpProvider
      );

      if (location.hostname === 'localhost') {
          insightsProvider.config('088b7134-7c47-4104-a6a6-26e2f518c289','kmt-local');
      }
      else if (location.hostname.indexOf('qa') !== -1) {
          insightsProvider.config('7d4d8e58-a8a0-4776-8a3f-75df53085597','kmt-qa');
      }
      else if (location.hostname.indexOf('dev') !== -1) {
          insightsProvider.config('92931a99-4371-4e53-b056-9a6dde55614e','kmt-dev');
      }
      else if (location.hostname.indexOf('pre') !== -1) {
          insightsProvider.config('82d73d1c-1d3a-4938-8b37-6aa512792b88','kmt-pre');
      }
      else {
          insightsProvider.config('969bd1b5-3eaf-4149-b62c-05f4a970acef','kmt-prod');
      }

      $locationProvider.html5Mode(true).hashPrefix('!');

      $urlRouterProvider.otherwise('/');

      $httpProvider.interceptors.push('ErrorInterceptor');
  });