'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OfficeCtrl', function ($scope, $stateParams, $q, DragonBreath, OfficeService, TableService) {

      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          partners: {
              title: 'Branches & Programs',
              path: 'branches',
              active: true,
              order: 2
          },
          participants: {
              title: 'Timeline',
              path: 'activity',
              active: true,
              order: 3
          },
          artifacts: {
              title: 'Attachments',
              path: 'artifacts',
              active: true,
              order: 4
          },
          moneyflows: {
              title: 'Funding',
              path: 'moneyflows',
              active: true,
              order: 5
          }
      };

      $scope.header = 'Branches & Programs';
      $scope.office = {};
      $scope.programs = [];
      $scope.branches = [];
      $scope.totalNumberOfPrograms = 0;
      $scope.skippedNumberOfPrograms = 0;
      $scope.numberOfPrograms = 0;
      $scope.programFilter = '';

      $scope.isLoadingOfficeById = true;
      $scope.isLoadingPrograms = false;
      $scope.isLoadingBranches = true;

      $scope.officeExists = true;
      $scope.showLoadingOfficeByIdError = false;
      $scope.loadingProgramsErrorOccurred = false;
      $scope.loadingBranchesErrorOccurred = false;

      var officeId = $stateParams.officeId;

      function reset() {
          $scope.officeExists = true;
          $scope.showLoadingOfficeByIdError = false;
          $scope.loadingProgramsErrorOccurred = false;
          $scope.loadingBranchesErrorOccurred = false;
      }

      function showLoadingBranches() {
          $scope.isLoadingBranches = true;
      }

      function hideLoadingBranches() {
          $scope.isLoadingBranches = false;
      }

      function showLoadingOfficeById() {
          $scope.isLoadingOfficeById = true;
      }

      function hideLoadingOfficeById() {
          $scope.isLoadingOfficeById = false;
      }

      function showNotFound() {
          $scope.officeExists = false;
      }

      function showLoadingOfficeByIdError() {
          $scope.showLoadingOfficeByIdError = true;
      }

      function showLoadingProgramsError() {
          $scope.loadingProgramsErrorOccurred = true;
      }

      function showLoadingBranchesError() {
          $scope.loadingBranchesErrorOccurred = true;
      }

      function updateHeader() {
          if ($scope.branches.length === 0) {
              $scope.header = "Programs";
          }
      }

      function getOfficeById(id) {
          var dfd = $q.defer();
          reset();
          OfficeService.get(id)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);

              });
          return dfd.promise;
      }

      function updatePagingDetails(total, start, count) {
          $scope.totalNumberOfPrograms = total;
          $scope.skippedNumberOfPrograms = start;
          $scope.numberOfPrograms = count;
      }

      function getChildOfficesById(officeId) {
          var dfd = $q.defer();

          OfficeService.getChildOffices(officeId)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);

              });
          return dfd.promise;
      }

      function getProgramsByOfficeId(officeId, params) {
          var dfd = $q.defer();

          OfficeService.getPrograms(params, officeId)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);

              });
          return dfd.promise;
      }

      $scope.getPrograms = function (tableState) {
          reset();
          $scope.isLoadingPrograms = true;
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter(),
              keyword: TableService.getKeywords()
          };
          $scope.programFilter = params.keyword;
          getProgramsByOfficeId(officeId, params)
            .then(function (data) {
                processData(data, tableState, params);
            }, function (errorCode) {
                showLoadingProgramsError();
            })
            .then(function () {
                $scope.isLoadingPrograms = false;
            });
      }
      
      function processData(data, tableState, params) {
          var programs = data.results;
          var total = data.total;
          var start = 0;
          if (programs.length > 0) {
              start = params.start + 1;
          };
          var count = params.start + programs.length;

          updatePagingDetails(total, start, count);

          var limit = TableService.getLimit();
          tableState.pagination.numberOfPages = Math.ceil(total / limit);

          $scope.programs = programs;
          $scope.programsLoading = false;
      };

      showLoadingOfficeById();
      getOfficeById(officeId)
          .then(function (data) {
              $scope.office = data;

          }, function (errorCode) {
              if (errorCode === 404) {
                  showNotFound();
              }
              else {
                  showLoadingOfficeByIdError();
              }
          })
        .then(function () {
            hideLoadingOfficeById();
        });

      showLoadingBranches();
      getChildOfficesById(officeId)
          .then(function (data) {
              var childOffices = data;
              $scope.branches = childOffices;
          }, function (errorCode) {
              showLoadingBranchesError();
          })
          .then(function(){
              hideLoadingBranches();
              updateHeader();
          });
      
  });
