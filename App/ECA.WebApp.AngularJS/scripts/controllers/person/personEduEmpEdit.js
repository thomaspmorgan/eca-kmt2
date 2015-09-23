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
      var tempId = 0;

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

      $scope.view.onEditEducationClick = function () {
          $scope.view.showEditEducation = true;
      };

      $scope.view.onEditEmploymentClick = function () {
          $scope.view.showEditEmployment = true;
      };

      $scope.view.saveEducationChanges = function () {
          $scope.view.isSavingChanges = true;
          $scope.education.personId = parseInt($scope.view.personId);

          if (isNewEduEmp($scope.education)) {
              tempId = angular.copy($scope.education.professionEducationId);
              return EduEmpService.addProfessionEducation($scope.education, $scope.view.personId)
                .then(onSaveEducationSuccess)
                .then(function () {
                    updateEducationFormDivId(tempId);
                })
                .catch(onSaveEducationError);
          }
          else {
              return EduEmpService.updateProfessionEducation($scope.education, $scope.view.personId)
                  .then(onSaveEducationSuccess)
                  .catch(onSaveEducationError);
          }
      };
      
      $scope.view.saveEmploymentChanges = function () {
          $scope.view.isSavingChanges = true;
          $scope.employment.personId = parseInt($scope.view.personId);

          if (isNewEduEmp($scope.employment)) {
              tempId = angular.copy($scope.employment.professionEducationId);
              return EduEmpService.addProfessionEducation($scope.employment, $scope.view.personId)
                .then(onSaveEmploymentSuccess)
                .then(function () {
                    updateEmploymentFormDivId(tempId);
                })
                .catch(onSaveEmploymentError);
          }
          else {
              return EduEmpService.updateProfessionEducation($scope.employment, $scope.view.personId)
                  .then(onSaveEmploymentSuccess)
                  .catch(onSaveEmploymentError);
          }
      };
      
      $scope.view.onAddEducationClick = function (entityEduEmps, personId) {
          console.assert(entityEduEmps, 'The entity education is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity education is defined but must be an array.');
          var title = "";
          var role = "";
          var newEducation = {
              professionEducationId: --tempId,
              title: title,
              role: role,
              startDate: null,
              endDate: null,
              organizationId: null,
              personOfEducationPersonId: personId,
              personOfProfessionPersonId: null,
              personId: personId,
              isNew: true
          };
          entityEduEmps.splice(0, 0, newEducation);
          $scope.view.collapseEduEmp = false;
      }
      
      $scope.view.onAddEmploymentClick = function (entityEduEmps, personId) {
          console.assert(entityEduEmps, 'The entity employment is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity employment is defined but must be an array.');
          var title = "";
          var role = "";
          var newEmployment = {
              professionEducationId: null,
              title: title,
              role: role,
              startDate: null,
              endDate: null,
              organizationId: null,
              personOfEducationPersonId: null,
              personOfProfessionPersonId: personId,
              personId: personId,
              isNew: true
          };
          entityEduEmps.splice(0, 0, newEmployment);
          $scope.view.collapseEduEmp = false;
      }
      
      $scope.view.onDeleteEducationClick = function (education) {
          if (isNewEduEmp(education)) {
              removeEducationFromView(education);
          }
          else {
              $scope.view.isDeletingEduEmp = true;
              return EduEmpService.deleteProfessionEducation(education, $scope.view.personId)
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

      $scope.view.onDeleteEmploymentClick = function (employment) {
          if (isNewEduEmp(employment)) {
              removeEmploymentFromView(employment);
          }
          else {
              $scope.view.isDeletingEmployment = true;
              return EduEmpService.deleteProfessionEducation(employment, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted employment.");
                  $scope.view.isDeletingEmployment = false;
                  removeEmploymentFromView(employment);
              })
              .catch(function () {
                  var message = "Unable to delete employment.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      }
      
      $scope.view.cancelEducationChanges = function (form) {
          $scope.view.showEditEducation = false;
          if (isNewEduEmp($scope.education)) {
              removeEducationFromView($scope.education);
          }
          else {
              $scope.education = angular.copy(originalEducation);
          }
      };

      $scope.view.cancelEmploymentChanges = function (form) {
          $scope.view.showEditEmployment = false;
          if (isNewEduEmp($scope.employment)) {
              removeEmploymentFromView($scope.employment);
          }
          else {
              $scope.employment = angular.copy(originalEmployment);
          }
      };
      
      function removeEducationFromView(education) {
          $scope.$emit(ConstantsService.removeNewEducationEventName, education);
      }

      function removeEmploymentFromView(employment) {
          $scope.$emit(ConstantsService.removeNewEmploymentEventName, employment);
      }

      function getEducationFormDivIdPrefix() {
          return 'educationForm';
      }

      function getEmploymentFormDivIdPrefix() {
          return 'employmentForm';
      }

      //function getEducationFormDivId() {
      //    return getEducationFormDivIdPrefix() + $scope.education.professionEducationId;
      //}

      //function getEmploymentFormDivId() {
      //    return getEmploymentFormDivIdPrefix() + $scope.employment.professionEducationId;
      //}

      function getEduEmpFormDivElement(id) {
          return document.getElementById(id);
      }

      function updateEducationFormDivId(tempId) {
          var id = getEducationFormDivIdPrefix() + tempId;
          var e = getEduEmpFormDivElement(id);
          e.id = getEducationFormDivIdPrefix() + $scope.education.professionEducationId;
      }

      function updateEmploymentFormDivId(tempId) {
          var id = getEmploymentFormDivIdPrefix() + tempId;
          var e = getEduEmpFormDivElement(id);
          e.id = getEmploymentFormDivIdPrefix() + $scope.employment.professionEducationId;
      }

      function onSaveEducationSuccess(response) {
          $scope.education = response.data;
          $scope.data.educations = response.data;
          originalEducation = angular.copy($scope.education);
          NotificationService.showSuccessMessage("Successfully saved education changes.");
          $scope.view.showEditEducation = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEmploymentSuccess(response) {
          $scope.employment = response.data;
          $scope.data.employments = response.data;
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
          if (eduemp.isNew) {
              return eduemp.isNew = true;
          }
          else {
              return false;
          }
      }
      
      $scope.$on(ConstantsService.removeNewEducationEventName, function (event, newEducation) {
          console.assert($scope.data, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.data.educations instanceof Array, 'The entity education is defined but must be an array.');

          var educations = $scope.data.educations;
          var index = educations.indexOf(newEducation);
          var removedItems = educations.splice(index, 1);
          $log.info('Removed one new education at index ' + index);
      });

      $scope.$on(ConstantsService.removeNewEmploymentEventName, function (event, newEmployment) {
          console.assert($scope.data, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.data.employments instanceof Array, 'The entity employment is defined but must be an array.');

          var employments = $scope.data.employments;
          var index = employments.indexOf(newEmployment);
          var removedItems = employments.splice(index, 1);
          $log.info('Removed one new employment at index ' + index);
      });

  });