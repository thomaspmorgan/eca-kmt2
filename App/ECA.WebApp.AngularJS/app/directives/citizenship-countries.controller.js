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

      $scope.view.onAddCitizenshipCountryClick = function (countriesOfCitizenship) {
          var newCountry = {
              locationId: --tempCountryId,
              locationName: "",
              isPrimary: false,
              isNew: true
          };
          countriesOfCitizenship.splice(0, 0, newCountry);
          $scope.view.collapseCitizenshipCountries = false;
      };
      
  });
