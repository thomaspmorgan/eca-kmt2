'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:SettingsCtrl
 * @description
 * # SettingsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SettingsCtrl', function ($scope) {
    $scope.awesomeThings = [
      'HTML5 Boilerplate',
      'AngularJS',
      'Karma'
    ];
  });
