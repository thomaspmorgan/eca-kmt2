'use strict';

/**
 * Controller for participant timeline
 */
angular.module('staticApp')
  .controller('PersonTimelineCtrl', function ($scope, ProjectService, ParticipantPersonsService, NotificationService, BrowserService, PersonService) {

      
      $scope.projectsLoading = false;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadProjects(personId);
            PersonService.getPersonById(personId)
            .then(function (response) {
                BrowserService.setDocumentTitleByPerson(response, 'Timeline');
            })
        });

      function loadProjects(personId) {
          $scope.participantInfo = {};
          $scope.projectsLoading = true;
          var params = { start: 0, limit: 300 };
          ProjectService.getProjectsByPersonId(personId, params)
            .then(function (data) {
                $scope.projects = data.data.results;
                $scope.projectsLoading = false;
          });
      };

      function loadParticipantInfo(projectId, participantId) {
          return ParticipantPersonsService.getParticipantPersonsById(projectId, participantId)         
          .then(function (data) {
              $scope.participantInfo[participantId] = data.data;
              $scope.participantInfo[participantId].show = true;
          }, function (error) {
              if (error.status === 404) {
                  $scope.participantInfo[participantId] = {};
                  $scope.participantInfo[participantId].show = true;
              } else {
                  $log.error('Unable to load participant info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant info for ' + participantId + '.');
              }
          });
      };

      $scope.participantInfo = {};
      $scope.toggleParticipantInfo = function (projectId, participantId) {
          if ($scope.participantInfo[participantId]) {
              if ($scope.participantInfo[participantId].show === true) {
                  $scope.participantInfo[participantId].show = false;
              } else {
                  $scope.participantInfo[participantId].show = true;
              }
          } else {
              loadParticipantInfo(projectId, participantId);
          }
      };
  });
