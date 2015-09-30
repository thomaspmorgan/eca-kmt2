'use strict';

/**
 * @ngdoc service
 * @name staticApp.NotificationService
 * @description
 * # NotificationService
 * Factory for handling notifications.
 */
angular.module('staticApp')
  .factory('MessageBox', function ($rootScope, $http, $q, $window, $log, $modal) {
      var service = {

          /**
          Displays a confirmation dialog box with the given options.  The options object may specify a title, message, okText, cancelText, okCallback and cancelCallback.
          */
          confirm: function (options) {
              var opt = options || {};
              var modalInstance = $modal.open({
                  animation: true,
                  templateUrl: 'scripts/directives/confirm-dialog.directive.html',
                  controller: 'ConfirmCtrl',
                  resolve: {
                      options: function () {
                          return opt;
                      }
                  }
              });
              modalInstance.result.then(function () {
                  $log.info('User confirmed.');

              }, function () {
                  $log.info('Modal dismissed at: ' + new Date());
              });
              return modalInstance;
          }
      };
      return service;
  });
