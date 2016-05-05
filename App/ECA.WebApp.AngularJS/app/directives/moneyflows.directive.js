'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:moneyflows
 * @description
 * # moneyflows
 */
angular.module('staticApp')
  .directive('moneyflows', function ($log) {
      var directive = {
          templateUrl: 'app/directives/moneyflows.directive.html',
          scope: {
              stateParamName: '=stateparamname',
              sourceEntityTypeId: '=sourceentitytypeid',
              resourceTypeId: '=resourcetypeid',
              entityName: '=entityname',
              entityparentid: '=entityparentid'
          },
          link: function (scope, element, attrs) {

          }
      };
      return directive;
  });
