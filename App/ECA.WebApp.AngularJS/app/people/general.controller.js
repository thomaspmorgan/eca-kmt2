'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personGeneralCtrl
 * # personGeneralEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personGeneralCtrl', function ($scope, PersonService, $stateParams) {

      $scope.generalLoading = true;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadGeneral(personId);
      });

      function loadGeneral(personId) {
          $scope.generalLoading = true;
          PersonService.getGeneralById(personId)
          .then(function (data) {
              $scope.general = data;
              $scope.sevisStatus.statusName = data.sevisStatus;
              $scope.generalLoading = false;
          });
      };
}); 