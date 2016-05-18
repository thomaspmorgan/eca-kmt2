'use strict';

angular.module('staticApp')
  .controller('OfficeEditCtrl', function ($scope, $state, $stateParams, $q, $log, $modal, FilterService, NavigationService, LookupService, OfficeService, StateService, NotificationService) {

      $scope.view = {};
      $scope.view.selectedThemes = [];
      $scope.view.selectedGoals = [];
      $scope.view.selectedPointsOfContact = [];
      $scope.view.selectedParentOffice = {};

      $scope.data.loadedOfficePromise.promise
        .then(function (office) {
            $scope.view.office = angular.copy(office);
            setAllUiSelectValues();
            loadParentOffices('');
            loadPointsOfContact('');
        });

      $scope.view.searchPointsOfContact = function (search) {
          return loadPointsOfContact(search);
      }

      $scope.view.searchParentOffices = function (search) {
          return loadParentOffices(search);
      }
      
      $scope.view.saveOffice = function () {
          var office = {
              officeId: $scope.view.office.id,
              name: $scope.view.office.name,
              officeSymbol: $scope.view.office.officeSymbol,
              description: $scope.view.office.description,
          };

          if ($scope.view.selectedPointsOfContact) {

              office.pointsOfContactIds = $scope.view.selectedPointsOfContact.map(function (obj) { return obj.id })
          }

          if ($scope.view.selectedParentOffice) {
              office.parentOfficeId = $scope.view.selectedParentOffice.organizationId;
          }

          OfficeService.update(office)
            .then(function () {
                NavigationService.updateBreadcrumbs();
                StateService.goToOfficeState(office.officeId, { reload: true });
            }, function () {
                NotificationService.showErrorMessage('Unable to save office.');
            });
      }

      var overviewStateName = StateService.stateNames.overview.office;
      $scope.view.cancelSaveOffice = function () {
          if ($scope.form.officeForm.$dirty) {
              MessageBox.confirm({
                  title: 'Unsaved Changes',
                  message: 'You have changes that have not been saved.  Are you sure you want to cancel?',
                  okText: 'Yes',
                  cancelText: 'No',
                  okCallback: function () {
                      $log.info('Cancelling changes...');
                      goToOfficeOverview();
                  },
                  onCancelClick: function () {
                      $log.info('Dismiss warning dialog and allow save changes...');
                  }
              });
          }
          else {
              goToOfficeOverview();
          }
      }

      function goToOfficeOverview() {
          $scope.$parent.isInEditState = false;
          $state.go(overviewStateName);
      }

      var officesWithSameNameFilter = FilterService.add('officeedit_officeswithsamename');
      $scope.view.validateUniqueOfficeName = function ($value) {
          var deferred = $q.defer();
          if ($value && $value.length > 0) {
              officesWithSameNameFilter.reset();
              officesWithSameNameFilter = officesWithSameNameFilter.skip(0).take(1).equal('name', $value).notEqual('organizationId', parseInt($stateParams.officeId));
              OfficeService.getAll(officesWithSameNameFilter.toParams())
                .then(function (response) {
                    if (response.data.total > 0) {
                        deferred.reject();
                    } else {
                        deferred.resolve();
                    }
                });
          } else {
              deferred.resolve();
          }
          return deferred.promise;
      }

      var officesWithSameOfficeSymbolFilter = FilterService.add('officeedit_officeswithsameofficesymbol');
      $scope.view.validateUniqueOfficeSymbol = function ($value) {
          var deferred = $q.defer();
          if ($value && $value.length > 0) {
              officesWithSameOfficeSymbolFilter.reset();
              officesWithSameOfficeSymbolFilter = officesWithSameOfficeSymbolFilter.skip(0).take(1).equal('officeSymbol', $value).notEqual('organizationId', parseInt($stateParams.officeId));
              OfficeService.getAll(officesWithSameOfficeSymbolFilter.toParams())
                .then(function (response) {
                    if (response.data.total > 0) {
                        deferred.reject();
                    } else {
                        deferred.resolve();
                    }
                });
          } else {
              deferred.resolve();
          }
          return deferred.promise;
      }

      $scope.$watch('form.officeForm.$invalid', function (isInvalid) {
          $scope.$parent.isEditButtonEnabled = isInvalid;
      });

      $scope.$on('saveOffice', function () {
          $scope.view.saveOffice();
      });

      $scope.$on('cancelSaveOffice', function () {
          $scope.view.cancelSaveOffice();
      })

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
              $scope.view.office.contacts.push(pointOfContact);
              $scope.view.selectedPointsOfContact.push(pointOfContact);
              setSelectedPointsOfContact();
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      function setAllUiSelectValues() {
          setSelectedPointsOfContact();
          setSelectedParentOffice();
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

      var pocFilter = FilterService.add('officeedit_pocfilter');
      function loadPointsOfContact(search) {
          pocFilter.reset();
          pocFilter = pocFilter.skip(0).take(maxLimit);
          if ($scope.view.selectedPointsOfContact && $scope.view.selectedPointsOfContact.length > 0) {
              pocFilter = pocFilter.notIn('id', $scope.view.selectedPointsOfContact.map(function (obj) { return obj.id }));
          }
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

      var parentOfficeFilter = FilterService.add('officeedit_parentofficefilter');
      function loadParentOffices(search) {
          parentOfficeFilter.reset();
          parentOfficeFilter = parentOfficeFilter.skip(0).take(maxLimit);
          if ($scope.view.office) {
              parentOfficeFilter = parentOfficeFilter.notEqual('organizationId', $scope.view.office.id);
              if ($scope.view.selectedParentOffice && $scope.view.selectedParentOffice.organizationId) {
                  parentOfficeFilter = parentOfficeFilter.notEqual('organizationId', $scope.view.selectedParentOffice.organizationId);
              }
          }
          if (search) {
              parentOfficeFilter = parentOfficeFilter.like('name', search);
          }
          OfficeService.getAll(parentOfficeFilter.toParams())
            .then(function (response) {
                $scope.view.parentOffices = response.data.results;
            });
      }
  });
