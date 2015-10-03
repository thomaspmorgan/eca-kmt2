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
        SearchService,
        NotificationService) {

      $scope.view = {};
      $scope.results = [];
      $scope.docinfo = {};
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
              Fields: ['description','id','name','documentTypeName','officeSymbol','status','pointsOfContact','themes','goals','foci','objectives','regions','countries','locations','websites'],
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

      // Gets document details on selection
      $scope.GetDocumentInfo = function (id) {
          $scope.isLoadingDocInfo = true;
          $scope.docinfo = $filter('filter')($scope.results, id)[0];
          $scope.isLoadingDocInfo = false;
      };

      // Creates a group header
      $scope.currentGroup = '';
      $scope.CreateHeader = function (group) {
          var showHeader = (group !== $scope.currentGroup);
          $scope.currentGroup = group;
          return showHeader;
      }

      // Closes the search modal
      $scope.onCloseSpotlightSearchClick = function () {
          $modalInstance.dismiss('close');
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

// Return document type title style
angular.module('staticApp')
  .filter('titleStyle', function () {
      return function (doctype) {
          if (typeof doctype !== 'undefined' && doctype !== null) {
              return doctype.toLowerCase() + '-color';
          }
          return 'program-color';
      };
  });


