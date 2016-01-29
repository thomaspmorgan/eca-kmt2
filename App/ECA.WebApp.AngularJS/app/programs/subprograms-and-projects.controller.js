'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SubProgramsAndProjectsCtrl', function (
      $scope,
      $stateParams,
      $state,
      $log,
      $modal,
      ProgramService,
      ProjectService,
      TableService,
      FilterService,
      LocationService,
      ConstantsService,
      NotificationService,
      StateService,
      LookupService,
      BrowserService,
      orderByFilter) {

      $scope.view = {};
      $scope.view.isLoadingProgram = true;
      $scope.header = "Sub-Programs and Projects";
      $scope.branches = [];
      $scope.subprograms = [];
      $scope.projects = [];
      $scope.regions = [];

      $scope.searchRegions = function (search) {
          return loadRegions(search);
      }

      var regionsFilter = FilterService.add('programs_regionsfilter');
      function loadRegions(search) {
          regionsFilter.reset();
          regionsFilter = regionsFilter.skip(0).take(100).equal('locationTypeId', ConstantsService.locationType.region.id);

          if (search && search.length > 0) {
              regionsFilter = regionsFilter.like('name', search);
          }
          return LocationService.get(regionsFilter.toParams())
          .then(function (data) {
              $scope.regions = data.results;
              return data.results;
          })
          .catch(function () {
              var message = 'Unable to load regions.';
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      $scope.getProjects = function (tableState) {
          $scope.projectsLoading = true;
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };
          return ProjectService.getProjectsByProgram($stateParams.programId, params)
            .then(function (response) {
                $scope.projectsLoading = false;
                $scope.projects = response.data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(response.data.total / limit);
                return $scope.projects;
            })
            .catch(function () {
                var message = "Unable to load projects";
                $log.error(message);
                NotificationService.showErrorMessage(message);
                $scope.projectsLoading = false;
            });

      }

      $scope.getSubPrograms = function (tableState) {
          $scope.subProgramsLoading = true;
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };
          return ProgramService.getSubPrograms($stateParams.programId, params)
            .then(function (response) {
                $scope.subProgramsLoading = false;
                $scope.subprograms = response.data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(response.data.total / limit);
                return $scope.subprograms;
            })
            .catch(function () {
                var message = "Unable to load projects";
                $log.error(message);
                NotificationService.showErrorMessage(message);
                $scope.projectsLoading = false;
            });
      }

      $scope.view.createSubProgram = function () {
          var addProgramModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/programs/add-program-modal.html',
              controller: 'AddProgramModalCtrl',
              backdrop: 'static',
              size: 'lg',
              resolve: {
                  office: function () {
                      return {
                          id: $scope.view.program.ownerOrganizationId,
                          name: $scope.view.program.ownerName
                      }
                  },
                  parentProgram: function () {
                      return $scope.view.program;
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

      $scope.view.createProject = function () {
          var addProgramModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/projects/add-project-modal.html',
              controller: 'AddProjectModalCtrl',
              size: 'lg',
              backdrop: 'static',
              resolve: {
                  office: function () {
                      return {
                          id: $scope.view.program.ownerOrganizationId,
                          name: $scope.view.program.ownerName
                      }
                  },
                  parentProgram: function () {
                      return $scope.view.program;
                  }
              }
          });
          addProgramModalInstance.result.then(function (addedProject) {
              $log.info('Finished adding project.');
              addProgramModalInstance.close(addedProject);
              StateService.goToProjectState(addedProject.id);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      $scope.view.isLoadingProgram = true;
      $scope.data.loadProgramPromise.promise
      .then(function (program) {
          BrowserService.setDocumentTitleByProgram(program, 'Sub-Programs and Projects');
          $scope.view.program = program;
          $scope.view.isLoadingProgram = false;
      })
      .catch(function (response) {
          $scope.view.isLoadingProgram = false;
      });

  });
