'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personGeneralCtrl
 * # personGeneralCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personGeneralCtrl', function ($scope, ParticipantService, PersonService, $stateParams) {

      //ParticipantService.getParticipantById($stateParams.participantId)
      //  .then(function (data) {
      //  $scope.participant = data;
      //  loadGeneral(data.personId);
      //});

      $scope.personIdDeferred.promise
        .then(function (personId) {
        loadGeneral(personId);
      });

      function loadGeneral(personId) {
          PersonService.getGeneralById(personId)
          .then(function (data) {
              $scope.general = data;
          });
      };
}); 