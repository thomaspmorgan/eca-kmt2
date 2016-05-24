'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:pointsofcontact
 * @description
 * # project points of contact
 */
angular.module('staticApp')
  .directive('pointsofcontact', function ($log) {
      var directive = {
          templateUrl: 'app/directives/points-of-contact.directive.html',
          scope: {
              model: '=model',
              editMode: '=editmode'
          }
      };
      return directive;
  });
