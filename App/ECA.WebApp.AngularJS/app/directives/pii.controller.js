'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personPiiViewCtrl
 * # personPiiViewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('personPiiViewCtrl', function ($scope, $timeout, PersonService, LocationService, ConstantsService, $q, BrowserService) {

      $scope.edit = {};
      $scope.edit.piiLoading = true;

      $scope.selectedCountriesOfCitizenship = [];

      PersonService.getPersonById($scope.personid)
        .then(function (data) {
            $scope.person = data;
            BrowserService.setDocumentTitleByPerson(data, 'Personal Information');
            loadPii(data.personId);
        });

      function loadPii(personId) {
          $scope.edit.piiLoading = true;
          PersonService.getPiiById(personId)
             .then(function (data) {
                 $scope.pii = data;
                 if ($scope.pii.placeOfBirth) {
                     $scope.pii.cityOfBirthId = $scope.pii.placeOfBirth.id;
                 }
                 $scope.pii_FormLastName = ($scope.pii.isSingleName) ? "NAME" : "LAST NAME";
                 if ($scope.pii.dateOfBirth) {
                     $scope.pii.dateOfBirth = new Date($scope.pii.dateOfBirth);
                     $scope.pii.isDateOfBirthUnknown = false;
                     $scope.dateOfBirthPlaceholder = '';
                 } else if ($scope.pii.isDateOfBirthUnknown) {
                     $scope.dateOfBirthPlaceholder = 'Unknown';
                     $scope.pii.dateOfBirth = undefined;
                 }
                 $scope.selectedCountriesOfCitizenship = $scope.pii.countriesOfCitizenship.map(function (obj) {
                     var location = {};
                     location.id = obj.id;
                     location.name = obj.value;
                     return location;
                 });
                 if ($scope.pii.countryOfBirthId) {
                     $scope.getCities("");
                 }
                 $scope.edit.piiLoading = false;
             });
      };

      $scope.$watch('personid', function () {
          loadPii($scope.personid);
      });

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

      LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id } })
        .then(function (data) {
            $scope.countries = data.results;
        });

}); 