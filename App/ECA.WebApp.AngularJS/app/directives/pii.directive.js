'use strict';

angular.module('staticApp')
  .directive('pii', function ($log) {
      var directive = {
          restrict: 'E',
          templateUrl: 'app/directives/pii.directive.html',
          scope: {
              personid: '@',
              updatepiicallback: '&'
          },
          controller: function ($scope) {
              $scope.editMode = false;

              $scope.onUpdatePii = function () {
                  $scope.updatepiicallback();
                  $scope.editMode = false;
              }
          }
      };
      return directive;
  });
