'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personPiiEditCtrl
 * # personPiiEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personPiiEditCtrl', function ($scope, $timeout, ParticipantService, PersonService, LookupService, LocationService, ConstantsService, $stateParams, NotificationService, $q) {

      $scope.piiLoading = true;
      $scope.datePickerOpen = false;
      $scope.maxDateOfBirth = new Date();
      $scope.selectedCountriesOfCitizenship = [];
      $scope.unknownCountry = 'Unknown';
      $scope.personIdDeferred = $q.defer();

      $scope.updateGender = function () {
          $scope.pii.gender = getObjectById($scope.pii.genderId, $scope.genders).value;
      };

      $scope.updateCountryOfBirth = function () {
          $scope.pii.countryOfBirth = getObjectById($scope.pii.countryOfBirthId, $scope.countries).name;
          if ($scope.pii.countryOfBirthId === 0) {
              $scope.pii.isPlaceOfBirthUnknown = true;
          }
          $scope.getCities("");
          clearCityOfBirth();
      }

      function clearCityOfBirth() {
          delete $scope.pii.cityOfBirth;
          delete $scope.pii.cityOfBirthId;
      }

      $scope.updateCityOfBirth = function () {
          $scope.pii.cityOfBirth = getObjectById($scope.pii.cityOfBirthId, $scope.cities).name;
      }

      $scope.updateMaritalStatus = function () {
          $scope.pii.maritalStatus = getObjectById($scope.pii.maritalStatusId, $scope.maritalStatuses).value;
      }

      $scope.formatCityOfBirth = function (model) {
          if (model && !$scope.cities) {
              return $scope.pii.cityOfBirth;
          }
          if (model && $scope.cities) {
              return getObjectById(model, $scope.cities).name;
          }
      }

      function getObjectById(id, array) {
          for (var i = 0; i < array.length; i++) {
              if (array[i].id === id) {
                  return array[i];
              }
          }
          return null;
      };
      
      $scope.toggleDobUnknown = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.pii.isDateOfBirthUnknown = $scope.pii.isDateOfBirthUnknown === false ? true : false;
          if ($scope.pii.isDateOfBirthUnknown === true) {
              $scope.dateOfBirthPlaceholder = 'Unknown';
              $scope.pii.dateOfBirth = '';
              $scope.datePickerOpen = false;
          } else {
              $scope.dateOfBirthPlaceholder = '';
          }
      };

      PersonService.getPersonById($stateParams.personId)
        .then(function (data) {
            $scope.person = data;
            loadPii(data.personId);
            $scope.personIdDeferred.resolve(data.personId);
            PersonService.getContactInfoById(data.personId)
              .then(function (data) {
                  $scope.contactInfo = data;
              });
        });

      function loadPii(personId) {
          $scope.piiLoading = true;
          PersonService.getPiiById(personId)
             .then(function (data) {
                 $scope.pii = data;
                 if ($scope.pii.dateOfBirth) {
                     $scope.pii.dateOfBirth = new Date($scope.pii.dateOfBirth);
                     $scope.pii.isDateOfBirthUnknown = false;
                     $scope.dateOfBirthPlaceholder = '';
                 } else if ($scope.pii.isDateOfBirthUnknown) {
                     $scope.dateOfBirthPlaceholder = 'Unknown';
                     $scope.pii.dateOfBirth = undefined;
                     $scope.datePickerOpen = false;
                 }
                 $scope.selectedCountriesOfCitizenship = $scope.pii.countriesOfCitizenship.map(function (obj) {
                     var location = {};
                     location.id = obj.id;
                     location.name = obj.value;
                     return location;
                 });
                 if ($scope.pii.countryOfBirthId) {
                     $scope.getCities("");
                     $scope.pii.isPlaceOfBirthUnknown = false;
                 } else if ($scope.pii.isPlaceOfBirthUnknown) {
                     $scope.pii.countryOfBirthId = 0;
                 }
                 $scope.piiLoading = false;
             });
      };

      $scope.getCities = function (val) {
          return LocationService.get({
              start: 0,
              limit: 25,
              filter: [{ property: 'name', comparison: 'like', value: val },
                       { property: 'countryId', comparison: 'eq', value: $scope.pii.countryOfBirthId },
                       { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id }]
          }).then(function (data) {
              $scope.cities = data.results;
              return $scope.cities;
          });
      }

      LookupService.getAllGenders({ limit: 300 })
         .then(function (data) {
             $scope.genders = data.results;
         });

      LookupService.getAllMaritalStatuses({ limit: 300 })
        .then(function (data) {
            $scope.maritalStatuses = data.results;
        });

      LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id } })
        .then(function (data) {
            var countriesOfBirth = data.results;
            countriesOfBirth.splice(0, 0, { id:0, name: $scope.unknownCountry })
            $scope.countries = countriesOfBirth;
        });

      $scope.cancelEditPii = function () {
          this.edit.Pii = false;
          loadPii($scope.person.personId);
      };

      $scope.saveEditPii = function () {
          setupPii();

          PersonService.updatePii($scope.pii, $scope.person.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("The edit was successful.");
                  $scope.edit.Pii = false;
                  loadPii($scope.person.personId);
              },
              function (error) {
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
              });
      };

      function setupPii() {
          $scope.pii.personId = $scope.person.personId;
          $scope.pii.countriesOfCitizenship = $scope.selectedCountriesOfCitizenship.map(function (obj) {
              return obj.id;
          });
      };

      $scope.openDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.datePickerOpen = true;
      };
}); 