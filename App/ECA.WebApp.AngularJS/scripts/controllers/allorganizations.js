'use strict';

/**
 * @ngdoc function
* @name staticApp.controller:AllOrganizationsCtrl
 * @description
 * # AllOrganizationsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOrganizationsCtrl', function ($scope, $stateParams) {

      $scope.organizations = [];
      $scope.start = 0;
      $scope.end = 0;
      $scope.total = 0;
  });