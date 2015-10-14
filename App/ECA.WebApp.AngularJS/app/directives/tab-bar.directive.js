'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:tabBar
 * @description
 * # tabBar
 */
angular.module('staticApp')
  .directive('tabBar', function () {
      return {
          templateUrl: 'app/directives/tab-bar.directive.html',
          restrict: 'E',
          replace: true,
          scope: {
              tabs: '=',
              onAdd: '&'
          },
          link: function postLink(scope, el, attrs) {
              if (attrs.tabedit !== undefined) {
                  scope.tabedit = true;
              } else {
                  scope.tabedit = false;
              }
              scope.addTab = function (tab) {
                  tab.active = true;
                  if (scope.onAdd) {
                      scope.onAdd();
                  }
              };
          }
      };
  });
