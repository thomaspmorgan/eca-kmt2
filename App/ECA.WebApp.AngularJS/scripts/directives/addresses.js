'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:addresses
 * @description
 * # addresses
 */
angular.module('staticApp')
  .directive('addresses', function ($log) {
      var directive = {
          templateUrl: '../views/directives/addresses.html',
          scope: {
              addressable: '=model',
              modelId: '=modelid',
              modelType: '=modeltype'
          }
      };
      return directive;
  });
