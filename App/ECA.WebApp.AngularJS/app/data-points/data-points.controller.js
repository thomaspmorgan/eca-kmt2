'use strict';

angular.module('staticApp')
  .controller('DataPointsCtrl', function ($scope, $q, $modalInstance, parameters, ConstantsService, OfficeService, NotificationService) {

      $scope.expandOfficeSection = true;
      $scope.expandProgramSection = true;
      $scope.expandProjectSection = true;
      $scope.expandPersonSection = true;

      $scope.close = function () {
          $modalInstance.close();
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

      $q.all(getResourceName(), getDataPointConfigurations());
  });