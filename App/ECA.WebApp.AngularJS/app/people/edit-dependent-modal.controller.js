'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: EditDependentModalCtrl
 * # EditDependentModalCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('EditDependentModalCtrl', function ($scope, $timeout, $modalInstance,
          PersonService, DependentService, LookupService, LocationService, ConstantsService, $stateParams,
          NotificationService, FilterService, $q, DateTimeService, dependent) {
      
      $scope.dependent = loadDependent(dependent.id);
      $scope.selectedCountriesOfCitizenship = [];
      $scope.countriesCitizenship = [];
      $scope.countriesResidence = [];
      $scope.cities = [];
      $scope.dependentLoading = true;
      $scope.datePickerOpen = false;
      $scope.maxDateOfBirth = new Date();

      function loadDependent(dependentId) {
          $scope.dependentLoading = true;
          return DependentService.getDependentById(dependentId)
             .then(function (data) {
                 $scope.dependent = data;
                 if ($scope.dependent.countriesOfCitizenship) {
                     $scope.selectedCountriesOfCitizenship = $scope.dependent.countriesOfCitizenship.map(function (obj) {
                         var location = {};
                         location.id = obj.id;
                         location.name = obj.value;
                         return location;
                     });
                 }                 
                 if ($scope.dependent.dateOfBirth) {
                     $scope.dependent.dateOfBirth = DateTimeService.getDateAsLocalDisplayMoment($scope.dependent.dateOfBirth).toDate();
                 }
                 
                 return loadDependentCities(null)
                 .then(function () {
                     $scope.dependentLoading = false;
                 })
                 .catch(function () {
                     $scope.dependentLoading = false;
                 });
             });
      };

      function saveEditDependent() {
          setupDependent();
          return DependentService.update($scope.dependent)
              .then(function (response) {
                  NotificationService.showSuccessMessage("The edit was successful.");
                  return response.config.data;
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
      
      function loadDependentCities(search) {
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
              else if ($scope.dependent.placeOfBirthId) {
                  params.filter.push({ property: 'id', comparison: ConstantsService.equalComparisonType, value: $scope.dependent.placeOfBirthId });
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
              limit: 300
          })
          .then(function (data) {
              $scope.birthCountryReasons = data.data.results;
          });
      }

      $scope.openDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.datePickerOpen = true;
      };

      $scope.onSaveDependentClick = function () {
          return saveEditDependent()
              .then(function (dependent) {
                  $modalInstance.close(dependent);
              });
      }

      $scope.onCloseDependentClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $q.all([loadResidenceCountries(), loadGenders(), loadDependentTypes(), loadBirthCountryReasons()]);
  });