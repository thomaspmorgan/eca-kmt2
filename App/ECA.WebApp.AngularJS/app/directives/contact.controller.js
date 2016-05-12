'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactCtrl
 * # personContactCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactCtrl', function ($scope, PersonService, $stateParams) {

      $scope.edit = {};
      $scope.edit.contactsLoading = true;

      var personId = $scope.personid;

      loadContactInfo(personId);

      function loadContactInfo(personId) {
          $scope.edit.contactsLoading = true;
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contactInfo = data;
              $scope.edit.contactsLoading = false;
          });
      };

      $scope.$watch('personid', function () {
          loadContactInfo($scope.personid);
      });
}); 