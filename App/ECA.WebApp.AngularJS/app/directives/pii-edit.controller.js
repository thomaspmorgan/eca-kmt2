'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personPiiEditCtrl
 * # personPiiEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personPiiEditCtrl', function (
      $rootScope,
      $scope,
      $timeout,
      PersonService,
      LookupService,
      LocationService,
      ParticipantPersonsService,
      ConstantsService,
      $stateParams,
      FilterService,
      NotificationService,
      $q,
      DateTimeService) {

      $scope.pii = {};
      $scope.selectedCountriesOfCitizenship = [];
      $scope.edit = {};
      $scope.edit.piiLoading = true;
      $scope.datePickerOpen = false;
      $scope.maxDateOfBirth = new Date();
      $scope.unknownCountryId = 0;

      var personId = $scope.personid;

      $scope.updateGender = function () {
          $scope.pii.gender = $scope.getObjectById($scope.pii.genderId, $scope.genders).value;
      };

      $scope.updateMaritalStatus = function () {
          $scope.pii.maritalStatus = $scope.getObjectById($scope.pii.maritalStatusId, $scope.maritalStatuses).value;
      }

      $scope.getObjectById = function (id, array) {
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


      loadPii(personId);
      function loadPii(personId) {
          $scope.edit.piiLoading = true;
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
                 if ($scope.pii.dateOfBirth) {
                     $scope.pii.dateOfBirth = DateTimeService.getDateAsLocalDisplayMoment($scope.pii.dateOfBirth).toDate();
                 }
                 return loadCities(null)
                 .then(function () {
                     $scope.edit.piiLoading = false;
                 })
                 .catch(function() {
                     $scope.edit.piiLoading = false;
                 });
             });
      };

      $scope.$watch('personid', function () {
          loadPii($scope.personid);
      });

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

      var countriesFilter = FilterService.add('pii_edit_countries_filter');
      function loadCountries(search) {
          countriesFilter.reset();
          countriesFilter = countriesFilter.skip(0).take(30).equal('locationTypeId', ConstantsService.locationType.country.id);
          if (search) {
              countriesFilter = countriesFilter.like('name', search);
          }
          return LocationService.get(countriesFilter.toParams())
            .then(function (data) {
                $scope.countries = data.results;
                return $scope.countries;
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
          $scope.$parent.$parent.editMode = false;
          loadPii(personId);
      };

      $scope.saveEditPii = function () {
          setupPii();

          PersonService.updatePii($scope.pii, personId)
              .then(function (response) {
                  NotificationService.showSuccessMessage("The edit was successful.");
                  loadPii(personId);
                  $scope.onUpdatePii();
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
          $scope.pii.personId = personId;
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