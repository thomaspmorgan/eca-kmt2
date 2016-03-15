'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:addresses
 * @description
 * # dependents
 */
angular.module('staticApp')
  .directive('dependents', function ($log) {
      var directive = {
          templateUrl: 'app/directives/dependents.directive.html',
          scope: {
              model: '=model',
              personId: '=personid',
              editMode: '=editmode'
          }
      };
      return directive;
  });
