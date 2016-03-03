'use strict';

angular.module('staticApp')
  .controller('ConsentCtrl', function ($scope, AuthService) {
      $scope.login = function () {
          AuthService.login();
      }
  });
