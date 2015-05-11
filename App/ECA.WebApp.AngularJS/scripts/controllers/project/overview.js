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
        ProjectService,
        OfficeService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.categoryLabel = "...";
      $scope.objectiveLabel = "...";
      
      function loadOfficeSettings() {
          var officeId = $stateParams.officeId;
          return OfficeService.getSettings(officeId)
              .then(function (response) {
                  $log.info('Loading office settings for office with id ' + officeId);
                  var categorySetting = OfficeService.getSettingsValue(response.data, ConstantsService.officeCategorySettingName) || 'Category';
                  var focusSetting = OfficeService.getSettingsValue(response.data, ConstantsService.officeFocusSettingName) || 'Focus';
                  var justificationSetting = OfficeService.getSettingsValue(response.data, ConstantsService.officeJustificationSettingName) || 'Justification';
                  var objectiveSetting = OfficeService.getSettingsValue(response.data, ConstantsService.officeObjectiveSettingName) || 'Objective';

                  $scope.categoryLabel = focusSetting + '/' + categorySetting;
                  $scope.objectiveLabel = objectiveSetting + '/' + justificationSetting;

              }, function (errorResponse) {
                  $log.error('Failed to load office settings.');
              });
      }

      function loadProject() {
          var projectId = $stateParams.projectId;
          return ProjectService.get(projectId)
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
