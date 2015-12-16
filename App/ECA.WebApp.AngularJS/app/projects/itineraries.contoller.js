'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ItinerariesCtrl', function (
      $scope,
      $state,
      $stateParams,
      $log,
      $q,
      ProjectService,
      NotificationService,
      AuthService,
      ConstantsService) {

      var projectId = parseInt($stateParams.projectId, 10);
      $scope.view = {};
      $scope.view.isLoading = true;
      $scope.view.project = null;
      $scope.view.itineraries = [];
      $scope.view.itinerariesCount = 0;

      $scope.permissions = {};
      $scope.permissions.editProject = false;

      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.project = project;
      });

      $scope.view.onNewItineraryClick = function () {
          $log.info("Clicked new traveling period.");
      }

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.editProject.value] = {
              hasPermission: function () {
                  $log.info('User has edit project permission in itinerary.controller.js.');
                  $scope.permissions.editProject = true;
              },
              notAuthorized: function () {
                  $log.info('User does not have edit project permission in itinerary.controller.js.');
                  $scope.permissions.editProject = false;
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      }

      function loadItineraries(projectId) {
          return ProjectService.getItineraries(projectId)
          .then(function (response) {
              $scope.view.itineraries = response.data;
              $scope.view.itinerariesCount = response.data.length;
          })
          .catch(function (response) {
              var message = "Unable to load travel periods.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          })
      }

      $scope.view.isLoading = true;
      $q.all([loadPermissions(), loadItineraries(projectId)])
        .then(function () {
            $scope.view.isLoading = false;
        })
        .catch(function () {
            $scope.view.isLoading = false;
        });
  });
