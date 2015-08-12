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
        OfficeService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.categoryLabel = "...";
      $scope.objectiveLabel = "...";
      $scope.sortedCategories = [];
      $scope.sortedObjectives = [];
      
      
      function loadOfficeSettings() {
          var officeId = $stateParams.officeId;
          return OfficeService.getSettings(officeId)
              .then(function (response) {
                  $log.info('Loading office settings for office with id ' + officeId);
                  console.assert(response.data.objectiveLabel, "The objective label must exist.");
                  console.assert(response.data.categoryLabel, "The category label must exist.");
                  console.assert(response.data.focusLabel, "The focus label must exist.");
                  console.assert(response.data.justificationLabel, "The justification label must exist.");
                  console.assert(typeof (response.data.isCategoryRequired) !== 'undefined', "The is category required bool must exist.");
                  console.assert(typeof (response.data.isObjectiveRequired) !== 'undefined', "The is objective required bool must exist.");

                  var objectiveLabel = response.data.objectiveLabel;
                  var categoryLabel = response.data.categoryLabel;
                  var focusLabel = response.data.focusLabel;
                  var justificationLabel = response.data.justificationLabel;

                  $scope.categoryLabel = categoryLabel + '/' + focusLabel;
                  $scope.objectiveLabel = objectiveLabel + '/' + justificationLabel;

              }, function (errorResponse) {
                  $log.error('Failed to load office settings.');
              });
      }

      function loadProject() {
          var projectId = $stateParams.projectId;
          return ProjectService.getById(projectId)
            .then(function (data) {
                $log.info('Successfully loaded project.');
                $scope.$parent.project = data.data;
                $scope.$parent.project.countryIsos = $scope.$parent.project.countryIsos || [];
                var startDate = new Date($scope.$parent.project.startDate);
                if (!isNaN(startDate.getTime())) {
                    $scope.$parent.project.startDate = startDate;
                }
                var endDate = new Date($scope.$parent.project.endDate);
                if (!isNaN(endDate.getTime())) {
                    $scope.$parent.project.endDate = endDate;
                }
                $scope.sortedCategories = orderByFilter($scope.$parent.project.categories, '+focusName');
                $scope.sortedObjectives = orderByFilter($scope.$parent.project.objectives, '+justificationName');

            }, function (errorResponse) {
                $log.error('Failed to load project with id ' + projectId);
            });
      }

      $scope.view.isLoading = true;
      $q.all([loadProject(), loadOfficeSettings()])
      .then(function (results) {
          //results is an array

      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });
  });
