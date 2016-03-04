'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personPiiEditCtrl
 * # personPiiEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personPiiEditCtrl', function ($scope, $timeout, PersonService, LookupService, LocationService, ConstantsService, $stateParams, NotificationService, $q, DateTimeService) {

      $scope.pii = {};
      $scope.selectedCountriesOfCitizenship = [];
      $scope.piiLoading = true;
      $scope.datePickerOpen = false;
      $scope.maxDateOfBirth = new Date();
      $scope.unknownCountryId = 0;
      $scope.personIdDeferred = $q.defer();

      $scope.updateGender = function () {
          $scope.pii.gender = getObjectById($scope.pii.genderId, $scope.genders).value;
      };

      $scope.updateMaritalStatus = function () {
          $scope.pii.maritalStatus = getObjectById($scope.pii.maritalStatusId, $scope.maritalStatuses).value;
      }

      function getObjectById(id, array) {
          for (var i = 0; i < array.length; i++) {
              if (array[i].id === id) {
                  return array[i];
              }
          }
          return null;
      };

      $scope.onSelectCityOfBirth = function () {
          $scope.pii.isPlaceOfBirthUnknown = false;
      }

      $scope.isPlaceOfBirthValid = function ($value) {
          if ($value === undefined && $scope.pii.isPlaceOfBirthUnknown) {
              return true;
          }
          if ($value === undefined && !$scope.pii.isPlaceOfBirthUnknown) {
              return false;
          }
          else {
              if (($value === 0 || $value === null) && !$scope.pii.isPlaceOfBirthUnknown) {
                  return false;
              }
              else if($scope.pii.isPlaceOfBirthUnknown) {
                  return $value === undefined || $value === null || $value === 0;
              }
              else {
                  return true;
              }
          }
      }

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
          return PersonService.getPiiById(personId)
             .then(function (data) {
                 $scope.pii = data;
                 if ($scope.pii.placeOfBirth) {
                     $scope.pii.cityOfBirthId = $scope.pii.placeOfBirth.id;
                 }
                 $scope.selectedCountriesOfCitizenship = $scope.pii.countriesOfCitizenship.map(function (obj) {
                     var location = {};
                     location.id = obj.id;
                     location.name = obj.value;
                     return location;
                 });
                 // Convert from UTC to local date time
                 $scope.pii.dateOfBirth = DateTimeService.getDateAsLocalDisplayMoment($scope.pii.dateOfBirth).toDate();
                 return loadCities(null)
                 .then(function () {
                     $scope.piiLoading = false;
                 })
                 .catch(function() {
                     $scope.piiLoading = false;
                 });
             });
      };

      $scope.isCityOfBirthValid = function ($value) {
          if (!$scope.pii.hasOwnProperty('isPlaceOfBirthUnknown') && !$scope.pii.hasOwnProperty('placeOfBirth')) {
              return true;
          }
          if ($scope.pii.isPlaceOfBirthUnknown) {
              return $scope.pii.cityOfBirthId === undefined || $scope.pii.cityOfBirthId === null;
          }
          else {
              return $value !== undefined && $value !== null && $value !== 0;
          }
      }

      $scope.searchCities = function (search) {
          return loadCities(search);
      }

      $scope.searchCountries = function (search) {
          return loadCountries(search);
      }

      $scope.onIsPlaceOfBirthUnknownChange = function () {
          if ($scope.pii.isPlaceOfBirthUnknown) {
              $scope.pii.cityOfBirthId = null;
          }          
      }

      $scope.onDateOfBirthUnknownChange = function () {
          $scope.pii.dateOfBirth = null;
          $scope.pii.isDateOfBirthEstimated = null;
      }

      function loadCities(search) {
          if (search || $scope.pii) {
              var params = {
                  limit: 30,
                  filter: [
                    { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id }
                  ]
              };
              if (search) {
                  params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
              }
              else if ($scope.pii.cityOfBirthId) {
                  params.filter.push({ property: 'id', comparison: ConstantsService.equalComparisonType, value: $scope.pii.cityOfBirthId });
              }
              return LocationService.get(params)
                .then(function (data) {
                    $scope.cities = data.results;
                    return $scope.cities;
                });
          }
      }

      function loadCountries(search) {
          if (search) {
              var params = {
                  limit: 30,
                  filter: [
                    { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.country.id }
                  ]
              };
              if (search) {
                  params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
              }
              return LocationService.get(params)
                .then(function (data) {
                    $scope.countries = data.results;
                    return $scope.countries;
                });
          }
      }

      //LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id } })
      //  .then(function (data) {
      //      $scope.countries = data.results;
      //  });

      function loadLocationById(id) {
          return LocationService.get({
              limit: 1,
              filter: [
                  { property: 'id', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id, value: id }
              ]
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

      $scope.cancelEditPii = function () {
          this.edit.Pii = false;
          loadPii($scope.person.personId);
      };

      $scope.saveEditPii = function () {
          setupPii();

          PersonService.updatePii($scope.pii, $scope.person.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("The edit was successful.");
                  loadPii($scope.person.personId);
                  $scope.edit.Pii = false;
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
                  } else {
                      if (error) {
                          NotificationService.showErrorMessage(error.status + ': ' + error.statusText);
                      }
                  }
              });
      };
      
      function setupPii() {
          $scope.pii.personId = $scope.person.personId;
          $scope.pii.countriesOfCitizenship = $scope.selectedCountriesOfCitizenship.map(function (obj) {
              return obj.id;
          });
          if ($scope.pii.dateOfBirth) {
              $scope.pii.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
      };

      $scope.openDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.datePickerOpen = true;
      };
  });