'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # ReportsArchiveCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ReportsArchiveCtrl', function ($scope, $stateParams, $q, $modal, ReportService, TableService, $log) {

      $scope.$log = $log;
      $scope.reports = [
          { Title: "Project Awards", Published: "4/28/2015", Author: "Tom Morgan", Clearance: "Cleared By Office"}
      ]
      $scope.currentpage = $stateParams.page || 1;
      $scope.limit = 200;

      $scope.totalNumberOfReports = -1;
      $scope.skippedNumberOfReports = -1;
      $scope.numberOfReports = -1;
      $scope.reportFilter = '';

      function updatePagingDetails(total, start, count) {
          $scope.totalNumberOfReports = total;
          $scope.skippedNumberOfReports = start;
          $scope.numberOfReports = count;
      };

      function reset() {
          $scope.loadingReportsErrorOccurred = false;
      };

      function showLoadingReportsError() {
          $scope.loadingReportsErrorOccurred = true;
      }

      function getReportsFiltered(params) {
          var dfd = $q.defer();
          //ReportService.getAll(params)
          //      .then(function (data, status, headers, config) {
          //          dfd.resolve(data.data);
          //      },
          //      function (data, status, headers, config) {
          //          var errorCode = data.status;
          //          dfd.reject(errorCode);

          //      });
          dfd.resolve($scope.reports);
          return dfd.promise;
      };

      $scope.getReports = function (tableState) {
          reset();
          $scope.reportsLoading = true;
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter(),
              keyword: TableService.getKeywords()
          };

          $scope.reportFilter = params.keyword;

          getReportsFiltered(params)
            .then(function (data) {
                //var reports = data.results;
                //var total = data.total;
                var reports = data;
                var total = data.length;
                var start = 0;
                if (reports.length > 0) {
                    start = params.start + 1;
                };
                updatePagingDetails(total, start, reports.length);
                $scope.reports = reports;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(total / limit);
            }, function (errorCode) {
                showLoadingReportsError();
            })
            .then(function () {
                $scope.reportsLoading = false;
            });
      };

      $scope.openReport = function (title) {
          if (title == 'Project Awards') {
              var modalInstance = $modal.open({
                  templateUrl: '/views/reports/projectAwards.html',
                  controller: 'ProjectAwardsCtrl',
                  size: 'sm'
              })
          }

          modalInstance.result.then(function () {
              $log.info('Report: ' + title + ' run at: ' + new Date());
          }, function () {
              $log.info('Report: ' + title + '  Dismissed at: ' + new Date());
          });
      };

  });
