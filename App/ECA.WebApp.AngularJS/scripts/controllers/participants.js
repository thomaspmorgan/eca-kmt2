'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ParticipantCtrl
 * @description
 * # ParticipantCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ParticipantCtrl', function ($scope, ParticipantService, $stateParams) {

    $scope.tabs = {
      overview: {
          title: 'Overview',
          path: 'overview',
          active: true,
          order: 1
      },
      activity: {
          title: 'Activities',
          path: 'activity',
          active: true,
          order: 2
      },
      personalInformation: {
          title: 'Personal Information',
          path: 'personalinformation',
          active: true,
          order: 3
      },
      relatedReports: {
          title: 'Related Reports',
          path: 'relatedreports',
          active: true,
          order: 4
      },
      impact: {
          title: 'Impact',
          path: 'impact',
          active: true,
          order: 5 
      },
    };

    $scope.showPii = true;

    $scope.activityImageSet = [
        'images/placeholders/participant/activities1.png',
        'images/placeholders/participant/activities2.png'
    ]

    ParticipantService.getParticipantById($stateParams.participantId)
      .then(function (data) {
          $scope.participant = data;
      });
  });