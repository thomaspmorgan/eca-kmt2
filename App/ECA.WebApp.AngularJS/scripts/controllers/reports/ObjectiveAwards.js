'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # Get parameters for ProjectAwards and execute report
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ObjectiveAwardsCtrl', function ($scope, $modalInstance, $log, ProgramService, OfficeService, LocationService, parameters, ReportService, DownloadService, ConstantsService, ProjectService, NotificationService) {

      $scope.parameters = parameters;

      $scope.parameters.program = null;
      $scope.parameters.objective = null;

      var programParams = null;
      var locationParams = null;
      var maxLimit = 300;

      $scope.isRunning = false;
      $scope.run = function () {

          var url = ReportService.getObjectiveAwards(parameters.program.programId, parameters.objective.id);
          $scope.isRunning = true;
          $log.debug('Report: ObjectiveAwards programId:[' + parameters.program.programId + ']');
          $log.info('Report: ObjectiveAwards run at: ' + new Date());
          DownloadService.get(url, 'application/pdf', 'ObjectiveAwards.pdf')
          .then(function () {

          }, function () {
              $log.error('Unable to download Objective awards report.');
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

      $scope.onTypeAheadSelect = function($item, $model, $label)
      {
          loadObjectives();
      }

      function loadObjectives(search) {
          var programId = $scope.parameters.program.programId;
          var officeId = $scope.parameters.program.owner_OrganizationId;
          console.assert(officeId, "The office id must be defined.");
          var params = {
              start: 0,
              limit: maxLimit,
              officeId: officeId
          };
          if (search) {
              params.filter = [{
                  comparison: ConstantsService.likeComparisonType,
                  property: 'name',
                  value: search
              }]
          }
          return ProjectService.getObjectives(programId, params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more objectives in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  //normalizeLookupProperties(response.data.results);
                  $scope.objectives = response.data.results;
              })
              .catch(function () {
                  $log.error('Unable to load objectives.');
                  NotificationService.showErrorMessage('Unable to load objectives.');
              });
      }

  });