'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AllOfficesCtrl
 * @description
 * # AllOfficesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOfficesCtrl', function ($scope, $stateParams, $q, $timeout, OfficeService, TableService) {

    $scope.offices = [];
    $scope.currentpage = $stateParams.page || 1;
    $scope.limit = 200;

    $scope.totalNumberOfOffice = -1;
    $scope.skippedNumberOfOffices = -1;
    $scope.numberOfOffices = -1;
    $scope.officeFilter = '';
    $scope.loadingOfficesErrorOccurred = false;

    function updatePagingDetails(total, start, count) {
        $scope.totalNumberOfOffices = total;
        $scope.skippedNumberOfOffices = start;
        $scope.numberOfOffices = count;
    };

    function reset() {
        $scope.loadingOfficesErrorOccurred = false;
    };

    function showLoadingOfficesError() {
        $scope.loadingOfficesErrorOccurred = true;
    }

    $scope.getOffices = function (tableState) {
        reset();
        $scope.officesLoading = true;
        TableService.setTableState(tableState);
        var params = {
            start: TableService.getStart(),
            limit: TableService.getLimit(),
            sort: TableService.getSort(),
            filter: TableService.getFilter(),
            keyword: TableService.getKeywords()
        };

        $scope.officeFilter = document.getElementById('searchBar').value;
        
        getOfficesFiltered(params)
          .then(function (data) {
              var offices = data.results;
              var total = data.total;
              var start = 0;
              if (offices.length > 0) {
                  start = params.start + 1;
              };
              var count = params.start + offices.length;
              updatePagingDetails(total, start, count);
              $scope.offices = offices;
              var limit = TableService.getLimit();
              tableState.pagination.numberOfPages = Math.ceil(total / limit);
          }, function (errorCode) {
              showLoadingOfficesError();
          })
          .then(function () {
              $scope.officesLoading = false;
          });
    };

    function getOfficesFiltered(params) {
        var dfd = $q.defer();
        OfficeService.getAll(params)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);

              });
        return dfd.promise;
    };

  });
