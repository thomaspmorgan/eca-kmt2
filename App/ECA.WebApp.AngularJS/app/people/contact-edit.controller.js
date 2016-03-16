'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactEditCtrl
 * # personContactEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactEditCtrl', function ($scope, PersonService, NotificationService, $stateParams, $log, $q) {

      $scope.contactsLoading = true;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadContactInfo(personId);
      });

      function loadContactInfo(personId) {
          $scope.contactsLoading = true;
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contactInfo = data;
              $scope.sevisStatus.statusName = data.sevisStatus;
              $scope.sevisStatus.statusNameId = data.sevisStatusId;
              $scope.contactsLoading = false;
          });
      };

      $scope.cancelEditContact = function () {
          $scope.edit.Contact = false;
      };

      $scope.saveEditContact = function () {
          PersonService.updateContactInfo($scope.contactInfo, $stateParams.personId)
          .then(function () {
              NotificationService.showSuccessMessage("The edit was successful.");
              loadContactInfo($stateParams.personId);
              $scope.edit.Contact = false;
          },
            function (error) {
                if (error.status === 400) {
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