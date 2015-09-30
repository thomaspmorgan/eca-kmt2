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
          templateUrl: 'scripts/directives/addresses.directive.html',
          scope: {
              addressable: '=model',
              modelId: '=modelid',
              modelType: '=modeltype',
              editMode: '=editmode'
          }
      };
      return directive;
  });
