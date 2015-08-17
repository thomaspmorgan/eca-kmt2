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
        $timeout,
        ProjectService,
        ProgramService,
        TableService,
        LocationService,
        LookupService,
        ConstantsService,
        AuthService,
        OfficeService,
        NotificationService) {

      console.assert(typeof ($scope.$parent.isInEditViewState) !== 'undefined', 'The isInEditViewState property on the parent scope must be defined.');
      $scope.$parent.isInEditViewState = true;
      console.assert($scope.$parent.showProjectEditCancelButton, 'The $scope.$parent.showProjectEditCancelButton function must be defined.');
      console.assert($scope.$parent.hideProjectEditCancelButton, 'The $scope.$parent.hideProjectEditCancelButton function must be defined.');


      $scope.editView = {};
      $scope.editView.params = $stateParams;
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
      $scope.editView.locations = [];

      $scope.editView.selectedPointsOfContact = [];
      $scope.editView.selectedGoals = [];
      $scope.editView.selectedThemes = [];
      $scope.editView.selectedCategories = [];
      $scope.editView.selectedObjectives = [];
      $scope.editView.selectedLocations = [];

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

      $scope.editView.searchLocations = function (data) {
          loadLocations(data);
      }

      $scope.editView.removeLocations = function () {
          $scope.$parent.project.locationIds = [];
      }

      $scope.editView.confirmFailOk = function () {
          $scope.editView.saveFailed = false;
      }

      $scope.$on(ConstantsService.saveProjectEventName, function () {
          saveProject();
      });

      $scope.$on(ConstantsService.cancelProjectEventName, function () {
          cancelEdit();
      });

      $scope.editView.onCancelClick = function () {
          cancelEdit();
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
          $scope.$parent.hideProjectEditCancelButton();
          $scope.editView.isEndDatePickerOpen = true;
      }

      $scope.editView.onAddLocationClick = function () {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: 'views/locations/locationmodal.html',
                //controller: 'MoneyFlowCtrl',
                size: 'lg',
                resolve: {
                    //entity: function () {
                    //    return moneyFlow;
                    //}
                }
            });

            modalInstance.result.then(function (newMoneyFlow) {
                $log.info('Finished adding locations.');
                //reloadMoneyFlowTable();
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
      }

      function cancelEdit() {
          if ($scope.form.projectForm.$dirty) {
              var modalInstance = $modal.open({
                  templateUrl: '/views/project/unsavedchanges.html',
                  controller: 'UnsavedChangesCtrl',
                  windowClass: 'modal-center-small',
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

      function disableProjectStatusButton() {
          $scope.$parent.isProjectStatusButtonEnabled = false;
      }

      function enableProjectStatusButton() {
          $scope.$parent.isProjectStatusButtonEnabled = true;
      }

      function showProjectEditCancelButton() {
          console.assert($scope.$parent.showProjectEditCancelButton, 'The $scope.$parent.showProjectEditCancelButton function should be defined.');
          $scope.showProjectEditCancelButton();
      }

      function goToProjectOverview() {
          console.assert(typeof ($scope.$parent.isInEditViewState) !== 'undefined', 'The isInEditViewState property on the parent scope must be defined.');
          $scope.$parent.isInEditViewState = false;
          $scope.$parent.hideProjectEditCancelButton();
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

      function updateLocations() {
          var propertyName = "locationIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedLocations');
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
          updateLocations();
          disableProjectStatusButton();
          $scope.$parent.hideProjectEditCancelButton();
          ProjectService.update($scope.$parent.project, $stateParams.projectId)
            .then(function (response) {
                $scope.$parent.project = response.data;
                showSaveSuccess();
                goToProjectOverview();
            }, function (error) {
                showProjectEditCancelButton();
                if (error.status == 400) {
                    if (error.data.message && error.data.modelState) {
                        for (var key in error.data.modelState) {
                            NotificationService.showErrorMessage(error.data.modelState[key][0]);
                        }
                    }
                    else if (error.data.Message && error.data.ValidationErrors) {
                        for (var key in error.data.ValidationErrors) {
                            NotificationService.showErrorMessage(error.data.ValidationErrors[key]);
                        }
                    } else {
                        NotificationService.showErrorMessage(error.data);
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
          return ProjectService.getById(projectId)
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
                setSelectedLocations();

            }, function (errorResponse) {
                $log.error('Failed to load project with id ' + projectId);
            })
            .then(function () {
                showProjectEditCancelButton();
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

      function setSelectedLocations() {
          var locationsName = 'locations';
          normalizeLookupProperties($scope.$parent.project[locationsName]);
          setSelectedItems(locationsName, 'selectedLocations');
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
          var programId = $stateParams.programId;
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
          return ProjectService.getCategories(programId, params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more categories in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.data.results);
                  $scope.editView.categories = response.data.results;
              })
            .catch(function () {
                $log.error('Unable to load categories.');
                NotificationService.showErrorMessage('Unable to load categories.');
            });
      }

      function loadObjectives(search) {
          var programId = $stateParams.programId;
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
          return ProjectService.getObjectives(programId, params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more objectives in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.data.results);
                  $scope.editView.objectives = response.data.results;
              })
              .catch(function () {
                  $log.error('Unable to load objectives.');
                  NotificationService.showErrorMessage('Unable to load objectives.');
              });
      }

      function loadLocations(search) {
          var params = {
              start: 0,
              limit: 10,
              filter: [{
                  comparison: ConstantsService.notEqualComparisonType,
                  property: 'locationTypeId',
                  value: ConstantsService.locationType.address.id
              }]
          };
          if (search) {
              params.filter.push({
                  comparison: ConstantsService.likeComparisonType,
                  property: 'name',
                  value: search
              })
          }
          return LocationService.get(params)
              .then(function (response) {
                  angular.forEach(response.results, function (result, index) {
                      if (!result.name) {
                          result.name = 'UNKNOWN';
                      }
                  });
                  normalizeLookupProperties(response.results);
                  $scope.editView.locations = response.results;
              })
              .catch(function () {
                  $log.error('Unable to load locations.');
                  NotificationService.showErrorMessage('Unable to load locations.');
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
                  console.assert(typeof (response.data.isCategoryRequired) !== 'undefined', "The is category required bool must exist.");
                  console.assert(typeof (response.data.isObjectiveRequired) !== 'undefined', "The is objective required bool must exist.");

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
          config[ConstantsService.permission.editProject.value] = {
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
