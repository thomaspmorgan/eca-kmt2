'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:memberships
 * @description
 * # memberships
 */
angular.module('staticApp')
  .directive('memberships', function ($log) {
      var directive = {
          templateUrl: 'scripts/directives/memberships.directive.html',
          scope: {
              model: '=model',
              personId: '=personid',
              editMode: '=editmode'
          }
      };
      return directive;
  });
