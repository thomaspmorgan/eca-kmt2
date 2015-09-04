'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactCtrl
 * # personContactCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactCtrl', function ($scope, PersonService, $stateParams) {

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadContact(personId);
      });

      function loadContact(personId) {
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contact = data;
          });
      };
}); 