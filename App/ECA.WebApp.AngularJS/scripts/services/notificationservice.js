'use strict';

/**
 * @ngdoc service
 * @name staticApp.authService
 * @description
 * # authService
 * Factory for handling authorization.
 */
angular.module('staticApp')
  .factory('NotificationService', function ($rootScope, $http, $q, $window, toaster) {
      var service = {};
      service.showMessage = function (type, message) {
          toaster.pop({
              type: type,
              title: '',
              body: message,
              showCloseButton: true
          });
      };
      service.showErrorMessage = function(message){
          service.showMessage('error', message);
      };
      service.showSuccessMessage = function(message){
          service.showMessage('success', message);
      };
      service.showInfoMessage = function (message) {
          service.showMessage('not', message);
      };
      service.showWarningMessage = function (message) {
          service.showMessage('warning', message);
      };
      service.showUnauthorizedMessage = function (message) {
          service.showMessage('warning', message);
      };
      return service;
  });
