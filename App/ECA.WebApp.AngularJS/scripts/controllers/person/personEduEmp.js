'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEducationEmploymentCtrl
 * # personEducationEmploymentCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEducationEmploymentCtrl', function ($scope, PersonService, EduEmpService, $stateParams, NotificationService) {

      $scope.personEduEmpLoading = true;

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEmployments(personId);
          loadEducations(personId);
      });

      function loadEmployments(personId) {
          $scope.personEduEmpLoading = true;
          PersonService.getEmploymentsById(personId)
          .then(function (data) {
              $scope.employments = data;
              $scope.personEduEmpLoading = false;
          });
      };
          
      function loadEducations(personId) {
          $scope.personEduEmpLoading = true;
          PersonService.getEducationsById(personId)
            .then(function (data) {
                $scope.educations = data;
                $scope.personEduEmpLoading = false;
            });
      };





}); 