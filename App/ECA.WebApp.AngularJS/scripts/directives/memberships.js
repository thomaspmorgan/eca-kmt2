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
          templateUrl: '../views/directives/memberships.html',
          scope: {
              model: '=model',
              personId: '=personid',
              editMode: '@editmode'
          }
      };
      return directive;
  });
