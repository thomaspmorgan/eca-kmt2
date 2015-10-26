'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive: snapshots-graphs
 * @description
 * # data snapshots-graphs
 */
angular.module('staticApp')
  .directive('snapshotsGraphs', function () {
      var directive = {
          templateUrl: 'app/directives/snapshots-graphs.directive.html',
          scope: {
              model: '=model',
              programId: '=programid'
          }
      };
      return directive;
  });
