'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportCtrl
 * @description
 * # Get parameters for a report and execute report
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ReportCtrl', function ($scope, $modalInstance, $log, ProgramService, LocationService, parameters, ReportService, ProjectService, DownloadService) {

      $scope.parameters = parameters;

      $scope.parameters.program = null;
      $scope.parameters.selectedFormat = null;
      $scope.parameters.objective = null;
      var maxLimit = 300;

      var programParams = null;
      var locationParams = null;
      $scope.isRunning = false;
      $scope.run = function () {

          var url = ReportService.getReport(parameters);
          $scope.isRunning = true;
          $log.debug('Report: ' + parameters.report.Title + ' programId:[' + parameters.program.programId + ']');
          $log.info('Report: ' + parameters.report.Title + ' run at: ' + new Date());
          DownloadService.get(url, parameters.selectedFormat.mimetype, parameters.report.Report + '.' + parameters.selectedFormat.key)
          .then(function () {

          }, function () {
              $log.error('Unable to run ' + parameters.report.Title + ' report.');
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

      $scope.getCountries = function (val) {
          locationParams = {
              start: null,
              limit: 25,
              sort: null,
              filter: [{ property: 'name', comparison: 'like', value: val },
                      { property: 'locationTypeId', comparison: 'eq', value: 3 }]
          };
          return LocationService.get(locationParams)
              .then(function (data) {
                  return data.results;
              });
      };

      $scope.onTypeAheadSelect = function ($item, $model, $label) {
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
                  if (response.data.total > maxLimit) {
                      $log.error('There are more objectives in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  //normalizeLookupProperties(response.data.results)
                  $scope.objectives = response.data.results;
              })
              .catch(function () {
                  $log.error('Unable to load objectives.');
                  NotificationService.showErrorMessage('Unable to load objectives.');
              });
      }

      $scope.needsCountry = function (str) {
          return (str.indexOf("Country") > -1);
      };

      $scope.needsObjective = function (str) {
          return (str.indexOf("Objective") > -1);
      };
  });