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
          replace: true,
          link: function postLink(scope, el, attrs) {
              if (attrs.edit !== undefined) {
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
