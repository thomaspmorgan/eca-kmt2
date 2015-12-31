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
      $scope.view.hierarchyKey = "hierarchy";
      $scope.view.alphabeticalKey = "alpha";
      $scope.view.listType = $scope.view.hierarchyKey;

      $scope.view.onCreateProgramClick = function () {
          var addProgramModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/programs/add-program-modal.html',
              controller: 'AddProgramModalCtrl',
              backdrop: 'static',
              size: 'lg',
              resolve: {
                  office: function () {
                      return $scope.view.office;
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

      $scope.view.onSearchChange = function () {
          $scope.view.listType = $scope.view.alphabeticalKey;
      };

      $scope.view.onProgramFiltersChange = function () {
          console.assert($scope.getAllProgramsTableState, "The table state function must exist.");
          var tableState = $scope.getAllProgramsTableState();
          return $scope.view.getPrograms(tableState);
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
          //remove keyword search parameter if viewing programs in hiearchy
          if ($scope.view.listType === $scope.view.hierarchyKey && tableState.search && tableState.search.predicateObject) {
              delete tableState.search.predicateObject.$;
          }

          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter(),
              keyword: TableService.getKeywords()
          };          
          if ($scope.view.listType === $scope.view.alphabeticalKey) {
              return loadProgramsAlphabetically(params, tableState);
          }
          else {
              return loadProgramsInHierarchy(params, tableState);
          }
      };

      function loadProgramsAlphabetically(params, tableState) {
          $scope.view.isLoadingPrograms = true;
          return OfficeService.getProgramsAlphabetically(params, officeId)
          .then(function (response) {
              angular.forEach(response.data.results, function (program, index) {
                  program.isRoot = true;
                  program.isHidden = false;
                  program.programLevel = 0;
              });
              processData(response, tableState, params);
          })
          .catch(function (response) {
              var message = "Unable to load programs.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      };

      function loadProgramsInHierarchy(params, tableState) {
          $scope.view.isLoadingPrograms = true;
          return OfficeService.getProgramsByHierarchy(params, officeId)
          .then(function (response) {
              processData(response, tableState, params);
          })
          .catch(function (response) {
              var message = "Unable to load programs.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      };


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
          $scope.view.isLoadingPrograms = false;
      };

      function loadChildOffices(officesId) {
          $scope.view.isLoadingBranches = true;
          return OfficeService.getChildOffices(officesId)
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
