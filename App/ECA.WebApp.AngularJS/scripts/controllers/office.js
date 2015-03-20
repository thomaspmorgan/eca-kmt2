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
              title: 'Activity',
              path: 'activity',
              active: true,
              order: 3
          },
          artifacts: {
              title: 'Artifacts',
              path: 'artifacts',
              active: true,
              order: 4
          },
          moneyflows: {
              title: 'Money Flows',
              path: 'moneyflows',
              active: true,
              order: 5
          }
      };

      $scope.office = {};
      $scope.programs = [];
      $scope.branches = [];
      $scope.totalNumberOfPrograms = -1;
      $scope.skippedNumberOfPrograms = -1;
      $scope.numberOfPrograms = -1;
      $scope.programFilter = '';

      $scope.isLoadingOfficeById = true;
      $scope.isLoadingPrograms = false;

      $scope.officeExists = true;
      $scope.showLoadingOfficeByIdError = false;
      $scope.loadingProgramsErrorOccurred = false;

      var officeId = $stateParams.officeId;

      function reset() {
          $scope.officeExists = true;
          $scope.showLoadingOfficeByIdError = false;
          $scope.loadingProgramsErrorOccurred = false;
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
          var levels = [1, 2];

          params.filter.push({
              property: 'programLevel',
              comparison: 'in',
              value: levels
          });

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
                var programs = data.results;
                var total = data.total;
                var start = 0;
                if (programs.length > 0) {
                    start = params.start + 1;
                }
                updatePagingDetails(total, start, programs.length);
                $scope.programs = programs;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(total / limit);
            }, function (errorCode) {
                showLoadingProgramsError();
            })
            .then(function () {
                $scope.isLoadingPrograms = false;
            });
      }

      

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

        getChildOfficesById(officeId)
        .then(function (data) {
            var childOffices = data;
            $scope.branches = childOffices;
        }, function (errorCode) {

        });
      
  });
