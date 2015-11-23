'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:exposetablestate
 * @description
 * # exposetablestate
 */
angular.module('staticApp')
  .directive('exposetablestate', function ($log, $parse, ConstantsService) {
      var directive = {
          restrict: 'A',
          require: 'stTable',
          
          link: function postLink(scope, element, attrs, ctrl) {
              var fnName = attrs['exposetablestate'];
              var exposeTargetStateTo = attrs['exposetablestateto'];

              var targetScope = scope;
              if (angular.isDefined(exposeTargetStateTo)) {
                  var fn = $parse(exposeTargetStateTo);
                  targetScope = fn(scope, {});
                  console.assert(targetScope, 'The target scope must exist.');
                  $log.info('Scope overridden in expose table state directive.  Expose table state function will be added to scope.');
              }

              if (targetScope[fnName]) {
                  throw Error('The object named [' + fnName + '] is already defined on the scope object.  Provide another name for the scope method to retrieve the current table state.');
              }
              targetScope[fnName] = ctrl.tableState;
          }
      };
      return directive;
  });
