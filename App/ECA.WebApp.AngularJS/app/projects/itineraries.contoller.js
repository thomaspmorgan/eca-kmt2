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
      $modal,
      ProjectService,
      NotificationService,
      AuthService,
      ConstantsService) {

      var projectId = parseInt($stateParams.projectId, 10);
      $scope.view = {};
      $scope.view.isLoading = true;
      $scope.view.isLoadingProject = true;
      $scope.view.project = null;
      $scope.view.itineraries = [];
      $scope.view.itinerariesCount = 0;

      $scope.permissions = {};
      $scope.permissions.editProject = false;

      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.project = project;
          $scope.view.isLoadingProject = false;
          $scope.view.onNewItineraryClick();
      });

      $scope.view.onNewItineraryClick = function () {
          var addItineraryModal = $modal.open({
              animation: true,
              templateUrl: 'app/projects/add-itinerary-modal.html',
              controller: 'AddItineraryModalCtrl',
              size: 'lg',
              backdrop: 'static',
              resolve: {
                  project: function () {
                      return $scope.view.project;
                  }
              }
          });
          addItineraryModal.result.then(function (addedItinerary) {
              $log.info('Finished adding itinerary.');

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
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
