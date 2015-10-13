'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:SettingsCtrl
 * @description
 * # SettingsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SettingsCtrl', function ($scope, $q, DownloadService, ConstantsService, AuthService, NotificationService) {
      
      $scope.logsView = {};
      $scope.logsView.isDownloading = false;      

      $scope.downloadLogs = function () {
          $scope.logsView.isDownloading = true;
          DownloadService.get('logs/all', 'application/zip', 'logs.zip')
          .then(function() {
              
          }, function () {
              NotificationService.showErrorMessage('Unable to download the logs.');
          })
          .then(function() {
              $scope.logsView.isDownloading = false;
          });
      }
  });
