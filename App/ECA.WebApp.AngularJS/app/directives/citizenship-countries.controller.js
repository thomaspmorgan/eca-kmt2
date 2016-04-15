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
      $scope.view.isDeletingCountry = false;
      $scope.view.isLoadingCountries = false;
      var tempCountryId = 0;

      $scope.view.onAddCitizenshipCountryClick = function (entityId) {
          var newCountry = {
              id: entityId,
              name: "",
              countryId: --tempCountryId,
              isNew: true
          };
          $scope.$parent.$parent.countriesOfCitizenship.splice(0, 0, newCountry);
          $scope.view.collapseCitizenshipCountries = false;
      };

      $scope.$on(ConstantsService.removenewCountryEventName, function (event, newCountry) {
          console.assert($scope.$parent.$parent.countriesOfCitizenship instanceof Array, 'The entity countries is defined but must be an array.');

          var countries = $scope.$parent.$parent.countriesOfCitizenship;
          var index = countries.indexOf(newCountry);
          var removedItems = countries.splice(index, 1);
          $log.info('Removed one new country at index ' + index);
      });

      $scope.$on(ConstantsService.primaryCountryChangedEventName, function (event, primaryCountry) {
          console.assert($scope.$parent.$parent.countriesOfCitizenship instanceof Array, 'The entity countries is defined but must be an array.');

          var countries = $scope.$parent.$parent.countriesOfCitizenship;
          var primaryCountryIndex = countries.indexOf(primaryCountry);
          angular.forEach(countries, function (country, index) {
              if (primaryCountryIndex !== index) {
                  country.isPrimary = false;
              }
          });
      });

  });
