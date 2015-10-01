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
        SearchService) {

      $scope.view = {};
      $scope.results = [];
      $scope.text = '';
      $scope.show = true;
      $scope.tophit = false;
      $scope.firstrun = true;
      $scope.isLoadingResults = false;

      $scope.autocomplete = function () {
          $scope.firstrun = true;

          var params = {
              Limit: 5,
              Filter: null,
              Facets: null,
              Fields: null,
              SearchTerm: $scope.text
          };

          $scope.results = SearchService.getAll(params);
                    

      //    var parseresults = function (results) {
      //        $scope.isLoadingResults = true;
      //        //var formatElement = function () {
      //        //    var el = element;
      //        //    return {
      //        //        'id': el.Fields.docId,
      //        //        'title': el.Fields.title,
      //        //        'subtitle': el.Fields.subtitle,
      //        //        'type': el.Fields.type
      //        //    };
      //        //};

      //        res = results;

      //        if ((res.length === 0) && ($scope.firstrun === true)) {
      //            $scope.firstrun = false;
      //            SearchService.search($scope.text, null, null, 200)
      //                  .then(function (response) {
      //                      $scope.results = parseresults(response);
      //                  }, function (error) {
      //                      $log.info("Spotlight search error: ", error);
      //                });
      //            return;
      //        } else if (res.length === 0) {
      //            $scope.results = false;
      //            $scope.tophit = false;
      //        } else {
      //            $scope.results = res;
      //        }

      //        //var out = {};
      //        //$scope.tophit = formatElement(res.shift());

      //        //var sorter = function () {
      //        //    var el = element;
      //        //    out[el.Fields.type] = out[el.Fields.type] || [];
      //        //    if (out[el.Fields.type].length < 10) {
      //        //        out[el.Fields.type].push(formatElement(el));
      //        //    }
      //        //};
      //        //res.forEach(sorter);

      //        //$scope.results = out;
      //        $scope.isLoadingResults = false;
      //    };

      //    var response = SearchService.search($scope.text, null, null, 200);
      //    parseresults(response);
      };

      $scope.onCloseSpotlightSearchClick = function () {
          $modalInstance.dismiss('close');
      }

  });
