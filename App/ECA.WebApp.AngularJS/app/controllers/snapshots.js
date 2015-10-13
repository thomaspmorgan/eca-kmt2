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
      PersonService,
      SnapshotService,
      NotificationService) {
      
      $scope.view = {};
      $scope.view.isSnapshotLoading = false;
      $scope.view.params = $stateParams;
      $scope.view.snapshot = {};

      $scope.programIdDeferred.promise
      .then(function (programId) {
          loadSnapshots(programId);
      });

      function loadSnapshots(programId) {
          $scope.view.isSnapshotLoading = true;
          SnapshotService.getProgramSnapshot(programId)
          .then(function (data) {
              $scope.view.snapshot = data;
              $scope.view.isSnapshotLoading = false;
          })
          .catch(function () {
              var message = 'Unable to load shapshot.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.view.isSnapshotLoading = false;
          });
      };


  });