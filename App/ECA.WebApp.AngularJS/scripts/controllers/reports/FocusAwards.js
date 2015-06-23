'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # Get parameters for ProjectAwards and execute report
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('FocusAwardsCtrl', function ($scope, $modalInstance, $log, ProgramService, LocationService, parameters, ReportService, DownloadService) {

      $scope.parameters = parameters;

      $scope.parameters.program = null;

      var programParams = null;
      var locationParams = null;
      $scope.isRunning = false;
      $scope.run = function () {

          var url = ReportService.getFocusAwards(parameters.program.programId);
          $scope.isRunning = true;
          $log.debug('Report: FocusAwards programId:[' + parameters.program.programId + ']');
          $log.info('Report: FocusAwards run at: ' + new Date());
          DownloadService.get(url, 'application/pdf', 'FocusAwards.pdf')
          .then(function () {

          }, function () {
              $log.error('Unable to download focus awards report.');
          })
          .then(function () {
              $scope.isRunning = false;
              $modalInstance.close($scope.parameters);
          })

      };

      $scope.cancel = function () {
          $scope.isRunning = false;
          $modalInstance.dismiss('cancel');
      };

      $scope.getPrograms = function (val) {
          programParams = {
              start: null,
              limit: 25,
              sort: null,
              filter: [{ property: 'name', comparison: 'like', value: val },
                      { property: 'programstatusid', comparison: 'eq', value: 1 }]
          };
          return ProgramService.getAllProgramsAlpha(programParams)
              .then(function (data) {
                  return data.results;
              });
      };

  });