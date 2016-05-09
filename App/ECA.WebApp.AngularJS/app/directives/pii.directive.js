'use strict';

angular.module('staticApp')
  .directive('pii', function ($log) {
      var directive = {
          restrict: 'E',
          templateUrl: 'app/directives/pii.directive.html',
          scope: {
              personid: '@'
          }
      };
      return directive;
  });
