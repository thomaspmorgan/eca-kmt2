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
      LocationService,
      FilterService,
      NotificationService,
      LookupService,
      ConstantsService,
      StateService,
      AuthService,
      OfficeService,
      ProgramService,
      orderByFilter

      ) {

      $scope.view = {};
      $scope.view.isSelectedCategoriesValid = true;
      $scope.view.isSelectedRegionsValid = false;
      $scope.view.isSelectedThemesValid = false;
      $scope.view.isSelectedGoalsValid = false;
      $scope.view.isSelectedContactsValid = false;
      $scope.view.isSelectedObjectivesValid = false;
      $scope.view.isObjectivesRequired = false;
      $scope.view.isCategoryRequired = false;
      $scope.view.minimumRequiredFoci = -1;
      $scope.view.maximumRequiredFoci = -1;
      $scope.view.maxDescriptionLength = 3000;
      $scope.view.maxNameLength = 255;

      $scope.view.programStatii = [];
      $scope.view.goals = [];
      $scope.view.themes = [];
      $scope.view.regions = [];
      $scope.view.selectedRegions = [];
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
      $scope.view.isLoadingLikePrograms = false;
      $scope.view.doesProgramExist = false;

      $scope.view.selectedThemes = [];
      $scope.view.selectedGoals = [];
      $scope.view.selectedCategories = [];
      $scope.view.selectedObjectives = [];
      $scope.view.selectedPointsOfContact = [];

      var programsWithSameNameFilter = FilterService.add('programedit_programswithsamename');
      $scope.view.onProgramNameChange = function () {
          var programName = $scope.view.program.name;
          var programId = $scope.view.program.id;
          if (programName && programName.length > 0) {
              programsWithSameNameFilter.reset();
              programsWithSameNameFilter = programsWithSameNameFilter.skip(0).take(1).equal('name', programName).notEqual('programId', programId);
              $scope.view.isLoadingLikePrograms = true;
              return ProgramService.getAllProgramsAlpha(programsWithSameNameFilter.toParams())
                  .then(function (response) {
                      $scope.view.matchingProgramsByName = response.data;
                      $scope.view.doesProgramExist = response.total > 0;
                      $scope.view.isLoadingLikePrograms = false;
                  })
                  .catch(function (response) {
                      $scope.view.isLoadingLikePrograms = false;
                      var message = "Unable to load matching programs.";
                      $log.error(message);
                      NotificationService.showErrorMessage(message);
                  });
          }
      }

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

      $scope.view.searchPointsOfContact = function(search){
          return loadPointsOfContact(search);
      }

      $scope.view.onObjectivesChange = function () {
          $scope.view.onObjectivesSelect();
      }

      $scope.view.onObjectivesSelect = function () {
          if ($scope.view.selectedObjectives.length > 0) {
              $scope.view.isSelectedObjectivesValid = true;
          }
          else {
              $scope.view.isSelectedObjectivesValid = false;
          }
      }

      $scope.view.onCategoriesChange = function () {
          $scope.view.onCategoriesSelect();
      }

      $scope.view.onCategoriesSelect = function () {
          $scope.view.isSelectedCategoriesValid = true;
          if ($scope.view.selectedCategories.length < $scope.view.minimumRequiredFoci) {
              $scope.view.isSelectedCategoriesValid = false;
          }
          if ($scope.view.selectedCategories.length > $scope.view.maximumRequiredFoci) {
              $scope.view.isSelectedCategoriesValid = false;
          }
      }

      $scope.view.onRegionsChange = function () {
          $scope.view.onRegionsSelect();
      }

      $scope.view.onRegionsSelect = function () {
          if ($scope.view.selectedRegions.length > 0) {
              $scope.view.isSelectedRegionsValid = true;
          }
          else {
              $scope.view.isSelectedRegionsValid = false;
          }
      }

      $scope.view.onThemesChange = function () {
          $scope.view.onThemesSelect();
      }

      $scope.view.onThemesSelect = function () {
          if ($scope.view.selectedThemes.length > 0) {
              $scope.view.isSelectedThemesValid = true;
          }
          else {
              $scope.view.isSelectedThemesValid = false;
          }
      }

      $scope.view.onGoalsChange = function () {
          $scope.view.onGoalsSelect();
      }

      $scope.view.onGoalsSelect = function () {
          if ($scope.view.selectedGoals.length > 0) {
              $scope.view.isSelectedGoalsValid = true;
          }
          else {
              $scope.view.isSelectedGoalsValid = false;
          }
      }

      $scope.view.onContactsChange = function () {
          $scope.view.onContactsSelect();
      }

      $scope.view.onContactsSelect = function () {
          if ($scope.view.selectedPointsOfContact.length > 0) {
              $scope.view.isSelectedContactsValid = true;
          }
          else {
              $scope.view.isSelectedContactsValid = false;
          }
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
          categoriesFilter = categoriesFilter.skip(0).take(maxLimit);
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

      var pocFilter = FilterService.add('programedit_pocfilter');
      function loadPointsOfContact(search) {
          pocFilter.reset();
          pocFilter = pocFilter.skip(0).take(maxLimit);
          if (search) {
              pocFilter = pocFilter.like('fullName', search);
          }
          return LookupService.getAllContacts(pocFilter.toParams())
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
                  $scope.view.pointsOfContact = response.results;
              });
      }

      var regionsFilter = FilterService.add('programedit_regions');
      function loadRegions() {
          regionsFilter.reset();
          regionsFilter = regionsFilter.skip(0).take(10)
            .equal('locationTypeId', ConstantsService.locationType.region.id)
            .sortBy('name');
          
          return LocationService.get(regionsFilter.toParams())
              .then(function (response) {
                  normalizeLookupProperties(response.results);
                  $scope.view.regions = response.results;
              })
              .catch(function () {
                  $log.error('Unable to load locations.');
                  NotificationService.showErrorMessage('Unable to load locations.');
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

      function setSelectedRegions() {
          var regionsName = 'regions';
          normalizeLookupProperties($scope.view.program[regionsName]);
          setSelectedItems(regionsName, 'selectedRegions');
      }

      $scope.data.loadProgramPromise.promise
      .then(function (program) {
          $scope.view.isLoadingProgram = false;
          $scope.view.program = program;
          $scope.view.categoryLabel = program.ownerOfficeCategoryLabel;
          $scope.view.objectiveLabel = program.ownerOfficeObjectiveLabel;
          setSelectedCategories();
          setSelectedGoals();
          setSelectedObjectives();
          setSelectedRegions();
          setSelectedPointsOfContact();
          setSelectedThemes();          
          $scope.view.isLoadingRequiredData = true;
          var officeId = program.ownerOrganizationId;
          $q.all([
                loadPermissions(),
                loadGoals(),
                loadThemes(),
                loadRegions(),
                loadCategories(officeId, program.categories),
                loadObjectives(officeId),
                loadOfficeSettings(program.ownerOrganizationId),
                loadProgramStatii()])
          .then(function (results) {
              $scope.view.onThemesChange();
              $scope.view.onGoalsChange();
              $scope.view.onContactsChange();
              $scope.view.onObjectivesChange();
              $scope.view.onRegionsChange();
              $scope.view.onCategoriesChange();
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
