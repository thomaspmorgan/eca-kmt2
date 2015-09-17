'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEducationEmploymentEditCtrl
 * # personEducationEmploymentEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEducationEmploymentEditCtrl', function ($scope, EduEmpService, $stateParams, NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.showEditEduEmp = false;
      $scope.view.isSavingChanges = false;

      var originalEduEmp = angular.copy($scope.eduemp);

      $scope.EduEmpLoading = true;

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEmployments(personId);
          loadEducations(personId);
      });

      /* EDUCATION */

      function loadEducations(personId) {
          var params = {
              start: 0,
              limit: 300
          };
          $scope.EduEmpLoading = true;
          EduEmpService.getEducations(personId, params)
            .then(function (data) {
                $scope.educations = data;
                $log.info('Loaded all educations.');
                $scope.EduEmpLoading = false;
            })
          .catch(function () {
              var message = 'Unable to load educations.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      };

      $scope.view.onEditEducationClick = function () {
          $scope.view.showEditEducation = true;
          var id = getEduEmpFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          }
          smoothScroll(getEduEmpFormDivElement(id), options);
      };

      $scope.view.saveEducationChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewEduEmp($scope.eduemp)) {
              var tempId = angular.copy($scope.eduemp.id);
              return EduEmpService.addEducation($scope.eduemp, $scope.view.params.personId)
                .then(onSaveEduEmpSuccess)
                .then(function () {
                    updateEduEmpFormDivId(tempId);
                })
                .catch(onSaveEduEmpError);
          }
          else {
              return EduEmpService.updateEducation($scope.eduemp, $scope.view.params.personId)
                  .then(onSaveEduEmpSuccess)
                  .catch(onSaveEduEmpError);
          }
      };

      $scope.view.onDeleteEducationClick = function () {
          if (isNewEduEmp($scope.eduemp)) {
              removeEduEmpFromView($scope.eduemp);
          }
          else {
              $scope.view.isDeletingEduEmp = true;
              return EduEmpService.deleteEduEmp($scope.eduemp, $scope.view.params.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted education.");
                  $scope.view.isDeletingEduEmp = false;
                  removeEduEmpFromView($scope.eduemp);
              })
              .catch(function () {
                  var message = "Unable to delete education.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      }

      $scope.view.cancelEducationChanges = function (form) {
          $scope.view.showEditEducation = false;
          if (isNewEduEmp($scope.eduemp)) {
              removeEduEmpFromView($scope.eduemp);
          }
          else {
              $scope.eduemp = angular.copy(originalEduEmp);
          }
      };


      /* EMPLOYMENT */

      function loadEmployments(personId) {
          var params = {
              start: 0,
              limit: 300
          };
          $scope.EduEmpLoading = true;
          EduEmpService.getEmployments(personId, params)
          .then(function (data) {
              $scope.employments = data;
              $log.info('Loaded all employments.');
              $scope.EduEmpLoading = false;
          })
          .catch(function () {
              var message = 'Unable to load employments.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      };

      $scope.view.onEditEmploymentClick = function () {
          $scope.view.showEditEmployment = true;
          var id = getEduEmpFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          }
          smoothScroll(getEduEmpFormDivElement(id), options);
      };

      $scope.view.saveEmploymentChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewEduEmp($scope.eduemp)) {
              var tempId = angular.copy($scope.eduemp.id);
              return EduEmpService.addEmployment($scope.eduemp, $scope.view.params.personId)
                .then(onSaveEduEmpSuccess)
                .then(function () {
                    updateEduEmpFormDivId(tempId);
                })
                .catch(onSaveEduEmpError);
          }
          else {
              return EduEmpService.updateEmployment($scope.eduemp, $scope.view.params.personId)
                  .then(onSaveEduEmpSuccess)
                  .catch(onSaveEduEmpError);
          }
      };

      $scope.view.onDeleteEmploymentClick = function () {
          if (isNewEduEmp($scope.eduemp)) {
              removeEduEmpFromView($scope.eduemp);
          }
          else {
              $scope.view.isDeletingEmployment = true;
              return EduEmpService.deleteEduEmp($scope.eduemp, $scope.view.params.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted employment.");
                  $scope.view.isDeletingEmployment = false;
                  removeEduEmpFromView($scope.eduemp);
              })
              .catch(function () {
                  var message = "Unable to delete employment.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      }

      $scope.view.cancelEmploymentChanges = function (form) {
          $scope.view.showEditEmployment = false;
          if (isNewEduEmp($scope.eduemp)) {
              removeEduEmpFromView($scope.eduemp);
          }
          else {
              $scope.eduemp = angular.copy(originalEduEmp);
          }
      };


      /* EDUCATION / EMPLOYMENT */
      
      function removeEduEmpFromView(eduemp) {
          $scope.$emit(ConstantsService.removeNewEduEmpEventName, eduemp);
      }

      function getEduEmpFormDivIdPrefix() {
          return 'eduempForm';
      }

      function getEduEmpFormDivId() {
          return getEduEmpFormDivIdPrefix() + $scope.eduemp.id;
      }

      function updateEduEmpFormDivId(tempId) {
          var id = getEduEmpFormDivIdPrefix() + tempId;
          var e = getEduEmpFormDivElement(id);
          e.id = getEduEmpFormDivIdPrefix() + $scope.eduemp.id;
      }

      function getEduEmpFormDivElement(id) {
          return document.getElementById(id);
      }

      function onSaveEduEmpSuccess(response) {
          $scope.eduemp = response.data;
          originalEduEmp = angular.copy($scope.eduemp);
          NotificationService.showSuccessMessage("Successfully saved changes.");
          $scope.view.showEditEduEmp = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEduEmpError() {
          var message = "Failed to save changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }


  });