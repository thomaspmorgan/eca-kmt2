'use strict';

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
        ProjectService,
        FilterService,
        LookupService,
        TableService,
        OfficeService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
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
              var modalInstance = $modal.open({
                  animation: false,
                  templateUrl: 'views/directives/confirmdialog.html',
                  controller: 'ConfirmCtrl',
                  resolve: {
                      options: function () {
                          return {
                              title: 'Confirm',
                              message: 'There are unsaved changes.  Are you sure you wish to cancel?',
                              okText: 'Yes',
                              cancelText: 'No'
                          };
                      }
                  }
              });
              modalInstance.result.then(function () {
                  $log.info('User confirmed cancel...');
                  $modalInstance.dismiss('cancel');

              }, function () {
                  $log.info('Modal dismissed at: ' + new Date());
              });
          }
          else {
              $modalInstance.dismiss('cancel');
          }
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
              if (response.status === 400 && response.data && response.data.ValidationErrors) {
                  showValidationErrors(response.data);
              }
              else {
                  var message = 'Unable to save project.';
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
          });
      }

      function showValidationErrors(error) {
          var validationModal = $modal.open({
              animation: false,
              templateUrl: 'views/directives/servervalidationdialog.html',
              controller: 'ServerValidationCtrl',
              size: 'lg',
              resolve: {
                  options: function () {
                      return {};
                  },
                  validationError: function () {
                      return error.ValidationErrors;
                  }
              }
          });
          validationModal.result.then(function () {
              $log.info('Finished validation errors.');
              validationModal.close();

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }
  });
