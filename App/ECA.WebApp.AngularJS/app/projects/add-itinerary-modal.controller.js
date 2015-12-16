'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddItineraryModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $modalInstance,
        project,
        MessageBox,
        ProjectService,
        FilterService,
        LookupService) {

      $scope.view = {};
      $scope.view.showConfirmCancel = false;
      $scope.view.maxNameLength = 100;
      $scope.view.project = project;
      $scope.view.isSavingItinerary = false;
      
      $scope.view.itinerary = { projectId: project.id };
      

      $scope.view.onSaveClick = function () {
          saveProject();
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.itineraryForm.$dirty) {
              $scope.view.showConfirmCancel = true;
          }
          else {
              $modalInstance.dismiss('cancel');
          }
      }

      $scope.view.onYesCancelChangesClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onNoDoNotCancelChangesClick = function () {
          $scope.view.showConfirmCancel = false;
      }


      var projectsWithSameNameFilter = FilterService.add('addprojectmodal_projectswithsamename');
      $scope.view.onProjectNameChange = function () {
          var projectName = $scope.view.project.name;
          if (projectName && projectName.length > 0) {
              projectsWithSameNameFilter.reset();
              projectsWithSameNameFilter = projectsWithSameNameFilter
                  .skip(0)
                  .take(1)
                  .equal('programId', parentProgram.id)
                  .equal('projectName', projectName);
              $scope.view.isLoadingLikeProjects = true;
              return ProjectService.get(projectsWithSameNameFilter.toParams())
                  .then(function (response) {
                      $scope.view.matchingProjectsByName = response.data.results;
                      $scope.view.doesProjectExist = response.data.total > 0;
                      $scope.view.isLoadingLikeProjects = false;
                  })
                  .catch(function (response) {
                      $scope.view.isLoadingLikeProjects = false;
                      var message = "Unable to load matching programs.";
                      $log.error(message);
                      NotificationService.showErrorMessage(message);
                  });
          }
      }

      function saveItinerary() {
          $scope.view.isSavingItinerary = true;
          return ProjectService.create($scope.view.project)
          .then(function (response) {
              $scope.view.isSavingItinerary = false;
              $modalInstance.close(response.data);
          })
          .catch(function (response) {
              $scope.view.isSavingItinerary = false;
              var message = 'Unable to save itinerary.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
  });
