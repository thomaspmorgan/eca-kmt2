'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramEditCtrl
 * @description
 * # ProgramEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramEditCtrl', function (
      $scope,
      $stateParams,
      $state,
      $log,
      orderByFilter
      ) {

      $scope.view = {};
      $scope.view.isLoadingProgram = true;
      $scope.view.sortedCategories = [];
      $scope.view.sortedObjectives = [];
      $scope.view.categoryLabel = '';
      $scope.view.objectiveLabel = '';

      $scope.data.loadProgramPromise.promise
      .then(function (program) {
          $scope.view.program = program;
          $scope.view.categoryLabel = program.ownerOfficeCategoryLabel;
          $scope.view.objectiveLabel = program.ownerOfficeObjectiveLabel;
          $scope.view.sortedCategories = orderByFilter($scope.program.categories, '+focusName');
          $scope.view.sortedObjectives = orderByFilter($scope.program.objectives, '+justificationName');
          $scope.view.isLoadingProgram = false;
      })
      .catch(function (response) {
          $scope.view.isLoadingProgram = false;
      });

  });
