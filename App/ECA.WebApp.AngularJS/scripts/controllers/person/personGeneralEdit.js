'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personGeneralCtrl
 * # personGeneralEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personGeneralEditCtrl', function ($scope, PersonService, NotificationService, LookupService, $stateParams) {

      $scope.toggleEdit = $scope.toggleEditGeneral;

      $scope.general = [];

      $scope.editView = [];
      $scope.editView.prominentCategories = [];
      $scope.editView.selectedProminentCategories = [];

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadProminentCategories();
            loadGeneral(personId);
      });

     function loadGeneral(personId) {
          PersonService.getGeneralById(personId)
          .then(function (data) {
              $scope.general = data;
              $scope.editView.selectedProminentCategories = $scope.general.prominentCategories.map(function (obj) {
                  var prominentCategory = {};
                  prominentCategory.id = obj.id;
                  prominentCategory.name = obj.value;
                  return prominentCategory;
              });
          });
      };

     function loadProminentCategories() {
         LookupService.getAllProminentCategories({ limit: 300 })
        .then(function (data) {
            $scope.editView.prominentCategories = data.data.results;
        });
     };

      $scope.cancelEditGeneral = function () {
          $scope.toggleEdit();
      };

      $scope.saveEditGeneral = function () {
          $scope.general.prominentCategories = $scope.editView.selectedProminentCategories.map(function (c) { return c.id });
          PersonService.updateGeneral($scope.general, $scope.general.personId)
          .then(function () {
              NotificationService.showSuccessMessage("The edit was successful.");
              loadGeneral($scope.general.personId);
              $scope.toggleEdit();
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