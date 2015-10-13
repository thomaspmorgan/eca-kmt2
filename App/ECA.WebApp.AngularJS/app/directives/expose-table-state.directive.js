'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:exposetablestate
 * @description
 * # exposetablestate
 */
angular.module('staticApp')
  .directive('exposetablestate', function ($log, ConstantsService) {
      var directive = {
          restrict: 'A',
          require: 'stTable',
          
          link: function postLink(scope, element, attrs, ctrl) {
              var fnName = attrs['exposetablestate'];
              if (scope[fnName]) {
                  throw Error('The object named [' + fnName + '] is already defined on the scope object.  Provide another name for the scope method to retrieve the current table state.');
              }
              scope[fnName] = function () {
                  return ctrl.tableState();
              }
          }
      };
      return directive;
  });
