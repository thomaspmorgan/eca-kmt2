'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:SelectSevisAccountCtrl
 * @description The SelectSevisAccountCtrl is used to select a sevis account.
 * # AddProjectCollaboratorCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SelectSevisAccountCtrl', function ($scope,
      $stateParams,
      $q,
      userInfo,
      $modalInstance,
      LookupService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.userInfo = userInfo;
      $scope.view.selectedSevisAccount = null;
      

      $scope.view.onCancelClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onOkClick = function () {
          $modalInstance.close($scope.view.selectedSevisAccount);
      }

  });
