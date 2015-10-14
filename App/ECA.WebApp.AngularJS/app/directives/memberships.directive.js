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
          templateUrl: 'app/directives/memberships.directive.html',
          scope: {
              model: '=model',
              personId: '=personid',
              editMode: '=editmode'
          }
      };
      return directive;
  });
