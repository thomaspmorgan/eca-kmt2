'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AboutCtrl
 * @description
 * # AboutCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AboutCtrl', function ($rootScope, $scope, $state, $log, AboutService) {
      $scope.view = {};
      $scope.view.isLoading = true;
      $scope.view.versionNumber = '';
      $scope.view.isDebugBuild = false;
      
      function isDebugBuild(about) {
          return about.buildConfiguration === 'Debug';
      }

      AboutService.get()
          .then(function (response) {
              var buildConfiguration = response.data.buildConfiguration;
              var version = response.data.version;
              $scope.view.versionNumber = version;
              $scope.view.isDebugBuild = isDebugBuild(response.data);
              $scope.view.isLoading = false;
          })
          .catch(function (response) {
              $log.error('Unable to load about information.');
              $scope.view.isLoading = false;
          })
  });
