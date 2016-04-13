'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:CitizenshipCountriesCtrl
 * @description The countries controller is used to control the list of countries.
 * # CitizenshipCountriesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('CitizenshipCountriesCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        LookupService,
        NotificationService,
        ConstantsService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseCitizenshipCountries = true;
      $scope.data = {};
      var tempCountryId = 0;

      $scope.view.onAddCitizenshipCountryClick = function (entityCountries, entityId) {
          console.assert(entityCountries, 'The entity countries is not defined.');
          console.assert(entityCountries instanceof Array, 'The entity countries is defined but must be an array.');
          var newCountry = {
              id: entityId,
              countryId: --tempCountryId,
              isNew: true
          };
          entityCountries.splice(0, 0, newCountry);
          $scope.view.collapseCitizenshipCountries = false;
      };

      $scope.$on(ConstantsService.removenewCountryEventName, function (event, newCountry) {
          console.assert($scope.countries instanceof Array, 'The entity countries is defined but must be an array.');

          var countries = $scope.countries;
          var index = countries.indexOf(newCountry);
          var removedItems = countries.splice(index, 1);
          $log.info('Removed one new country at index ' + index);
      });

      $scope.$on(ConstantsService.primaryCountryChangedEventName, function (event, primaryCountry) {
          console.assert($scope.countries instanceof Array, 'The entity countries is defined but must be an array.');

          var countries = $scope.countries;
          var primaryCountryIndex = countries.indexOf(primaryCountry);
          angular.forEach(countries, function (country, index) {
              if (primaryCountryIndex !== index) {
                  country.isPrimary = false;
              }
          });
      });

  });
