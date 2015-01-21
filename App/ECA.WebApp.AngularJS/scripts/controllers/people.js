'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:PeopleCtrl
 * @description
 * # PeopleCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('PeopleCtrl', function ($scope, PersonService, $stateParams) {

    PersonService.get($stateParams.personId)
        .then(function (person) {
            $scope.person = person;
            console.log($scope);

        });

    $scope.updatePerson = function () {
        savePerson();
    };

    function savePerson() {
        PersonService.update($scope.person, $stateParams.personId)
            .then(function (person) {
              $scope.person = person;
            });
    }

    $scope.activityImageSet = [
        'images/placeholders/participant/activities1.png',
        'images/placeholders/participant/activities2.png'
    ]
  });
