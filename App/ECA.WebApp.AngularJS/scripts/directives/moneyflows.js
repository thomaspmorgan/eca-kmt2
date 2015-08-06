'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:addresses
 * @description
 * # addresses
 */
angular.module('staticApp')
  .directive('moneyflows', function ($log) {
      var directive = {
          templateUrl: '../views/directives/moneyflows.html',
          scope: {
              stateParamName: '=stateparamname',
              sourceEntityTypeId: '=sourceentitytypeid',
              resourceTypeId: '=resourcetypeid'
          }
      };
      return directive;
  });
