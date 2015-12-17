'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectOverviewCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        orderByFilter,
        ProjectService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isLoadingOfficeSettings = false;
      $scope.categoryLabel = "...";
      $scope.objectiveLabel = "...";
      $scope.sortedCategories = [];
      $scope.sortedObjectives = [];
      
      $scope.view.isLoadingOfficeSetting = true;
      $scope.$parent.data.loadOfficeSettingsPromise.promise
        .then(function (settings) {
            console.assert(settings.objectiveLabel, "The objective label must exist.");
            console.assert(settings.categoryLabel, "The category label must exist.");
            console.assert(settings.focusLabel, "The focus label must exist.");
            console.assert(settings.justificationLabel, "The justification label must exist.");
            console.assert(typeof (settings.isCategoryRequired) !== 'undefined', "The is category required bool must exist.");
            console.assert(typeof (settings.isObjectiveRequired) !== 'undefined', "The is objective required bool must exist.");

            var objectiveLabel = settings.objectiveLabel;
            var categoryLabel = settings.categoryLabel;
            var focusLabel = settings.focusLabel;
            var justificationLabel = settings.justificationLabel;

            $scope.categoryLabel = categoryLabel + ' / ' + focusLabel;
            $scope.objectiveLabel = objectiveLabel + ' / ' + justificationLabel;
            $scope.view.isLoadingOfficeSetting = false;
        });

      $scope.view.isLoading = true;
      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.sortedCategories = orderByFilter($scope.$parent.project.categories, '+focusName');
          $scope.sortedObjectives = orderByFilter($scope.$parent.project.objectives, '+justificationName');
          $scope.view.isLoading = false;
      });
  });
