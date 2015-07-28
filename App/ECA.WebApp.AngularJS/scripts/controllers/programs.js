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
      ProjectService, TableService, LocationService, ConstantsService, LookupService, orderByFilter) {

      $scope.listOfNewItemOptions = ['Sub-program', 'Project'];

      $scope.currentCreatedItem = '';

      $scope.confirmClose = false;
      $scope.confirmFail = false;
      $scope.confirmSave = false;
      $scope.newProjectId = null;
      $scope.isSavingProject = false;
      $scope.validations = [];

      $scope.themes = [];
      $scope.categories = [];
      $scope.objectives = [];
      $scope.goals = [];
      $scope.regions = [];
      $scope.pointsOfContact = [];
      $scope.selectedFilterCountries = [];

      $scope.newProject = {
          title: '',
          description: ''
      };
      $scope.newProgram = {
          name: '',
          description: '',
          parentProgramId: null,
          ownerOrganizationId: null,
          programStatusId: null,
          startDate: new Date(),
          themes: [],
          categories: [],
          objectives: [],
          goals: [],
          regions: [],
          contacts: [],
          website: null
      };
      $scope.out = {
          Themes: [],
          Regions: [],
          Goals: [],
          Contacts: [],
          Categories: [],
          Objectives: [],
          OwnerOrganizationId: []
      };
      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          projects: {
              title: 'Sub-Programs & Projects',
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
          },
          funding: {
              title: 'Funding',
              path: 'moneyflows',
              active: true,
              order: 6
          }
      };

      $scope.header = "Sub-Programs and Projects";
      $scope.branches = [];
      $scope.subprograms = [];
      $scope.projects = [];

      $scope.sortedCategories = [];
      $scope.sortedObjectives = [];

      $scope.lookupParams = {
          start: null,
          limit: 100,
          sort: null,
          filter: null
      };

      $scope.parentLookupParams = {
          start: null,
          limit: 25,
          sort: null,
          filter: null
      };

      $scope.searchCountries = function (search) {
          return loadCountries(search);
      }

      function loadCountries(search) {
          var params = {
              start: 0,
              limit: 10,
              filter: [
                  {
                      comparison: ConstantsService.equalComparisonType,
                      value: ConstantsService.locationType.country.id,
                      property: 'locationTypeId'
                  }
              ]
          };
          if (search && search.length > 0) {
              params.filter.push({
                  comparison: ConstantsService.likeComparisonType,
                  value: search,
                  property: 'name'
              });
          }
          return LocationService.get(params)
          .then(function (data) {
              $scope.countries = data.results;
          });
      }

      LookupService.getAllThemes($scope.lookupParams)
        .then(function (data) {
            $scope.themes = data.results;
            angular.forEach($scope.themes, function (value, key) {
                $scope.themes[key].ticked = false;
            });
        });

      LookupService.getAllGoals($scope.lookupParams)
        .then(function (data) {
            $scope.goals = data.results;
            angular.forEach($scope.goals, function (value, key) {
                $scope.goals[key].ticked = false;
            })
        });

      LookupService.getAllContacts($scope.lookupParams)
          .then(function (data) {
              $scope.pointsOfContact = data.results;
              angular.forEach($scope.pointsOfContact, function (value, key) {
                  $scope.pointsOfContact[key].ticked = false;
              })
          });

      LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.region.id } })
            .then(function (data) {
                $scope.regions = data.results;
            });

      ProgramService.get($stateParams.programId)
          .then(function (program) {
              $scope.program = program;

              $scope.categoryLabel = program.ownerOfficeCategoryLabel;
              $scope.objectiveLabel = program.ownerOfficeObjectiveLabel;

              $scope.sortedCategories = orderByFilter($scope.program.categories, '+focusName');
              $scope.sortedObjectives = orderByFilter($scope.program.objectives, '+justificationName');
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
            .then(function () {
                $scope.projectsLoading = false;
            });

      }

      $scope.getSubPrograms = function (tableState) { // get the subprograms (first children of this program)

          $scope.subProgramsLoading = true;
          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          ProgramService.getSubPrograms($stateParams.programId, params)
            .then(function (response) {
                $scope.subprograms = response.data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(response.data.total / limit);
            })
            .then(function () {
                $scope.subProgramsLoading = false;
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
                    for (var key in createErrorData.data.ValidationErrors) {
                        $scope.validations.push(createErrorData.data.ValidationErrors[key]);
                    }
                    $scope.confirmFail = true;
                }
                else {
                    $scope.errorMessage = 'An Error has occurred.';
                    $scope.confirmFail = true;
                }
            })
          .then(function () {
              $scope.isSavingProject = false;
          });
      };

      $scope.clickCreate = function ($event) {
          $event.preventDefault();
          $scope.showCreateOptions = true;
      };

      $scope.modalClose = function () {
          if (unsavedChanges()) {
              $scope.confirmClose = true;
          }
          else {
              $scope.createProject = false;
              $scope.createProgram = false;
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

      $scope.createItem = function (createdItem) {
          switch (createdItem) {
              case "subprogram":
                  $scope.createNewSubProgram();
                  break;
              case "project":
                  $scope.createProject = true;
                  break;
          }

          $scope.showCreateOptions = false;

      };

      $scope.createNewSubProgram = function () {
          $scope.newProgram.parentProgramId = $stateParams.programId;
          $scope.newProgram.parentProgram = "Ambassadors Fund For Cultural Preservation (AFCP)";
          $scope.createProgram = true;

      };


  });
