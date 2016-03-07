'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
  .controller('PersonInformationCtrl', function ($scope, $stateParams, $state, $timeout, $q, $filter, smoothScroll, MessageBox, StateService, ConstantsService) {

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
      $scope.sevisStatus = { statusName: "" };

      var notifyStatuses = ConstantsService.sevisStatuses;

      $scope.editGeneral = function () {
          return CreateMessageBox($scope.edit.General)
          .then(function (response) {
              $scope.edit.General = response;
          });
      }

      $scope.editPii = function () {
          return CreateMessageBox($scope.edit.Pii)
          .then(function (response) {
              $scope.edit.Pii = response;
          });
      }

      $scope.editContact = function () {
          return CreateMessageBox($scope.edit.Contact)
          .then(function (response) {
              $scope.edit.Contact = response;
          });
      }

      $scope.editEduEmp = function () {
          return CreateMessageBox($scope.edit.EduEmp)
          .then(function (response) {
              $scope.edit.EduEmp = response;
          });
      }

      function CreateMessageBox(userSection) {
          var defer = $q.defer();
          if (notifyStatuses.indexOf($filter('uppercase')($scope.sevisStatus.statusName)) !== -1) {
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
      
      function handleSectionState() {

          if ($state.current.name === StateService.stateNames.people.section.pii) {
              scrollToSection('pii',
                  collapseAllSections,
                  function (element) {
                      $scope.showPii = true;
                  });
          }
          else if ($state.current.name === StateService.stateNames.people.section.general) {
              scrollToSection('general',
                  collapseAllSections,
                  function (element) {
                      $scope.showGeneral = true;
                  });
          }
          else if ($state.current.name === StateService.stateNames.people.section.contact) {
              scrollToSection('contact',
                  collapseAllSections,
                  function (element) {
                      $scope.showContact = true;
                  });
          }
      }

      function collapseAllSections() {
          $scope.showGeneral = false;
          $scope.showPii = false;
          $scope.showContact = false;
          $scope.showEduEmp = false;
          $scope.showEvalNotes = false;
              }

      function scrollToSection(sectionId, callbackBefore, callbackAfter) {
              $timeout(function () {
              var section = document.getElementById(sectionId);
                  var options = {
                      duration: 500,
                      easing: 'easeIn',
                  offset: 175,
                  callbackBefore: callbackBefore,
                  callbackAfter: callbackAfter
                  }
                  smoothScroll(section, options);
              });
          }
      handleSectionState();
  });
