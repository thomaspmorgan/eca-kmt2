'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:emailAddresses
 * @description
 * # emailAddresses
 */
angular.module('staticApp')
  .directive('emailaddresses', function ($log) {
      var directive = {
          templateUrl: '../views/directives/emailAddresses.html',
          scope: {
              emailAddressable: '=model',
              modelId: '=modelid',
              modelType: '=modeltype',
              editMode: '=editmode'
          },
          link: link
      };
      return directive;
      
      function link(scope, $el, attrs) {
          var edit = attrs.editmode;
      }
  });
