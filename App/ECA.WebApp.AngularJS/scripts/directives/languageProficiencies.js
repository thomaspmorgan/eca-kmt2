'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:languageProficiencies
 * @description
 * # languageProficiencies
 */
angular.module('staticApp')
  .directive('languageproficiencies', function ($log) {
      var directive = {
          templateUrl: '../views/directives/languageProficiencies.html',
          scope: {
              model: '=model',
              personId: '=personid',
              editMode: '=editmode'
          }
      };
      return directive;
  });
