'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:addresses
 * @description
 * # dependents
 */
angular.module('staticApp')
  .directive('ecaRequired', function () {
      var directive = {
          templateUrl: 'app/directives/eca-required.directive.html',
          scope: {
              tooltiptext: '@',
              iconfontname: '@',
              colorclass: '@'
          },
          compile: function (element, attrs) {
              if (!attrs.tooltiptext) {
                  attrs.tooltiptext = 'Required';
              }
              if (!attrs.iconfontname) {
                  attrs.iconfontname = 'label';
              }
              if (!attrs.colorclass) {
                  attrs.colorclass = 'asterisk';
              }
          }
      };
      return directive;
  });
