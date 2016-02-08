'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:NavigationCtrl
 * @description
 * # NavigationCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('NavigationCtrl', function ($rootScope, $scope, $state, $modal, MessageBox, AuthService, BookmarkService, NotificationService, StateService, BrowserService) {
      $scope.view = {};
      $scope.$watch(
          function () {
              return BrowserService._model.title;
          },
          function (newValue, oldValue) {
              if (newValue !== oldValue) {
                  $scope.view.title = newValue
              }
          });
  });
