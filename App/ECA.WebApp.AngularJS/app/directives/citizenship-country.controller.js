'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:CitizenshipCountryCtrl
 * @description The citizenship country control is use to control a single country of citizenship.
 * # CitizenshipCountryCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('CitizenshipCountryCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        LookupService,
        LocationService,
        FilterService,
        ConstantsService,
        DependentService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.showEditCitizenshipCountry = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isDeletingCountry = false;
      $scope.view.isLoadingCountries = false;
      $scope.view.searchLimit = 10;

      var originalCountry = angular.copy($scope.citizenship);
      
      $scope.view.saveCountryChanges = function () {
          $scope.view.isSavingChanges = true;

          var countryName = getCountryName($scope.citizenship);
          $scope.citizenship.locationName = countryName;

          if (isNewCountry($scope.citizenship)) {
              var tempId = angular.copy($scope.citizenship.id);
              updateCountries($scope.citizenship);
          }
          else {
              updateCountries($scope.citizenship);
          }

          $scope.view.isSavingChanges = false;
          $scope.view.showEditCitizenshipCountry = false;
      };

      function updateCountries(country) {
          var index = $scope.$parent.$parent.$parent.$parent.dependent.countriesOfCitizenship.map(function (e) { return e.locationId; }).indexOf(country.locationId);
          if (index < 0) {
              $scope.$parent.$parent.$parent.$parent.dependent.countriesOfCitizenship.splice(0, 0, country);
          } else {
              $scope.$parent.$parent.$parent.$parent.dependent.countriesOfCitizenship[index] = country;
          }
      };

      function getCountryName(country) {
          var index = $scope.countries.map(function (e) { return e.id; }).indexOf(country.locationId);
          return $scope.countries[index].name;
      };

      $scope.view.cancelCountryChanges = function () {
          $scope.view.showEditCitizenshipCountry = false;
      };

      $scope.view.onDeleteCountryClick = function () {
          if (isNewCountry($scope.citizenship)) {
              removeCountryFromView($scope.citizenship);
          }
          else {
              $scope.view.isDeletingCountry = true;
              console.assert($scope.modelId, 'The entity model id must be defined.');
              var modelId = $scope.modelId;
              return DependentService.deleteCountry($scope.citizenship, modelId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted country.");
                  $scope.view.isDeletingCountry = false;
                  removeCountryFromView($scope.citizenship);
              })
              .catch(function () {
                  var message = "Unable to delete country.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      };

      $scope.view.getCountries = function ($viewValue) {
          return getCountries($viewValue);
      }

      $scope.view.onIsPrimaryChange = function () {
          $scope.$emit(ConstantsService.removeNewCitizenshipCountryEventName, $scope.citizenship);
      }

      $scope.view.onEditCountryClick = function () {
          $scope.view.showEditCitizenshipCountry = true;
      };

      function loadCountries() {
          var params = {
              limit: 300,
              filter: [
              { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.country.id },
              { property: 'isActive', comparison: 'eq', value: true }
              ]
          };

          return LocationService.get(params)
          .then(function (data) {
              $scope.countries = data.results;
              return $scope.countries;
          });
      }

      function removeCountryFromView(country) {
          $scope.$emit(ConstantsService.removeNewCountryEventName, country);
      }

      function getCountryFormDivIdPrefix() {
          return 'countryForm';
      }

      function getCountryFormDivId() {
          return getCountryFormDivIdPrefix() + $scope.citizenship.locationId;
      }

      function updateCountryFormDivId(tempId) {
          var id = getCountryFormDivIdPrefix() + tempId;
          var e = getCountryFormDivElement(id);
          e.id = getCountryFormDivIdPrefix() + $scope.citizenship.locationId.toString();
      }

      function getCountryFormDivElement(id) {
          return document.getElementById(id);
      }

      function onSaveCountrySuccess(response) {
          $scope.citizenship = response.data;
          originalCountry = angular.copy($scope.citizenship);
          NotificationService.showSuccessMessage("Successfully saved changes to country.");
          $scope.view.showEditCitizenshipCountry = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveCountryError() {
          var message = "Failed to save country changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewCountry(country) {
          return country.isNew;
      }
      
      $scope.view.isLoadingCountries = true;
      $q.all([loadCountries()])
          .then(function () {
              $scope.view.isLoadingCountries = false;
          })
          .catch(function () {
              $scope.view.isLoadingCountries = false;
          });
  });
