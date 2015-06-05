'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personGeneralCtrl
 * # personGeneralCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEducationEmploymentCtrl', function ($scope, ParticipantService, PersonService, $stateParams) {

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEmployments(personId);
          loadEducations(personId);
      });

      function loadEmployments(personId) {
          PersonService.getEmploymentsById(personId)
          .then(function (data) {
              $scope.employments = data;
          });
      };
          
      function loadEducations(personId) {
            PersonService.getEducationsById(personId)
            .then(function (data) {
                $scope.educations = data;
            });
      };
}); 