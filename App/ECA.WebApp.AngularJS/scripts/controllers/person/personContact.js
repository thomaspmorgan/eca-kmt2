'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactCtrl
 * # personContactCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactCtrl', function ($scope, PersonService, $stateParams) {

      $scope.loadingContacts = true;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadContact(personId);
      });

      function loadContact(personId) {
          $scope.loadingContacts = true;
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contact = data;
              $scope.loadingContacts = false;
          });
      };
}); 