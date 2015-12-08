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
      LocationService,
      AuthService,
      StateService,
      ConstantsService,
      orderByFilter) {

      $scope.view = {};
      $scope.view.travelPeriod = $scope.$parent.travelPeriod;
      $scope.view.project = null;

      $scope.view.onEditClick = function (travelPeriod) {
          $log.info('edit');
      }

      $scope.view.onCommentClick = function (travelPeriod) {
          $log.info('comment');
      }

      $scope.view.onDeleteClick = function (travelPeriod) {
          $log.info('delete');
      }

      $scope.$parent.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.project = project;
      });
  });
