'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddNewParticipantCtrl
 * @description The controller to add a new participant
 * # AddNewParticipantCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddNewParticipantCtrl', function ($q, $scope, $stateParams, $modalInstance, LookupService, LocationService, ConstantsService, PersonService, ProjectService, NotificationService) {

      $scope.isDobDatePickerOpen = false;
      $scope.dateFormat = 'MM/dd/yyyy';
      $scope.maxDate = new Date();

      $scope.newParticipant = {};
      $scope.newParticipant.countriesOfCitizenship = [];
      $scope.newParticipant.selectedDuplicate = undefined;
      $scope.newParticipant.isDateOfBirthUnknown = false;

      $scope.duplicates = [];

      $scope.add = function () {

          if ($scope.duplicates.length === 0) {

              var newParticipant = getNewParticipant();

              var params = {
                  limit: 300,
                  filter: [{ property: 'firstName', comparison: 'eq', value: newParticipant.firstName },
                           { property: 'lastName', comparison: 'eq', value: newParticipant.lastName },
                           { property: 'genderId', comparison: 'eq', value: newParticipant.gender }]
              };
              
              if (newParticipant.dateOfBirth) {
                  params.filter.push({ property: 'dateOfBirth', comparison: 'eq', value: newParticipant.dateOfBirth });
              }

              if (newParticipant.cityOfBirth) {
                  params.filter.push({ property: 'cityOfBirthId', comparison: 'eq', value: newParticipant.cityOfBirth });
              }

              PersonService.getPeople(params)
                .then(function (response) {
                    if (response.data.total > 0) {
                        $scope.duplicates = response.data.results;
                    } else {
                        createNewParticipant(newParticipant);
                    }
                }, function () {
                    NotificationService.showErrorMessage('Unable to load people.');
                });
          } else {
              if ($scope.newParticipant.selectedDuplicate) {
                  var existingParticipant = getExistingParticipant();
                  addExistingParticipant(existingParticipant);
              } else {
                  createNewParticipant(newParticipant);
              }
              
          };
      }

      function createNewParticipant(newParticipant) {
          PersonService.create(newParticipant)
               .then(function () {
                   NotificationService.showSuccessMessage('The participant was created successfully.');
               }, function () {
                   NotificationService.showErrorMessage('There was an error creating the new participant.');
               }).finally(function () {
                   $modalInstance.close(newParticipant);
               });
      }

      function addExistingParticipant(existingParticipant) {
          ProjectService.addPersonParticipant(existingParticipant)
            .then(function () {
                NotificationService.showSuccessMessage('The participant was added successfully.');
            }, function () {
                NotificationService.showErrorMessage('There was an error creating the new participant.');
            }).finally(function () {
                $modalInstance.close(existingParticipant);
            });
      }

      function getNewParticipant() {

          var newParticipant = {};

          newParticipant.projectId = $stateParams.projectId;
          newParticipant.participantTypeId = $scope.newParticipant.participantType.id;
          newParticipant.firstName = $scope.newParticipant.firstName;
          newParticipant.lastName = $scope.newParticipant.lastName;
          newParticipant.gender = $scope.newParticipant.gender.id;
          newParticipant.isDateOfBirthUnknown = $scope.newParticipant.isDateOfBirthUnknown;

          if ($scope.newParticipant.dateOfBirth) {
              newParticipant.dateOfBirth = $scope.newParticipant.dateOfBirth;
              newParticipant.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
          
          if ($scope.newParticipant.cityOfBirth) {
              newParticipant.cityOfBirth = $scope.newParticipant.cityOfBirth.id;
          }

          if ($scope.newParticipant.countriesOfCitizenship) {
              newParticipant.countriesOfCitizenship = $scope.newParticipant.countriesOfCitizenship.map(function (obj) {
                  return obj.id;
              });
          }

          return newParticipant;
      }

      function getExistingParticipant() {

          var existingParticipant = {};

          existingParticipant.projectId = $stateParams.projectId;
          existingParticipant.personId = $scope.newParticipant.selectedDuplicate.personId;
          existingParticipant.participantTypeId = $scope.newParticipant.participantType.id;
    
          return existingParticipant;
      }

      $scope.cancel = function () {
          $modalInstance.close();
      }

      $scope.openDobDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.isDobDatePickerOpen = true;
      }

      $scope.$watch('newParticipant.dateOfBirth', function () {
          var date = $scope.newParticipant.dateOfBirth;
          if (date) {
              $scope.newParticipant.isDateOfBirthUnknown = false;
          }
      });

      $scope.toggleDobUnknown = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.newParticipant.isDateOfBirthUnknown = $scope.newParticipant.isDateOfBirthUnknown === false ? true : false;
          if ($scope.newParticipant.isDateOfBirthUnknown === true) {
              $scope.dateOfBirthPlaceholder = 'Unknown'
              $scope.newParticipant.dateOfBirth = undefined;
              $scope.isDobDatePickerOpen = false;
          } else {
              $scope.dateOfBirthPlaceholder = '';
          }
      };

      $scope.searchCountries = function (search) {
          loadCountries(search);
      }

      $scope.searchCountriesCopy = function (search) {
          loadCountriesCopy(search);
      }

      $scope.searchCities = function (search) {
          loadCities(search);
      }

      $scope.countryOfBirthSelected = function () {
          $scope.newParticipant.cityOfBirth = undefined;
          loadCities();
      }

      function loadGenders() {
          return LookupService.getAllGenders({ limit: 300 })
            .then(function (data) {
                $scope.genders = data.results;
            });
      }

      function loadParticipantTypes() {

          return LookupService.getParticipantTypes({ limit: 300 })
            .then(function (data) {
                $scope.participantTypes = data.data.results;
            });
      }

      function loadCountries(search) {

          var params = {
              limit: 300,
              filter: [
                { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.country.id },
                { property: 'name', comparison: ConstantsService.likeComparisonType, value: search },
                { property: 'name', comparison: ConstantsService.isNotNullComparisonType }
              ]
          };

          return LocationService.get(params)
            .then(function (data) {
                $scope.countries = data.results;
            });
      }

      function loadCountriesCopy(search) {

          var params = {
              limit: 300,
              filter: [
                { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.country.id },
                { property: 'name', comparison: ConstantsService.likeComparisonType, value: search },
                { property: 'name', comparison: ConstantsService.isNotNullComparisonType }
              ]
          };

          if ($scope.newParticipant.countriesOfCitizenship.length > 0) {
              var idsToRemove = $scope.newParticipant.countriesOfCitizenship.map(function (c) { return c.id; });
              params.filter.push({
                  comparison: ConstantsService.notInComparisonType,
                  property: 'id',
                  value: idsToRemove
              });
          }

          return LocationService.get(params)
            .then(function (data) {
                $scope.countriesCopy = data.results;
            });
      }

      function loadCities(search) {

          if ($scope.newParticipant.countryOfBirth) {

              var params = {
                  limit: 300,
                  filter: [
                    { property: 'countryId', comparison: 'eq', value: $scope.newParticipant.countryOfBirth.id },
                    { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id },
                    { property: 'name', comparison: ConstantsService.isNotNullComparisonType }
                  ]
              };

              if (search) {
                  params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
              }

              return LocationService.get(params)
                .then(function (data) {
                    $scope.cities = data.results;
                });

              }
      }

      $q.all([loadGenders(), loadParticipantTypes()]);

  });