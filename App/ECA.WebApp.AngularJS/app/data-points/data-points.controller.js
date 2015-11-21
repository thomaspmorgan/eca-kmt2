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

      $q.all(getResourceName());
  });