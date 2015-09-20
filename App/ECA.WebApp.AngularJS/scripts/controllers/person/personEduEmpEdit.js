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
      $scope.view.tempId = 0;

      $scope.data = {};
      $scope.data.educations = [];
      $scope.data.employments = [];

      $scope.EduEmpLoading = true;
      $scope.startDatePickerOpen = false;
      $scope.endDatePickerOpen = false;
      $scope.maxDateOfBirth = new Date();

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
      
      $scope.openStartDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.startDatePickerOpen = true;
      };

      $scope.openEndDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.endDatePickerOpen = true;
      };

      $scope.$on(ConstantsService.removeNewEduEmpEventName, function (event, newEduEmp) {
          console.assert($scope.model, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.model.eduemp instanceof Array, 'The entity education/profession is defined but must be an array.');

          var eduemp = $scope.model.eduemp;
          var index = eduemp.indexOf(newEduEmp);
          //var removedItems = eduemp.splice(index, 1);
          $log.info('Removed one new education/profession at index ' + index);
      });


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
                $scope.data.educations = response.data.results;
                $scope.EduEmpLoading = false;
            })
          .catch(function () {
              var message = 'Unable to load educations.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.EduEmpLoading = false;
          });
      };

      $scope.view.onEditEducationClick = function () {
          $scope.view.showEditEducation = true;
      };

      $scope.view.saveEducationChanges = function () {
          $scope.view.isSavingChanges = true;
          $scope.education.personId = parseInt($scope.view.personId);

          if (isNewEducation($scope.education)) {
              $scope.view.tempId = angular.copy($scope.education.professionEducationId);
              return EduEmpService.addEducation($scope.education, $scope.view.personId)
                .then(onSaveEducationSuccess)
                .then(function () {
                    updateEducationFormDivId($scope.view.tempId);
                })
                .catch(onSaveEducationError);
          }
          else {
              return EduEmpService.updateEducation($scope.education, $scope.view.personId)
                  .then(onSaveEducationSuccess)
                  .catch(onSaveEducationError);
          }
      };
      
      $scope.view.onAddEducationClick = function (entityEduEmps, personId) {
          console.assert(entityEduEmps, 'The entity education is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity education is defined but must be an array.');
          var title = "";
          var role = "";
          var startDate = "";
          var endDate = "";
          var newEducation = {
              id: --$scope.view.tempId,
              title: title,
              role: role,
              startDate: startDate,
              endDate: endDate,
              organizationId: null,
              personOfEducationPersonId: personId,
              personOfProfessionPersonId: null,
              personId: personId
          };
          entityEduEmps.splice(0, 0, newEducation);
          $scope.view.collapseEduEmp = false;
      }

      $scope.view.onDeleteEducationClick = function () {
          if (isNewEducation($scope.education)) {
              removeEducationFromView($scope.education);
          }
          else {
              $scope.view.isDeletingEduEmp = true;
              EduEmpService.deleteEduEmp($scope.education, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted education.");
                  $scope.view.isDeletingEduEmp = false;
                  removeEducationFromView($scope.education);
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
          return education.personId;
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
              $scope.data.employments = response.data.results;
              $scope.EduEmpLoading = false;
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
          $scope.employment.personId = parseInt($scope.view.personId);

          if (isNewEmployment($scope.employment)) {
              $scope.view.tempId = angular.copy($scope.employment.professionEducationId);
              EduEmpService.addEmployment($scope.employment, $scope.view.personId)
                .then(onSaveEmploymentSuccess)
                .then(function () {
                    updateEmploymentFormDivId($scope.view.tempId);
                })
                .catch(onSaveEmploymentError);
          }
          else {
              EduEmpService.updateEmployment($scope.employment, $scope.view.personId)
                  .then(onSaveEmploymentSuccess)
                  .catch(onSaveEmploymentError);
          }
      };

      $scope.view.onAddEmploymentClick = function (entityEduEmps, personId) {
          console.assert(entityEduEmps, 'The entity employment is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity employment is defined but must be an array.');
          var title = "";
          var role = "";
          var startDate = "";
          var endDate = "";
          var newEmployment = {
              id: --$scope.view.tempId,
              title: title,
              role: role,
              startDate: startDate,
              endDate: endDate,
              organizationId: null,
              personOfEducationPersonId: null,
              personOfProfessionPersonId: personId,
              personId: personId
          };
          entityEduEmps.splice(0, 0, newEmployment);
          $scope.view.collapseEduEmp = false;
      }

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

      function removeEmploymentFromView(employment) {
          $scope.$emit(ConstantsService.removeNewEduEmpEventName, employment);
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
          NotificationService.showSuccessMessage("Successfully saved employment changes.");
          $scope.view.showEditEmployment = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEmploymentError() {
          var message = "Failed to save employment changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewEmployment(employment) {
          return employment.personId;
      }

  });