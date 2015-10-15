'use strict';

/**
 * @ngdoc directive
 * @name staticApp.controller:SpotlightCtrl
 * @description
 * # searchbar
 */
angular.module('staticApp')
  .controller('SpotlightCtrl', function (
        $scope,
        $rootScope,
        $modal,
        $q,
        $log,
        AuthService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.isSpotlightIconVisible = false;

      $scope.view.onSpotlightSearchClick = function () {
          var spotlightModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/search/search-bar.html',
              controller: 'searchbarCtrl',
              windowClass: 'search-modal-resize',
              resolve: {}
          });
      }

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.application.value, 'The constants service must have the application resource type value.');
          var kmtId = ConstantsService.kmtApplicationResourceId;
          var resourceType = ConstantsService.resourceType.application.value;
          var config = {};
          config[ConstantsService.permission.search.value] = {
              hasPermission: function () {
                  $log.info('User has search permission in spotlight-controller.js controller.');
                  $scope.view.isSpotlightIconVisible = true;
              },
              notAuthorized: function () {
                  
              }
          };
          return AuthService.getResourcePermissions(resourceType, kmtId, config)
            .then(function (result) {

            }, function () {
                $log.error('Unable to load user permissions.');
            });
      }

      $q.all([loadPermissions()])
      .then(function () {

      });
  });