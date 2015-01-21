'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:LoginCtrl
 * @description
 * # LoginCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('LoginCtrl', function ($scope,authService,$state) {
  	$scope.login = '';
  	$scope.pw = '';
  	$scope.incorrectLogin = '';
  	$scope.authorize = function(){
  		authService.login($scope.login,$scope.pw).then(
  			function(){
  				$scope.incorrectLogin = '';
  				// TODO - make this redirect to the page that the user originally wanted to log in from
  				$state.go('home.shortcuts');
  			},
  			function(err){
  				$scope.incorrectLogin = 'That user name and password didn\'t work.';
  			}
  		);
  	};
  });
