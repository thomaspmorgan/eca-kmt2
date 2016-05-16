'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:copyToAllParticipants
 * @description
 * # dependents
 */
angular.module('staticApp')
  .directive('copyToAllParticipants', function () {
      var directive = {
          templateUrl: 'app/directives/copy-to-all-participants.directive.html',
          scope: {
              tooltiptext: '@',
              colorclass: '@'
          },
          compile: function (element, attrs) {
              if (!attrs.tooltiptext) {
                  attrs.tooltiptext = 'Set for other participants';
              }
              if (!attrs.colorclass) {
                  attrs.colorclass = '';
              }
          }
      };
      return directive;
  });
