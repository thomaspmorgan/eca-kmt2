'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive: snapshots
 * @description
 * # data snapshots
 */
angular.module('staticApp')
  .directive('snapshots', function ($log) {
      var directive = {
          templateUrl: 'scripts/directives/snapshots.directive.html',
          scope: {
              model: '=model',
              programId: '=programid'
          }
      };
      return directive;
  });
