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
        $modal,
        $filter,
        MessageBox,
        LocationService,
        FilterService,
        NotificationService,
        LookupService,
        ConstantsService,
        NavigationService,
        StateService,
        AuthService,
        OfficeService,
        BrowserService,
        ProgramService,
        orderByFilter
      ) {

      console.assert($scope.$parent.view.isEditButtonEnabled !== undefined, 'The parent should have a flag to determine the enabled state of the edit button.');
      var programId = parseInt($stateParams.programId, 10);
      $scope.view = {};
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
      $scope.view.parentPrograms = [];
      $scope.view.isLoadingProgram = true;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSaving = false;
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
      $scope.view.originalProgram = null;

      $scope.view.dataPointConfigurations = {};

      var programsWithSameNameFilter = FilterService.add('programedit_programswithsamename');
      $scope.view.validateUniqueProgramName = function ($value) {
          var dfd = $q.defer();
          if ($value && $value.length > 0) {
              programsWithSameNameFilter.reset();
              programsWithSameNameFilter = programsWithSameNameFilter.skip(0).take(1).equal('name', $value).notEqual('programId', programId);
              $scope.view.isLoadingLikePrograms = true;
              ProgramService.getAllProgramsAlpha(programsWithSameNameFilter.toParams())
                  .then(function (response) {
                      $scope.view.matchingProgramsByName = response.data;
                      $scope.view.isLoadingLikePrograms = false;
                      $scope.view.doesProgramExist = response.total > 0;
                      if ($scope.view.doesProgramExist) {
                          dfd.reject();
                      }
                      else {
                          dfd.resolve();
                      }
                  })
                  .catch(function (response) {
                      $scope.view.isLoadingLikePrograms = false;
                      var message = "Unable to load matching programs.";
                      $log.error(message);
                      NotificationService.showErrorMessage(message);
                      return false;
                  });
          }
          else {
              dfd.resolve();
          }
          return dfd.promise;
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

      $scope.view.searchPointsOfContact = function (search) {
          return loadPointsOfContact(search);
      }

      $scope.$on(ConstantsService.saveProgramEventName, function () {
          return saveProgram();
      })

      $scope.$on(ConstantsService.cancelProgramChangesEventName, function () {
          return doCancel();
      })

      $scope.view.onSaveClick = function () {
          return saveProgram();
      }

      $scope.view.onCancelClick = function () {
          doCancel();
      }

      $scope.view.validateMinimumGoals = function ($value) {
          if (!$value) {
              return false;
          }
          else {
              return $value.length > 0;
          }
      }

      $scope.view.validateMinimumThemes = function ($value) {
          if (!$value) {
              return false;
          }
          else {
              return $value.length > 0;
          }
      }

      $scope.view.validateMinimumPointsOfContact = function ($value) {
          if (!$value) {
              return false;
          }
          else {
              return $value.length > 0;
          }
      }

      $scope.view.validateMinimumRegions = function ($value) {
          if (!$value) {
              return false;
          }
          else {
              return $value.length > 0;
          }
      }

      var searchParentProgramsFilter = FilterService.add('editprogram_searchparentprograms');
      $scope.view.searchParentPrograms = function ($search) {
          searchParentProgramsFilter.reset();
          searchParentProgramsFilter = searchParentProgramsFilter
            .skip(0)
            .take(25);
          if ($search && $search.length > 0) {
              searchParentProgramsFilter = searchParentProgramsFilter.like('name', $search);
          }
          return ProgramService.getValidParentPrograms(programId, searchParentProgramsFilter.toParams())
          .then(function (response) {
              $scope.view.parentPrograms = response.data.results;
              return response.data.results;
          })
          .catch(function (response) {
              var message = 'Unable to load available parent programs.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      $scope.view.addWebsite = function () {
          $scope.view.program.websites.push({ value: undefined });
      }

      $scope.view.deleteWebsite = function ($index) {
          $scope.view.program.websites.splice($index, 1);
      }

      $scope.view.onAddPointsOfContactClick = function () {
          var modalInstance = $modal.open({
              animation: true,
              backdrop: 'static',
              templateUrl: 'app/points-of-contact/points-of-contact-modal.html',
              controller: 'PointsOfContactModalCtrl',
              windowClass: 'full-screen-modal',
              resolve: {}
          });

          modalInstance.result.then(function (pointOfContact) {
              pointOfContact.value = pointOfContact.fullName;
              if (pointOfContact.position) {
                  pointOfContact.value += ' (' + pointOfContact.position + ')';
              }
              $scope.view.program.contacts.push(pointOfContact);
              $scope.view.selectedPointsOfContact.push(pointOfContact);
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      function doCancel() {
          if ($scope.form.programForm.$dirty) {
              MessageBox.confirm({
                  title: 'Unsaved Changes',
                  message: "There are unsaved changes to this program.  Are you sure you wish to cancel?",
                  okText: 'Yes, Cancel Changes',
                  cancelText: 'No',
                  okCallback: toOverview
              });
          }
          else {
              toOverview();
          }
      }

      function toOverview() {
          $scope.$parent.view.isEditButtonEnabled = true;
          $scope.view.program = $scope.view.originalProgram;
          setAllUiSelectValues();
          StateService.goToProgramState($scope.view.program.id, { reload: true });
      }

      function saveProgram() {
          $scope.view.isSaving = true;
          setIds('regions', $scope.view.selectedRegions, 'id');
          setIds('themes', $scope.view.selectedThemes, 'id');
          setIds('goals', $scope.view.selectedGoals, 'id');
          setIds('categories', $scope.view.selectedCategories, 'id');
          setIds('contacts', $scope.view.selectedContacts, 'id');
          setIds('objectives', $scope.view.selectedObjectives, 'id');
          setIds('contacts', $scope.view.selectedPointsOfContact, 'id');
          if ($scope.view.program.parentProgram) {
              $scope.view.program.parentProgramId = $scope.view.program.parentProgram.programId;
          }
          else {
              $scope.view.program.parentProgramId = null;
          }

          // Remove undefined elements and empty strings
          $scope.view.program.websites = $scope.view.program.websites.filter(function (n) { return (n.value && n.value.length > 0) });

          return ProgramService.update($scope.view.program)
          .then(function (response) {
              var updatedProgram = response.data;
              $scope.view.originalProgram = angular.copy(updatedProgram);
              $scope.view.program = updatedProgram;
              $scope.view.isSaving = false;
              NavigationService.updateBreadcrumbs();
              return StateService.goToProgramState($scope.view.program.id, { reload: true });
          })
          .catch(function (response) {
              $scope.view.isSaving = false;
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

      var maxLimit = 300;
      function loadPermissions() {
          console.assert(ConstantsService.resourceType.program.value, 'The constants service must have the program resource type value.');
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
            .isTrue('isActive')
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

      function setSelectedParentProgram() {
          $scope.view.program.parentProgram = {
              programId: $scope.view.program.parentProgramId,
              name: $scope.view.program.parentProgramName
          };
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

      function setAllUiSelectValues() {
          setSelectedCategories();
          setSelectedGoals();
          setSelectedObjectives();
          setSelectedRegions();
          setSelectedPointsOfContact();
          setSelectedThemes();
          setSelectedParentProgram();
      }

      function onFormValidStateChange() {
          var isInvalid = $scope.form.programForm.$invalid;
          if (isInvalid) {
              $scope.$parent.view.isEditButtonEnabled = false;
          }
          else {
              $scope.$parent.view.isEditButtonEnabled = true;
          }
      }

      $scope.$parent.view.isEditButtonEnabled = false;
      $scope.data.loadProgramPromise.promise
      .then(function (program) {
          BrowserService.setDocumentTitleByProgram(program, 'Edit');
          $scope.view.isLoadingProgram = false;
          $scope.view.program = program;
          if ($scope.view.program.websites.length === 0) {
              $scope.view.program.websites.push({ value: undefined });
          }
          $scope.view.categoryLabel = program.ownerOfficeCategoryLabel;
          $scope.view.objectiveLabel = program.ownerOfficeObjectiveLabel;

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
                $scope.view.searchParentPrograms(null),
                loadProgramStatii()])
          .then(function (results) {
              setAllUiSelectValues();
              $scope.view.originalProgram = angular.copy(program);
              $scope.view.isLoadingRequiredData = false;
              $scope.$watch(function () {
                  return $scope.form.programForm.$invalid;
              }, onFormValidStateChange);
              onFormValidStateChange();
          })
          .catch(function () {
              var message = "Unable to load required data.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
              $scope.view.isLoadingRequiredData = false;
          });
      })
      .catch(function (response) {
          $scope.view.isLoadingProgram = false;
      });

      $scope.data.loadDataPointConfigurationsPromise.promise
      .then(function (dataConfigurations) {
          var array = $filter('filter')(dataConfigurations, { categoryId: ConstantsService.dataPointCategory.program.id });
          for (var i = 0; i < array.length; i++) {
              $scope.view.dataPointConfigurations[array[i].propertyId] = array[i].isRequired;
          }
      });
  });
