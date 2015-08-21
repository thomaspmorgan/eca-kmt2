'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:memberships
 * @description
 * # memberships
 */
angular.module('staticApp')
  .directive('language', function ($log) {
      var directive = {
          templateUrl: '../views/directives/languangeProficiencies.html',
          scope: {
              model: '=model',
              personId: '=personid',
              editMode: '@editmode'
          }
      };
      return directive;
  });
