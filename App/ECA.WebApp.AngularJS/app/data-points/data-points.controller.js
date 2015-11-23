﻿'use strict';

angular.module('staticApp')
  .controller('DataPointsCtrl', function ($scope, $q, $modalInstance, parameters, ConstantsService, OfficeService, NotificationService) {

      $scope.showOfficeSection = true;
      $scope.showProgramSection = true;
      $scope.showProjectSection = true;
      $scope.showPersonSection = true;

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
                    
                    var dataPointConfigurations = { office: {} };
                    for (var i = 0; i < response.data.length; i++) {
                        if (response.data[i].category === "office") {
                            dataPointConfigurations.office[response.data[i].property] = response.data[i].isHidden;
                        }
                    }

                    console.log(dataPointConfigurations);

                }, function () {
                    NotificationService.showErrorMessage('Unable to load data point configurations for office with id = ' + parameters.foreignResourceId + ".");
                });
          }
      }

      $q.all(getResourceName(), getDataPointConfigurations());
  });