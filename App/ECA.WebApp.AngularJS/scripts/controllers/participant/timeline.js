'use strict';

/**
 * Controller for participant timeline
 */
angular.module('staticApp')
  .controller('ParticipantTimelineCtrl', function ($scope, ProjectService) {

      $scope.projectsLoading = false;

      $scope.personIdDeferred.promise
        .then(function (personId) {
            loadProjects(personId)
        });

      function loadProjects(personId) {
          $scope.projectsLoading = true;
          var params = { start: 0, limit: 300 };
          ProjectService.getProjectsByPersonId(personId, params)
            .then(function (data) {
                $scope.projects = data.data.results;
                $scope.projectsLoading = false;
          });
      };
  });
