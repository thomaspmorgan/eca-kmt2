'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('NotificationService', function ($rootScope, $http, $q, $window) {
      var service = {};
      service.showMessage = function (type, message) {
          $rootScope.areAlertsCollapsed = false;
          $rootScope.notifications.push({ type: type, msg: message });
      };
      service.showErrorMessage = function(message){
          service.showMessage('danger', message);
      };
      service.showSuccessMessage = function(message){
          service.showMessage('success', message);
      };
      service.showInfoMessage = function (message) {
          service.showMessage('info', message);
      };
      service.showWarningMessage = function (message) {
          service.showMessage('warning', message);
      };
      service.showUnauthorizedMessage = function (message) {
          service.showMessage('warning', message);
      };
      service.removeAlert = function (index) {
          $rootScope.notifications.splice(index, 1);
          if ($rootScope.notifications.length === 0) {
              $rootScope.areAlertsCollapsed = true;
          }
      };

      return service;
  });
