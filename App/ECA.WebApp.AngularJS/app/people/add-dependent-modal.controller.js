'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: AddDependentModalCtrl
 * # AddDependentModalCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddDependentModalCtrl', function ($scope, $timeout, $modalInstance,
          PersonService, DependentService, LookupService, LocationService, ConstantsService,
          $stateParams, NotificationService, FilterService, $q, DateTimeService) {

      $scope.dependent = getNewDependent();
      $scope.dependent.projectId = parseInt($stateParams.projectId, 10);
      $scope.selectedCountriesOfCitizenship = [];
      $scope.countriesCitizenship = [];
      $scope.countriesResidence = [];
      $scope.cities = [];
      $scope.datePickerOpen = false;
      $scope.maxDateOfBirth = new Date();

      function saveNewDependent() {
          setupDependent();
          return DependentService.create($scope.dependent, $scope.dependent.personId)
              .then(function (response) {
                  NotificationService.showSuccessMessage("Finished adding dependent.");
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
          $scope.dependent.countriesOfCitizenship = $scope.selectedCountriesOfCitizenship.map(function (obj) {
              return obj.id;
          });
          if ($scope.dependent.dateOfBirth) {
              $scope.dependent.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
      };

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

      function loadDependentCitizenshipCountries(search) {
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

      LookupService.getAllGenders({
          limit: 300,
          filter: [{
              property: 'id', comparison: ConstantsService.inComparisonType, value: [1, 2]
          }]
      })
         .then(function (data) {
             $scope.genders = data.results;
         });
      
      function getNewDependent() {
          return {
          };
      }

      $scope.onSaveDependentClick = function () {
          return saveNewDependent()
              .then(function (dependent) {
                  $modalInstance.close(dependent);
              });
      }

      $scope.onCloseDependentClick = function () {
          $modalInstance.dismiss('cancel');
      }

  });