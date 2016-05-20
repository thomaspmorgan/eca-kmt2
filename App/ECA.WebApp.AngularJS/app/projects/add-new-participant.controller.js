'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddNewParticipantCtrl
 * @description The controller to add a new participant
 * # AddNewParticipantCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddNewParticipantCtrl', function ($q, $scope, $stateParams, $modalInstance, LookupService, LocationService, ConstantsService, PersonService, ProjectService, NotificationService, OrganizationService) {

      $scope.personTabActive = true;

      // Initialize model for person tab
      $scope.newPerson = {};
      $scope.newPerson.countriesOfCitizenship = [];
      $scope.newPerson.selectedDuplicate = undefined;
      $scope.newPerson.isDateOfBirthUnknown = false;
      $scope.newPerson.isPlaceOfBirthUnknown = false;
      $scope.newPerson.isSingleName = false;


      $scope.formLastName = "SURNAME / PRIMARY NAME";
      $scope.unknownCountry = 'Unknown';

      // Initialize model for organization tab
      $scope.newOrganization = {};
      $scope.newOrganization.organizationRoles = [];
      $scope.newOrganization.pointsOfContact = [];

      $scope.personDuplicates = [];
      $scope.organizationDuplicates = [];

      $scope.isDobDatePickerOpen = false;
      $scope.dateFormat = 'MM/dd/yyyy';
      $scope.maxDate = new Date();

      $scope.selectPersonTab = function () {
          if (!$scope.personTabActive) {
              // Reset person form
              $scope.newPerson = {};
              $scope.newPerson.countriesOfCitizenship = [];
              resetDuplicates();
              $scope.personTabActive = true;
          }
      }

      $scope.selectOrganizationTab = function () {
          if ($scope.personTabActive) {
              // Reset organization form
              $scope.newOrganization = {};
              $scope.newOrganization.organizationRoles = [];
              $scope.newOrganization.pointsOfContact = [];
              resetDuplicates();
              $scope.personTabActive = false;
          }
      }

      $scope.resetPersonDuplicates = function () {
          if ($scope.personDuplicates.length > 0) {
              $scope.personDuplicates = [];
          }
      }

      $scope.resetOrganizationDuplicates = function () {
          if ($scope.organizationDuplicates.length > 0) {
              $scope.organizationDuplicates = [];
          }
      }

      $scope.openDobDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.isDobDatePickerOpen = true;
      }

      $scope.$watch('newPerson.dateOfBirth', function () {
          var date = $scope.newPerson.dateOfBirth;
          if (date) {
              $scope.newPerson.isDateOfBirthUnknown = false;
          }
      });

      $scope.$watch('newPerson.cityOfBirth', function () {
          var city = $scope.newPerson.cityOfBirth;
          if (city) {
              $scope.newPerson.isPlaceOfBirthUnknown = false;
          }
      });

     

      $scope.searchCountries = function (search) {
          loadCountries(search);
      }

      $scope.searchCities = function (search) {
          loadCities(search);
      }

      $scope.onIsPlaceOfBirthUnknownChange = function () {
          $scope.newPerson.cityOfBirth = null;
      }

      $scope.countryOfBirthSelected = function () {
          if ($scope.newPerson.countryOfBirth == 0) {
              $scope.newPerson.isPlaceOfBirthUnknown = true;
          }
          $scope.newPerson.cityOfBirth = undefined;
          loadCities();
      }

      $scope.add = function () {
          if ($scope.personTabActive) {
              // Add person
              addNewOrExistingPerson();
          } else {
              // Add organization
              addNewOrExistingOrganization();
          }
      }

      $scope.cancel = function () {
          $modalInstance.close();
      }

      $scope.isCityOfBirthValid = function ($value) {
          if (!$scope.newPerson.hasOwnProperty('isPlaceOfBirthUnknown') && !$scope.newPerson.hasOwnProperty('cityOfBirth')) {
              return true;
          }
          if ($scope.newPerson.isPlaceOfBirthUnknown) {
              return $scope.newPerson.cityOfBirth === undefined || $scope.newPerson.cityOfBirth === null;
          }
          else {
              return $value !== undefined && $value !== null && $value !== 0;
          }
      }

      $scope.onDateOfBirthUnknownChange = function () {
          $scope.newPerson.dateOfBirth = null;
          $scope.newPerson.isDateOfBirthEstimated = null;
      }

      $scope.onIsSingleNameChange = function () {
          if ($scope.formLastName == "SURNAME / PRIMARY NAME") {
              $scope.formLastName = "NAME";
              $scope.newPerson.firstName = null;
              $scope.newPerson.isSingleName = true;
          }
          else {
              $scope.formLastName = "SURNAME / PRIMARY NAME";
              $scope.newPerson.isSingleName = false;
          }

      }

      function resetDuplicates() {
          $scope.personDuplicates = [];
          $scope.organizationDuplicates = [];
      }

      function setupNewPerson() {
          $scope.newPerson.projectId = parseInt($stateParams.projectId);
          if ($scope.newPerson.dateOfBirth) {
              $scope.newPerson.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
      }

      function addNewOrExistingPerson() {
          if ($scope.personDuplicates.length === 0) {
              var params = getPersonDuplicateParams();
              PersonService.getPeople(params)
              .then(function (response) {
                  if (response.data.total > 0) {
                      $scope.personDuplicates = response.data.results;
                  } else {
                      addNewPerson();
                  }
              });
          } else {
              if ($scope.newPerson.selectedDuplicate) {
                  addExistingPerson();
              } else {
                  addNewPerson();
              }
          }
      }

      function getPersonDuplicateParams() {
          var params = {
              limit: 300,
              filter: [{ property: 'lastName', comparison: 'eq', value: $scope.newPerson.lastName }]
          };
          if (!$scope.newPerson.isSingleName) {
              params.filter.push({ property: 'firstName', comparison: 'eq', value: $scope.newPerson.firstName  });
          }
          if ($scope.newPerson.dateOfBirth) {
              params.filter.push({ property: 'dateOfBirth', comparison: 'eq', value: $scope.newPerson.dateOfBirth });
          }
          if ($scope.newPerson.cityOfBirth) {
              params.filter.push({ property: 'cityOfBirthId', comparison: 'eq', value: $scope.newPerson.cityOfBirth });
          }
          return params;
      }

      function addNewPerson() {
          setupNewPerson();
          PersonService.create($scope.newPerson)
          .then(showSuccess, showError)
          .finally(function () {
              $modalInstance.close($scope.newPerson);
          });
      }

      function setupNewPerson() {
          $scope.newPerson.projectId = parseInt($stateParams.projectId);
          if ($scope.newPerson.dateOfBirth) {
              $scope.newPerson.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
      }

      function showSuccess() {
          NotificationService.showSuccessMessage('The participant was added successfully.');
      }

      function showError() {
          NotificationService.showErrorMessage('There was an error adding the participant.');
      }

      function addExistingPerson() {
          var existingPerson = {
              projectId: $stateParams.projectId,
              personId: $scope.newPerson.selectedDuplicate.personId,
              participantTypeId: $scope.newPerson.participantTypeId
          };
          ProjectService.addPersonParticipant(existingPerson)
          .then(showSuccess, showError)
          .finally(function () {
              $modalInstance.close(existingPerson);
          });
      }

      function addNewOrExistingOrganization() {
          if ($scope.organizationDuplicates.length === 0) {
              var params = getOrganizationDuplicateParams();
              OrganizationService.getOrganizations(params)
              .then(function (response) {
                  if (response.total > 0) {
                      $scope.organizationDuplicates = response.results;
                  } else {
                      addNewOrganization();
                  }
              });
          } else {
              if ($scope.newOrganization.selectedDuplicate) {
                  addExistingOrganization();
              } else {
                  addNewOrganization();
              }
          }
      }

      function getOrganizationDuplicateParams() {
          var params = {
              limit: 300,
              filter: [{ property: 'name', comparison: 'eq', value: $scope.newOrganization.name }]
          };
          return params;
      }

      function addNewOrganization() {
          setupNewOrganization();
          OrganizationService.createParticipantOrganization($scope.newOrganization)
          .then(showSuccess, showError)
          .finally(function () {
              $modalInstance.close($scope.newOrganization);
          });
      }

      function addExistingOrganization() {
          var existingOrganization = {
              projectId: $stateParams.projectId,
              organizationId: $scope.newOrganization.selectedDuplicate.organizationId,
              participantTypeId: $scope.newOrganization.participantTypeId
          };
          ProjectService.addOrganizationParticipant(existingOrganization)
          .then(showSuccess, showError)
          .finally(function () {
              $modalInstance.close(existingOrganization);
          });
      }

      function setupNewOrganization() {
          $scope.newOrganization.projectId = parseInt($stateParams.projectId);
      }

      function loadPersonParticipantTypes() {
          return LookupService.getParticipantTypes({
              limit: 300,
              filter: [{ property: 'isPerson', comparison: ConstantsService.equalComparisonType, value: true }]
          })
          .then(function (data) {
              $scope.personParticipantTypes = data.data.results;
          });
      }

      function loadGenders() {
          return LookupService.getAllGenders({ limit: 300 })
            .then(function (data) {
                $scope.genders = data.results;
            });
      }

      function getLoadCountriesParams(search) {
          return {
              limit: 300,
              filter: [
                { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.country.id },
                { property: 'name', comparison: ConstantsService.likeComparisonType, value: search },
                { property: 'name', comparison: ConstantsService.isNotNullComparisonType }
              ]
          };
      }

      function loadCountries(search) {
          return LocationService.get(getLoadCountriesParams(search))
            .then(function (data) {
                var results = data.results;
                $scope.countries = results;
            });
      }

      function loadCities(search) {
          var params = {
              limit: 300,
              filter: [
                  { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id },
                  { property: 'name', comparison: ConstantsService.isNotNullComparisonType }
              ]
          };

          if (search) {
              params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
          } else if ($scope.newPerson.cityOfBirth) {
              params.filter.push({ property: 'id', comparison: ConstantsService.equalComparisonType, value: $scope.newPerson.cityOfBirth });
          }

          return LocationService.get(params)
          .then(function (data) {
              $scope.cities = data.results;
          });
      }

      function loadOrganizationParticipantTypes() {
          return LookupService.getParticipantTypes({
              limit: 300,
              filter: [{ property: 'isPerson', comparison: ConstantsService.equalComparisonType, value: false }]
          })
          .then(function (data) {
              $scope.organizationParticipantTypes = data.data.results;
          });
      }

      function loadOrganizationTypes() {
          return OrganizationService.getTypes({
              limit: 300,
              filter: {
                  comparison: ConstantsService.notInComparisonType,
                  property: 'id',
                  value: [ConstantsService.organizationType.office.id, ConstantsService.organizationType.branch.id, ConstantsService.organizationType.division.id]
              }
          })
           .then(function (data) {
               $scope.organizationTypes = data.data.results;
           });
      }

      function loadOrganizationRoles() {
          return LookupService.getOrganizationRoles({ limit: 300 })
            .then(function (data) {
                $scope.organizationRoles = data.data.results;
            });
      }

      function loadPointsOfContact() {
          return LookupService.getAllContacts({ limit: 300 })
            .then(function (data) {
                $scope.pointsOfContact = data.results;
            });
      }

      // Lookups for person
      $q.all([loadPersonParticipantTypes(), loadGenders()]);
      // Lookups for organization
      $q.all([loadOrganizationParticipantTypes(), loadOrganizationTypes(), loadOrganizationRoles(), loadPointsOfContact()]);
  });