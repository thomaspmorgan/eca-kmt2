'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:staticSwitcher
 * @description
 * # staticSwitcher
 */
angular.module('staticApp')
  .directive('staticSwitcher', function () {
    return {
      template: '<img ng-click="nextImage()" class="" ng-src="{{currentImage}}">',
      restrict: 'E',
      scope: {
      	images: '='
      },
      link: function postLink(scope) {
      	var i = 0;
      	scope.currentImage = scope.images[i];
      	scope.nextImage = function () {
      		i++;
      		scope.currentImage = scope.images[i];
      	};
      }
    };
  });
