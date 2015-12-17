'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # Get parameters for ProjectsWithGrantNumbers and execute report
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectsWithGrantNumberCtrl', function ($scope, $modalInstance, $log, ProgramService, LocationService, parameters, ReportService, DownloadService) {

      $scope.parameters = parameters;

      $scope.parameters.program = null;
      $scope.parameters.selectedFormat = null;

      var programParams = null;
      var locationParams = null;
      $scope.isRunning = false;
      $scope.run = function () {

          var url = ReportService.getProjectsWithGrantNumber(parameters.program.programId, parameters.selectedFormat.key);
          $scope.isRunning = true;
          $log.debug('Report: ProjectsWithGrantNumber programId:[' + parameters.program.programId + ']');
          $log.info('Report: ProjectsWithGrantNumber run at: ' + new Date());
          DownloadService.get(url, parameters.selectedFormat.mimetype, 'ProjectsWithGrantNumbers.' + parameters.selectedFormat.key)
          .then(function () {

          }, function () {
              $log.error('Unable to download projects with grant numbers report.');
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