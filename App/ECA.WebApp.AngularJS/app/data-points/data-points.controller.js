'use strict';

angular.module('staticApp')
  .controller('DataPointsCtrl', function ($scope,$state, $q, $modalInstance, parameters, ConstantsService, OfficeService, ProgramService, ProjectService, DataPointConfigurationService, NotificationService) {

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
          else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              ProgramService.get(parameters.foreignResourceId)
                .then(function (response) {
                    $scope.resourceName = response.name;
                }, function () {
                    NotificationService.showErrorMessage('Unable to load program with id = ' + parameters.foreignResourceId + ".");
                });
          }
          else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              ProjectService.getById(parameters.foreignResourceId)
                .then(function (response) {
                    $scope.resourceName = response.data.name;
                }, function () {
                    NotificationService.showErrorMessage('Unable to load project with id = ' + parameters.foreignResourceId + ".");
                });
          }
      }

      function getDataPointConfigurations() {

          var params = {};

          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              params.officeId = parameters.foreignResourceId;
          }
          else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              params.programId = parameters.foreignResourceId;
          }
          else if (parameters.resourceType.id === ConstantsService.resourceType.project.id) {
              params.projectId = parameters.foreignResourceId;
          }
          DataPointConfigurationService.getDataPointConfigurations(params)
            .then(function (response) {
                $scope.dataConfigurations = response.data;
                loadOfficeSettings();
            }, function () {
                NotificationService.showErrorMessage('Unable to load data point configurations for id = ' + parameters.foreignResourceId + ".");
            });
      }

      function reloadCurrentState(dataConfiguration) {
          if (($state.current.name === "offices.overview" && dataConfiguration.categoryId === ConstantsService.dataPointCategory.office.id) ||
              ($state.current.name === "programs.overview" && dataConfiguration.categoryId === ConstantsService.dataPointCategory.program.id) || 
              ($state.current.name === "programs.edit" && dataConfiguration.categoryId === ConstantsService.dataPointCategory.program.id)) {
              $state.go($state.current, { }, { reload: true });
          }
      }

      function setResourceTypeId() {
          $scope.resourceTypeId = parameters.resourceType.id;
      }
      
      function loadOfficeSettings() {
          if (parameters.resourceType.id === ConstantsService.resourceType.office.id) {
              OfficeService.getSettings(parameters.foreignResourceId)
             .then(function (response) {
                 updateWithOfficeSettings(response.data);
             });
          } else if (parameters.resourceType.id === ConstantsService.resourceType.program.id) {
              ProgramService.get(parameters.foreignResourceId)
             .then(function (response) {
                 OfficeService.getSettings(response.ownerOrganizationId)
                 .then(function (response) {
                     updateWithOfficeSettings(response.data);
                 });
             });
          }
      }

      function updateWithOfficeSettings(officeSettings) {
          for (var i = 0; i < $scope.dataConfigurations.length; i++) {
              if ($scope.dataConfigurations[i].propertyId === ConstantsService.dataPointProperty.categories.id) {
                  if (officeSettings.isCategoryRequired) {
                      $scope.dataConfigurations[i].propertyName = officeSettings.categoryLabel + ' / ' + officeSettings.focusLabel;
                  } else {
                      $scope.dataConfigurations.splice(i, 1);
                      i--;
                  }
              }
              if ($scope.dataConfigurations[i].propertyId === ConstantsService.dataPointProperty.objectives.id) {
                  if (officeSettings.isObjectiveRequired) {
                      $scope.dataConfigurations[i].propertyName = officeSettings.objectiveLabel + ' / ' + officeSettings.justificationLabel;
                  } else {
                      $scope.dataConfigurations.splice(i, 1);
                      i--;
                  }
              }
          }
      }

      $q.all(setResourceTypeId(), getResourceName(), getDataPointConfigurations());
  });