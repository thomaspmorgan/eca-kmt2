'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:LogoutCtrl
 * @description
 * # LogoutCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('LogoutCtrl', function ($scope, $modalInstance, Idle) {
      $scope.continue = function () {
          Idle.watch();
          $modalInstance.close();
      }
  });
