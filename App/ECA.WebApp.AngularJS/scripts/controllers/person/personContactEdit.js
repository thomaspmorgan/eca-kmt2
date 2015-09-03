'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personContactEditCtrl
 * # personContactEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personContactEditCtrl', function ($scope, PersonService, $stateParams, $log) {

      $log.info('$scope.edit.Contact is:' + $scope.edit.Contact);

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadContactInfo(personId);
      });

      function loadContactInfo(personId) {
          PersonService.getContactInfoById(personId)
          .then(function (data) {
              $scope.contact = data;
          });
      };
}); 