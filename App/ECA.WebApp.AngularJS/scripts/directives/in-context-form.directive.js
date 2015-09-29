'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:inContextForm
 * @description
 * # inContextForm
 */
angular.module('staticApp')
  .directive('inContextForm', function () {
    return {
      restrict: 'A',
      controller: function ($scope) {
        $scope.guidance = {};
        $scope.guidance.text = '';
      	this.changeGuidance = function (text, element) {
      		$scope.guidance.offset = element.offsetTop;
      		$scope.guidance.text = text;
          $scope.$broadcast('guidanceChanged');
      	};
      }
    };
  });
