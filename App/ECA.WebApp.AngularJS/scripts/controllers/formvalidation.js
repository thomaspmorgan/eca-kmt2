'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:FormvalidationCtrl
 * @description
 * # FormvalidationCtrl
 * Controller of the staticApp
 */
 angular.module('staticApp')
 .controller('FormvalidationCtrl', function ($scope) {
 	$scope.master = {};

 	$scope.update = function(user) {
 		$scope.master = angular.copy(user);
 	};

 	$scope.reset = function() {
 		$scope.user = angular.copy($scope.master);
 	};

 	$scope.isUnchanged = function(user) {
 		return angular.equals(user, $scope.master);
 	};

 	$scope.reset();
 });
