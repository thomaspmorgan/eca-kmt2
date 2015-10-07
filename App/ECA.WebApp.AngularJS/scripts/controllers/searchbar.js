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
        $rootScope,
        $stateParams,
        $q,
        $log,
        $location,
        $modalInstance,
        $filter,
        $sanitize,
        SearchService,
        StateService,
        NotificationService) {

      $scope.view = {};
      $scope.results = [];
      $scope.docinfo = null;
      $scope.tophitinfo = {};
      $scope.searchFieldNames = [];
      $scope.currentpage = 0;
      $scope.pagesize = 10;
      $scope.totalpages = 0;
      $scope.text = '';
      $scope.isLoadingResults = false;
      $scope.isLoadingDocInfo = false;

      // Return field names for search results
      $scope.init = function () {
          SearchService.getFieldNames()
          .then(function (response) {
              $scope.searchFieldNames = response.data;
          })
          .catch(function () {
              var message = 'Unable to load search field names.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      $scope.init();

      // Return number of pages in results
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
              SelectFields: $scope.searchFieldNames,
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

      // Detect array in sub group
      $scope.isArray = angular.isArray;

      // Set the current page when paging
      $scope.selectPage = function (index) {
          $scope.currentpage = index;
      }

      // Save the previous search term
      if ($rootScope.searchText.length) {
          $scope.text = $rootScope.searchText;
          $scope.autocomplete();
      }

      // Creates a group header
      $scope.currentGroup = '';
      $scope.CreateHeader = function (group, index) {
          var showHeader = (group !== $scope.currentGroup || index === 0);
          $scope.currentGroup = group;
          return showHeader;
      };
      
      // Closes the search modal
      $scope.onCloseSpotlightSearchClick = function () {
          $rootScope.searchText = $scope.text;
          $modalInstance.dismiss('close');
      };

      // Closes the search modal and reloads selection
      $scope.onGoToSpotlightSearchClick = function (url) {
          $rootScope.searchText = $scope.text;
          $modalInstance.dismiss('close');
          $location.path(url, true);
      };

      // Link document to details page
      $scope.getHref = function (docType, docId) {
          if (docType && docId) {
              if (docType === 'Office') {
                  return StateService.getOfficeState(docId);
              } else if (docType === 'Program') {
                  return StateService.getProgramState(docId);
              } else if (docType === 'Project') {
                  return StateService.getProjectState(docId);
              } else if (docType === 'Person') {
                  return StateService.getPersonState(docId);
              } else if (docType === 'Organization') {
                  return StateService.getOrganizationState(docId);
              }
              else {
                  throw Error('The document type is not supported.');
              }
          }
      }
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

angular.module('staticApp')
  .filter('addSpacing', function () {
      return function (input) {
          return input.replace(/([a-z])([A-Z])/g, '$1 $2');
      }
  });


// Sets paging start point
angular.module('staticApp')
  .filter('startFrom', function () {
      return function (input, start) {
          start = +start;
          return input.slice(start);
      }
  });
