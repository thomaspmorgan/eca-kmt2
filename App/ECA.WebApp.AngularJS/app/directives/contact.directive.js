'use strict';

angular.module('staticApp')
  .directive('contact', function ($log, ParticipantPersonsService) {
      var directive = {
          restrict: 'E',
          templateUrl: 'app/directives/contact.directive.html',
          scope: {
              personid: '@',
          },
          controller: function ($scope) {
              $scope.editMode = false;
              $scope.editLocked = false;

              $scope.$watch("personid", function (personId) {
                  ParticipantPersonsService.getIsParticipantPersonLocked(personId)
                  .then(function (response) {
                      $scope.editLocked = response.data;
                  });
                  $scope.editMode = false;
              });

              $scope.editContact = function () {
                  if (!$scope.editLocked) {
                      $scope.editMode = true;
                  }
              }
          }
      };
      return directive;
  });
