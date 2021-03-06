'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personGeneralCtrl
 * # personGeneralEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personGeneralCtrl', function ($scope, PersonService, $stateParams) {

      $scope.edit.generalLoading = true;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadGeneral(personId);
      });

      function loadGeneral(personId) {
          $scope.edit.generalLoading = true;
          PersonService.getGeneralById(personId)
          .then(function (data) {
              $scope.general = data;
              $scope.edit.generalLoading = false;
          });
      };
}); 