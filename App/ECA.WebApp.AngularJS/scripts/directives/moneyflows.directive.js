﻿'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:addresses
 * @description
 * # addresses
 */
angular.module('staticApp')
  .directive('moneyflows', function ($log) {
      var directive = {
          templateUrl: 'scripts/directives/moneyflows.directive.html',
          scope: {
              stateParamName: '=stateparamname',
              sourceEntityTypeId: '=sourceentitytypeid',
              resourceTypeId: '=resourcetypeid',
              entityName: '=entityname'
          }
      };
      return directive;
  });