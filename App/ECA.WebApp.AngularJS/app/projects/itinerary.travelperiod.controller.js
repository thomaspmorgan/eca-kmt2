'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:TravelPeriodCtrl
 * @description
 * # TravelPeriodCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('TravelPeriodCtrl', function (
      $scope,
      $state,
      $stateParams,
      $log,
      $q,
      ProjectService,
      AuthService,
      StateService,
      ConstantsService,
      orderByFilter) {

      $scope.view = {};
      $scope.view.travelPeriod = $scope.$parent.travelPeriod;
  });
