'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personGeneralCtrl
 * # personGeneralEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personGeneralCtrl', function ($scope, PersonService, $stateParams) {



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