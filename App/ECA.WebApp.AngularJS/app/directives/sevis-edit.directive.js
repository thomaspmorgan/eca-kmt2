'use strict';

angular.module('staticApp')
  .directive('sevisEdit', function ($log) {
      var directive = {
          restrict: 'E',
          templateUrl: 'app/directives/sevis-edit.directive.html',
          scope: {
              editLocked: '=ngDisabled'
          }
      };
      return directive;
  });