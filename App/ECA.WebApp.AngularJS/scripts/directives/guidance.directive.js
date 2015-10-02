'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:guidance
 * @description
 * # guidance
 */
angular.module('staticApp')
  .directive('guidance', function () {
    return {
    	require: '^inContextForm',
      template: '<div class="contextContainer"></div>',
      restrict: 'E',
      replace: true,
      link: function postLink(scope, element) {
        scope.$on('guidanceChanged', function () {
		      element.text(scope.guidance.text);
		      element.css({'margin-top': scope.guidance.offset+'px'});
        });
      }
    };
  });
