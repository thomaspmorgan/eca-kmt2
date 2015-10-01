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

angular.module('staticApp')
    .directive('showonparentrow',
   function () {
       return { link: function (scope, element, attrs) {
           var row = element.closest('tr');
            row.bind('mouseenter', function () { element.show(); });
            row.bind('mouseleave', function () { element.hide(); });
               
           }
       };
   });

/*
 * Sets focus on specific input (usage: focus-me="true")
 */
angular.module('staticApp')
  .directive('focusMe', function ($timeout) {
      return {
          scope: { trigger: '@focusMe' },
          link: function (scope, element) {
              scope.$watch('trigger', function (value) {
                  if (value === "true") {
                      $timeout(function () {
                          element[0].focus();
                      });
                  }
              });
          }
      };
  });
