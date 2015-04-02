'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectOverviewCtrl', function ($scope, $stateParams, $q, $log, $timeout, ProjectService, ProgramService, TableService, LookupService, ConstantsService) {

      $scope.view = {};
      $scope.view.params = $stateParams;

      $scope.view.saveFailed = false;
      $scope.view.errorMessage = "";
      $scope.view.validations = [];
      $scope.view.notifications = [];
      $scope.view.areAlertsCollapsed = false;
      $scope.view.isLoading = false;
      $scope.view.isSaving = false;

      $scope.editView = {};
      $scope.editView.out = {
          themes: [],
          goals: [],
          pointsOfContact: []
      };
      $scope.editView.show = false;
      $scope.editView.foci = [];
      $scope.editView.projectStati = [];
      $scope.editView.pointsOfContact = [];
      $scope.editView.themes = [];
      $scope.editView.goals = [];

      $scope.editView.loadProjectStati = function () {
          loadProjectStati();
      }

      $scope.editView.loadFoci = function () {
          loadFoci();
      }

      $scope.editView.removePointsOfContact = function () {
          $scope.$parent.project.pointsOfContactIds = [];
      }

      $scope.editView.searchPointsOfContact = function (data) {
          loadPointsOfContact(data);
      }

      $scope.editView.searchThemes = function (data) {
          loadThemes(data);
      }

      $scope.editView.removeThemes = function () {
          $scope.$parent.project.themeIds = [];
      }

      $scope.editView.searchGoals = function (data) {
          loadGoals(data);
      }

      $scope.editView.removeGoals = function () {
          $scope.$parent.project.goalIds = [];
      }

      $scope.view.confirmFailOk = function () {
          $scope.view.saveFailed = false;
      }

      $scope.view.onCancelClick = function () {
          $scope.editView.show = false;
          $scope.view.isLoading = true;
          loadProject()
            .then(function () {

            }, function () {

            }).then(function () {
                $scope.view.isLoading = false;
            });
      }

      $scope.view.onSaveClick = function ($event) {
          saveProject($event);
      }

      $scope.view.updateFocusView = function () {
          $scope.$parent.project.focus = getFocusById($scope.editView.foci, $scope.$parent.project.focusId).name;
      }

      $scope.view.updateStatusView = function () {
          $scope.$parent.project.status = getStatusById($scope.editView.projectStati, $scope.$parent.project.projectStatusId).name;
      }

      var editProjectEventName = ConstantsService.editProjectEventName;
      $scope.$on(editProjectEventName, function () {
          $log.info('Handling event [' + editProjectEventName + '] in overview.js controller.');
          setTickedPointsOfContact();
          setTickedThemes();
          setTickedGoals();
          $scope.editView.show = true;
      });

      function setTickedItems(projectPropertyName, editViewInputModelName) {
          
          if (!$scope.$parent.project.hasOwnProperty(projectPropertyName)) {
              $log.error('The $scope.project does not have a property named ' + projectPropertyName);
          }
          if (!$scope.editView.hasOwnProperty(editViewInputModelName)) {
              $log.error('The $scope.editView does not have a property named ' + editViewInputModelName);
          }
          var inputModels = $scope.editView[editViewInputModelName];
          for (var i = 0; i < inputModels.length; i++) {
              var inputModel = inputModels[i];
              for (var j = 0; j < $scope.$parent.project[projectPropertyName].length; j++) {
                  var projectPropertyValue = $scope.$parent.project[projectPropertyName][j];
                  if (inputModel.id === projectPropertyValue.id) {
                      inputModel.ticked = true;
                  }
              }
          }
      }

      function setTickedPointsOfContact() {
          setTickedItems('contacts', 'pointsOfContact');
      }

      function setTickedThemes() {
          setTickedItems('themes', 'themes');
      }

      function setTickedGoals() {
          setTickedItems('goals', 'goals');
      }

      function getStatusById(stati, id) {
          return getLookupById(stati, id);
      }

      function getFocusById(foci, id) {
          return getLookupById(foci, id);
      }

      function getLookupById(lookups, id) {
          for (var i = 0; i < lookups.length; i++) {
              var l = lookups[i];
              if (l.id === id) {
                  return l;
              }
          }
          return null;
      }

      function mapOutModelToIds(outModelName, projectPropertyName, idPropertyName) {
          $log.info('Mapping editView.out.' + outModelName + ' to project.' + projectPropertyName);
          if (!$scope.editView.out.hasOwnProperty(outModelName)) {
              $log.error("The editView.out does not have a property named " + outModelName);
          }
          if (!$scope.$parent.project.hasOwnProperty(projectPropertyName)) {
              $log.debug('Creating array property on project model named ' + projectPropertyName);
              $scope.$parent.project[projectPropertyName] = [];
          }
          if ($scope.editView.out[outModelName] && $scope.editView.out[outModelName].length > 0) {
              $scope.$parent.project[projectPropertyName] = $scope.editView.out[outModelName].map(function (model) {
                  if (!model.hasOwnProperty(idPropertyName)) {
                      $log.error('The model does not have a property named ' + idPropertyName);
                  }
                  return model[idPropertyName];
              });
          }
      }

      function saveProject($event) {
          $scope.view.isSaving = true;
          $scope.view.saveFailed = false;

          mapOutModelToIds('goals', 'goalIds', 'id');
          mapOutModelToIds('pointsOfContact', 'pointsOfContactIds', 'id');
          mapOutModelToIds('themes', 'themeIds', 'id');
          mapOutModelToIds('goals', 'goalIds', 'id');

          ProjectService.update($scope.$parent.project, $stateParams.projectId)
            .then(function (response) {
                $scope.$parent.project = response.data;
                $scope.editView.show = false;
                showSaveSuccess();
            }, function (errorResponse) {
                $scope.view.saveFailed = true;
                $scope.view.errorMessage = "An error occurred while saving the project.";
                if (errorResponse.data && errorResponse.data.Message) {
                    $scope.view.errorMessage = errorResponse.data.Message;
                }
                if (errorResponse.data && errorResponse.data.ValidationErrors) {
                    for (var key in errorResponse.data.ValidationErrors) {
                        $scope.view.validations.push(errorResponse.data.ValidationErrors[key]);
                    }
                }
            })
            .then(function () {
                $scope.view.isSaving = false;
            });
      }

      var removeAlertTimeout = 1500;
      var hideAlertsTimeout = 3000;
      function showSaveSuccess() {
          $scope.view.areAlertsCollapsed = false;
          var index = $scope.view.notifications.push({ type: 'success', msg: 'Successfully saved project changes.' });
          $timeout(function () {
              $scope.view.areAlertsCollapsed = true;
              $timeout(function () {
                  removeAlert(index);
              }, removeAlertTimeout);
          }, hideAlertsTimeout);
      }

      function removeAlert(index) {
          $scope.view.notifications = $scope.view.notifications.splice(index, 1);
      }

      function loadProjectStati() {
          return LookupService.getAllProjectStati({ start: 0, limit: 300 })
            .then(function (response) {
                $scope.editView.projectStati = response.data.results;
            }, function (errorResponse) {

            });
      }

      function loadFoci() {
          return LookupService.getAllFocusAreas({ start: 0, limit: 300 })
              .then(function (response) {
                  $scope.editView.foci = response.results;
              });
      }

      function loadProject() {
          var projectId = $stateParams.projectId;
          return ProjectService.get(projectId)
            .then(function (data) {
                $scope.$parent.project = data.data;
                $scope.$parent.project.startDate = new Date($scope.$parent.project.startDate);
                $scope.$parent.project.endDate = new Date($scope.$parent.project.endDate);
                $scope.$parent.project.countryIsos = $scope.$parent.project.countryIsos || [];
            }, function (errorResponse) {
                $log.error('Failed to load project with id ' + projectId);
            })
      }

      var maxLimit = 300;
      function loadPointsOfContact(search) {
          var params = {
              start: 0,
              limit: maxLimit
          };
          if (search && search.keyword) {
              params.filter = {
                  comparison: 'like',
                  property: 'fullName',
                  value: search.keyword
              }
          }
          return LookupService.getAllContacts(params)
              .then(function (response) {
                  $scope.editView.pointsOfContact = response.results;
              });
      }

      function loadThemes(search) {
          var params = {
              start: 0,
              limit: maxLimit
          };
          if (search && search.keyword) {
              params.filter = {
                  comparison: 'like',
                  property: 'name',
                  value: search.keyword
              }
          }
          return LookupService.getAllThemes(params)
              .then(function (response) {
                  $scope.editView.themes = response.results;
              });
      }

      function loadGoals(search) {
          var params = {
              start: 0,
              limit: maxLimit
          };
          if (search && search.keyword) {
              params.filter = {
                  comparison: 'like',
                  property: 'name',
                  value: search.keyword
              }
          }
          return LookupService.getAllGoals(params)
              .then(function (response) {
                  $scope.editView.goals = response.results;
              });
      }

      $scope.view.isLoading = true;
      $q.all([loadThemes(null), loadPointsOfContact(null), loadFoci(), loadProjectStati(), loadGoals(null), loadProject()])
      .then(function (results) {
          //results is an array

      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });
  });
