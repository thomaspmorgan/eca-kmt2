'use strict';

angular.module('staticApp')
  .controller('OfficeEditCtrl', function ($scope, $q, $log, FilterService, LookupService, OfficeService) {

      $scope.view = {};
      $scope.view.selectedThemes = [];
      $scope.view.selectedGoals = [];
      $scope.view.selectedPointsOfContact = [];
      $scope.view.selectedParentOffice = {};

      $scope.data.loadedOfficePromise.promise
        .then(function (office) {
            $scope.view.office = angular.copy(office);
            $q.all([
                loadGoals(),
                loadThemes()])
          .then(function (results) {
              setAllUiSelectValues();
          })
        });

      $scope.view.searchPointsOfContact = function (search) {
          return loadPointsOfContact(search);
      }

      $scope.view.searchParentOffices = function (search) {
          return loadParentOffices(search);
      }
      
      $scope.view.saveOffice = function () {
          console.log($scope.view.office);
      }

      $scope.$watch('form.officeForm.$invalid', function (isInvalid) {
          $scope.$parent.isEditButtonEnabled = isInvalid;
      });

      $scope.$on('saveOffice', function () {
          $scope.view.saveOffice();
      });

      function setAllUiSelectValues() {
          setSelectedGoals();
          setSelectedThemes();
          setSelectedPointsOfContact();
          setSelectedParentOffice();
      }

      function setSelectedGoals() {
          normalizeLookupProperties($scope.office.goals);
          setSelectedItems('goals', 'selectedGoals');
      }

      function setSelectedThemes() {
          normalizeLookupProperties($scope.office.themes);
          setSelectedItems('themes', 'selectedThemes');
      }

      function setSelectedPointsOfContact() {
          setSelectedItems('contacts', 'selectedPointsOfContact');
      }

      function setSelectedParentOffice() {
          $scope.view.selectedParentOffice = {
              organizationId: $scope.view.office.parentOfficeId,
              name: $scope.view.office.parentOfficeName
          };
      }

      function setSelectedItems(propertyName, selectedPropertyName) {
          console.assert($scope.view.office.hasOwnProperty(propertyName), "The project property " + propertyName + " does not exist.");
          console.assert($scope.view.hasOwnProperty(selectedPropertyName), "The view " + selectedPropertyName + " property does not exist.");
          console.assert(Array.isArray($scope.view[selectedPropertyName]), "The view " + selectedPropertyName + " property must be an array.");

          var officeItems = $scope.view.office[propertyName];
          $scope.view[selectedPropertyName] = [];
          for (var i = 0; i < officeItems.length; i++) {
              var officeItem = officeItems[i];
              $scope.view[selectedPropertyName].push(officeItem);
          }
      }

      var maxLimit = 300;
      var pocFilter = FilterService.add('officeedit_pocfilter');

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

      var maxLimit = 300;

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

      var pocFilter = FilterService.add('officeedit_pocfilter');
      function loadPointsOfContact(search) {
          pocFilter.reset();
          pocFilter = pocFilter.skip(0).take(maxLimit);
          if (search) {
              pocFilter = pocFilter.like('fullName', search);
              pocFilter = pocFilter.notIn('id', $scope.selectedPointsOfContact.map(function (obj) { return obj.id }));
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

      var parentOfficeFilter = FilterService.add('officeedit_parentofficefilter');
      function loadParentOffices(search) {
          parentOfficeFilter.reset();
          parentOfficeFilter = parentOfficeFilter.skip(0).take(maxLimit);
          if (search) {
              parentOfficeFilter = parentOfficeFilter.like('name', search);
              parentOfficeFilter = parentOfficeFilter.notIn('organizationId', [$scope.view.office.id]);
              if ($scope.view.selectedParentOffice.organizationId) {
                  parentOfficeFilter = parentOfficeFilter.notIn('organizationId', [$scope.view.selectedParentOffice.organizationId]);
              }
          }
          OfficeService.getAll(parentOfficeFilter.toParams())
            .then(function (response) {
                $scope.view.parentOffices = response.data.results;
            });
      }
  });
