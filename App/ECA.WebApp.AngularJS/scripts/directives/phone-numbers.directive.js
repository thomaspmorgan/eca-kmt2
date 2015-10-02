'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:phoneNumbers
 * @description
 * # phoneNumbers
 */
angular.module('staticApp')
  .directive('phonenumbers', function ($log) {
      var directive = {
          templateUrl: 'scripts/directives/phone-numbers.directive.html',
          scope: {
              phoneNumberable: '=model',
              modelId: '=modelid',
              modelType: '=modeltype',
              editMode: '=editmode'
          }
      };
      return directive;
      
  });
