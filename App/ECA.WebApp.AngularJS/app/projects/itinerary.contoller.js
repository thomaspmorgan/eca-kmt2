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
      NotificationService,
      AuthService,
      ConstantsService) {

      var projectId = parseInt($stateParams.projectId, 10);
      $scope.view = {};
      $scope.view.isLoading = true;
      $scope.view.project = null;
      $scope.view.travelPeriods = [];
      $scope.view.travelPeriodsCount = 0;

      //$scope.view.travelPeriods.push({
      //    id: 1,
      //    name: 'Travel Period 1',
      //    groupCount: 4,
      //    participantCount: 10,
      //    arrivalDestination: 'NYC',
      //    departureDestination: 'Europe',
      //    startDate: new Date(),
      //    endDate: new Date()
      //});
      //$scope.view.travelPeriods.push({
      //    id: 2,
      //    name: 'Travel Period 2',
      //    groupCount: 1,
      //    participantCount: 1,
      //    arrivalDestination: 'Nashville',
      //    departureDestination: 'LAX',
      //    startDate: new Date(),
      //    endDate: new Date()
      //});

      $scope.permissions = {};
      $scope.permissions.editProject = false;

      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.project = project;
      });

      $scope.view.onNewTravelingPeriodClick = function () {
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

      function toDate(itinerary, datePropertyName) {
          var date = new Date(itinerary[datePropertyName]);
          if (!isNaN(date.getTime())) {
              itinerary[datePropertyName] = date;
          }
      }

      function loadItineraries(projectId) {
          return ProjectService.getItineraries(projectId)
          .then(function (response) {
              angular.forEach(response.data, function (itinerary, index) {
                  toDate(itinerary, 'startDate');
                  toDate(itinerary, 'endDate');
                  toDate(itinerary, 'lastRevisedOn');
              });
              $scope.view.travelPeriods = response.data;
              $scope.view.travelPeriodsCount = response.data.length;
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
