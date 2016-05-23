'use strict';

angular.module('staticApp')
  .directive('pii', function ($log, ParticipantPersonsService) {
      var directive = {
          restrict: 'E',
          templateUrl: 'app/directives/pii.directive.html',
          scope: {
              personid: '@',
              updatepiicallback: '&'
          },
          controller: function ($scope) {
              $scope.editMode = false;
              $scope.editLocked = false;

              $scope.onUpdatePii = function () {
                  $scope.updatepiicallback();
                  $scope.editMode = false;
              }

              $scope.$watch("personid", function (personId) {
                  ParticipantPersonsService.getIsParticipantPersonLocked(personId)
                  .then(function (response) {
                      $scope.editLocked = response.data;
                  });
                  $scope.editMode = false;
              });

              $scope.editPii = function () {
                  if (!$scope.editLocked) {
                      $scope.editMode = true;
                  }
              }
          }
      };
      return directive;
  });
