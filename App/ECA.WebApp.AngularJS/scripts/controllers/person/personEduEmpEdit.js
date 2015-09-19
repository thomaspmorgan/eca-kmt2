'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEducationEmploymentEditCtrl
 * # personEducationEmploymentEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEducationEmploymentEditCtrl', function (
      $scope,
      EduEmpService,
      $stateParams,
      $q,
      $log,
      NotificationService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.personId = $stateParams.personId;
      $scope.view.showEditEducation = false;
      $scope.view.showEditEmployment = false;
      $scope.view.isSavingChanges = false;

      $scope.data = {};
      $scope.data.educations = [];
      $scope.data.employments = [];

      $scope.EduEmpLoading = true;

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEmployments(personId);
          loadEducations(personId);
      });
      
      var originalEducation = angular.copy($scope.education);
      var originalEmployment = angular.copy($scope.employment);

      $scope.cancelEditEduEmp = function () {
          $scope.edit.EduEmp = false;
      };

      /* EDUCATION */

      function loadEducations(personId) {
          var params = {
              start: 0,
              limit: 300
          };
          $scope.EduEmpLoading = true;
          EduEmpService.getEducations(personId, params)
            .then(function (response) {
                $log.info('Loaded all educations.');
                angular.forEach(response.data.results, function (education, index) {
                    education.professionEducationId = education.id;
                });
                var educations = response.data.results;
                $scope.data.educations = response.data.results;
                $scope.EduEmpLoading = false;
                //return educations;
            })
          .catch(function () {
              var message = 'Unable to load educations.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.EduEmpLoading = false;
          });
      };

      $scope.view.onEditEducationClick = function (education) {
          $scope.selected = angular.copy(education);
          $scope.view.showEditEducation = true;
      };

      $scope.view.saveEducationChanges = function (education) {
          $scope.view.isSavingChanges = true;
          if (isNewEducation(education)) {
              var tempId = angular.copy(education.professionEducationId);
              EduEmpService.addEducation(education, $scope.view.personId)
                .then(onSaveEducationSuccess)
                .then(function () {
                    updateEducationFormDivId(tempId);
                })
                .catch(onSaveEducationError);
          }
          else {
              EduEmpService.updateEducation(education, $scope.view.personId)
                  .then(onSaveEducationSuccess)
                  .catch(onSaveEducationError);
          }
      };

      $scope.view.onDeleteEducationClick = function (education) {
          if (isNewEducation(education)) {
              removeEducationFromView(education);
          }
          else {
              $scope.view.isDeletingEduEmp = true;
              EduEmpService.deleteEduEmp(education, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted education.");
                  $scope.view.isDeletingEduEmp = false;
                  removeEducationFromView(education);
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
          if (isNewEducation($scope.education)) {
              removeEducationFromView($scope.education);
          }
          else {
              $scope.education = angular.copy(originalEducation);
          }
      };
      
      function removeEducationFromView(education) {
          $scope.$emit(ConstantsService.removeNewEduEmpEventName, education);
      }

      function getEducationFormDivIdPrefix() {
          return 'educationForm';
      }

      function getEducationFormDivId() {
          return getEducationFormDivIdPrefix() + $scope.education.professionEducationId;
      }

      function getEducationFormDivElement(id) {
          return document.getElementById(id);
      }

      function updateEducationFormDivId(tempId) {
          var id = getEducationFormDivIdPrefix() + tempId;
          var e = getEducationFormDivElement(id);
          e.id = getEducationFormDivIdPrefix() + $scope.education.professionEducationId;
      }

      function onSaveEducationSuccess(response) {
          $scope.education = response.data;
          originalEducation = angular.copy($scope.education);
          NotificationService.showSuccessMessage("Successfully saved education changes.");
          $scope.view.showEditEducation = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEducationError() {
          var message = "Failed to save education changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewEducation(education) {
          if (education.isNew) {
              return education.isNew = true;
          }
          else {
              return false;
          }
      }

      /* EMPLOYMENT */

      function loadEmployments(personId) {
          var params = {
              start: 0,
              limit: 300
          };
          $scope.EduEmpLoading = true;
          EduEmpService.getEmployments(personId, params)
          .then(function (response) {
              $log.info('Loaded all employments.');
              angular.forEach(response.data.results, function (employment, index) {
                  employment.professionEducationId = employment.id;
              });
              var employments = response.data.results;
              $scope.data.employments = response.data.results;
              $scope.EduEmpLoading = false;
              //return employments;
          })
          .catch(function () {
              var message = 'Unable to load employments.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.EduEmpLoading = false;
          });
      };

      $scope.view.onEditEmploymentClick = function () {
          $scope.view.showEditEmployment = true;
      };

      $scope.view.saveEmploymentChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewEmployment($scope.employment)) {
              var tempId = angular.copy($scope.employment.professionEducationId);
              EduEmpService.addEmployment($scope.employment, $scope.view.personId)
                .then(onSaveEmploymentSuccess)
                .then(function () {
                    updateEmploymentFormDivId(tempId);
                })
                .catch(onSaveEmploymentError);
          }
          else {
              EduEmpService.updateEmployment($scope.employment, $scope.view.personId)
                  .then(onSaveEmploymentSuccess)
                  .catch(onSaveEmploymentError);
          }
      };

      $scope.view.onDeleteEmploymentClick = function () {
          if (isNewEmployment($scope.employment)) {
              removeEmploymentFromView($scope.employment);
          }
          else {
              $scope.view.isDeletingEmployment = true;
              EduEmpService.deleteEduEmp($scope.employment, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted employment.");
                  $scope.view.isDeletingEmployment = false;
                  removeEmploymentFromView($scope.employment);
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
          if (isNewEmployment($scope.employment)) {
              removeEmploymentFromView($scope.employment);
          }
          else {
              $scope.employment = angular.copy(originalEmployment);
          }
      };

      function removeEmploymentFromView(eduemp) {
          $scope.$emit(ConstantsService.removeNewEduEmpEventName, eduemp);
      }

      function getEmploymentFormDivIdPrefix() {
          return 'employmentForm';
      }

      function getEmploymentFormDivId() {
          return getEmploymentFormDivIdPrefix() + $scope.employment.professionEducationId;
      }

      function getEmploymentFormDivElement(id) {
          return document.getElementById(id);
      }

      function updateEmploymentFormDivId(tempId) {
          var id = getEmploymentFormDivIdPrefix() + tempId;
          var e = getEmploymentFormDivElement(id);
          e.id = getEmploymentFormDivIdPrefix() + $scope.employment.professionEducationId;
      }

      function onSaveEmploymentSuccess(response) {
          $scope.employment = response.data;
          originalEmployment = angular.copy($scope.employment);
          NotificationService.showSuccessMessage("Successfully saved education changes.");
          $scope.view.showEditEmployment = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEmploymentError() {
          var message = "Failed to save changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewEmployment(education) {
          if (education.isNew) {
              return education.isNew = true;
          }
          else {
              return false;
          }
      }

  });