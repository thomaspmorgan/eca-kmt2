'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ItineraryCtrl', function (
      $scope,
      $state,
      $stateParams,
      $log,
      $q,
      ProjectService,
      AuthService,
      StateService,
      ConstantsService,
      orderByFilter) {

      $scope.view = {};
      $scope.view.isLoading = true;
      $scope.view.project = null;
      $scope.view.travelPeriods = [];

      $scope.view.travelPeriods.push({
          id: 1,
          name: 'Travel Period 1',
          groupCount: 4,
          participantCount: 10,
          arrivalDestination: 'NYC',
          departureDestination: 'Europe',
          startDate: new Date(),
          endDate: new Date()
      });
      $scope.view.travelPeriods.push({
          id: 2,
          name: 'Travel Period 2',
          groupCount: 1,
          participantCount: 1,
          arrivalDestination: 'Nashville',
          departureDestination: 'LAX',
          startDate: new Date(),
          endDate: new Date()
      });

      $scope.permissions = {};
      $scope.permissions.editProject = false;

      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.isLoading = false;
          $scope.view.project = project;
          return loadPermissions()
          .then(function () {
              $scope.view.isLoading = false;
          })
      });

      $scope.view.onNewTravelingPeriodClick = function () {
          $log.info("Clicked new traveling period.");
      }
      
      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var projectId = $stateParams.projectId;
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
  });
