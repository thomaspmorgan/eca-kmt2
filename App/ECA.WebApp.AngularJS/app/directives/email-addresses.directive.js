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
          templateUrl: 'app/directives/email-addresses.directive.html',
          scope: {
              emailAddressable: '=model',
              modelId: '=modelid',
              modelType: '=modeltype',
              editMode: '=editmode'
          }
      };
      return directive;
      
  });
