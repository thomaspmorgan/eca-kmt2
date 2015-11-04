﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: SnapshotsCtrl
 * # SnapshotsCtrl
 * Controller for snapshot statistics
 */
angular.module('staticApp')
  .controller('SnapshotsCtrl', function ($scope,
      $stateParams,
      $q,
      $log,
      $interval,
      SnapshotService,
      NotificationService) {
      
      $scope.view = {};
      $scope.view.isSnapshotLoading = false;
      $scope.view.params = $stateParams;
      $scope.view.totalRelatedProjects = {};
      $scope.view.totalParticipants = {};
      $scope.view.totalBudget = {};
      $scope.view.totalCostByParticipant = {};
      $scope.view.totalFundingSources = {};
      $scope.view.totalCountries = {};
      $scope.view.totalBeneficiaries = {};
      $scope.view.totalImpactStories = {};
      $scope.view.totalAlumni = {};
      $scope.view.totalProminence = {};

      $scope.init = function () {
          $scope.view.isSnapshotLoading = true;

          GetProgramCountryCount();
          GetProgramRelatedProjectsCount();
          GetProgramFundingSourcesCount();
          GetProgramImpactStoryCount();
          GetProgramAlumniCount();
          GetProgramProminenceCount();

          $q.all([GetProgramBudgetTotal(), GetProgramParticipantCount()])
          .then(function () {
              GetProgramCostPerParticipant();
          })
          .catch(function () {
              $scope.view.isSnapshotLoading = false;
          });          

          $scope.view.isSnapshotLoading = false;
      };

      function GetProgramRelatedProjectsCount() {
          SnapshotService.GetProgramRelatedProjectsCount($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalRelatedProjects = response.data;
            })
            .catch(function () {
                var message = 'Unable to load related projects count.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramParticipantCount() {
          return SnapshotService.GetProgramParticipantCount($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalParticipants = response.data;
            })
            .catch(function () {
                var message = 'Unable to load participant count.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramBudgetTotal() {
          return SnapshotService.GetProgramBudgetTotal($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalBudget = response.data;
            })
            .catch(function () {
                var message = 'Unable to load budget total.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramCostPerParticipant() {
          if ($scope.view.totalBudget.dataValue > 0 && $scope.view.totalParticipants.dataValue > 0) {
              $scope.view.totalCostByParticipant = $scope.view.totalBudget.dataValue / $scope.view.totalParticipants.dataValue;
          }
      }

      function GetProgramFundingSourcesCount() {
          SnapshotService.GetProgramFundingSourcesCount($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalFundingSources = response.data;
            })
            .catch(function () {
                var message = 'Unable to load funding sources count.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramCountryCount() {
          SnapshotService.GetProgramCountryCount($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalCountries = response.data;
            })
            .catch(function () {
                var message = 'Unable to load country count.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramImpactStoryCount() {
          SnapshotService.GetProgramImpactStoryCount($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalImpactStories = response.data;
            })
            .catch(function () {
                var message = 'Unable to load impact stories count.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramAlumniCount() {
          SnapshotService.GetProgramAlumniCount($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalAlumni = response.data;
            })
            .catch(function () {
                var message = 'Unable to load alumni count.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramProminenceCount() {
          SnapshotService.GetProgramProminenceCount($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalProminence = response.data;
            })
            .catch(function () {
                var message = 'Unable to load prominence count.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }






      $scope.init();
      
  });