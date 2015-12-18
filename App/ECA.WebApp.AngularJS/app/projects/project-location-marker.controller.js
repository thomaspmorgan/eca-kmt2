'use strict';


/**
 * @ngdoc function
 * @name staticApp.controller:AddLocationCtrl
 * @description The add location controller is used to control adding new locations.
 * # AddLocationCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')

  .controller('ProjectLocationMarkerCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log
        ) {
      $scope.view = {};
      $scope.view.mapLocation = $scope.mapLocation;
  
  });
