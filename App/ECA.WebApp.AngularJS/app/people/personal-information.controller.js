'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
  .controller('PersonInformationCtrl', function ($scope, $stateParams, $timeout, $q, smoothScroll, MessageBox) {

      $scope.showEvalNotes = true;
      $scope.showEduEmp = true;
      $scope.showGeneral = true;
      $scope.showPii = false;
      $scope.showContact = true;
      $scope.edit = {};
      $scope.edit.General = false;
      $scope.edit.Pii = false;
      $scope.edit.Contact = false;
      $scope.edit.EduEmp = false;
      $scope.sevisStatus = {statusName: ""};

      // TODO: use constant service
      var notifyStatuses = ["Sent To DHS", "Queued To Submit", "Validated", "SEVIS Received"];

      $scope.editGeneral = function () {
          CreateMessageBox($scope.edit.General)
          .then(function (response) {
              $scope.edit.General = status;
          });
      }

      $scope.editPii = function () {
          CreateMessageBox($scope.edit.Pii)
          .then(function (response) {
              $scope.edit.Pii = response;
          });
      }

      $scope.editContact = function () {
          CreateMessageBox($scope.edit.Contact)
          .then(function (response) {
              $scope.edit.Contact = status;
          });
      }

      $scope.editEduEmp = function () {
          CreateMessageBox($scope.edit.EduEmp)
          .then(function (response) {
              $scope.edit.EduEmp = status;
          });
      }

      function CreateMessageBox(userSection) {
          var defer = $q.defer();
          if (notifyStatuses.indexOf($scope.sevisStatus.statusName) !== -1) {
              MessageBox.confirm({
                  title: 'Confirm Edit',
                  message: 'The SEVIS participant status of this person is ' + $scope.sevisStatus.statusName + '. Are you sure you want to edit?',
                  okText: 'Yes',
                  cancelText: 'No',
                  okCallback: function () {
                      userSection = true
                      defer.resolve(userSection);
                  }
              });
          } else {
              userSection = !userSection
              defer.resolve(userSection);
          }

          return defer.promise;
      }
      
      // SEVIS validation: expand section and set active tab where error is located.
      $scope.$on('$viewContentLoaded', function () {

          var section = $stateParams.section;

          if (section)
          {
              switch (section) {
                  case "general":
                      $scope.showGeneral = true;
                      break;
                  case "pii":
                      $scope.showPii = true;
                      break;
                  case "contact":
                      $scope.showContact = true;
                      break;
                  case "eduemp":
                      $scope.showEduEmp = true;
                      break;
              }

              $timeout(function () {
                  var options = {
                      duration: 500,
                      easing: 'easeIn',
                      offset: 150,
                      callbackBefore: function (element) { },
                      callbackAfter: function (element) { }
                  }
                  smoothScroll(section, options);
              });
          }
      });

  });
