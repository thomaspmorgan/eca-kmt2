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
      templateUrl: 'views/partials/tabbar.html',
      restrict: 'E',
      scope: {
      	tabs: '=',
        onAdd: '&'
      },
      replace: true,
      link: function postLink(scope, el, attrs) {
        if (attrs.edit !== undefined) {
          scope.edit = true;
        } else {
          scope.edit = false;
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
