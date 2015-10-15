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
      $scope.view.isSnapshotLoading = false;
      $scope.view.params = $stateParams;
      $scope.view.snapshot = {};

      $scope.init = function () {
          $scope.view.isSnapshotLoading = true;
          SnapshotService.getProgramSnapshot($scope.view.params.programId)
          .then(function (response) {
              $scope.view.snapshot = response.data;
              if ($scope.view.snapshot.budget !== null && $scope.view.snapshot.participants !== null) {
                  $scope.view.snapshot.costPerParticipant = $scope.view.snapshot.budget / $scope.view.snapshot.participants;
              }
              $scope.view.isSnapshotLoading = false;
          })
          .catch(function () {
              var message = 'Unable to load shapshot.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.view.isSnapshotLoading = false;
          });
      };

      $scope.init();
      
  });