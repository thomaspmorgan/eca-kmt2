'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramsCtrl', function ($scope, $stateParams, $state, ProgramService, ProjectService, TableService) {

      $scope.confirmClose = false;
      $scope.confirmFail = false;
      $scope.confirmSave = false;
      $scope.newProjectId = null;
      $scope.isSavingProject = false;
      $scope.validations = [];


      $scope.newProject = {
          title: '',
          description: ''
      };

      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          projects: {
              title: 'Branches & Projects',
              path: 'projects',
              active: true,
              order: 2
          },
          activity: {
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
          impact: {
              title: 'Impact',
              path: 'impact',
              active: true,
              order: 5
          }
      };

      $scope.branches = [];
      $scope.subprograms = [];
      $scope.projects = [];

      ProgramService.get($stateParams.programId)
          .then(function (program) {
              $scope.program = program;
          });

      $scope.projectsLoading = false;

      $scope.getProjects = function (tableState) {

          $scope.projectsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()

          };

          ProjectService.getProjectsByProgram($stateParams.programId, params)
            .then(function (response) {
                $scope.projects = response.data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(response.data.total / limit);
                
            })
            .then(function(){
                $scope.projectsLoading = false;
            });
      }

      $scope.saveProject = function () {
          var project = {
              name: $scope.newProject.title,
              description: $scope.newProject.description,
              projectStatusId: 5,
              programId: $scope.program.id
          }
          $scope.isSavingProject = true;
          ProjectService.create(project)
            .then(function (createSuccessData) {
                var createdProject = createSuccessData.data;
                $scope.confirmSave = true;
                $scope.newProjectId = createdProject.id;
            }, function (createErrorData) {
                if (createErrorData.status === 400 && createErrorData.data && createErrorData.data.ValidationErrors) {
                    $scope.errorMessage = createErrorData.data.Message;
                    for (var key in createErrorData.data.ValidationErrors)
                    {
                        $scope.validations.push(createErrorData.data.ValidationErrors[key]);
                    }
                    $scope.confirmFail = true;
                }
                else {
                    $scope.errorMessage = 'An Error has occurred.';
                    $scope.confirmFail = true;
                }   
            })
          .then(function() {
              $scope.isSavingProject = false;
          });
      };

      $scope.modalClose = function () {
          if (unsavedChanges()) {
              $scope.confirmClose = true;
          }
          else {
              this.modal.createProject = false;
          }
      };

      function unsavedChanges() {
          var unsavedChanges = false;
          if ($scope.newProject.title.length > 0 || $scope.newProject.description.length > 0) {
              unsavedChanges = true;
          }
          return unsavedChanges;
      }

      $scope.modalClear = function () {
          angular.forEach($scope.newProject, function (value, key) {
              $scope.newProject[key] = '';
          });
          $scope.formScope.projectForm.$setPristine();
      };

      $scope.setFormScope = function (scope) {
          $scope.formScope = scope;
      };

      $scope.updateProgram = function () {
          saveProgram();
      };

      $scope.addTab = function () {
          if ($scope.tabs.moneyflows.active && !$scope.program.moneyFlowReferences) {
              $scope.program.moneyFlowReferences = [];
          }
          if ($scope.tabs.artifacts.active && !$scope.program.artifactReferences) {
              $scope.program.artifactReferences = [];
          }
          saveProgram();
      };

      function saveProgram() {
          ProgramService.update($scope.program, $stateParams.programId)
              .then(function (program) {
                  $scope.program = program;
              });
      }

      $scope.confirmCloseYes = function () {
          $scope.confirmClose = false;
          this.modal.createProject = false;
      };

      $scope.confirmCloseNo = function () {
          this.modal.createProject = true;
          $scope.confirmClose = false;
      };

      $scope.confirmSaveYes = function () {
          $scope.confirmSave = false;
          $state.go('projects.overview', { officeId: $scope.program.ownerOrganizationId, programId: $scope.program.id, projectId: $scope.newProjectId });
          this.modal.createProject = false;
      };

      $scope.confirmFailOk = function () {
          $scope.confirmFail = false;
      };
  });
