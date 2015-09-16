'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEducationEmploymentEditCtrl
 * # personEducationEmploymentEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEducationEmploymentEditCtrl', function ($scope, PersonService, NotificationService, $stateParams, $log) {

      $scope.eduemp = [];

      $scope.personEduEmpLoading = true;
      
      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEmployments(personId);
          loadEducations(personId);
      });
      
      $scope.cancelEditEduEmp = function () {
          $scope.edit.EduEmp = false;
      };
      
      $scope.saveEditEducation = function () {
          PersonService.updateEduEmp($scope.eduemp, $scope.eduemp.Id)
          .then(function () {
              NotificationService.showSuccessMessage("The edit was successful.");
              loadEmployments($scope.general.personId);
              loadEducations($scope.general.personId);
              $scope.edit.EduEmp = false;
          },
            function (error) {
                if (error.status == 400) {
                    if (error.data.message && error.data.modelState) {
                        for (var key in error.data.modelState) {
                            NotificationService.showErrorMessage(error.data.modelState[key][0]);
                        }
                    }
                    else if (error.data.Message && error.data.ValidationErrors) {
                        for (var key in error.data.ValidationErrors) {
                            NotificationService.showErrorMessage(error.data.ValidationErrors[key]);
                        }
                    } else {
                        NotificationService.showErrorMessage(error.data);
                    }
                }
            });
      }

  });