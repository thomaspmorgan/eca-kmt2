'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramOverviewCtrl', function (
      $scope,
      $stateParams,
      $state,
      $log,
      orderByFilter
      ) {

      $scope.view = {};
      $scope.view.isLoadingProgram = true;
      $scope.view.isLoadingOfficeSettings = true;
      $scope.view.sortedCategories = [];
      $scope.view.sortedObjectives = [];
      $scope.view.categoryLabel = '';
      $scope.view.objectiveLabel = '';

      $scope.data.loadProgramPromise.promise
      .then(function (program) {
          $scope.view.program = program;
          $scope.view.sortedCategories = orderByFilter(program.categories, '+focusName');
          $scope.view.sortedObjectives = orderByFilter(program.objectives, '+justificationName');
          $scope.view.isLoadingProgram = false;
      })
      .catch(function (response) {
          $scope.view.isLoadingProgram = false;
      });

      $scope.data.loadOfficeSettingsPromise.promise
      .then(function (settings) {
          var objectiveLabel = settings.objectiveLabel;
          var categoryLabel = settings.categoryLabel;
          var focusLabel = settings.focusLabel;
          var justificationLabel = settings.justificationLabel;         
          $scope.view.categoryLabel = categoryLabel + ' / ' + focusLabel;
          $scope.view.objectiveLabel = objectiveLabel + ' / ' + justificationLabel;

          $scope.view.isLoadingOfficeSettings = false;
      })
      .catch(function (response) {
          $scope.view.isLoadingOfficeSettings = false;
      });

  });
