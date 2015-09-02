'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:BranchesAndProgramsCtrl
 * @description
 * # BranchesAndProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('BranchesAndProgramsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        StateService,
        TableService,
        OfficeService,
        ConstantsService,
        NotificationService) {

      var officeId = $stateParams.officeId;
      $scope.view = {};
      $scope.view.isOfficeLoading = false;
      $scope.view.office = {};

      $scope.view.programs = [];
      $scope.view.branches = [];
      $scope.view.totalNumberOfPrograms = 0;
      $scope.view.skippedNumberOfPrograms = 0;
      $scope.view.numberOfPrograms = 0;
      $scope.view.programFilter = '';
      $scope.view.isLoadingPrograms = false;
      $scope.view.isLoadingBranches = true;
      $scope.view.header = 'Branches & Programs';
      $scope.view.programsLimit = 25;

      $scope.view.onCreateProgramClick = function () {
          var addProgramModalInstance = $modal.open({
              animation: false,
              templateUrl: 'views/program/addprogrammodal.html',
              controller: 'AddProgramModalCtrl',
              size: 'lg',
              resolve: {
                  office: function () {
                      return $scope.view.office
                  },
                  parentProgram: function () {
                      return null;
                  }
              }
          });
          addProgramModalInstance.result.then(function (addedProgram) {
              $log.info('Finished adding program.');
              addProgramModalInstance.close(addedProgram);
              StateService.goToProgramState(addedProgram.id);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      function getIds(element) {
          return element.id;
      };

      function updatePagingDetails(total, start, count) {
          $scope.view.totalNumberOfPrograms = total;
          $scope.view.skippedNumberOfPrograms = start;
          $scope.view.numberOfPrograms = count;
      }

      function setFundingTabEnabled(isEnabled) {
          $scope.isFundingTabEnabled = isEnabled;
      }

      function updateHeader() {
          if ($scope.view.branches.length === 0) {
              $scope.view.header = "Programs";
          }
      }

      $scope.view.getPrograms = function (tableState) {
          $scope.view.isLoadingPrograms = true;
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter(),
              keyword: TableService.getKeywords()
          };
          return OfficeService.getPrograms(params, officeId)
              .then(function (data, status, headers, config) {
                  $scope.view.isLoadingPrograms = false;
                  processData(data, tableState, params);
              })
              .catch(function (response) {
                  var message = "Unable to load office programs.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              })
      }

      function processData(response, tableState, params) {
          var programs = response.data.results;
          var total = response.data.total;
          var start = 0;
          if (programs.length > 0) {
              start = params.start + 1;
          };
          var count = params.start + programs.length;

          updatePagingDetails(total, start, count);

          var limit = TableService.getLimit();
          tableState.pagination.numberOfPages = Math.ceil(total / limit);

          $scope.view.programs = programs;
          $scope.view.programsLoading = false;
      };

      function loadChildOffices(officeId) {
          $scope.view.isLoadingBranches = true;
          return OfficeService.getChildOffices(officeId)
              .then(function (response, status, headers, config) {
                  $scope.view.isLoadingBranches = false;
                  $scope.view.branches = response.data;
                  updateHeader();
              })
          .catch(function (response) {
              $scope.view.isLoadingBranches = false;
              var message = "Unable to load child offices and branches.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          })
      }

      isLoadingOffice(true);
      $scope.data.loadedOfficePromise.promise
      .then(function (office) {
          $scope.view.office = office;
          var params = {
              start: 0,
              limit: 300
          };
          return $q.all([loadChildOffices(officeId)])
          .then(function () {
              $log.info('Loaded office specific data.');
              isLoadingOffice(false);
          });
      });

      function isLoadingOffice(isLoading) {
          $scope.view.isOfficeLoading = isLoading;
      }

  });
