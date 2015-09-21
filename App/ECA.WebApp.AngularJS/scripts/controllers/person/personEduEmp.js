'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEducationEmploymentCtrl
 * # personEducationEmploymentCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEducationEmploymentCtrl', function ($scope, EduEmpService, $stateParams, $q, $log, NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseEduEmp = true;

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

}); 