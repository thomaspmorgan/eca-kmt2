'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:citizenshipcountries
 * @description
 * # citizenshipcountries
 */
angular.module('staticApp')
  .directive('citizenshipcountries', function ($log) {
      var directive = {
          templateUrl: 'app/directives/citizenship-countries.directive.html',
          scope: {
              addressable: '=model',
              modelId: '=modelid',
              modelType: '=modeltype',
              editMode: '=editmode'
          }
      };
      return directive;
  });
