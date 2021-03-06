﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddProgramModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $filter,
        $modalInstance,
        MessageBox,
        office,
        parentProgram,
        ProgramService,
        FilterService,
        LookupService,
        OfficeService,
        ConstantsService,
        NotificationService,
        DataPointConfigurationService) {

      var officeId = office.id;
      $scope.view = {};
      $scope.view.maxDescriptionLength = 3000;
      $scope.view.maxNameLength = 255;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingProgram = false;
      $scope.view.isLoadingPrograms = false;
      $scope.view.isLoadingLikePrograms = false;
      $scope.view.matchingProgramsByName = [];
      $scope.view.doesProgramExist = false;
      $scope.view.showConfirmCancel = false;
      $scope.view.themes = [];
      $scope.view.goals = [];
      $scope.view.themes = [];
      $scope.view.contacts = [];
      $scope.view.regions = [];
      $scope.view.program = {
          name: '',
          description: '',
          contacts: [],
          themes: [],
          goals: [],
          regions: [],
          objectives: [],
          categories: [],
          startDate: new Date(),
          ownerOrganizationId: officeId,
          ownerOrganizationName: office.name,
          parentProgramId: parentProgram ? parentProgram.id : null,
          parentProgramName: parentProgram ? parentProgram.name : null,
          isSubProgram: parentProgram ? true : false,
          websites: [{ value: undefined }]
      };
      $scope.view.selectedContacts = [];
      $scope.view.selectedRegions = [];
      $scope.view.selectedThemes = [];
      $scope.view.selectedCategories = [];
      $scope.view.selectedGoals = [];
      $scope.view.selectedObjectives = [];
      $scope.view.isStartDateCalendarOpen = false;
      $scope.view.categoryLabel = '';
      $scope.view.objectiveLabel = '';
      $scope.view.showCategoryFocus = false;
      $scope.view.showObjectiveJustification = false;
      $scope.view.isObjectivedRequired = false;
      $scope.view.isCategoryRequired = false;
      $scope.view.minimumRequiredFoci = -1;
      $scope.view.maximumRequiredFoci = -1;

      $scope.view.isSelectedCategoriesValid = true;
      $scope.view.isSelectedRegionsValid = false;
      $scope.view.isSelectedThemesValid = false;
      $scope.view.isSelectedGoalsValid = false;
      $scope.view.isSelectedContactsValid = false;
      $scope.view.isSelectedObjectivesValid = false;

      $scope.view.dataPointConfigurations = {};

      var maxLimit = 300;

      if (parentProgram) {
          $scope.view.title = 'Add Sub-Program to ' + parentProgram.name;
      }
      else {
          $scope.view.title = 'Add Program to the ' + office.name + ' (Office)';
      }

      $scope.view.onSelectParentProgramBlur = function ($event) {
          if ($scope.view.program.parentProgram === '') {
              $scope.view.program.parentProgramId = null;
          }
      }

      $scope.view.onSelectParentProgram = function ($item, $model, $label) {
          $scope.view.program.parentProgramId = $item.programId;
      }

      $scope.view.searchPointsOfContact = function (search) {
          return loadContacts(search);
      }

      $scope.view.searchPrograms = function (search) {
          return loadPrograms(search, $scope.view.isLoadingPrograms);
      }

      $scope.view.onSaveClick = function () {
          saveProgram();
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.programForm.$dirty) {
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

      $scope.view.openStartDateCalendar = function (event) {
          event.preventDefault();
          event.stopPropagation();
          $scope.view.isStartDateCalendarOpen = true;
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

      $scope.view.onObjectivesChange = function () {
          $scope.view.onObjectivesSelect();
      }

      $scope.view.onObjectivesSelect = function () {
          $scope.view.isSelectedObjectivesValid = true;
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

      $scope.view.onCategoriesChange = function () {
          $scope.view.onCategoriesSelect();
      }

      $scope.view.onCategoriesSelect = function () {
          $scope.view.isSelectedCategoriesValid = true;
      }

      $scope.view.onContactsChange = function () {
          $scope.view.onContactsSelect();
      }


      $scope.view.onContactsSelect = function () {
          if ($scope.view.selectedContacts.length > 0) {
              $scope.view.isSelectedContactsValid = true;
          }
          else {
              $scope.view.isSelectedContactsValid = false;
          }
      }

      var programsWithSameNameFilter = FilterService.add('programmodal_programswithsamename');
      $scope.view.onProgramNameChange = function () {
          var programName = $scope.view.program.name;
          if (programName && programName.length > 0) {
              programsWithSameNameFilter.reset();
              programsWithSameNameFilter = programsWithSameNameFilter.skip(0).take(1).equal('name', programName);
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

      $scope.view.addWebsite = function () {
          $scope.view.program.websites.push({ value: undefined });
      }

      $scope.view.deleteWebsite = function ($index) {
          $scope.view.program.websites.splice($index, 1);
      }

      var programsFilter = FilterService.add('programmodal_loadprograms');
      function loadPrograms(search, loadingIndicator) {
          loadingIndicator = true;
          programsFilter.reset();
          programsFilter = programsFilter.skip(0).take(25);
          if (search && search.length > 0) {
              programsFilter = programsFilter.like('name', search);
          }
          return ProgramService.getAllProgramsAlpha(programsFilter.toParams())
          .then(function (data) {
              loadingIndicator = false;
              return data.results;
          })
          .catch(function (response) {
              loadingIndicator = false;
              var message = "Unable to load programs.";
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

      function loadCategories(officeId) {
          return OfficeService.getCategories(officeId, { limit: maxLimit })
            .then(function (response) {
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

      var themesFilter = FilterService.add('programmodal_themes');
      function loadThemes() {
          themesFilter.reset();
          themesFilter = themesFilter.skip(0).take(maxLimit).isTrue('isActive');
          return LookupService.getAllThemes(themesFilter.toParams())
            .then(function (data) {
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

      var goalsFilter = FilterService.add('programmodal_goals');
      function loadGoals() {
          goalsFilter.reset();
          goalsFilter = goalsFilter.skip(0).take(maxLimit).isTrue('isActive');
          return LookupService.getAllGoals(goalsFilter.toParams())
            .then(function (data) {
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

      var contactsFilter = FilterService.add('programmodal_contacts');
      function loadContacts(search) {
          contactsFilter.reset();
          contactsFilter = contactsFilter.skip(0).take(maxLimit);
          if (search && search.length > 0) {
              contactsFilter = contactsFilter.like('fullName', search);
          }
          return LookupService.getAllContacts(contactsFilter.toParams())
            .then(function (data) {
                $scope.view.pointsOfContact = data.results;
                if (data.total > maxLimit) {
                    NotificationService.showWarningMessage("There are more contacts than can be loaded in one time, some categories may be available.");
                }
            })
          .catch(function () {
              var message = "Unable to load contacts.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var regionFilter = FilterService.add('programmodal_regionfilter');
      function loadRegions() {
          regionFilter.reset();
          regionFilter = regionFilter.skip(0).take(maxLimit).isTrue('isActive').equal('locationTypeId', ConstantsService.locationType.region.id);
          return LookupService.getAllRegions(regionFilter.toParams())
            .then(function (data) {
                $scope.view.regions = data.results;
            })
          .catch(function () {
              var message = "Unable to load goals.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function saveProgram() {
          $scope.view.isSavingProgram = true;
          setIds('regions', $scope.view.selectedRegions, 'id');
          setIds('themes', $scope.view.selectedThemes, 'id');
          setIds('goals', $scope.view.selectedGoals, 'id');
          setIds('categories', $scope.view.selectedCategories, 'id');
          setIds('contacts', $scope.view.selectedContacts, 'id');
          setIds('objectives', $scope.view.selectedObjectives, 'id');

          // Remove undefined elements and empty strings
          $scope.view.program.websites = $scope.view.program.websites.filter(function (n) { return (n.value && n.value.length > 0) });

          return ProgramService.create($scope.view.program)
          .then(function (response) {
              $scope.view.isSavingProgram = false;
              $modalInstance.close(response.data);
          })
          .catch(function (response) {
              $scope.view.isSavingProgram = false;
              var message = 'Unable to save program.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function setIds(arrayPropertyName, values, idPropertyName) {
          console.assert($scope.view.program[arrayPropertyName], "The program must have a property named " + arrayPropertyName);
          console.assert(Array.isArray($scope.view.program[arrayPropertyName]), 'The program property ' + arrayPropertyName + ' must be an array.');
          $scope.view.program[arrayPropertyName] = [];
          angular.forEach(values, function (v, index) {
              console.assert(v[idPropertyName], "The array item must have a property named " + idPropertyName);
              $scope.view.program[arrayPropertyName].push(v[idPropertyName]);
          });
      }

      function loadDataPointConfigurations() {
          var params = { officeId: officeId };
          DataPointConfigurationService.getDataPointConfigurations(params)
               .then(function (response) {
                   var array = $filter('filter')(response.data, { categoryId: ConstantsService.dataPointCategory.program.id });
                   for (var i = 0; i < array.length; i++) {
                       $scope.view.dataPointConfigurations[array[i].propertyId] = array[i].isRequired;
                   }
               }, function () {
                   NotificationService.showErrorMessage('Unable to load data point configurations for office with id = ' + parameters.foreignResourceId + ".");
               });
      }

      $scope.view.isLoadingRequiredData = true;
      $q.all([loadThemes(),
          loadGoals(),
          loadCategories(officeId),
          loadOfficeSettings(officeId),
          loadRegions(),
          loadContacts(),
          loadObjectives(officeId),
          loadDataPointConfigurations()])
          .then(function () {
              $log.info('Loaded required data.');
              $scope.view.isLoadingRequiredData = false;
          })
      .catch(function () {
          var message = "Unable to load required data.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isLoadingRequiredData = false;
      });

  });
