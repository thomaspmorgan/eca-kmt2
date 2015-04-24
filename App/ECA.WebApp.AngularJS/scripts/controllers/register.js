'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('RegisterCtrl', function ($scope, $stateParams, $q, $log, AuthService) {

      $scope.view = {};
      $scope.view.isRegistering = true;
      $scope.user = {};
      $scope.user.displayName = "";

      $q.when(AuthService.register()).
          then(function () {
              $q.when(AuthService.getUserInfo())
              .then(function (response) {
                  var userInfo = response.data;
                  $scope.user.displayName = userInfo.displayName;
                  $scope.view.isRegistering = false;
              }, function (errorResponse) {

              })

          }, function () {
              console.log('Unable to register user.');
          });

  });
