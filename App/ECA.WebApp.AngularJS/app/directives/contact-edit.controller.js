'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactEditCtrl
 * # personContactEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactEditCtrl', function ($scope, PersonService, ParticipantPersonsService, NotificationService, $stateParams, $log, $q) {

      $scope.edit = {};
      $scope.edit.contactsLoading = true;

      var personId = $scope.personid;

      loadContactInfo(personId);

      function loadContactInfo(personId) {
          $scope.edit.contactsLoading = true;
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contactInfo = data;
              $scope.edit.contactsLoading = false;
          });
      };

      $scope.$watch('personid', function () {
          loadContactInfo($scope.personid);
      });

      $scope.cancelEditContact = function () {
          $scope.$parent.$parent.editMode = false;
      };

      $scope.saveEditContact = function () {
          PersonService.updateContactInfo($scope.contactInfo, personId)
          .then(function () {
              NotificationService.showSuccessMessage("The edit was successful.");
              loadContactInfo(personId);
              $scope.$parent.$parent.editMode = false;
          },
            function (error) {
                if (error.status === 400) {
                    if (error.data.message && error.data.modelState) {
                        for (var key in error.data.modelState) {
                            NotificationService.showErrorMessage(error.data.modelState[key][0]);
                        }
                    } else if (error.data.Message && error.data.ValidationErrors) {
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