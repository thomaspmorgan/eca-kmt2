'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:formElement
 * @description
 * # formElement
 */
angular.module('staticApp')
  .directive('formElement', function () {
    return {
      restrict: 'A',
      require: '^inContextForm',
      link: function postLink(scope, element, attrs, parentCtrl) {
      	var event = attrs.formElement || 'focus';
        element.on(event, function() {
        	parentCtrl.changeGuidance(attrs.guidance, element[0]);
        });
      }
    };
  });
