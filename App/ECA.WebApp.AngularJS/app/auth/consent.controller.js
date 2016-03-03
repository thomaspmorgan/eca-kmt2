'use strict';

angular.module('staticApp')
  .controller('ConsentCtrl', function ($scope, $window, AuthService) {
      $scope.continue = function () {
          AuthService.login();
      }

      $scope.disagree = function () {
          $window.location.href = 'http://eca.state.gov';
      }
  });
