'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # Get parameters for ProjectAwards and execute report
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectAwardsCtrl', function ($scope, $modalInstance, $log, ProgramService, LocationService, parameters, ReportService, DownloadService) {

    $scope.parameters = parameters;

    $scope.parameters.program = null;
    $scope.parameters.country = null;

    var programParams = null;
    var locationParams = null;
    
    $scope.run = function () {
        
        var url = ReportService.getProjectAwards(parameters.program.programId, parameters.country.id);
        $log.debug('Report: ProjectAwards programId:[' + parameters.program.programId + '], countryId:[' + parameters.country.id + ']');
        $log.info('Report: ProjectAwards run at: ' + new Date());
        DownloadService.get(url, 'application/pdf', 'ProgramAwards.pdf')
        .then(function() {

        }, function() {
            $log.error('Unable to download project awards report.');
        })
        .then(function () {
            $modalInstance.close($scope.parameters);
        })
        
    };

    $scope.cancel = function () {
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
  });