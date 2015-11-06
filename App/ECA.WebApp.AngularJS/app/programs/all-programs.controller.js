'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllProgramsCtrl', function ($scope,
      $q,
      $stateParams,
      $state,
      smoothScroll,
      ProgramService,
      LookupService,
      TableService,
      orderByFilter,
      ConstantsService,
      AuthService,
      OfficeService,
      $timeout,
      $log,
      NotificationService) {

      $scope.view = {};
      $scope.view.totalNumberOfPrograms = 0;
      $scope.view.skippedNumberOfPrograms = 0;
      $scope.view.numberOfPrograms = 0;
      $scope.view.ecaUserId = null;
      $scope.view.showDraftsOnly = false;
      $scope.view.programs = [];
      $scope.view.programsLoading = false;
      $scope.view.programFilter = [];
      $scope.view.hierarchyKey = "hierarchy";
      $scope.view.alphabeticalKey = "alpha";
      $scope.view.listType = $scope.view.hierarchyKey;

      $scope.view.onSearchChange = function () {
          $scope.view.listType = $scope.view.alphabeticalKey;
      };

      $scope.view.onProgramFiltersChange = function () {
          console.assert($scope.getAllProgramsTableState, "The table state function must exist.");
          var tableState = $scope.getAllProgramsTableState();
          return $scope.view.getPrograms(tableState);
      }

      $scope.view.onExpandClick = function (program) {
          program.isExpanded = true;
          showChildren(program);
          scrollToProgram(program);
      }

      $scope.view.onCollapseClick = function (program) {
          program.isExpanded = false;
          hideChildren(program);
          scrollToProgram(program);
      }

      function scrollToProgram(program) {
          var id = program.divId;
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 115,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          }
          var element = document.getElementById(id)
          smoothScroll(element, options);
      }

      function showChildren(program) {
          angular.forEach(program.children, function (child, index) {
              child.isHidden = false;
              child.isExpanded = false;
          });
      }

      function hideChildren(program) {
          for (var i = 0; i < program.children.length; i++) {
              var child = program.children[i];
              if (child.children && child.children.length > 0) {
                  hideChildren(child);
              }
              child.isHidden = true;
              child.isExpanded = false;
          }
      }

      function loadUserId() {
          return AuthService.getUserInfo()
              .then(function (response) {
                  $scope.view.ecaUserId = response.data.ecaUserId;
                  return response.data.ecaUserId;
              })
              .catch(function () {
                  $log.error('Unable to load user info.');
              });
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

          if (!params.filter) {
              params.filter = [];
          }

          if ($scope.view.showDraftsOnly) {
              params.filter.push({ property: 'programStatusId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.programStatus.draft.id });
              params.filter.push({ property: 'createdByUserId', comparison: ConstantsService.equalComparisonType, value: $scope.view.ecaUserId });
          }
          else {
              params.filter.push({ property: 'programStatusId', comparison: ConstantsService.notEqualComparisonType, value: ConstantsService.programStatus.draft.id });
          }
          if ($scope.view.listType === $scope.view.alphabeticalKey) {
              return loadProgramsAlphabetically(params, tableState);
          }
          else {
              return loadProgramsInHierarchy(params, tableState);
          }
      };

      function loadProgramsAlphabetically(params, tableState) {
          $scope.view.programsLoading = true;
          return ProgramService.getAllProgramsAlpha(params)
          .then(function (data) {
              angular.forEach(data.results, function (program, index) {
                  program.isRoot = true;
              });
              processData(data, tableState, params);
          })
          .catch(function (response) {
              var message = "Unable to load programs.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      };

      function loadProgramsInHierarchy(params, tableState) {
          $scope.view.programsLoading = true;
          return ProgramService.getAllProgramsHierarchy(params)
          .then(function (data) {
              processData(data, tableState, params);
          });
      };

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

          angular.forEach(programs, function (program, index) {
              if (program.children) {
                  if (program.isRoot) {
                      program.isHidden = false;
                  }
                  else {
                      program.isHidden = true;
                  }
                  program.isExpanded = false;
              }
              program.divId = 'program' + program.programId;
          });

          $scope.view.programs = programs;
          $scope.view.programsLoading = false;
          return programs;
      };

      function updatePagingDetails(total, start, count) {
          $scope.view.totalNumberOfPrograms = total;
          $scope.view.skippedNumberOfPrograms = start;
          $scope.view.numberOfPrograms = count;
      };


      $q.all([loadUserId()])
      .then(function (results) {

      })
      .catch(function () {

      });
  });

