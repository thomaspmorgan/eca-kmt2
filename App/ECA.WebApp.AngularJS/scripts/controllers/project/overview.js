'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectOverviewCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        ProjectService,
        ProgramService,
        TableService,
        LookupService,
        ConstantsService,
        AuthService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;

      $scope.view.saveFailed = false;
      $scope.view.errorMessage = "";
      $scope.view.validations = [];
      $scope.view.isLoading = false;
      $scope.view.isSaving = false;

      $scope.editView = {};
      $scope.editView.show = false;
      $scope.editView.foci = [];
      $scope.editView.projectStati = [];
      $scope.editView.pointsOfContact = [];
      $scope.editView.themes = [];
      $scope.editView.goals = [];
      $scope.editView.selectedPointsOfContact = [];
      $scope.editView.selectedGoals = [];
      $scope.editView.selectedThemes = [];

      $scope.permissions = {};
      $scope.permissions.canEdit = true;
      

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
          hideEditView();
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

      $scope.closeAlert = function (index) {
          removeAlert(index);
      }

      var editProjectEventName = ConstantsService.editProjectEventName;
      $scope.$on(editProjectEventName, function () {
          $log.info('Handling event [' + editProjectEventName + '] in overview.js controller.');
          showEditView();
      });

      function allowEdit(canEdit) {
          $scope.permissions.canEdit = canEdit;
      }

      function showEditView() {
          if($scope.permissions.canEdit){
              $scope.editView.show = true;
          }
          else {
              NotificationService.showUnauthorizedMessage('You are not authorized to edit this project.');
          }
      }

      function hideEditView() {
          $scope.editView.show = false;
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

      function updateRelationshipIds(idsPropertyName, editViewSelectedPropertyName) {
          console.assert($scope.$parent.project.hasOwnProperty(idsPropertyName), "The project must have the property named " + idsPropertyName);
          console.assert($scope.editView.hasOwnProperty(editViewSelectedPropertyName), "The edit view must have the property named " + editViewSelectedPropertyName);
          $scope.$parent.project[idsPropertyName] = [];
          $scope.$parent.project[idsPropertyName] = $scope.editView[editViewSelectedPropertyName].map(function (c) {
              return c.id;
          });
      }

      function updatePointsOfContactIds() {
          var propertyName = "pointsOfContactIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedPointsOfContact');
      }

      function updateThemes() {
          var propertyName = "themeIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedThemes');
      }

      function updateGoals() {
          var propertyName = "goalIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedGoals');
      }

      function saveProject($event) {
          $scope.view.isSaving = true;
          $scope.view.saveFailed = false;

          updatePointsOfContactIds();
          updateThemes();
          updateGoals();

          ProjectService.update($scope.$parent.project, $stateParams.projectId)
            .then(function (response) {
                $scope.$parent.project = response.data;
                hideEditView();
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

      function showSaveSuccess() {
          NotificationService.showSuccessMessage('Successfully saved project changes.');
      }

      var maxLimit = 300;
      function loadProjectStati() {
          return LookupService.getAllProjectStati({ start: 0, limit: maxLimit })
            .then(function (response) {
                if (response.data.total > maxLimit) {
                    $log.error('There are more project stati in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                }
                $scope.editView.projectStati = response.data.results;
            }, function (errorResponse) {

            });
      }

      function loadFoci() {
          return LookupService.getAllFocusAreas({ start: 0, limit: maxLimit })
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more foci in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  $scope.editView.foci = response.results;
              });
      }

      function loadProject() {
          var projectId = $stateParams.projectId;
          return ProjectService.get(projectId)
            .then(function (data) {
                $scope.$parent.project = data.data;
                $scope.$parent.project.countryIsos = $scope.$parent.project.countryIsos || [];
                var startDate = new Date($scope.$parent.project.startDate);
                if (!isNaN(startDate.getTime())) {
                    $scope.$parent.project.startDate = startDate;
                }
                var endDate = new Date($scope.$parent.project.endDate);
                if (!isNaN(endDate.getTime())) {
                    $scope.$parent.project.endDate = endDate;
                }
                setSelectedPointsOfContact();
                setSelectedGoals();
                setSelectedThemes();

            }, function (errorResponse) {
                $log.error('Failed to load project with id ' + projectId);
                if (errorResponse.status === 401) {
                    showUnauthorizedMessage();
                }
            });
      }

      function setSelectedItems(projectPropertyName, editViewSelectedPropertyName) {
          console.assert($scope.$parent.project.hasOwnProperty(projectPropertyName), "The project property " + projectPropertyName + " does not exist.");
          console.assert($scope.editView.hasOwnProperty(editViewSelectedPropertyName), "The edit view " + editViewSelectedPropertyName + " property does not exist.");
          console.assert(Array.isArray($scope.editView[editViewSelectedPropertyName]), "The edit view " + editViewSelectedPropertyName + " property must be an array.");

          var projectItems = $scope.$parent.project[projectPropertyName];
          $scope.editView[editViewSelectedPropertyName] = $scope.editView[editViewSelectedPropertyName].splice(0, $scope.editView[editViewSelectedPropertyName].length);
          for (var i = 0; i < projectItems.length; i++) {
              var projectItem = projectItems[i];
              $scope.editView[editViewSelectedPropertyName].push(projectItem);
          }
      }

      function setSelectedPointsOfContact() {
          setSelectedItems('contacts', 'selectedPointsOfContact');
      }

      function setSelectedGoals() {
          setSelectedItems('goals', 'selectedGoals');
      }

      function setSelectedThemes() {
          setSelectedItems('themes', 'selectedThemes');
      }

      function normalizeLookupProperties(lookups) {
          console.assert(Array.isArray(lookups), "The given value must be an array.");
          for (var i = 0; i < lookups.length; i++) {
              lookups[i].value = lookups[i].name;
          }
      }

      function loadPointsOfContact(search) {
          var params = {
              start: 0,
              limit: maxLimit
          };
          if (search) {
              params.filter = [{
                  comparison: ConstantsService.likeComparisonType,
                  property: 'fullName',
                  value: search
              }]
          }
          return LookupService.getAllContacts(params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more contacts in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  for (var i = 0; i < response.results.length; i++) {
                      var position = "";
                      if (response.results[i].position) {
                          position = " (" + response.results[i].position + ")";
                      }
                      response.results[i].value = response.results[i].fullName + position;
                  }
                  $scope.editView.pointsOfContact = response.results;
              });
      }

      function loadThemes(search) {
          var params = {
              start: 0,
              limit: maxLimit
          };
          if (search) {
              params.filter = [{
                  comparison: ConstantsService.likeComparisonType,
                  property: 'name',
                  value: search
              }];
          }
          return LookupService.getAllThemes(params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more themes in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.results);
                  $scope.editView.themes = response.results;
              });
      }

      function loadGoals(search) {
          var params = {
              start: 0,
              limit: maxLimit
          };
          if (search) {
              params.filter = [{
                  comparison: ConstantsService.likeComparisonType,
                  property: 'name',
                  value: search
              }]
          }
          return LookupService.getAllGoals(params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more goals in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.results);
                  $scope.editView.goals = response.results;
              });
      }

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var projectId = $stateParams.projectId;
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.editproject.value] = {
              hasPermission: function () {
                  allowEdit(true);
                  $log.info('User has edit project permission.');
              },
              notAuthorized: function () {
                  allowEdit(false);
                  $log.info('User not authorized to edit project.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
                $log.info('Successfully loaded permissions.');
            }, function() {
                $log.error('Unable to load user permissions.');
            });
      }

      $scope.view.isLoading = true;
      $q.all([loadPermissions(), loadThemes(null), loadPointsOfContact(null), loadFoci(), loadProjectStati(), loadGoals(null), loadProject()])
      .then(function (results) {
          //results is an array

      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });
  });
