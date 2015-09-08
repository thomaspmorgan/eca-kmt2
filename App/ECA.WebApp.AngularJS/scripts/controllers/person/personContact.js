'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactCtrl
 * # personContactCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactCtrl', function ($scope, PersonService, $stateParams) {

      $scope.contactsLoading = true;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadContactInfo(personId);
      });

      function loadContactInfo(personId) {
          $scope.contactsLoading = true;
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contactInfo = data;
              $scope.contactsLoading = false;
          });
      };
}); 