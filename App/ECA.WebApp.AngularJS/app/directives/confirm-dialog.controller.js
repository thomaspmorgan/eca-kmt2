'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ConfirmCtrl
 * @description The confrim controller is used to prompt a user for response.
 * # ConfirmCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ConfirmCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modalInstance,
        options
        ) {

      if (!options.title) {
          $log.info('title not defined.  Using default title.');
      }
      if (!options.message) {
          $log.info('message not defined.  Using default message.');
      }
      if (!options.okText) {
          $log.info('okText not defined.  Using default ok button text.');
      }
      if (!options.cancelText) {
          $log.info('cancelText not defined.  Using default ok button text.');
      }
      if (!options.okCallback) {
          $log.info('The okCallback function is not defined, using empty function.');
          options.okCallback = function () { };
      }
      if (!options.cancelCallback) {
          $log.info('The cancelCallback function is not defined, using empty function.');
          options.cancelCallback = function () { };
      }
      $scope.title = options.title || 'Confirm';
      $scope.message = options.message || 'Are you sure?';
      $scope.okText = options.okText || 'Ok';
      $scope.cancelText = options.cancelText || 'Cancel';
      $scope.hideCancel = options.hideCancel || false;
      
      $scope.onOkClick = function () {
          $log.info('User confirmed, executing okCallback.');
          $modalInstance.close();
          options.okCallback();
      }
      $scope.onCancelClick = function () {
          $log.info('User canceled, executing cancelCallback.');
          $modalInstance.dismiss('cancel');
          options.cancelCallback();
      }
  });
