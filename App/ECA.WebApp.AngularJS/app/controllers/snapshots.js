'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: SnapshotsCtrl
 * # SnapshotsCtrl
 * Controller for snapshot statistics
 */
angular.module('staticApp')
  .controller('SnapshotsCtrl', function ($scope,
      $stateParams,
      $q,
      $log,
      SnapshotService,
      NotificationService) {
      
      $scope.view = {};
      //$scope.view.isSnapshotLoading = false;
      $scope.view.params = $stateParams;
      $scope.view.snapshot = {};

      //$scope.programIdDeferred = $q.defer();

      //$scope.programIdDeferred.promise
      //.then(function () {
      //    loadSnapshots();
      //});

      $scope.init = function () {
          //$scope.isSnapshotLoading = true;
          SnapshotService.getProgramSnapshot($scope.view.params.programId)
          .then(function (response) {
              $scope.view.snapshot = response.data;
              //$scope.programIdDeferred.resolve(response.data.programId);
              //$scope.isSnapshotLoading = false;
          })
          .catch(function () {
              var message = 'Unable to load shapshot.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              //$scope.isSnapshotLoading = false;
          });
      };

      $scope.init();
      
  });