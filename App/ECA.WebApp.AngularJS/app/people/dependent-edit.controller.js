﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personDependentEditCtrl
 * # personDependentEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personDependentEditCtrl', function ($scope, $timeout, $modalInstance,
          PersonService, DependentService, LookupService, LocationService, ConstantsService,
          $stateParams, NotificationService, FilterService, $q, DateTimeService, dependent) {
      
      $scope.dependent = loadDependent(dependent.id);
      $scope.selectedCountriesOfCitizenship = [];
      $scope.countriesCitizenship = [];
      $scope.countriesResidence = [];
      $scope.cities = [];
      $scope.dependentLoading = true;
      $scope.datePickerOpen = false;
      $scope.maxDateOfBirth = new Date();

      $scope.updateGender = function () {
          $scope.dependent.gender = getObjectById($scope.dependent.genderId, $scope.genders).value;
      };

      function loadDependent(dependentId) {
          $scope.dependentLoading = true;
          return DependentService.getDependentById(dependentId)
             .then(function (data) {
                 $scope.dependent = data;
                 if ($scope.dependent.placeOfBirth) {
                     $scope.dependent.cityOfBirthId = $scope.dependent.placeOfBirth.id;
                 }
                 $scope.selectedCountriesOfCitizenship = $scope.dependent.countriesOfCitizenship.map(function (obj) {
                     var location = {};
                     location.id = obj.id;
                     location.name = obj.value;
                     return location;
                 });
                 // Convert from UTC to local date time
                 if ($scope.dependent.dateOfBirth) {
                     $scope.dependent.dateOfBirth = DateTimeService.getDateAsLocalDisplayMoment($scope.dependent.dateOfBirth).toDate();
                 }

                 loadCountries();

                 return $scope.loadCities(null)
                 .then(function () {
                     $scope.dependentLoading = false;
                 })
                 .catch(function () {
                     $scope.dependentLoading = false;
                 });
             });
      };

      $scope.onSelectCityOfBirth = function () {
          $scope.dependent.isPlaceOfBirthUnknown = false;
      }

      $scope.isPlaceOfBirthValid = function ($value) {
            if ($value === 0 || $value === null) {
                return false;
            }
            else {
                return true;
            }
      }

      $scope.isCityOfBirthValid = function ($value) {
            return $value !== undefined && $value !== null && $value !== 0;
      }

      $scope.searchCities = function (search) {
          return $scope.loadCities(search);
      }

      $scope.searchCountries = function (search) {
          return $scope.loadCountries(search);
      }
      
      $scope.loadCities = function (search) {
          if (search || $scope.dependent) {
              var params = {
                  limit: 30,
                  filter: [
                    { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id }
                  ]
              };
              if (search) {
                  params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
              }
              else if ($scope.dependent.cityOfBirthId) {
                  params.filter.push({ property: 'id', comparison: ConstantsService.equalComparisonType, value: $scope.dependent.cityOfBirthId });
              }
              return LocationService.get(params)
                .then(function (data) {
                    $scope.cities = data.results;
                    return $scope.cities;
                });
          }
      }

      $scope.loadCountries = function (search) {
          if (search) {
              var params = {
                  limit: 300,
                  filter: [
                    { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.country.id },
                    { property: 'isActive', comparison: 'eq', value: true }
                  ]
              };
              if (search) {
                  params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
              }
              return LocationService.get(params)
                .then(function (data) {
                    $scope.countriesCitizenship = data.results;
                    return $scope.countriesCitizenship;
                });
          }
      }

      var countriesFilter = FilterService.add('addResidence_countries');
      var countriesParams = countriesFilter
          .skip(0)
          .take(300)
          .equal('locationTypeId', ConstantsService.locationType.country.id)
          .isTrue('isActive')
          .sortBy('name')
          .toParams();
      function loadCountries() {
          return LocationService.get(countriesParams)
          .then(function (response) {
              $scope.countriesResidence = response.results;
          })
          .catch(function () {
              var message = "Unable to load countries.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      $scope.loadLocationById = function (id) {
          return LocationService.get({
              limit: 1,
              filter: [
                  { property: 'id', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id, value: id }
              ]
          });
      }

      LookupService.getAllGenders({
          limit: 300,
          filter: [{
              property: 'id', comparison: ConstantsService.inComparisonType, value: [1,2] }]
         })
         .then(function (data) {
             $scope.genders = data.results;
         });
      
      $scope.cancelEditDependent = function () {
          loadDependent($scope.person.personId);
      };

      $scope.saveEditDependent = function () {
          setupDependent();

          DependentService.update($scope.dependent, $scope.person.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("The edit was successful.");
                  loadDependent($scope.person.personId);
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

      function setupDependent() {
          $scope.dependent.personId = $scope.person.personId;
          $scope.dependent.countriesOfCitizenship = $scope.selectedCountriesOfCitizenship.map(function (obj) {
              return obj.id;
          });
          if ($scope.dependent.dateOfBirth) {
              $scope.dependent.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
      };

      $scope.openDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.datePickerOpen = true;
      };

      $scope.onSaveClick = function () {
          return saveNewLocation()
              .then(function (loc) {
                  $modalInstance.close(loc);
              });
      }

      $scope.onCloseClick = function () {
          $modalInstance.dismiss('cancel');
      }

      function getObjectById(id, array) {
          for (var i = 0; i < array.length; i++) {
              if (array[i].id === id) {
                  return array[i];
              }
          }
          return null;
      };

  });