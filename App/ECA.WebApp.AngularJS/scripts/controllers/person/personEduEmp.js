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
      $scope.data.loadEduEmpPromise = $q.defer();
      $scope.data.educations = [];
      $scope.data.employments = [];

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEducations(personId);
          loadEmployments(personId);
      });

      function loadEducations(personId) {
          var params = {
              start: 0,
              limit: 300
          };
          $scope.EduEmpLoading = true;
          return EduEmpService.getEducations(personId, params)
            .then(function (response) {
                $log.info('Loaded all educations.');
                var educations = response.data.results;
                $scope.data.loadEduEmpPromise.resolve(educations);
                $scope.data.educations = response.data.results;
                $scope.EduEmpLoading = false;
                return educations;
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
          return EduEmpService.getEmployments(personId, params)
          .then(function (response) {
              $log.info('Loaded all employments.');
              var employments = response.data.results;
              $scope.data.loadEduEmpPromise.resolve(employments);
              $scope.data.employments = response.data.results;
              $scope.EduEmpLoading = false;
              return employments;
          })
          .catch(function () {
              var message = 'Unable to load employments.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.EduEmpLoading = false;
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