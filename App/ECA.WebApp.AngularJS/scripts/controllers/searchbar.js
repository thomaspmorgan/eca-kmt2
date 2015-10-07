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
        $filter,
        $sanitize,
        SearchService,
        NotificationService) {

      $scope.view = {};
      $scope.results = [];
      $scope.docinfo = {};
      $scope.tophitinfo = {};
      $scope.currentpage = 0;
      $scope.pagesize = 10;
      $scope.totalpages = 0;
      $scope.text = '';
      $scope.isLoadingResults = false;
      $scope.isLoadingDocInfo = false;
      
      var numberOfPages = function () {
          $scope.totalpages = Math.ceil($scope.results.length / $scope.pagesize);
          $scope.pagearray = new Array($scope.totalpages);
          return $scope.totalpages;
      }

      // Execute search as user types
      $scope.autocomplete = function () {
          var params = {
              Start: 0,
              Limit: 100,
              Filter: "",
              Facets: [],
              SelectFields: ['description', 'id', 'name', 'documentTypeName', 'officeSymbol', 'themes', 'regions', 'goals', 'websites', 'objectives', 'foci', 'status', 'pointsOfContact'],
              SearchTerm: $scope.text,
              HightlightPreTag: "<strong>",
              HighlightPostTag: "</strong>"
          };

          $scope.currentpage = 0;
          $scope.docinfo = null;
          $scope.isLoadingResults = true;

          SearchService.postSearch(params)
          .then(function (response) {
              $log.info('Loaded all search results.');
              $scope.tophitinfo = response.data.results.slice(0, 1);
              $scope.results = response.data.results.slice(1);
              $scope.totalResults = $scope.tophitinfo.length + $scope.results.length;
              numberOfPages();
              $scope.isLoadingResults = false;
          })
          .catch(function () {
              var message = 'Unable to load search results.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.totalResults = -1;
              $scope.isLoadingResults = false;
          });
      };

      // Gets document details on selection
      $scope.GetDocumentInfo = function (id) {
          $scope.isLoadingDocInfo = true;
          SearchService.getDocInfo(id)
          .then(function (response) {
              $log.info('Loaded document information.');
              $scope.docinfo = response.data;
              $scope.isLoadingDocInfo = false;
          })
          .catch(function () {
              var message = 'Unable to load document information.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.isLoadingDocInfo = false;
          });
      };

      // Creates a group header
      $scope.currentGroup = '';
      $scope.CreateHeader = function (group, index) {
          var showHeader = (group !== $scope.currentGroup || index === 0);
          $scope.currentGroup = group;
          return showHeader;
      };
      
      // Closes the search modal
      $scope.onCloseSpotlightSearchClick = function () {
          $modalInstance.dismiss('close');
      };

  });

// Retuns item type icon text
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

// Sets paging start point
angular.module('staticApp')
  .filter('startFrom', function () {
      return function (input, start) {
          start = +start;
          return input.slice(start);
      }
  });
