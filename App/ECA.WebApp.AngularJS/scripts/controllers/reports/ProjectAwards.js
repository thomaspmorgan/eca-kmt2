'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ReportsArchiveCtrl
 * @description
 * # Get parameters for ProjectAwards and execute report
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectAwardsCtrl', function ($scope, $modalInstance, ProgramService, LocationService, parameters) {

    $scope.parameters = parameters;

    $scope.parameters.program = null;
    $scope.parameters.country = null;

    var programParams = null;
    var locationParams = null;
    
    $scope.run = function () {
        $modalInstance.close($scope.parameters);
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