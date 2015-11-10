'use strict';

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

          GetAllProgramCounts()
          .then(function () {
              GetProgramCostPerParticipant();
              $scope.view.isSnapshotLoading = false;
          })
          .catch(function () {
              $scope.view.isSnapshotLoading = false;
          });
      };

      function GetAllProgramCounts() {
          return SnapshotService.GetProgramCounts($scope.view.params.programId)
            .then(function (response) {
                $scope.view.totalRelatedProjects = response.data.programRelatedProjectsCount;
                $scope.view.totalParticipants = response.data.programParticipantCount;
                $scope.view.totalBudget = response.data.programBudgetTotal;
                $scope.view.totalFundingSources = response.data.programFundingSourcesCount;
                $scope.view.totalCountries = response.data.programCountryCountAsync;
                $scope.view.totalBeneficiaries = response.data.programBeneficiaryCount;
                $scope.view.totalImpactStories = response.data.programImpactStoryCount;
                $scope.view.totalAlumni = response.data.programAlumniCount;
                $scope.view.totalProminence = response.data.programProminenceCount;
            })
            .catch(function () {
                var message = 'Unable to load program snapshot counts.';
                NotificationService.showErrorMessage(message);
                $log.error(message);
            });
      }

      function GetProgramCostPerParticipant() {
          if ($scope.view.totalBudget.dataValue > 0 && $scope.view.totalParticipants.dataValue > 0) {
              $scope.view.totalCostByParticipant = $scope.view.totalBudget.dataValue / $scope.view.totalParticipants.dataValue;
          }
      }
      
      $scope.init();
  });