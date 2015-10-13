'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive: snapshots
 * @description
 * # data snapshots
 */
angular.module('staticApp')
  .directive('snapshots', function () {
      return {
          templateUrl: 'scripts/directives/snapshots.directive.html',
          scope: {
              programId: '=programid'
          }
      };
  });
