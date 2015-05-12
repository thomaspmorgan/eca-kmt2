﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectEditCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $state,
        $modal,
        ProjectService,
        ProgramService,
        TableService,
        LookupService,
        ConstantsService,
        AuthService,
        OfficeService,
        NotificationService) {

      console.assert(typeof ($scope.$parent.isInEditViewState) !== 'undefined', 'The isInEditViewState property on the parent scope must be defined.');
      $scope.$parent.isInEditViewState = true;

      $scope.editView = {};
      $scope.editView.params = $stateParams;
      $scope.editView.saveFailed = false;
      $scope.editView.errorMessage = "";
      $scope.editView.validations = [];
      $scope.editView.isLoading = false;
      $scope.editView.isSaving = false;      
      $scope.editView.dateFormat = 'dd-MMMM-yyyy';
      $scope.editView.isStartDatePickerOpen = false;
      $scope.editView.isEndDatePickerOpen = false;
      $scope.editView.showCategoryFocus = true;
      $scope.editView.showObjectiveJustification = true;

      $scope.editView.foci = [];
      $scope.editView.projectStati = [];
      $scope.editView.pointsOfContact = [];
      $scope.editView.themes = [];
      $scope.editView.goals = [];
      $scope.editView.categories = [];
      $scope.editView.objectives = [];

      $scope.editView.selectedPointsOfContact = [];
      $scope.editView.selectedGoals = [];
      $scope.editView.selectedThemes = [];
      $scope.editView.selectedCategories = [];
      $scope.editView.selectedObjectives = [];

      $scope.editView.categoryLabel = "...";
      $scope.editView.objectiveLabel = "...";


      $scope.editView.loadProjectStati = function () {
          loadProjectStati();
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

      $scope.editView.searchCategories = function (data) {
          loadCategories(data);
      }

      $scope.editView.removeCategories = function () {
          $scope.$parent.project.categoryIds = [];
      }

      $scope.editView.searchObjectives = function (data) {
          loadObjectives(data);
      }

      $scope.editView.removeObjectives = function () {
          $scope.$parent.project.ObjectiveIds = [];
      }

      $scope.editView.confirmFailOk = function () {
          $scope.editView.saveFailed = false;
      }

      $scope.$on(ConstantsService.saveProjectEventName, function () {
          saveProject();
      });

      $scope.editView.onCancelClick = function () {
          if ($scope.form.projectForm.$dirty) {
              var modalInstance = $modal.open({
                  templateUrl: '/views/project/unsavedchanges.html',
                  controller: 'UnsavedChangesCtrl',
                  windowClass: 'modal-center',
                  backdrop: 'static',
                  resolve: {},
                  size: 'lg'
              });
              modalInstance.result.then(function () {
                  $log.info('Cancelling changes...');
                  goToProjectOverview();
              }, function () {
                  $log.info('Dismiss warning dialog and allow save changes...');
              });
          }
          else {
              goToProjectOverview();
          }
      }

      $scope.editView.onSaveClick = function ($event) {
          saveProject($event);
      }

      $scope.editView.updateFocusView = function () {
          $scope.$parent.project.focus = getFocusById($scope.editView.foci, $scope.$parent.project.focusId).name;
      }

      $scope.editView.updateStatusView = function () {
          $scope.$parent.project.status = getStatusById($scope.editView.projectStati, $scope.$parent.project.projectStatusId).name;
      }

      $scope.editView.openStartDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.editView.isStartDatePickerOpen = true;
      }

      $scope.editView.openEndDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.editView.isEndDatePickerOpen = true;
      }

      function disableProjectStatusButton() {
          $scope.$parent.isProjectStatusButtonEnabled = false;
      }

      function enableProjectStatusButton() {
          $scope.$parent.isProjectStatusButtonEnabled = true;
      }

      function goToProjectOverview() {
          console.assert(typeof ($scope.$parent.isInEditViewState) !== 'undefined', 'The isInEditViewState property on the parent scope must be defined.');
          $scope.$parent.isInEditViewState = false;
          $scope.form.projectForm.$setUntouched();
          $scope.form.projectForm.$setPristine();
          $state.go('projects.overview');
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

      function updateCategories() {
          var propertyName = "categoryIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedCategories');
      }

      function updateObjectives() {
          var propertyName = "objectiveIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedObjectives');
      }

      function updateGoals() {
          var propertyName = "goalIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedGoals');
      }

      function saveProject($event) {
          $scope.editView.isSaving = true;
          $scope.editView.saveFailed = false;
          $scope.editView.validations = [];

          updatePointsOfContactIds();
          updateThemes();
          updateGoals();
          updateCategories();
          updateObjectives();
          disableProjectStatusButton();
          ProjectService.update($scope.$parent.project, $stateParams.projectId)
            .then(function (response) {
                $scope.$parent.project = response.data;
                showSaveSuccess();
                goToProjectOverview();
            }, function (errorResponse) {
                $scope.editView.saveFailed = true;
                $scope.editView.errorMessage = "An error occurred while saving the project.";
                if (errorResponse.data && errorResponse.data.Message) {
                    $scope.editView.errorMessage = errorResponse.data.Message;
                }
                if (errorResponse.data && errorResponse.data.ValidationErrors) {
                    for (var key in errorResponse.data.ValidationErrors) {
                        $scope.editView.validations.push(errorResponse.data.ValidationErrors[key]);
                    }
                }
            })
            .then(function () {
                $scope.editView.isSaving = false;
                enableProjectStatusButton();
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
                setSelectedCategories();
                setSelectedObjectives();

            }, function (errorResponse) {
                $log.error('Failed to load project with id ' + projectId);
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

      function setSelectedCategories() {
          var categoriesName = 'categories';
          normalizeLookupProperties($scope.$parent.project[categoriesName]);
          setSelectedItems(categoriesName, 'selectedCategories');
      }

      function setSelectedObjectives() {
          var objectivesName = 'objectives';
          normalizeLookupProperties($scope.$parent.project[objectivesName]);
          setSelectedItems(objectivesName, 'selectedObjectives');
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

      function loadCategories(search) {
          console.assert($stateParams.officeId, "The office id must be defined.");
          var params = {
              start: 0,
              limit: maxLimit,
              officeId: $stateParams.officeId
          };

          if (search) {
              params.filter = [{
                  comparison: ConstantsService.likeComparisonType,
                  property: 'name',
                  value: search
              }]
          }
          return LookupService.getAllCategories(params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more categories in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.results);
                  $scope.editView.categories = response.results;
              });
      }

      function loadObjectives(search) {
          console.assert($stateParams.officeId, "The office id must be defined.");
          var params = {
              start: 0,
              limit: maxLimit,
              officeId: $stateParams.officeId
          };
          if (search) {
              params.filter = [{
                  comparison: ConstantsService.likeComparisonType,
                  property: 'name',
                  value: search
              }]
          }
          return LookupService.getAllObjectives(params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more objectives in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.results);
                  $scope.editView.objectives = response.results;
              });
      }

      function loadOfficeSettings() {
          var officeId = $stateParams.officeId;
          return OfficeService.getSettings(officeId)
              .then(function (response) {
                  $log.info('Loading office settings for office with id ' + officeId);
                  console.assert(response.data.objectiveLabel, "The objective label must exist.");
                  console.assert(response.data.categoryLabel, "The category label must exist.");
                  console.assert(response.data.focusLabel, "The focus label must exist.");
                  console.assert(response.data.justificationLabel, "The justification label must exist.");
                  console.assert(typeof(response.data.isCategoryRequired) !== 'undefined', "The is category required bool must exist.");
                  console.assert(typeof(response.data.isObjectiveRequired) !== 'undefined', "The is objective required bool must exist.");

                  var objectiveLabel = response.data.objectiveLabel;
                  var categoryLabel = response.data.categoryLabel;
                  var focusLabel = response.data.focusLabel;
                  var justificationLabel = response.data.justificationLabel;
                  var isCategoryRequired = response.data.isCategoryRequired;
                  var isObjectiveRequired = response.data.isObjectiveRequired;

                  if (isCategoryRequired) {
                      $log.info('Category is required by office, category focus fields should be visible');
                  }
                  if (isObjectiveRequired) {
                      $log.info('Objective is required by office, objective justification fields should be visible.');
                  }

                  $scope.editView.categoryLabel = categoryLabel + '/' + focusLabel;
                  $scope.editView.objectiveLabel = objectiveLabel + '/' + justificationLabel;
                  $scope.editView.showCategoryFocus = isCategoryRequired;
                  $scope.editView.showObjectiveJustification = isObjectiveRequired;
              }, function (errorResponse) {
                  $log.error('Failed to load office settings.');
              });
      }

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var projectId = $stateParams.projectId;
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.editproject.value] = {
              hasPermission: function () {
                  $log.info('User has edit project permission in edit.js controller.');
              },
              notAuthorized: function () {
                  $state.go('forbidden');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {

            }, function () {
                $log.error('Unable to load user permissions.');
            });
      }

      $scope.editView.isLoading = true;
      $q.all([loadPermissions(), loadThemes(null), loadPointsOfContact(null), loadObjectives(), loadCategories(), loadProjectStati(), loadGoals(null), loadProject(), loadOfficeSettings()])
      .then(function (results) {
          //results is an array

      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.editView.isLoading = false;
      });
  });
