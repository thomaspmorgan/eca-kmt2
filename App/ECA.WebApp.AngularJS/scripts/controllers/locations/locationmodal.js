'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:LocationModalCtrl
 * @description The location modal controller is used to control the location modal view.
 * # LocationModalCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('LocationModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        smoothScroll,
        LookupService,
        LocationService,
        ConstantsService,
        NotificationService,
        TableService,
        FilterService
        ) {
      $scope.view = {};
      $scope.view.newLocationTabKey = 'new';
      $scope.view.locationListTabKey = 'list';

      setActiveTab($scope.view.locationListTabKey);
      $scope.view.params = $stateParams;
      $scope.view.start = 0;
      $scope.view.limit = 10;
      $scope.view.total = 0;

      $scope.view.onLocationsTabClick = function () {
          setActiveTab($scope.view.locationListTabKey);
      };

      $scope.view.onAddLocationTabClick = function () {
          setActiveTab($scope.view.newLocationTabKey);
      };

      function setActiveTab(key) {
          $scope.view.activeTab = key;
      }
  });
