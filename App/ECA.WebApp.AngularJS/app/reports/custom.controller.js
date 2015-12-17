'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # ReportsArchiveCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ReportsCustomCtrl', function ($scope, $stateParams, $q, $modal, ReportService, TableService, $log, $window) {

      $scope.$log = $log;
      $scope.reports = [
          { Title: "Project Awards By Year", Published: "4/28/2015", Author: "Tom Morgan", Prompts: "Program, Country" },
          { Title: "Region Awards", Published: "6/22/2015", Author: "Tom Morgan", Prompts: "Program" },
          { Title: "Post Awards", Published: "6/22/2015", Author: "Tom Morgan", Prompts: "Program" },
          { Title: "Focus Awards", Published: "6/23/2015", Author: "Tom Morgan", Prompts: "Program" },
          { Title: "Focus-Category Awards", Published: "6/23/2015", Author: "Tom Morgan", Prompts: "Program" },
          { Title: "Country Awards", Published: "7/2/2015", Author: "Tom Morgan", Prompts: "Program" },
          { Title: "Objective Awards", Published: "7/6/2015", Author: "Tom Morgan", Prompts: "Program, Objective" },
          { Title: "Year Awards", Published: "7/7/2015", Author: "Tom Morgan", Prompts: "Program" },
          { Title: "Projects with Grant Number", Published: "7/7/2015", Author: "Tom Morgan", Prompts: "Program" }        
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

      $scope.openReport = function (title) {
          if (title == 'Project Awards By Year')
          {
              var template = "/app/reports/project-awards.html";
              var controller = "ProjectAwardsCtrl";
          }
          else if (title == 'Region Awards')
          {
              var template = "/app/reports/region-awards.html";
              var controller = "RegionAwardsCtrl";
          }
          else if (title == 'Post Awards') {
              var template = "/app/reports/post-awards.html";
              var controller = "PostAwardsCtrl";
          }
          else if (title == 'Focus Awards') {
              var template = "/app/reports/focus-awards.html";
              var controller = "FocusAwardsCtrl";
          }
          else if (title == 'Focus-Category Awards') {
              var template = "/app/reports/focus-category-awards.html";
              var controller = "FocusCategoryAwardsCtrl";
          }
          else if (title == 'Country Awards') {
              var template = "/app/reports/country-awards.html";
              var controller = "CountryAwardsCtrl";
          }
          else if (title == 'Objective Awards') {
              var template = "/app/reports/objective-awards.html";
              var controller = "ObjectiveAwardsCtrl";
          }
          else if (title == 'Year Awards') {
              var template = "/app/reports/year-awards.html";
              var controller = "YearAwardsCtrl";
          }
          else if (title == 'Projects with Grant Number') {
              var template = "/app/reports/projects-with-grant-number.html";
              var controller = "ProjectsWithGrantNumberCtrl";
          }
          else
          {
              return;
          }
       
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
            $log.info('Report: ' + title + '  Dismissed at: ' + new Date());
         });
      };

  });
