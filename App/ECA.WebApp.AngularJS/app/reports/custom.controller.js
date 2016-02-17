'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # ReportsArchiveCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ReportsCustomCtrl', function ($scope, $stateParams, $q, $modal, ReportService, TableService, BrowserService, $log, $window) {

      BrowserService.setDocumentTitleByReport('Custom');
      $scope.$log = $log;
      $scope.reports = [
          { Title: "Project Awards By Year", Published: "4/28/2015", Author: "Tom Morgan", Prompts: "Program, Country", Report: "ProjectAwards" },
          { Title: "Region Awards", Published: "6/22/2015", Author: "Tom Morgan", Prompts: "Program", Report: "RegionAwards"},
          { Title: "Post Awards", Published: "6/22/2015", Author: "Tom Morgan", Prompts: "Program" , Report: "PostAwards"},
          { Title: "Focus Awards", Published: "6/23/2015", Author: "Tom Morgan", Prompts: "Program" , Report: "FocusAwards"},
          { Title: "Focus-Category Awards", Published: "6/23/2015", Author: "Tom Morgan", Prompts: "Program", Report: "FocusCategoryAwards" },
          { Title: "Country Awards", Published: "7/2/2015", Author: "Tom Morgan", Prompts: "Program", Report: "CountryAwards"},
          { Title: "Objective Awards", Published: "7/6/2015", Author: "Tom Morgan", Prompts: "Program, Objective", Report: "ObjectiveAwards" },
          { Title: "Year Awards", Published: "7/7/2015", Author: "Tom Morgan", Prompts: "Program", Report: "YearAwards"},
          { Title: "Projects with Grant Number", Published: "7/7/2015", Author: "Tom Morgan", Prompts: "Program", Report: "ProjectsWithGrantNumber" }        
      ];
      $scope.formats = [{ type: 'Portable Document Format - PDF', key: 'pdf', mimetype: 'application/pdf' },
                        { type: 'Microsoft Excel - XLSX', key: 'xlsx', mimetype: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' },
                        { type: 'Microsoft Word - DOCX', key: 'docx', mimetype: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' }
      ];
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

      $scope.openReport = function (report) {
              var template = "/app/reports/report.html";
              var controller = "ReportCtrl";
              $scope.parameters.report = report;
       
         var modalInstance = $modal.open({
            templateUrl: template,
            controller: controller,
            scope: $scope,
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
            $log.info('Report: ' + report.title + '  Dismissed at: ' + new Date());
         });
      };

  });
