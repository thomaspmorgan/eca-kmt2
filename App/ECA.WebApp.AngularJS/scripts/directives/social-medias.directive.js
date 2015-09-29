'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:socialmedias
 * @description
 * # socialmedias
 */
angular.module('staticApp')
  .directive('socialmedias', function ($log) {
      var directive = {
          templateUrl: '../views/directives/socialmedias.html',
          scope: {
              socialable: '=model',
              modelId: '=modelid',
              modelType: '=modeltype',
              editMode: '=editmode'
          }
      };
      return directive;
  });
