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
              model: '=model',
              modelId: '=dependentid'
          }
      };
      return directive;
  });
