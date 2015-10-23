'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive: snapshots
 * @description
 * # data snapshots
 */
angular.module('staticApp')
  .directive('snapshots', function () {
      var directive = {
          templateUrl: 'app/directives/snapshots.directive.html',
          scope: {
              model: '=model',
              programId: '=programid'
          }
      };
      return directive;
  });
