﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: AddDependentModalCtrl
 * # AddDependentModalCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddDependentModalCtrl', function ($scope, $timeout, $modalInstance, $stateParams,
          PersonService, DependentService, LookupService, LocationService, ConstantsService,
          NotificationService, FilterService, $q, DateTimeService, person) {

      $scope.dependent = getNewDependent();
      $scope.dependent.countriesOfCitizenship = [];
      $scope.person = person;
      $scope.countriesResidence = [];
      $scope.cities = [];
      $scope.datePickerOpen = false;
      $scope.isDependentLoading = true;
      $scope.isSavingDependent = false;
      $scope.minDateOfBirth = new Date();
      
      function saveNewDependent() {
          $scope.isSavingDependent = true;
          setupDependent();
          return DependentService.create($scope.dependent)
              .then(function (response) {
                  NotificationService.showSuccessMessage("Finished adding dependent.");
                  $scope.isSavingDependent = false;
                  return response.data;
              },
              function (error) {
                  $scope.isSavingDependent = false;
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
          if ($scope.dependent.dateOfBirth) {
              $scope.dependent.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
      };

      $scope.setMinBirthDate = function (dependentTypeId) {
          var minDate = new moment();
          if (dependentTypeId === ConstantsService.dependentType.child.id) {
              minDate.subtract(ConstantsService.childDependentMaxAge, 'y'); // child
          } else if (dependentTypeId === ConstantsService.dependentType.spouse.id) {
              minDate.subtract(100, 'y'); // spouse
          }
          $scope.minDateOfBirth = minDate;
      }

      $scope.isDependentPlaceOfBirthValid = function ($value) {
          if ($value === 0 || $value === null) {
              return false;
          }
          else {
              return true;
          }
      }

      $scope.isDependentCityOfBirthValid = function ($value) {
          return $value !== undefined && $value !== null && $value !== 0;
      }

      $scope.searchDependentCities = function (search) {
          return loadDependentCities(search);
      }

      $scope.searchDependentCountries = function (search) {
          return loadDependentCitizenshipCountries(search);
      }

      $scope.setBirthCountryReasonState = function ($item, $model) {
          if ($item.countryId === 193) {
              $scope.dependent.isBirthCountryUSA = true;
          } else {
              $scope.dependent.isBirthCountryUSA = false;
              $scope.dependent.birthCountryReasonId = null;
          }
      }
      
      function loadDependentCities(search) {
          if (search) {
              var params = {
                  limit: 30,
                  filter: [
                    { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id }
                  ]
              };
              if (search) {
                  params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
              }
              return LocationService.get(params)
                .then(function (data) {
                    $scope.cities = data.results;
                    return $scope.cities;
                });
          }
      }
      
      function loadResidenceCountries() {
            var params = {
                limit: 300,
                filter: [
                { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.country.id },
                { property: 'isActive', comparison: 'eq', value: true }
                ]
            };

            return LocationService.get(params)
            .then(function (data) {
                $scope.countriesResidence = data.results;
                return $scope.countriesResidence;
            });
      }

      function loadGenders() {
            LookupService.getAllGenders({
                limit: 300,
                filter: [{
                    property: 'id', comparison: ConstantsService.inComparisonType, value: [1, 2]
                }]
            })
            .then(function (data) {
                $scope.genders = data.results;
            });
        }

        function loadDependentTypes() {
            LookupService.getDependentTypes({
                limit: 300,
                filter: [{ property: 'sevisDependentTypeCode', comparison: ConstantsService.isNotNullComparisonType }]
            })
            .then(function (data) {
                $scope.dependenttypes = data.data.results;
            });
        }

        function loadBirthCountryReasons() {
            LookupService.getBirthCountryReasons({
                limit: 300,
                filter: [{
                    property: 'birthReasonCode', comparison: ConstantsService.isNotNullComparisonType }]
            })
            .then(function (data) {
                $scope.birthCountryReasons = data.data.results;
            });
        }

      function getNewDependent() {
          return {
          };
      }

      $scope.openDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.datePickerOpen = true;
      };

      $scope.onSaveDependentClick = function () {
          return saveNewDependent()
              .then(function (dependent) {
                  $modalInstance.close(dependent);
              });
      }

      $scope.onCloseDependentClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.isDependentLoading = true;
      $q.all([loadResidenceCountries(), loadGenders(), loadDependentTypes(), loadBirthCountryReasons()])
          .then(function () {
              $scope.isDependentLoading = false;
          })
          .catch(function () {
              $scope.isDependentLoading = false;
          });
  });