'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:HomeCtrl
 * @description
 * # HomeCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('HomeCtrl', function ($rootScope, $scope, AuthService) {
      $scope.tabs = {
          shortcuts: {
              title: 'Your Shortcuts',
              path: 'shortcuts',
              active: true,
              order: 1
          },
          notfications: {
              title: 'Notifications & Timeline',
              path: 'notifications',
              active: true,
              order: 2
          },
          news: {
              title: 'News (3)',
              path: 'news',
              active: true,
              order: 3
          }
      };
});
