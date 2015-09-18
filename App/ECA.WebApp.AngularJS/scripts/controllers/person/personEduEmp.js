'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEducationEmploymentCtrl
 * # personEducationEmploymentCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEducationEmploymentCtrl', function ($scope, EduEmpService, $stateParams, $q, $log, NotificationService, ConstantsService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseEduEmp = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadEduEmpsPromise = $q.defer();

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEmployments(personId);
          loadEducations(personId);
      });

      function loadEducations(personId) {
          $scope.EduEmpLoading = true;
          EduEmpService.getEducationsById(personId)
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

      function loadEmployments(personId) {
          $scope.EduEmpLoading = true;
          EduEmpService.getEmploymentsById(personId)
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

      $scope.view.onAddEducationClick = function (entityEduEmps, personId) {
          console.assert(entityEduEmps, 'The entity education is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity education is defined but must be an array.');
          var newEducation = {
              professionEducationId: --tempId,
              title: title,
              role: role,
              startDate: startDate,
              endDate: endDate,
              organization: organization,
              personOfEducation_PersonId: personId,
              personId: personId
          };
          entityEduEmps.splice(0, 0, newEducation);
          $scope.view.collapseEduEmp = false;
      }

      $scope.view.onAddEmploymentClick = function (entityEduEmps, personId) {
          console.assert(entityEduEmps, 'The entity employment is not defined.');
          console.assert(entityEduEmps instanceof Array, 'The entity employment is defined but must be an array.');
          var newEmployment = {
              professionEducationId: --tempId,
              title: title,
              role: role,
              startDate: startDate,
              endDate: endDate,
              organization: organization,
              personOfProfession_PersonId: personId,
              personId: personId
          };
          entityEduEmps.splice(0, 0, newEmployment);
          $scope.view.collapseEduEmp = false;
      }

      $scope.$on(ConstantsService.removeNewEduEmpEventName, function (event, newEduEmp) {
          console.assert($scope.model, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.model.eduemp instanceof Array, 'The entity education/profession is defined but must be an array.');

          var eduemp = $scope.model.eduemp;
          var index = eduemp.indexOf(newEduEmp);
          var removedItems = eduemp.splice(index, 1);
          $log.info('Removed one new education/profession at index ' + index);
      });

}); 