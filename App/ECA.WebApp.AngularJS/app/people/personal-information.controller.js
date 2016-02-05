'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
  .controller('PersonInformationCtrl', function ($scope, $stateParams, $timeout, smoothScroll, MessageBox) {

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

      var notifyStatuses = ["Sent To DHS", "Queued To Submit", "Validated", "SEVIS Received"];

      $scope.editGeneral = function () {
          if (notifyStatuses.indexOf($scope.sevisStatus.statusName) !== -1) {
              MessageBox.confirm({
                  title: 'Confirm Edit',
                  message: 'The SEVIS participant status of this person is ' + $scope.sevisStatus.statusName + '. Are you sure you want to edit?',
                  okText: 'Yes',
                  cancelText: 'No',
                  okCallback: function () {
                      $scope.edit.General = true
                  }
              });
          } else {
              $scope.edit.General = !$scope.edit.General
          }
      }

      $scope.editContact = function () {
          if (notifyStatuses.indexOf($scope.sevisStatus.statusName) !== -1) {
              MessageBox.confirm({
                  title: 'Confirm Edit',
                  message: 'The SEVIS participant status of this person is ' + $scope.sevisStatus.statusName + '. Are you sure you want to edit?',
                  okText: 'Yes',
                  cancelText: 'No',
                  okCallback: function () {
                      $scope.edit.Contact = true
                  }
              });
          } else {
              $scope.edit.Contact = !$scope.edit.Contact
          }
      }

      $scope.editPii = function () {
          if (notifyStatuses.indexOf($scope.sevisStatus.statusName) !== -1) {
              MessageBox.confirm({
                  title: 'Confirm Edit',
                  message: 'The SEVIS participant status of this person is ' + $scope.sevisStatus.statusName + '. Are you sure you want to edit?',
                  okText: 'Yes',
                  cancelText: 'No',
                  okCallback: function () {
                      $scope.edit.Pii = true
                  }
              });
          } else {
              $scope.edit.Pii = !$scope.edit.Pii
          }
      }

      $scope.editEduEmp = function () {
          if (notifyStatuses.indexOf($scope.sevisStatus.statusName) !== -1) {
              MessageBox.confirm({
                  title: 'Confirm Edit',
                  message: 'The SEVIS participant status of this person is ' + $scope.sevisStatus.statusName + '. Are you sure you want to edit?',
                  okText: 'Yes',
                  cancelText: 'No',
                  okCallback: function () {
                      $scope.edit.EduEmp = true
                  }
              });
          } else {
              $scope.edit.EduEmp = !$scope.edit.EduEmp
          }
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
