'use strict';

angular.module('staticApp')
  .directive('contact', function ($log) {
      var directive = {
          restrict: 'E',
          templateUrl: 'app/directives/contact.directive.html',
          scope: {
              personid: '@',
          },
          controller: function ($scope) {
              $scope.editMode = false;
          }
      };
      return directive;
  });
