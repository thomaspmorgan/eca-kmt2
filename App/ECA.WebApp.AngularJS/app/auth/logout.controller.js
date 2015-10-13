'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:LogoutCtrl
 * @description
 * # LogoutCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('LogoutCtrl', function (authService,$state) {
  	authService.logout();
  	$state.go('login');
  });
