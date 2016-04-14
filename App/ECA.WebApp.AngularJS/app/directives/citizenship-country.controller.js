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

      var originalCountry = angular.copy($scope.country);
      
      $scope.view.saveCountryChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewCountry($scope.country)) {
              var tempId = angular.copy($scope.country.id);
              var countryName = getCountryName($scope.country);
              $scope.country.name = countryName;
              updateCountries($scope.country);
          }
          else {
              updateCountries($scope.country);
          }

          $scope.view.isSavingChanges = false;
          $scope.view.showEditCitizenshipCountry = false;
      };

      function updateCountries(country) {
          var index = $scope.$parent.$parent.$parent.$parent.countriesOfCitizenship.map(function (e) { return e.id; }).indexOf(country.id);
          $scope.$parent.$parent.$parent.$parent.countriesOfCitizenship[index] = country;
      };

      function getCountryName(country) {
          var index = $scope.countries.map(function (e) { return e.id; }).indexOf(country.id);
          return $scope.countries[index].name;
      };

      $scope.view.cancelCountryChanges = function () {
          $scope.view.showEditCitizenshipCountry = false;
      };

      $scope.view.onDeleteCountryClick = function () {
          if (isNewCountry($scope.country)) {
              removeCountryFromView($scope.country);
          }
          else {
              $scope.view.isDeletingCountry = true;
              console.assert($scope.modelId, 'The entity model id must be defined.');
              var modelId = $scope.modelId;
              return DependentService.deleteCountry($scope.country, modelId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted country.");
                  $scope.view.isDeletingCountry = false;
                  removeCountryFromView($scope.country);
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

      $scope.view.onSelectCountry = function ($item, $model, $label) {
          $scope.address.country = $item.name;
          $scope.address.countryId = $item.id;
          $scope.view.isCountryInactive = !$item.isActive;
      }

      $scope.view.onSelectCountryBlur = function ($event) {
          if ($scope.address.country === '') {
              delete $scope.address.countryId;
              delete $scope.address.country;
          }
      };

      //$scope.view.onIsPrimaryChange = function () {
      //    $scope.$emit(ConstantsService.primaryCountryChangedEventName, $scope.country);
      //}

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
          return getCountryFormDivIdPrefix() + $scope.country.id;
      }

      function updateCountryFormDivId(tempId) {
          var id = getCountryFormDivIdPrefix() + tempId;
          var e = getCountryFormDivElement(id);
          e.id = getCountryFormDivIdPrefix() + $scope.country.id.toString();
      }

      function getCountryFormDivElement(id) {
          return document.getElementById(id);
      }

      function onSaveCountrySuccess(response) {
          $scope.country = response.data;
          originalCountry = angular.copy($scope.country);
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
