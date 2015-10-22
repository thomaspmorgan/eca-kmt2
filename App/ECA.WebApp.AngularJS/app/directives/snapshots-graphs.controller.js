'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: SnapshotsGraphsCtrl
 * # SnapshotsCtrl
 * Controller for snapshot graph statistics
 */
angular.module('staticApp')
  .controller('SnapshotsGraphsCtrl', function ($scope,
      $stateParams,
      $q,
      $log,
      SnapshotService,
      NotificationService) {

      $scope.view = {};
      $scope.view.isSnapshotGraphLoading = false;
      $scope.view.params = $stateParams;
      $scope.view.budgetByYear = [];
      $scope.view.mostFundedCountries = [];
      $scope.view.topThemes = [];
      $scope.view.participantLocations = [];
      $scope.view.participantsByYear = [];
      $scope.view.participantGenders = [];
      $scope.view.participantAges = [];
      $scope.view.participantEducation = [];

      $scope.init = function () {
          $scope.view.isSnapshotGraphLoading = true;

          GetProgramBudgetByYear();
          //GetProgramMostFundedCountries();
          GetProgramTopThemes();
          //GetProgramParticipantLocations();
          GetProgramParticipantsByYear();
          //GetProgramParticipantGender();
          //GetProgramParticipantAge();
          //GetProgramParticipantEducation();

          $scope.view.isSnapshotGraphLoading = false;
      };

      function GetProgramBudgetByYear() {
          SnapshotService.GetProgramBudgetByYear($scope.view.params.programId)
            .then(function (response) {
                $scope.view.budgetByYear = response.data;
            })
            .catch(function () {
                var message = 'Unable to load budget by year.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramMostFundedCountries() {
          SnapshotService.GetProgramMostFundedCountries($scope.view.params.programId)
            .then(function (response) {
                $scope.view.mostFundedCountries = response.data;
            })
            .catch(function () {
                var message = 'Unable to load most funded countries.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramTopThemes() {
          SnapshotService.GetProgramTopThemes($scope.view.params.programId)
            .then(function (response) {
                $scope.view.topThemes = response.data;
            })
            .catch(function () {
                var message = 'Unable to load top themes.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramParticipantLocations() {
          SnapshotService.GetProgramParticipantLocations($scope.view.params.programId)
            .then(function (response) {
                $scope.view.participantLocations = response.data;
            })
            .catch(function () {
                var message = 'Unable to load participant locations.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramParticipantsByYear() {
          SnapshotService.GetProgramParticipantsByYear($scope.view.params.programId)
            .then(function (response) {
                $scope.view.participantsByYear = response.data;
            })
            .catch(function () {
                var message = 'Unable to load participant counts by year.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramParticipantGender() {
          SnapshotService.GetProgramParticipantGender($scope.view.params.programId)
            .then(function (response) {
                $scope.view.participantGenders = response.data;
            })
            .catch(function () {
                var message = 'Unable to load participant genders.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramParticipantAge() {
          SnapshotService.GetProgramParticipantAge($scope.view.params.programId)
            .then(function (response) {
                $scope.view.participantAges = response.data;
            })
            .catch(function () {
                var message = 'Unable to load participant ages.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramParticipantEducation() {
          SnapshotService.GetProgramParticipantEducation($scope.view.params.programId)
            .then(function (response) {
                $scope.view.participantEducation = response.data;
            })
            .catch(function () {
                var message = 'Unable to load participant education.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      $scope.init();


  })
.factory('d3', [function () {
    return d3;
}])
.factory('nv', [function () {
    return nv;
}]);