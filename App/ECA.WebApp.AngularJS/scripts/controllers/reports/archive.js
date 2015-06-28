'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # ReportsArchiveCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ReportsArchiveCtrl', function ($scope, $stateParams, $q, $modal, ReportService, TableService, $log, $window) {

      $scope.$log = $log;
      $scope.reports = [];
      $scope.parameters = [];
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
          if (title == 'Project Awards')
          {
              var template = "/views/reports/projectAwards.html";
              var controller = "ProjectAwardsCtrl";
          }
          else if (title == 'Region Awards')
          {
              var template = "/views/reports/regionAwards.html";
              var controller = "RegionAwardsCtrl";
          }
          else if (title == 'Post Awards') {
              var template = "/views/reports/postAwards.html";
              var controller = "PostAwardsCtrl";
          }
          else if (title == 'Focus Awards') {
              var template = "/views/reports/focusAwards.html";
              var controller = "FocusAwardsCtrl";
          }
          else if (title == 'Focus-Category Awards') {
              var template = "/views/reports/focusCategoryAwards.html";
              var controller = "FocusCategoryAwardsCtrl";
          }
          else
          {
              return;
          }
       
         var modalInstance = $modal.open({
            templateUrl: template,
            controller: controller,
            resolve: {
                parameters: function () {
                    return $scope.parameters;
                }
            },
            size: 'lg'
         });


         modalInstance.result.then(function (parameters) {
            $scope.parameters = parameters;
                  
         }, function () {
            $log.info('Report: ' + title + '  Dismissed at: ' + new Date());
         });
      };

  });
