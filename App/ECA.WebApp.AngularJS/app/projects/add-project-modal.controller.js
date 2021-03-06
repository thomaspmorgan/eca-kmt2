﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddProjectModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $modalInstance,
        parentProgram,
        MessageBox,
        ProjectService,
        FilterService,
        LookupService,
        TableService,
        OfficeService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.showConfirmCancel = false;
      $scope.view.maxNameLength = 500;
      $scope.view.maxDescriptionLength = 3000;
      $scope.view.title = 'Add Project to ' + parentProgram.name;
      $scope.view.isSavingProject = false;
      $scope.view.isLoadingLikeProjects = false;
      $scope.view.matchingProjectsByName = [];
      $scope.view.doesProjectExist = false;
      $scope.view.project = {
          name: '',
          description: '',
          programId: parentProgram.id
      };
      $scope.view.onSaveClick = function () {
          saveProject();
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.projectForm.$dirty) {
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

      function saveProject() {
          $scope.view.isSavingProject = true;
          return ProjectService.create($scope.view.project)
          .then(function (response) {
              $scope.view.isSavingProject = false;
              $modalInstance.close(response.data);
          })
          .catch(function (response) {
              $scope.view.isSavingProject = false;
              var message = 'Unable to save program.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
  });
