'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectCtrl', function ($scope, $state, $stateParams, $log, $q,
      ProjectService,
      AuthService,
      StateService,
      ConstantsService,
      NotificationService,
      OfficeService,
      orderByFilter,
      DataPointConfigurationService) {

      $scope.project = {};
      $scope.modalForm = {};
      $scope.currencyTypes = {};
      $scope.currentProjectId = $stateParams.projectId;

      $scope.currentlyEditing = false;

      $scope.isProjectEditCancelButtonVisible = false;
      $scope.showProjectEditCancelButton = false;
      $scope.isProjectStatusButtonInEditMode = false;
      $scope.isInEditViewState = false;
      $scope.projectStatusButtonText = "...";
      $scope.isTransactionDatePickerOpen = false;

      $scope.data = {};
      $scope.data.loadProjectByIdPromise = $q.defer();
      $scope.data.loadOfficeSettingsPromise = $q.defer();
      $scope.data.loadDataPointConfigurationsPromise = $q.defer();

      $scope.tabs = {
          overview: { title: 'Overview', path: 'overview', active: true, order: 1 },
          partners: { title: 'Partners', path: 'partners', active: false, order: 3 },
          participants: { title: 'Participants', path: 'participants', active: true, order: 2 },
          artifacts: { title: 'Attachments', path: 'artifacts', active: true, order: 4 },
          moneyflows: { title: 'Funding', path: 'moneyflows', active: true, order: 5 },
          impact: { title: 'Impact', path: 'impact', active: false, order: 6 },
          activity: { title: 'Timeline', path: 'activity', active: true, order: 7 },
          itinerary: { title: 'Travel', path: 'itineraries', active: true, order: 8 }
      };

      function enabledProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = true;
      }

      function disableProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = false;
      }

      ProjectService.getById($stateParams.projectId)
        .then(function (data) {
            $scope.project = data.data;
            $log.info('Successfully loaded project.');
            if (angular.isArray($scope.project.participants)) {
                $scope.tabs.participants.active = true;
            }
            if (angular.isArray($scope.project.agreements)) {
                $scope.tabs.partners.active = true;
            }
            if (angular.isArray($scope.project.moneyFlows)) {
                $scope.tabs.moneyflows.active = true;
            }
            $scope.project.countryIsos = $scope.project.countryIsos || [];
            var startDate = new Date($scope.project.startDate);
            if (!isNaN(startDate.getTime())) {
                $scope.project.startDate = startDate;
            }
            var endDate = new Date($scope.project.endDate);
            if (!isNaN(endDate.getTime())) {
                $scope.project.endDate = endDate;
            }
            $scope.data.loadProjectByIdPromise.resolve($scope.project);
            var officeId = $scope.project.ownerId;
            OfficeService.getSettings(officeId)
            .then(function (response) {
                var data = response.data;
                $scope.data.loadOfficeSettingsPromise.resolve(data);
            })
            .catch(function () {
                $log.error('Unable to load office settings.');
                NotificationService.showErrorMessage('Unable to load office settings.');
            });
        });

      var editStateName = StateService.stateNames.edit.project;

      $scope.showProjectEditCancelButton = function () {
          $scope.isProjectEditCancelButtonVisible = true;
      }

      $scope.hideProjectEditCancelButton = function () {
          $scope.isProjectEditCancelButtonVisible = false;
      }
      $scope.isInEditViewState = $state.current.name === editStateName;
      if ($scope.isInEditViewState) {
          $scope.showProjectEditCancelButton();
      }
      else {
          $scope.hideProjectEditCancelButton();
      }
      $scope.onProjectStatusButtonClick = function ($event) {
          if ($state.current.name === editStateName) {
              $scope.$broadcast(ConstantsService.saveProjectEventName);
          }
          else {
              $state.go(editStateName);
              $scope.showProjectEditCancelButton();
          }
      };

      $scope.onCancelButtonClick = function ($event) {
          $scope.$broadcast(ConstantsService.cancelProjectEventName);
      }

      $scope.params = $stateParams;

      $scope.updateProject = function () {
          saveProject();
      };

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var projectId = $stateParams.projectId;
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.editProject.value] = {
              hasPermission: function () {
                  enabledProjectStatusButton();
                  $log.info('User has edit project permission in project.js controller.');
              },
              notAuthorized: function () {
                  disableProjectStatusButton();
                  $log.info('User not authorized to edit project  in project.js controller.');
              }
          };
          config[ConstantsService.permission.viewProject.value] = {
              hasPermission: function () {
                  $scope.tabs.moneyflows.active = true;
                  $log.info('User has view project permission in project.controller.js controller.');
              },
              notAuthorized: function () {
                  $scope.tabs.moneyflows.active = false;
                  $log.info('User not authorized to view project in project.controller.js controller.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      }

      function loadDataPointConfigurations() {
          var params = { projectId: $stateParams.projectId };
          return DataPointConfigurationService.getDataPointConfigurations(params)
           .then(function (response) {
               var data = response.data;
               $scope.data.loadDataPointConfigurationsPromise.resolve(data);
           }, function () {
               NotificationService.showErrorMessage('Unable to load data point configurations for id = ' + params.projectId + ".");
           });
      }

      $q.all([loadPermissions(), loadDataPointConfigurations()])
      .then(function () {

      }, function () {

      });
  });
