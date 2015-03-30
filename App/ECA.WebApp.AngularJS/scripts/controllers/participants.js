'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ParticipantCtrl
 * @description
 * # ParticipantCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ParticipantCtrl', function ($scope, ParticipantService, PersonService, $stateParams) {

    $scope.tabs = {
      personalInformation: {
          title: 'Personal Information',
          path: 'personalinformation',
          active: true,
          order: 1
      },
      activity: {
          title: 'Activities',
          path: 'activity',
          active: true,
          order: 2
      },
      relatedReports: {
          title: 'Related Reports',
          path: 'relatedreports',
          active: true,
          order: 3
      },
      impact: {
          title: 'Impact',
          path: 'impact',
          active: true,
          order: 4
      },
    };

    $scope.showPii = true;
    $scope.showContact = true;

    $scope.activityImageSet = [
        'images/placeholders/participant/activities1.png',
        'images/placeholders/participant/activities2.png'
    ];

    ParticipantService.getParticipantById($stateParams.participantId)
      .then(function (data) {
          $scope.participant = data;
          PersonService.getPiiById(data.personId)
            .then(function (data) {
                $scope.pii = data;
            });
      });
  });
