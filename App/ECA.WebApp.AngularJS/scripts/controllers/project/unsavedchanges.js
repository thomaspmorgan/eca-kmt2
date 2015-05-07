'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('UnsavedChangesCtrl', function ($scope, $modalInstance) {

      $scope.onYesCancelClick = function () {
          $modalInstance.close();
      }

      $scope.onDoNotCancelClick = function () {
          $modalInstance.dismiss('no');
      }
  });
