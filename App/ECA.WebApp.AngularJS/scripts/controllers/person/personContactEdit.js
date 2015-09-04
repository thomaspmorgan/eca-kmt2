'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactEditCtrl
 * # personContactEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactEditCtrl', function ($scope, PersonService, $stateParams, $log) {

      $scope.loadingContacts = true;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadContactInfo(personId);
      });

      function loadContactInfo(personId) {
          $scope.loadingContacts = true;
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contact = data;
              $scope.loadingContacts = false;
          });
      };

      $scope.cancelEditContact = function () {
          $scope.edit.Contact = false;
      };

      $scope.saveEditContact = function () {
          //$scope.general.prominentCategories = $scope.editView.selectedProminentCategories.map(function (c) { return c.id });
          //PersonService.updateGeneral($scope.general, $scope.general.personId)
          //.then(function () {
          //    NotificationService.showSuccessMessage("The edit was successful.");
          //    loadGeneral($scope.general.personId);
          $scope.edit.Contact = false;
          //},
          //  function (error) {
          //      if (error.status == 400) {
          //          if (error.data.message && error.data.modelState) {
          //              for (var key in error.data.modelState) {
          //                  NotificationService.showErrorMessage(error.data.modelState[key][0]);
          //              }
          //          }
          //          else if (error.data.Message && error.data.ValidationErrors) {
          //              for (var key in error.data.ValidationErrors) {
          //                  NotificationService.showErrorMessage(error.data.ValidationErrors[key]);
          //              }
          //          } else {
          //              NotificationService.showErrorMessage(error.data);
          //          }
          //      }
          //  });
      }
}); 