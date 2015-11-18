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
      $scope.view.previousSearch = '';
      $scope.view.isSpotlightSearchOpen = false;

      $scope.view.onSpotlightSearchClick = function () {
          $scope.view.isSpotlightSearchOpen = true;
          var spotlightModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/search/search-modal.html',
              controller: 'SearchModalCtrl',
              windowClass: 'full-screen-modal',
              backdrop: 'static',
              keyboard: false,
              resolve: {
                  previousSearch: function () {
                      return $scope.view.previousSearch;
                  }
              }
          });
          spotlightModalInstance.result.then(function (previousText) {
              $log.info('Finished searching.');
              $scope.view.previousSearch = previousText;
              $scope.view.isSpotlightSearchOpen = false;
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
              $scope.view.isSpotlightSearchOpen = false;
          });
      }

      function addHokeyEventListener() {
          angular.element(document).on('keypress', function (event) {
              var fKeyCode = 6;
              if (event.ctrlKey
                  && event.shiftKey
                  && event.keyCode === fKeyCode
                  && !$scope.view.isSpotlightSearchOpen) {
                  $scope.view.onSpotlightSearchClick();
              }
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
                  addHokeyEventListener();
              },
              notAuthorized: function () {
                  $scope.view.isSpotlightIconVisible = false;
              }
          };
          return AuthService.getResourcePermissions(resourceType, kmtId, config)
            .then(function (result) {

            }, function () {
                $log.error('Unable to load user permissions.');
            });
      }
      
      var hasLoaded = false;
      $scope.$watch(function () {
          return $rootScope.userInfo.isAuthenticated;
      }, function (newValue, oldValue) {
          if (newValue && !hasLoaded) {
              loadPermissions()
              .then(function () {
                  hasLoaded = true;
              });
          }
      })
  });