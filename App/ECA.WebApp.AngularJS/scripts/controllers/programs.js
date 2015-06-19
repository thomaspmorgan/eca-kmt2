'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramsCtrl', function ($scope, $stateParams, $state, ProgramService, 
      ProjectService, TableService, LocationService, ConstantsService, orderByFilter) {

      $scope.confirmClose = false;
      $scope.confirmFail = false;
      $scope.confirmSave = false;
      $scope.newProjectId = null;
      $scope.isSavingProject = false;
      $scope.validations = [];

      $scope.showIncludedRegion = [];
      $scope.regions = {};

      $scope.filteredRegions = [];

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
              title: 'Subprograms & Projects',
              path: 'projects',
              active: true,
              order: 2
          },
          activity: {
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
          impact: {
              title: 'Impact',
              path: 'impact',
              active: true,
              order: 5
          }
      };

      $scope.header = "Subprograms and Projects";
      $scope.branches = [];
      $scope.subprograms = [];
      $scope.projects = [];

      $scope.sortedCategories = [];
      $scope.sortedObjectives = [];
      
      ProgramService.get($stateParams.programId)
          .then(function (program) {
              $scope.program = program;

              $scope.categoryLabel = program.ownerOfficeCategoryLabel;
              $scope.objectiveLabel = program.ownerOfficeObjectiveLabel;

              $scope.sortedCategories = orderByFilter($scope.program.categories, '+focusName');
              $scope.sortedObjectives = orderByFilter($scope.program.objectives, '+justificationName');
          });

      LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.region.id } })
            .then(function (data) {
                $scope.regions = data.results;
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
                angular.forEach($scope.projects, function (value) {
                    $scope.showIncludedRegion[value.projectId] = true;
                });
            });
          //move to getSubPrograms once written
          updateHeader();
      }

      //add subprograms

      function updateHeader() {
          if ($scope.subprograms.length === 0) {
              $scope.header = "Projects";
              $scope.tabs.projects.title = "Projects";
          }
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

      $scope.changeRegionFilter = function () {
          var regions = $('#regionSelect').val();

          var items = $("#regionSelect option:selected").map(function () {
              return $(this).text();
          }).get();

          $scope.selectedRegions = items.join();

          if (regions == null) {
              // all regions should be displayed
              angular.forEach($scope.projects, function (value) {
                  $scope.showIncludedRegion[value.projectId] = true;
              });
          }
          else {
              angular.forEach($scope.projects, function (value) {

                  $scope.showIncludedRegion[value.projectId] = false;
                  angular.forEach(regions, function (selectedRegion) {

                      if (value.regionIds.indexOf(parseInt(selectedRegion)) > -1) {
                          $scope.showIncludedRegion[value.projectId] = true;
                      }
                  });
              });
          };
      };
  });
