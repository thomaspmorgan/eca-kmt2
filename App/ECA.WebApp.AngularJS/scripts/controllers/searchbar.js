'use strict';

/**
 * @ngdoc directive
 * @name staticApp.controller:searchbarCtrl
 * @description
 * # searchbar
 */
angular.module('staticApp')
  .controller('searchbarCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modalInstance,
        SearchService,
        NotificationService) {

      $scope.view = {};
      $scope.results = [];
      $scope.text = '';
      $scope.show = true;
      $scope.tophit = false;
      $scope.firstrun = true;
      $scope.isLoadingResults = false;
      $scope.isLoadingDocInfo = false;

      $scope.autocomplete = function () {
          $scope.firstrun = true;

          var params = {
              Limit: 25,
              Filter: null,
              Facets: null,
              Fields: null,
              SearchTerm: $scope.text
          };

          $scope.isLoadingResults = true;
          SearchService.getAll(params)
          .then(function (response) {
              $log.info('Loaded all search results.');
              $scope.results = response.results;
              $scope.isLoadingResults = false;
          })
          .catch(function () {
              var message = 'Unable to load search results.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.isLoadingResults = false;
          });
      };

      $scope.GetSVGDocument = function (id) {
          $scope.isLoadingDocInfo = true;
          SearchService.get(id)
          .then(function (response) {
              $scope.docinfo = response;
              $scope.isLoadingDocInfo = false;
          })
          .catch(function () {
              var message = 'Unable to load document information.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.isLoadingDocInfo = false;
          });
      };


      $scope.currentGroup = '';
      $scope.CreateHeader = function (group) {
          var showHeader = (group != $scope.currentGroup);
          $scope.currentGroup = group;
          return showHeader;
      }

      $scope.onCloseSpotlightSearchClick = function () {
          $modalInstance.dismiss('close');
      }

  });

angular.module('staticApp')
  .filter('resultIconFilter', function() {
    return function(item) {
        if (typeof item !== 'undefined' && item !== null) {
            if (item === "Project") {
                return item.substring(0, 2);
            } else {
                return item.substring(0, 1);
            }
        }
        return "";
    };
});
