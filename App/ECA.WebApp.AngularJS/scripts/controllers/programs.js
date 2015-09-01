'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramsCtrl', function (
      $scope,
      $stateParams,
      $state,
      $log,
        $q,
      NotificationService,
      ProgramService) {

      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          projects: {
              title: 'Sub-Programs & Projects',
              path: 'projects',
              active: true,
              order: 2
          },
          activity: {
              title: 'Timeline',
              path: 'activity',
              active: true,
              order: 3
          },
          artifacts: {
              title: 'Attachments',
              path: 'artifacts',
              active: true,
              order: 4
          },
          impact: {
              title: 'Impact',
              path: 'impact',
              active: true,
              order: 5
          },
          funding: {
              title: 'Funding',
              path: 'moneyflows',
              active: true,
              order: 6
          }
      };

      $scope.data = {};
      $scope.data.loadProgramPromise = $q.defer();
      $scope.view = {};
      $scope.view.isLoadingProgram = false;

      function loadProgramById(programId) {
          $scope.view.isLoadingProgram = true;
          return ProgramService.get(programId)
            .then(function (data) {
                $scope.view.isLoadingProgram = false;
                $scope.program = data;
                $scope.data.loadProgramPromise.resolve($scope.program);
            })
            .catch(function (response) {
                $scope.view.isLoadingProgram = false;
                var message = 'Unable to load program by id.';
                $log.error(message);
                NotificationService.showErrorMessage(message);
            });
      }

      $q.all([loadProgramById($stateParams.programId)])
        .then(function () {})
  });
