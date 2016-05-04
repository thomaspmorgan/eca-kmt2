'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:SelectSevisAccountCtrl
 * @description The SelectSevisAccountCtrl is used to select a sevis account.
 * # AddProjectCollaboratorCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SevisBatchProcessingInfoModalCtrl', function ($scope,
      $stateParams,
      $q,
      $log,
      sevisCommStatus,
      projectId,
      $modalInstance,
      ParticipantPersonsSevisService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.sevisCommStatus = sevisCommStatus;
      $scope.view.details = null;
      $scope.view.isLoadingDetails = false;

      $scope.view.onCloseClick = function () {
          $modalInstance.close();
      }

      $scope.view.isLoadingDetails = true;
      ParticipantPersonsSevisService.getBatchInfo(projectId, sevisCommStatus.participantId, sevisCommStatus.batchId)
      .then(function (response) {
          $scope.view.isLoadingDetails = false;
          $scope.view.details = response.data;
      })
      .catch(function (response) {
          $scope.view.isLoadingDetails = false;
          var message = 'Unable to load batch details.';
          $log.error(message);
      });
  });
