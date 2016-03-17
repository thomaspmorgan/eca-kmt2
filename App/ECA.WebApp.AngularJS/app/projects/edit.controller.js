'use strict';

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
        $filter,
        smoothScroll,
        BrowserService,
        ProjectService,
        ProgramService,
        TableService,
        LocationService,
        LookupService,
        ConstantsService,
        AuthService,
        OfficeService,
        FilterService,
        StateService,
        NotificationService) {

      console.assert(typeof ($scope.$parent.isInEditViewState) !== 'undefined', 'The isInEditViewState property on the parent scope must be defined.');
      $scope.$parent.isInEditViewState = true;
      console.assert($scope.$parent.showProjectEditCancelButton, 'The $scope.$parent.showProjectEditCancelButton function must be defined.');
      console.assert($scope.$parent.hideProjectEditCancelButton, 'The $scope.$parent.hideProjectEditCancelButton function must be defined.');


      $scope.editView = {};
      $scope.editView.maxNameLength = 500;
      $scope.editView.maxDescriptionLength = 3000;
      $scope.editView.params = $stateParams;
      $scope.editView.isLoading = false;
      $scope.editView.isSaving = false;
      $scope.editView.dateFormat = 'dd-MMMM-yyyy';
      $scope.editView.isStartDatePickerOpen = false;
      $scope.editView.isEndDatePickerOpen = false;
      $scope.editView.showCategoryFocus = true;
      $scope.editView.showObjectiveJustification = true;
      $scope.editView.isCategoryRequired = false;
      $scope.editView.isObjectivesRequired = false;

      $scope.editView.foci = [];
      $scope.editView.projectStati = [];
      $scope.editView.pointsOfContact = [];
      $scope.editView.themes = [];
      $scope.editView.goals = [];
      $scope.editView.categories = [];
      $scope.editView.objectives = [];
      $scope.editView.locations = [];
      $scope.editView.regions = [];
      $scope.editView.visitorTypes = [];

      $scope.editView.selectedPointsOfContact = [];
      $scope.editView.selectedGoals = [];
      $scope.editView.selectedThemes = [];
      $scope.editView.selectedCategories = [];
      $scope.editView.selectedObjectives = [];
      $scope.editView.selectedLocations = [];
      $scope.editView.selectedRegions = [];
      $scope.editView.selectedVisitorType = null;

      $scope.editView.categoryLabel = "...";
      $scope.editView.objectiveLabel = "...";
      $scope.editView.minimumRequiredFoci = -1;
      $scope.editView.maximumRequiredFoci = -1;
      $scope.editView.locationUiSelectId = 'selectLocations';
      $scope.editView.isLoadingOfficeSetting = false;

      $scope.editView.dataPointConfigurations = {};

      $scope.editView.validateMinimumObjectives = function ($value) {
          if (!$scope.editView.isObjectivesRequired) {
              return true;
          }
          if (!$value) {
              return false;
          }
          else {
              return $value.length > 0;
          }
      }

      $scope.editView.validateMinimumCategories = function ($value) {
          if (!$scope.editView.isCategoryRequired) {
              return true;
          }
          if (!$value) {
              return false;
          }
          if ($value.length < $scope.editView.minimumRequiredFoci) {
              return false;
          }
          return true;
      }

      $scope.editView.validateMaximumCategories = function ($value) {
          if (!$scope.editView.isCategoryRequired) {
              return true;
          }
          if (!$value) {
              return true;
          }
          if ($value.length > $scope.editView.maximumRequiredFoci) {
              return false;
          }
          return true;
      }

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

      $scope.editView.searchRegions = function (data) {
          loadRegions(data);
      }

      $scope.editView.removeLocations = function () {
          $scope.$parent.project.locationIds = [];
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

      $scope.editView.isLoadingOfficeSetting = true;
      $scope.$parent.data.loadOfficeSettingsPromise.promise
        .then(function (settings) {
            console.assert(settings.objectiveLabel, "The objective label must exist.");
            console.assert(settings.categoryLabel, "The category label must exist.");
            console.assert(settings.focusLabel, "The focus label must exist.");
            console.assert(settings.justificationLabel, "The justification label must exist.");
            console.assert(typeof (settings.isCategoryRequired) !== 'undefined', "The is category required bool must exist.");
            console.assert(typeof (settings.isObjectiveRequired) !== 'undefined', "The is objective required bool must exist.");

            var objectiveLabel = settings.objectiveLabel;
            var categoryLabel = settings.categoryLabel;
            var focusLabel = settings.focusLabel;
            var justificationLabel = settings.justificationLabel;
            var isCategoryRequired = settings.isCategoryRequired;
            var isObjectivesRequired = settings.isObjectiveRequired;

            $scope.editView.categoryLabel = categoryLabel + ' / ' + focusLabel;
            $scope.editView.objectiveLabel = objectiveLabel + ' / ' + justificationLabel;
            $scope.editView.showCategoryFocus = isCategoryRequired;
            $scope.editView.showObjectiveJustification = isObjectivesRequired;

            $scope.editView.minimumRequiredFoci = settings.minimumRequiredFoci;
            $scope.editView.maximumRequiredFoci = settings.maximumRequiredFoci;
            $scope.editView.isCategoryRequired = isCategoryRequired;
            $scope.editView.isObjectivesRequired = isObjectivesRequired;
            $scope.editView.isLoadingOfficeSetting = false;
        });

      $scope.data.loadDataPointConfigurationsPromise.promise
      .then(function (dataConfigurations) {
          var array = $filter('filter')(dataConfigurations, { categoryId: ConstantsService.dataPointCategory.project.id });
          for (var i = 0; i < array.length; i++) {
              $scope.editView.dataPointConfigurations[array[i].propertyId] = array[i].isRequired;
          }
      });

      $scope.$parent.data.loadDefaultExchangeVisitorFundingPromise.promise.then(function (defaultVisitorExchangeFunding) {
          $scope.editView.sevisFunding = defaultVisitorExchangeFunding;
      });

      $scope.editView.onAddPointsOfContactClick = function () {
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
              $scope.$parent.project.contacts.push(pointOfContact);
              $scope.editView.selectedPointsOfContact.push(pointOfContact);
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      $scope.editView.onAdvancedSearchClick = function () {
          var modalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/locations/search-locations.html',
              controller: 'SearchLocationsCtrl',
              windowClass: 'full-screen-modal',
              resolve: {}
          });

          modalInstance.result.then(function (selectedLocations) {
              $log.info('Finished searching locations.');
              scrollToLocations();
              normalizeLookupProperties(selectedLocations)
              angular.forEach(selectedLocations, function (selectedLocation, index) {
                  var addLocation = true;
                  angular.forEach($scope.editView.selectedLocations, function (l, jIndex) {
                      if (selectedLocation.id === l.id) {
                          addLocation = false;
                      }
                  });
                  if (addLocation) {
                      $scope.editView.selectedLocations.push(selectedLocation);
                  }
              });

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      function onFormValidStateChange() {
          var isInvalid = $scope.form.projectForm.$invalid;
          if (isInvalid) {
              $scope.$parent.isProjectStatusButtonEnabled = false;
          }
          else {
              $scope.$parent.isProjectStatusButtonEnabled = true;
          }
      }

      function scrollToLocations() {
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          }
          var element = document.getElementById($scope.editView.locationUiSelectId);
          smoothScroll(element, options);
      }

      function cancelEdit() {
          if ($scope.form.projectForm.$dirty) {
              var modalInstance = $modal.open({
                  templateUrl: '/app/projects/unsaved-changes.html',
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
          StateService.goToProjectState($stateParams.projectId, { reload: true })
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

      function updateRegions() {
          var propertyName = "regionIds";
          $scope.$parent.project[propertyName] = $scope.$parent.project[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedRegions');
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
          updateRegions();
          disableProjectStatusButton();
          $scope.$parent.hideProjectEditCancelButton();
          ProjectService.update($scope.$parent.project, $stateParams.projectId)
            .then(function (response) {
                $scope.$parent.project = response.data;
                showSaveSuccess();
                goToProjectOverview();
            }, function (error) {
                showProjectEditCancelButton();
                if (error.status === 400) {
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

      function loadVisitorTypes() {
          return LookupService.getVisitorTypes({ start: 0, limit: maxLimit })
          .then(function (response) {
              $scope.editView.visitorTypes = response.data.results;
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

      function setSelectedRegions() {
          var regionsName = 'regions';
          normalizeLookupProperties($scope.$parent.project[regionsName]);
          setSelectedItems(regionsName, 'selectedRegions');
      }

      function normalizeLookupProperties(lookups) {
          console.assert(Array.isArray(lookups), "The given value must be an array.");
          for (var i = 0; i < lookups.length; i++) {
              lookups[i].value = lookups[i].name;
          }
      }

      var pocFilter = FilterService.add('projectedit_pocfilter');
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
                  $scope.editView.pointsOfContact = response.results;
              });
      }

      var themesFilter = FilterService.add('projectedit_themesfilter');
      function loadThemes(search) {
          themesFilter.reset();
          themesFilter = themesFilter.skip(0).take(maxLimit);
          if (search) {
              themesFilter = themesFilter.like('name', search);
          }
          return LookupService.getAllThemes(themesFilter.toParams())
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more themes in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.results);
                  $scope.editView.themes = response.results;
              });
      }

      var goalsFilter = FilterService.add('projectedit_goalsfilter');
      function loadGoals(search) {
          goalsFilter.reset();
          goalsFilter = goalsFilter.skip(0).take(maxLimit);
          if (search) {
              goalsFilter = goalsFilter.like('name', search);
          }
          return LookupService.getAllGoals(goalsFilter.toParams())
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more goals in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  normalizeLookupProperties(response.results);
                  $scope.editView.goals = response.results;
              });
      }

      var categoriesFilter = FilterService.add('projectedit_categoriesfilter');
      function loadCategories(search) {
          categoriesFilter.reset();
          categoriesFilter = categoriesFilter.skip(0).take(maxLimit);
          if (search) {
              categoriesFilter = categoriesFilter.like('name', search);
          }
          var officeId = $scope.$parent.project.ownerId;
          return OfficeService.getCategories(officeId, categoriesFilter.toParams())
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

      var objectivesFilter = FilterService.add('projectedit_objectivesfilter');
      function loadObjectives(search) {
          objectivesFilter.reset();
          objectivesFilter = objectivesFilter.skip(0).take(maxLimit);
          if (search) {
              objectivesFilter = objectivesFilter.like('name', search);
          }
          var officeId = $scope.$parent.project.ownerId;
          return OfficeService.getObjectives(officeId, objectivesFilter.toParams())
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

      var locationsFilter = FilterService.add('projectedit_locations');
      function loadLocations(search) {
          locationsFilter.reset();
          locationsFilter = locationsFilter.skip(0).take(10)
            .notEqual('locationTypeId', ConstantsService.locationType.address.id)
            .isTrue('isActive')
            .isNotNull('name')
            .sortBy('name');
          if ($scope.editView.selectedLocations.length > 0) {
              locationsFilter = locationsFilter.notIn('id', $scope.editView.selectedLocations.map(function (l) { return l.id; }));
          }
          if (search) {
              locationsFilter = locationsFilter.like('name', search);
          }
          return LocationService.get(locationsFilter.toParams())
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

      var regionsFilter = FilterService.add('projectedit_regions');
      function loadRegions(search) {
          regionsFilter.reset();
          regionsFilter = regionsFilter.skip(0).take(10)
            .equal('locationTypeId', ConstantsService.locationType.region.id)
            .isTrue('isActive')
            .isNotNull('name')
            .sortBy('name');
          if ($scope.editView.selectedRegions.length > 0) {
              regionsFilter = regionsFilter.notIn('id', $scope.editView.selectedRegions.map(function (l) { return l.id; }));
          }
          if (search) {
              regionsFilter = regionsFilter.like('name', search);
          }
          return LocationService.get(regionsFilter.toParams())
              .then(function (response) {
                  angular.forEach(response.results, function (result, index) {
                      if (!result.name) {
                          result.name = 'UNKNOWN';
                      }
                  });
                  normalizeLookupProperties(response.results);
                  $scope.editView.regions = response.results;
              })
              .catch(function () {
                  $log.error('Unable to load regions.');
                  NotificationService.showErrorMessage('Unable to load regions.');
              });
      }

      function loadUSGovernmentAgencies() {
          var usGovernmentAgenciesFilter = FilterService.add('editProject-usGovernmentAgencies');
          usGovernmentAgenciesFilter = usGovernmentAgenciesFilter.skip(0).take(maxLimit);
          return LookupService.getSevisUSGovernmentAgencies(usGovernmentAgenciesFilter.toParams())
          .then(function (response) {
              if (response.data.total > maxLimit) {
                  var message = "The number of USGovernmentAgencies loaded is less than the total number.  Some USGovernmentAgencies may not be shown."
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
              $scope.editView.usGovernmentAgencies = response.data.results;
              return $scope.editView.usGovernmentAgencies;
          })
          .catch(function (response) {
              var message = "Unable to load USGovernmentAgencies.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function loadInternationalOrganizations() {
          var internationalOrganizationsFilter = FilterService.add('editProject-internationalOrganizations');
          internationalOrganizationsFilter = internationalOrganizationsFilter.skip(0).take(maxLimit);
          return LookupService.getSevisInternationalOrganizations(internationalOrganizationsFilter.toParams())
          .then(function (response) {
              if (response.data.total > maxLimit) {
                  var message = "The number of InternationalOrganizations loaded is less than the total number.  Some InternationalOrganizations may not be shown."
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
              $scope.editView.internationalOrganizations = response.data.results;
              return $scope.editView.internationalOrganizations;
          })
          .catch(function (response) {
              var message = "Unable to load InternationalOrganizations.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
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
                  StateService.goToForbiddenState();
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {

            }, function () {
                $log.error('Unable to load user permissions.');
            });
      }

      $scope.editView.isLoading = true;
      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          BrowserService.setDocumentTitleByProject(project, 'Edit');
          if (project.visitorTypeId === ConstantsService.visitorType.exchangeVisitor.id) {
              loadUSGovernmentAgencies();
              loadInternationalOrganizations();
          }
          $q.all([loadPermissions(), loadThemes(null), loadPointsOfContact(null), loadObjectives(), loadCategories(), loadProjectStati(), loadVisitorTypes(), loadGoals(null)])
          .then(function (results) {
              //results is an array
              setSelectedPointsOfContact();
              setSelectedGoals();
              setSelectedThemes();
              setSelectedCategories();
              setSelectedObjectives();
              setSelectedLocations();
              setSelectedRegions();
              showProjectEditCancelButton();
              $scope.$watch(function () {
                  return $scope.form.projectForm.$invalid;
              }, onFormValidStateChange);
              onFormValidStateChange();
              $scope.editView.isLoading = false;
          })
          .catch(function () {
              $log.error('Unable to project edit data.');
              $scope.editView.isLoading = false;
          });
      })
      .catch(function () {
          $log.error('Failed to load project in edit.js project controller.');
          $scope.editView.isLoading = false;
      })
  });
