'use strict';

angular.module('staticApp')
  .controller('DataPointsCtrl', function ($scope,$state, $q, $modalInstance, parameters, ConstantsService, OfficeService, DataPointConfigurationService, NotificationService) {

      $scope.expandOfficeSection = true;
      $scope.expandProgramSection = true;
      $scope.expandProjectSection = true;
      $scope.expandPersonSection = true;

      $scope.close = function () {
          $modalInstance.close();
      }

      $scope.dataConfigurationChanged = function (dataConfiguration) {
          if (dataConfiguration.isRequired) {
              DataPointConfigurationService.createDataPointConfiguration(dataConfiguration)
                .then(function () {
                    NotificationService.showSuccessMessage('The data point configuration was successfully created.');
                }, function () {
                    getDataPointConfigurations();
                    NotificationService.showErrorMessage('There was an error creating the data point configuration.');
                }).finally(function () {
                    reloadCurrentState(dataConfiguration);
                });
          } else {
              DataPointConfigurationService.deleteDataPointConfiguration(dataConfiguration)
                .then(function () {
                    NotificationService.showSuccessMessage('The data point configuration was successfully removed.');
                }, function () {
                    getDataPointConfigurations();
                    NotificationService.showErrorMessage('There was an error removing the data point configuration.');
                }).finally(function () {
                    reloadCurrentState(dataConfiguration);
                });
          }
      }

      function getResourceName() {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              OfficeService.get(parameters.foreignResourceId)
                .then(function (response) {
                    $scope.resourceName = response.data.name;
                }, function () {
                    NotificationService.showErrorMessage('Unable to load office with id = ' + parameters.foreignResourceId + ".");
                });
          }
      }

      function getDataPointConfigurations() {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              OfficeService.getDataPointConfigurations(parameters.foreignResourceId)
                .then(function (response) {
                    $scope.dataConfigurations = response.data;
                }, function () {
                    NotificationService.showErrorMessage('Unable to load data point configurations for office with id = ' + parameters.foreignResourceId + ".");
                });
          }
      }

      function reloadCurrentState(dataConfiguration) {
          if ($state.current.name === "offices.overview" && dataConfiguration.categoryId === ConstantsService.dataPointCategory.office.id) {
              $state.go($state.current, { }, { reload: true });
          }
      }

      $q.all(getResourceName(), getDataPointConfigurations());
  });