'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personDependentViewCtrl
 * # personDependentViewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddDependentModalCtrl', function ($scope, $modalInstance, DependentService, PersonService, LocationService, ConstantsService, $stateParams, $q) {

      $scope.dependentLoading = true;
      $scope.selectedCountriesOfCitizenship = [];
      $scope.personIdDeferred = $q.defer();

      PersonService.getPersonById($stateParams.personId)
        .then(function (data) {
            $scope.person = data;
            $scope.personIdDeferred.resolve(data.personId);
        });

      $scope.getCities = function (val) {
          return LocationService.get({
              start: 0,
              limit: 25,
              filter: [{ property: 'name', comparison: 'like', value: val },
                       { property: 'countryId', comparison: 'eq', value: $scope.dependent.countryOfBirthId },
                       { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id },
                       { property: 'isActive', comparison: 'eq', value: true }]
          }).then(function (data) {
              $scope.cities = data.results;
              return $scope.cities;
          });
      }

      $scope.isPlaceOfBirthValid = function ($value) {
          if ($value === 0 || $value === null) {
              return false;
          }
          else {
              return true;
          }
      }

      LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id } })
        .then(function (data) {
            $scope.countries = data.results;
        });
      
      $scope.onSaveClick = function () {
          return saveNewLocation()
              .then(function (loc) {
                  $modalInstance.close(loc);
              });
      }

      $scope.onCloseClick = function () {
          $modalInstance.dismiss('cancel');
      }

  });