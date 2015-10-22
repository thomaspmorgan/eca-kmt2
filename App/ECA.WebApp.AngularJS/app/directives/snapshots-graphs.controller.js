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
                var graphData = [];
                graphData.push(response.data);
                $scope.view.budgetByYear.data = graphData;
                var graphXAxisTickValues = response.data.values.map(function (item) {
                    return item['key'];
                });

                $scope.view.budgetByYear.options = {
                    chart: {
                        type: 'lineChart',
                        height: 300,
                        width: 350,
                        margin : {
                            top: 10,
                            right: 10,
                            bottom: 40,
                            left: 75
                        },
                        x: function(d){ return d.key; },
                        y: function(d){ return d.value; },
                        useInteractiveGuideline: true,
                        dispatch: {
                            stateChange: function(e){ },
                            changeState: function(e){ },
                            tooltipShow: function(e){ },
                            tooltipHide: function(e){ }
                        },
                        xAxis: {
                            tickSize: (1,0),
                            tickValues: graphXAxisTickValues
                        },
                        yAxis: {
                            tickFormat: function (d) {
                                return d3.format('$,d')(d);
                            }
                        },
                        callback: function(chart){
                        }
                    },
                    title: {
                        enable: false
                    },
                    subtitle: {
                        enable: false
                    },
                    caption: {
                        enable: false
                    }
                };
            })
            .catch(function () {
                var message = 'Unable to load budget by year chart.';
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