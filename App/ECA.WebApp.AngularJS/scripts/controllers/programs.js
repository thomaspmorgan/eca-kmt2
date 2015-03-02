'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramsCtrl', function ($scope, $stateParams, $state, ProgramService, ProjectService) {

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
              console.log(program.countryIsos);
          });

      $scope.projectsLoading = false;

      $scope.getProjects = function (tableState) {

          $scope.projectsLoading = true;

          var pagination = tableState.pagination;
          var start = pagination.start || 0;
          var limit = pagination.number || 25;

          var sort = getSort(tableState);
          var filter = getFilter(tableState);

          var params = {
              start: start,
              limit: limit,
              sort: sort,
              filter: filter

          };

          ProjectService.getProjectsByProgram($stateParams.programId, params)
            .then(function (data) {
                $scope.projects = data.results;
                pagination.numberOfPages = Math.floor(data.total/limit);
                $scope.projectsLoading = false;
            });
      }

      function getSort(tableState) {
          var sort = [];
          var predicate = tableState.sort.predicate;
          var reverse = tableState.sort.reverse;
          if (predicate !== null && reverse !== undefined) {
              sort.push({
                  property: predicate.replace(/'/g, ""),
                  direction: reverse === false ? "asc" : "desc"
              })
          }
          return sort;
      }

      function getFilter(tableState) {
          var filter = [];
          var predicateObject = tableState.search.predicateObject;
          if (predicateObject !== undefined) {
              for (var key in predicateObject) {
                  filter.push({
                      property: key,
                      value: predicateObject[key],
                      comparison: 'like'
                  });
              }
          }
          return filter;
      }

      $scope.saveProject = function () {
          var project = {
              name: $scope.newProject.title,
              description: $scope.newProject.description,
              projectStatusId: 5,
              startDate: new Date(Date.now()).toUTCString(),
              programId: $scope.program.programId
          }
          ProjectService.create(project)
            .then(function (createdProject) {
                $state.go('projects.overview', { officeId: $scope.program.owner.organizationId,  programId: $scope.program.programId, projectId: createdProject.projectId});
            });
      };

      $scope.modalClose = function () {
          var close = true;
          if (unsavedChanges()) {
              close = confirm('You have unsaved changes!\nAre you sure you want to close?');
          }
          return close;
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
  });
