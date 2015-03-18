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

      $scope.isLoading = true;
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

      function setBusy() {
          $scope.isLoading = true;
      }

      function setIdle() {
          $scope.isLoading = false;
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
          var levels = [1, 2];
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };
          params.filter.push({
              property: 'programLevel',
              comparison: 'in',
              value: levels
          });
          getProgramsByOfficeId(officeId, params)
            .then(function (data) {
                var programs = data.results;
                var total = data.total;
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

      setBusy();
      getOfficeById(officeId)
          .then(function (data) {
              setIdle();
              $scope.office = data;

          }, function (errorCode) {
              setIdle();
              if (errorCode === 404) {
                  showNotFound();
              }
              else {
                  showServerError();
              }
          });
  });
