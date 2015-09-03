'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramEditCtrl
 * @description
 * # ProgramEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramEditCtrl', function (
      $scope,
      $stateParams,
      $state,
      $log,
      $q,
      FilterService,
      NotificationService,
      LookupService,
      ConstantsService,
      StateService,
      AuthService,
      OfficeService,
      orderByFilter

      ) {

      $scope.view = {};
      $scope.view.programStatii = [];
      $scope.view.goals = [];
      $scope.view.themes = [];
      $scope.view.isLoadingProgram = true;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.sortedObjectives = [];
      $scope.view.categoryLabel = '';
      $scope.view.objectiveLabel = '';
      $scope.view.minimumRequiredFoci = -1;
      $scope.view.maximumRequiredFoci = -1;
      $scope.view.isCategoryRequired = false;
      $scope.view.isObjectiveRequired = false;
      $scope.view.isEndDatePickerOpen = false;
      $scope.view.isStartDatePickerOpen = false;
      $scope.view.dateFormat = 'dd-MMMM-yyyy';

      $scope.view.selectedThemes = [];
      $scope.view.selectedGoals = [];
      $scope.view.selectedCategories = [];
      $scope.view.selectedObjectives = [];

      $scope.view.openStartDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isStartDatePickerOpen = true;
      }

      $scope.view.openEndDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isEndDatePickerOpen = true;
      }
      var maxLimit = 300;
      function loadPermissions() {
          console.assert(ConstantsService.resourceType.program.value, 'The constants service must have the program resource type value.');
          var programId = $stateParams.programId;
          var resourceType = ConstantsService.resourceType.program.value;
          var config = {};
          config[ConstantsService.permission.editProgram.value] = {
              hasPermission: function () {
                  $log.info('User has edit program permission in edit.js controller.');
              },
              notAuthorized: function () {
                  StateService.goToForbiddenState();
              }
          };
          return AuthService.getResourcePermissions(resourceType, programId, config)
            .then(function (result) {

            }, function () {
                $log.error('Unable to load user permissions.');
            });
      }

      function loadProgramStatii() {
          var params = { limit: maxLimit };
          return LookupService.getAllProgramStati(params)
          .then(function (response) {
              if (response.data.total > params.limit) {
                  var message = "Unable to load all program stattii.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
              $scope.view.programStatii = response.data.results;
              return response.data.results;
          })
          .catch(function (response) {
              var message = "Unable to load program statuses.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function loadOfficeSettings(officeId) {
          return OfficeService.getSettings(officeId)
              .then(function (response) {
                  var objectiveLabel = response.data.objectiveLabel;
                  var categoryLabel = response.data.categoryLabel;
                  var focusLabel = response.data.focusLabel;
                  var justificationLabel = response.data.justificationLabel;
                  var isCategoryRequired = response.data.isCategoryRequired;
                  var isObjectiveRequired = response.data.isObjectiveRequired;

                  $scope.view.minimumRequiredFoci = response.data.minimumRequiredFoci;
                  $scope.view.maximumRequiredFoci = response.data.maximumRequiredFoci;

                  $scope.view.categoryLabel = categoryLabel + '/' + focusLabel;
                  $scope.view.objectiveLabel = objectiveLabel + '/' + justificationLabel;

                  $scope.view.isCategoryRequired = isCategoryRequired;
                  $scope.view.isObjectiveRequired = isObjectiveRequired;

              })
          .catch(function () {
              $log.error('Unable to load office settings.');
              NotificationService.showErrorMessage('Unable to load office settings.');
          });
      }

      var categoriesFilter = FilterService.add('editprogram_categoriesfilter');
      function loadCategories(officeId, selectedCategories) {
          categoriesFilter.reset();
          categoriesFilter = categoriesFilter.skip(0).take(maxLimit).notIn('id', selectedCategories.map(function (c) { return c.id; }));
          return OfficeService.getCategories(officeId, categoriesFilter.toParams())
            .then(function (response) {
                normalizeLookupProperties(response.data.results);
                $scope.view.categories = response.data.results;
                if (response.data.total > maxLimit) {
                    NotificationService.showWarningMessage("There are more categories than can be loaded in one time, some categories may be available.");
                }
            })
          .catch(function (response) {
              var message = 'Unable to load categories.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }


      function loadObjectives(officeId) {
          return OfficeService.getObjectives(officeId, { limit: maxLimit })
            .then(function (response) {
                normalizeLookupProperties(response.data.results);
                $scope.view.objectives = response.data.results;
                if (response.data.total > maxLimit) {
                    NotificationService.showWarningMessage("There are more objectives than can be loaded in one time, some categories may be available.");
                }
            })
          .catch(function (response) {
              var message = 'Unable to load objectives.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function loadThemes() {
          return LookupService.getAllThemes({ limit: maxLimit })
            .then(function (data) {
                normalizeLookupProperties(data.results);
                $scope.view.themes = data.results;
                if (data.total > maxLimit) {
                    NotificationService.showWarningMessage("There are more themes than can be loaded in one time, some categories may be available.");
                }
            })
          .catch(function () {
              var message = "Unable to load themes.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function loadGoals() {
          return LookupService.getAllGoals({ limit: maxLimit })
            .then(function (data) {
                normalizeLookupProperties(data.results);
                $scope.view.goals = data.results;
                if (data.total > maxLimit) {
                    NotificationService.showWarningMessage("There are more goals than can be loaded in one time, some categories may be available.");
                }
            })
          .catch(function () {
              var message = "Unable to load goals.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function setSelectedItems(programPropertyName, viewSelectedPropertyName) {
          console.assert($scope.view.program.hasOwnProperty(programPropertyName), "The project property " + programPropertyName + " does not exist.");
          console.assert($scope.view.hasOwnProperty(viewSelectedPropertyName), "The view " + viewSelectedPropertyName + " property does not exist.");
          console.assert(Array.isArray($scope.view[viewSelectedPropertyName]), "The view " + viewSelectedPropertyName + " property must be an array.");

          var programItems = $scope.view.program[programPropertyName];
          $scope.view[viewSelectedPropertyName] = [];
          for (var i = 0; i < programItems.length; i++) {
              var programItem = programItems[i];
              $scope.view[viewSelectedPropertyName].push(programItem);
          }
      }

      function setSelectedPointsOfContact() {
          setSelectedItems('contacts', 'selectedPointsOfContact');
      }

      function setSelectedGoals() {
          normalizeLookupProperties($scope.view.program.goals);
          setSelectedItems('goals', 'selectedGoals');
      }

      function setSelectedThemes() {
          normalizeLookupProperties($scope.view.program.themes);
          setSelectedItems('themes', 'selectedThemes');
      }

      function setSelectedCategories() {
          var categoriesName = 'categories';
          normalizeLookupProperties($scope.view.program[categoriesName]);
          setSelectedItems(categoriesName, 'selectedCategories');
      }

      function setSelectedObjectives() {
          var objectivesName = 'objectives';
          normalizeLookupProperties($scope.view.program[objectivesName]);
          setSelectedItems(objectivesName, 'selectedObjectives');
      }

      function normalizeLookupProperties(lookups) {
          console.assert(Array.isArray(lookups), "The given value must be an array.");
          for (var i = 0; i < lookups.length; i++) {
              var lookup = lookups[i];
              if (lookup.name) {
                  lookup.value = lookup.name;
              }
              else if (lookup.value) {
                  lookup.name = lookup.value;
              }
              else {
                  throw Error('Unable to normalize lookup.');
              }
          }
      }

      //function setSelectedLocations() {
      //    var locationsName = 'locations';
      //    normalizeLookupProperties($scope.$parent.project[locationsName]);
      //    setSelectedItems(locationsName, 'selectedLocations');
      //}

      $scope.data.loadProgramPromise.promise
      .then(function (program) {
          $scope.view.isLoadingProgram = false;
          $scope.view.program = program;
          $scope.view.categoryLabel = program.ownerOfficeCategoryLabel;
          $scope.view.objectiveLabel = program.ownerOfficeObjectiveLabel;
          setSelectedCategories();
          setSelectedGoals();
          setSelectedObjectives();
          //setSelectedPointsOfContact();
          setSelectedThemes();
          $scope.view.isLoadingRequiredData = true;
          var officeId = program.ownerOrganizationId;
          $q.all([
                loadPermissions(),
                loadGoals(),
                loadThemes(),
                loadCategories(officeId, program.categories),
                loadObjectives(officeId),
                loadOfficeSettings(program.ownerOrganizationId),
                loadProgramStatii()])
          .then(function (results) {
              $scope.view.isLoadingRequiredData = false;
          })
          .catch(function () {
              var message = "Unable to load required data.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
              $scope.view.isLoadingRequiredData = false;
          })
      })
      .catch(function (response) {
          $scope.view.isLoadingProgram = false;
      });

  });
