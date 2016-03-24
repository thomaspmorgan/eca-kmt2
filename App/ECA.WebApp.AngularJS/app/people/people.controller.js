'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ParticipantCtrl
 * @description
 * # ParticipantCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('PeopleCtrl', function ($scope, PersonService, $stateParams, $q, NavigationService) {

      $scope.tabs = {
          personalInformation: {
              title: 'Personal Information',
              path: 'personalinformation',
              active: true,
              order: 1
          },
          activity: {
              title: 'Timeline',
              path: 'timeline',
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
          funding: {
              title: 'Funding',
              path: 'moneyflows',
              active: true,
              order: 5
          }
      };

      $scope.activityImageSet = [
          'images/placeholders/participant/activities1.png',
          'images/placeholders/participant/activities2.png'
      ];

      $scope.personIdDeferred = $q.defer();

      $scope.onPersonPiiUpdated = function () {
          loadPersonById($stateParams.personId);
          NavigationService.updateBreadcrumbs();
      }

      function loadPersonById(personId) {
          return PersonService.getPersonById(personId)
          .then(function (data) {
              $scope.person = data;
              return data;
          });
      }

      loadPersonById($stateParams.personId)
      .then(function(data) {
          $scope.personIdDeferred.resolve(data.personId);
      });

  });
