'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEvaluationNotesCtrl
 * # personEvaluationNotesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personEvaluationNotesCtrl', function ($scope, PersonService) {

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEvaluationNotes(personId);
      });

      function loadEvaluationNotes(personId) {
          PersonService.getEvaluationNotesById(personId)
          .then(function (data) {
              $scope.evaluationNotes = data;
          });
      };
  });