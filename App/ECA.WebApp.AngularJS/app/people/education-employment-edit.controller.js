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
      $stateParams,
      $q,
      $log,
      EduEmpService,
      NotificationService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.personId = parseInt($stateParams.personId);
      $scope.view.showEditEducation = false;
      $scope.view.showEditEmployment = false;
      $scope.view.isSavingChanges = false;
      $scope.view.educations = [];
      $scope.view.employments = [];

      $scope.EduEmpLoading = true;
      $scope.startDatePickerOpen = false;
      $scope.endDatePickerOpen = false;
      $scope.maxDate = new Date();
      var tempEduEmpId = 0;

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

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEmployments(personId);
          loadEducations(personId);
      });

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
                    var startDate = new Date(education.startDate);
                    if (!isNaN(startDate.getTime())) {
                        education.startDate = startDate;
                    }
                    var endDate = new Date(education.endDate);
                    if (!isNaN(endDate.getTime())) {
                        education.endDate = endDate;
                    }
                    else {
                        education.endDate = null;
                    }
                });
                $scope.view.educations = response.data.results;
                $scope.EduEmpLoading = false;
            })
          .catch(function () {
              var message = 'Unable to load educations.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.EduEmpLoading = false;
          });
      };
      
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
                  var startDate = new Date(employment.startDate);
                  if (!isNaN(startDate.getTime())) {
                      employment.startDate = startDate;
                  }
                  var endDate = new Date(employment.endDate);
                  if (!isNaN(endDate.getTime())) {
                      employment.endDate = endDate;
                  }
                  else {
                      employment.endDate = null;
                  }
              });
              $scope.view.employments = response.data.results;
              $scope.EduEmpLoading = false;
          })
          .catch(function () {
              var message = 'Unable to load employments.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.EduEmpLoading = false;
          });
      };

      $scope.view.onEditEducationClick = function () {
          $scope.view.showEditEducation = true;
      };

      $scope.view.onEditEmploymentClick = function () {
          $scope.view.showEditEmployment = true;
      };

      $scope.view.saveEducationChanges = function () {
          $scope.view.isSavingChanges = true;
          $scope.education.personId = $scope.view.personId;
          if (isNewEduEmp($scope.education)) {
              tempEduEmpId = angular.copy($scope.education.professionEducationId);
              return EduEmpService.addProfessionEducation($scope.education, $scope.view.personId)
                .then(onSaveEducationSuccess)
                .then(function () {
                    updateEducationFormDivId(tempEduEmpId);
                    updateEducations(tempEduEmpId, $scope.education);
                })
                .catch(onSaveEducationError);
          }
          else {
              return EduEmpService.updateProfessionEducation($scope.education, $scope.view.personId)
                  .then(onSaveEducationSuccess)
                  .catch(onSaveEducationError);
          }
      };

      function updateEducations(tempId, education) {
          var index = $scope.view.educations.map(function (e) { return e.professionEducationId; }).indexOf(tempId);
          $scope.view.educations[index] = education;
      };

      $scope.view.saveEmploymentChanges = function () {
          $scope.view.isSavingChanges = true;
          $scope.employment.personId = $scope.view.personId;

          if (isNewEduEmp($scope.employment)) {
              tempEduEmpId = angular.copy($scope.employment.professionEducationId);
              return EduEmpService.addProfessionEducation($scope.employment, $scope.view.personId)
                .then(onSaveEmploymentSuccess)
                .then(function () {
                    updateEmploymentFormDivId(tempEduEmpId);
                    updateEmployments(tempEduEmpId, $scope.employment);
                })
                .catch(onSaveEmploymentError);
          }
          else {
              return EduEmpService.updateProfessionEducation($scope.employment, $scope.view.personId)
                  .then(onSaveEmploymentSuccess)
                  .catch(onSaveEmploymentError);
          }
      };

      function updateEmployments(tempId, employment) {
          var index = $scope.view.employments.map(function (e) { return e.professionEducationId; }).indexOf(tempId);
          $scope.view.employments[index] = employment;
      };

      $scope.view.onAddEducationClick = function (entityEduEmps) {
          console.assert(entityEduEmps, 'The entity education is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity education is defined but must be an array.');
          var title = "";
          var role = "";
          var newEducation = {
              professionEducationId: --tempEduEmpId,
              title: title,
              role: role,
              startDate: null,
              endDate: null,
              organizationId: null,
              personOfEducation_PersonId: $scope.view.personId,
              personOfProfession_PersonId: null,
              personId: $scope.view.personId,
              isNew: true
          };
          entityEduEmps.splice(0, 0, newEducation);
          $scope.view.collapseEduEmp = false;
      }
      
      $scope.view.onAddEmploymentClick = function (entityEduEmps) {
          console.assert(entityEduEmps, 'The entity employment is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity employment is defined but must be an array.');
          var title = "";
          var role = "";
          var newEmployment = {
              professionEducationId: --tempEduEmpId,
              title: title,
              role: role,
              startDate: null,
              endDate: null,
              organizationId: null,
              personOfEducation_PersonId: null,
              personOfProfession_PersonId: $scope.view.personId,
              personId: $scope.view.personId,
              isNew: true
          };
          entityEduEmps.splice(0, 0, newEmployment);
          $scope.view.collapseEduEmp = false;
      }
      
      $scope.view.onDeleteEducationClick = function (index) {
          //if (isNewEduEmp($scope.education)) {
          //    removeEducationFromView(index);
          //} else {
              $scope.view.isDeletingEduEmp = true;
              return EduEmpService.deleteProfessionEducation($scope.education, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted education.");
                  $scope.view.isDeletingEduEmp = false;
                  removeEducationFromView(index);
              })
              .catch(function () {
                  var message = "Unable to delete education.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          //}
      }

      $scope.view.onDeleteEmploymentClick = function (index) {
          //if (isNewEduEmp($scope.employment)) {
          //    removeEmploymentFromView(index);
          //} else {
              $scope.view.isDeletingEmployment = true;
              return EduEmpService.deleteProfessionEducation($scope.employment, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted employment.");
                  $scope.view.isDeletingEmployment = false;
                  removeEmploymentFromView(index);
              })
              .catch(function () {
                  var message = "Unable to delete employment.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          //}
      }
      
      $scope.view.cancelEducationChanges = function () {
          $scope.view.showEditEducation = false;
          if (isNewEduEmp($scope.education)) {
              removeEducationFromView($scope.education);
          }
          else {
              $scope.education = angular.copy(originalEducation);
          }
      };

      $scope.view.cancelEmploymentChanges = function () {
          $scope.view.showEditEmployment = false;
          if (isNewEduEmp($scope.employment)) {
              removeEmploymentFromView($scope.employment);
          }
          else {
              $scope.employment = angular.copy(originalEmployment);
          }
      };
      
      function removeEducationFromView(index) {
          $scope.$emit(ConstantsService.removeNewEducationEventName, index);
      }

      function removeEmploymentFromView(index) {
          $scope.$emit(ConstantsService.removeNewEmploymentEventName, index);
      }

      function getEducationFormDivIdPrefix() {
          return 'educationForm';
      }

      function getEmploymentFormDivIdPrefix() {
          return 'employmentForm';
      }
      
      function getEduEmpFormDivElement(id) {
          return document.getElementById(id);
      }

      function updateEducationFormDivId(EduEmpId) {
          var id = getEducationFormDivIdPrefix() + EduEmpId;
          var e = getEduEmpFormDivElement(id);
          e.id = getEducationFormDivIdPrefix() + $scope.education.professionEducationId.toString();
      }

      function updateEmploymentFormDivId(EduEmpId) {
          var id = getEmploymentFormDivIdPrefix() + EduEmpId;
          var e = getEduEmpFormDivElement(id);
          e.id = getEmploymentFormDivIdPrefix() + $scope.employment.professionEducationId.toString();
      }

      function onSaveEducationSuccess(response) {
          $scope.education = response.config.data;
          originalEducation = angular.copy($scope.education);
          NotificationService.showSuccessMessage("Successfully saved education changes.");
          $scope.view.showEditEducation = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEmploymentSuccess(response) {
          $scope.employment = response.config.data;
          originalEmployment = angular.copy($scope.employment);
          NotificationService.showSuccessMessage("Successfully saved employment changes.");
          $scope.view.showEditEmployment = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEducationError() {
          var message = "Failed to save education changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function onSaveEmploymentError() {
          var message = "Failed to save employment changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewEduEmp(eduemp) {
          return eduemp.isNew;
      }
      
      $scope.$on(ConstantsService.removeNewEducationEventName, function (event, index) {
          console.assert($scope.view, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.view.educations instanceof Array, 'The entity education is defined but must be an array.');

          $scope.view.educations.splice(index, 1);
          $log.info('Removed one new education at index ' + index);
      });

      $scope.$on(ConstantsService.removeNewEmploymentEventName, function (event, index) {
          console.assert($scope.view, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.view.employments instanceof Array, 'The entity employment is defined but must be an array.');

          $scope.view.employments.splice(index, 1);
          $log.info('Removed one new employment at index ' + index);
      });

  });