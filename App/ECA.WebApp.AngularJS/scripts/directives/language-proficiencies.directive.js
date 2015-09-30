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
          templateUrl: 'language-proficiencies.directive.html',
          scope: {
              model: '=model',
              personId: '=personid',
              editMode: '=editmode'
          }
      };
      return directive;
  });
