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
            loadContactInfo(personId);
      });

      function loadContactInfo(personId) {
          $scope.loadingContacts = true;
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contactInfo = data;
              $scope.loadingContacts = false;
          });
      };
}); 