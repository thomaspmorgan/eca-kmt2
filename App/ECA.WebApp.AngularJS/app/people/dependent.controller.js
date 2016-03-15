'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personDependentViewCtrl
 * # personDependentViewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personDependentCtrl', function ($scope, $modalInstance, DependentService, PersonService, LocationService, ConstantsService, $stateParams, $q) {
      
      $scope.dependentLoading = true;
      $scope.selectedCountriesOfCitizenship = [];
      $scope.personIdDeferred = $q.defer();

      PersonService.getPersonById($stateParams.personId)
        .then(function (data) {
            $scope.person = data;
            loadDependent(data.personId);
            $scope.personIdDeferred.resolve(data.personId);
        });

      function loadDependent(personId) {
          $scope.dependentLoading = true;
          DependentService.getDependentById(personId)
             .then(function (data) {
                 $scope.dependent = data;
                 if ($scope.dependent.placeOfBirth) {
                     $scope.dependent.cityOfBirthId = $scope.dependent.placeOfBirth.id;
                 }
                 if ($scope.dependent.dateOfBirth) {
                     $scope.dependent.dateOfBirth = new Date($scope.dependent.dateOfBirth);
                     $scope.dateOfBirthPlaceholder = '';
                 }
                 $scope.selectedCountriesOfCitizenship = $scope.dependent.countriesOfCitizenship.map(function (obj) {
                     var location = {};
                     location.id = obj.id;
                     location.name = obj.value;
                     return location;
                 });
                 if ($scope.dependent.countryOfBirthId) {
                     $scope.getCities("");
                 }
                 $scope.dependentLoading = false;
             });
      };

      $scope.getCities = function (val) {
          return LocationService.get({
              start: 0,
              limit: 25,
              filter: [{ property: 'name', comparison: 'like', value: val },
                       { property: 'countryId', comparison: 'eq', value: $scope.dependent.countryOfBirthId },
                       { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id }]
          }).then(function (data) {
              $scope.cities = data.results;
              return $scope.cities;
          });
      }

      LocationService.get({
          limit: 300, filter: [{ property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id },
                           { property: 'isActive', comparison: 'eq', value: true }]
      })
        .then(function (data) {
            $scope.countries = data.results;
        });

      $scope.view.onSaveClick = function () {
          return saveNewLocation()
              .then(function (loc) {
                  $modalInstance.close(loc);
              });
      }

      $scope.view.onCloseClick = function () {
          $modalInstance.dismiss('cancel');
      }



  });