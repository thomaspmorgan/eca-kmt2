'use strict';

/**
 * @ngdoc directive
 * @name staticApp.controller:SearchModalCtrl
 * @description
 * # searchbar
 */
angular.module('staticApp')
  .controller('SearchModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $location,
        previousSearch,
        $modalInstance,
        $filter,
        $anchorScroll,
        $timeout,
        $state,
        SearchService,
        StateService,
        NotificationService) {

      var limit = 10;

      $scope.isLoadingRequiredData = true;
      $scope.results = [];
      $scope.docinfo = null;
      $scope.tophitinfo = {};
      $scope.currentPage = 0;
      $scope.pageSize = 10;
      $scope.totalpages = 0;
      $scope.numberOfDisplayedPages = 10;
      $scope.suggestions = [];
      $scope.text = previousSearch;
      $scope.isLoadingResults = false;
      $scope.isLoadingDocInfo = false;
      $scope.currentParams = {
          Start: 0,
          Limit: limit,
          Filter: "",
          Facets: [],
          SelectFields: [],
          SearchTerm: '',
          HighlightPreTag: "<strong>",
          HighlightPostTag: "</strong>"
      };

      var htmlRegex = /(<([^>]+)>)/ig;

      function loadFieldNames() {
          $scope.isLoadingRequiredData = true;
          return SearchService.getFieldNames()
          .then(function (response) {
              $scope.currentParams.SelectFields = response.data;
              $scope.isLoadingRequiredData = false;
          })
          .catch(function () {
              var message = 'Unable to load search field names.';
              $scope.isLoadingRequiredData = false;
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function calculatePageDetails() {
          $scope.totalPages = Math.ceil($scope.totalResults / $scope.pageSize);
          $scope.pages = [];
          var start = Math.max(1, $scope.currentPage - Math.abs(Math.floor($scope.numberOfDisplayedPages / 2)));
          var end = start + $scope.numberOfDisplayedPages;
          if (end > $scope.totalPages) {
              end = $scope.totalPages + 1;
              start = Math.max(1, end - $scope.numberOfDisplayedPages);
          }
          for (var i = start; i < end; i++) {
              $scope.pages.push(i);
          }
      }

      function focusSearchField() {
          var searchFieldId = '#spotlightSearchTextField';
          var element = angular.element(searchFieldId);
          $timeout(function () {
              element[0].focus();
          }, 300);
      }

      // Execute search as user types
      $scope.autocomplete = function () {
          $scope.currentParams.SearchTerm = $scope.text;
          $scope.currentParams.Start = 0;
          $scope.currentPage = 0;
          $scope.docinfo = null;
          return doSearch($scope.currentParams);
      };

      // Gets document details on selection
      $scope.getDocumentInfo = function (docItem) {
          $scope.isLoadingDocInfo = true;
          $scope.docinfo = null;
          return SearchService.getDocInfo(docItem.document.id)
          .then(function (response) {
              $log.info('Loaded document information.');
              $scope.docinfo = response.data;
              replacePlainTextWithHighlights(docItem.hitHighlights, $scope.docinfo);
              $location.hash('title');
              $anchorScroll();
              $scope.isLoadingDocInfo = false;
          })
          .catch(function () {
              var message = 'Unable to load document information.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.isLoadingDocInfo = false;
          });
      };

      function replacePlainTextWithHighlights(highlights, document) {
          for (var property in highlights) {
              var propertyHighlights = highlights[property];
              for (var i = 0; i < propertyHighlights.length; i++) {
                  var highlight = propertyHighlights[i];
                  var originalText = highlight.replace(htmlRegex, '');
                  if (angular.isArray(document[property])) {
                      for (var j = 0; j < document[property].length; j++) {
                          document[property][j] = document[property][j].replace(originalText, highlight);
                      }
                  }
                  else {
                      document[property] = document[property].replace(originalText, highlight);
                  }
              }
          }
      }

      // Detect array in sub group
      $scope.isArray = angular.isArray;

      // Set the current page when paging
      $scope.selectPage = function (page) {
          $scope.currentPage = page;
          $scope.currentParams.Start = $scope.currentPage * limit;
          return doSearch($scope.currentParams);
      }

      // Save the previous search term
      if (previousSearch) {
          $scope.autocomplete();
      }

      // Closes the search modal
      $scope.onCloseSpotlightSearchClick = function () {
          $modalInstance.close($scope.text);
      };

      // Closes the search modal and reloads selection
      $scope.onGoToSpotlightSearchClick = function (url) {
          $modalInstance.close($scope.text);
          $location.path(url, true);
      };

      // Link document to details page
      $scope.getHref = function (docType, docId) {
          if (docType && docId) {
              var plainTextDocType = docType.replace(htmlRegex, '')
              if (plainTextDocType === 'Office') {
                  return StateService.getOfficeState(docId);
              } else if (plainTextDocType === 'Program') {
                  return StateService.getProgramState(docId);
              } else if (plainTextDocType === 'Project') {
                  return StateService.getProjectState(docId);
              } else if (plainTextDocType === 'Person') {
                  return StateService.getPersonState(docId);
              } else if (plainTextDocType === 'Organization') {
                  return StateService.getOrganizationState(docId);
              }
              else {
                  throw Error('The document type ' + docType + ' is not supported.');
              }
          }
      }

      function getDocumentTypeAbbreviation(documentTypeName) {
          if (documentTypeName === "Project") {
              return documentTypeName.substring(0, 2);
          }
          else {
              return documentTypeName.substring(0, 1);
          }
      }

      function doSearch(params) {
          $scope.isLoadingResults = true;
          return SearchService.postSearch(params)
            .then(function (response) {
                $scope.count = response.data.results.length;
                angular.forEach(response.data.results, function (result, index) {
                    result.document.documentTypeNameAbbreviation = getDocumentTypeAbbreviation(result.document.documentTypeName);
                    result.document.plaintTextDocumentTypeName = result.document.documentTypeName;
                    replacePlainTextWithHighlights(result.hitHighlights, result.document);
                })
                if ($scope.currentPage === 0) {
                    $scope.tophitinfo = response.data.results.slice(0, 1);
                    $scope.results = response.data.results.slice(1);
                }
                else {
                    $scope.tophitinfo = null;
                    $scope.results = response.data.results;
                }
                $scope.totalResults = response.data.count;
                calculatePageDetails();
                $scope.isLoadingResults = false;
            })
            .catch(function () {
                var message = 'Unable to load search results.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
                $scope.totalResults = -1;
                $scope.isLoadingResults = false;
            });
      }

      $q.all([loadFieldNames()])
      .then(function () {
          focusSearchField();
      });
  });

angular.module('staticApp')
  .filter('addSpacing', function () {
      return function (input) {
          return input.replace(/([a-z])([A-Z])/g, '$1 $2');
      }
  });
