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
        ConstantsService,
        DependentService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.showEditCitizenshipCountry = false;
      $scope.view.isSavingChanges = false;
      $scope.view.isLoadingRequiredData = false;
      var originalCountry = angular.copy($scope.country);
      
      $scope.view.saveCountryChanges = function () {
          $scope.view.isSavingChanges = true;
          console.assert($scope.modelId, 'The entity model id must be defined.');
          var modelId = $scope.modelId;

          if (isNewCountry($scope.country)) {
              var tempId = angular.copy($scope.country.id);
              return DependentService.addCountry($scope.country, modelId)
                .then(onSaveCountrySuccess)
                .then(function () {
                    updateCountryFormDivId(tempId);
                    updateCountries(tempId, $scope.country);
                })
                .catch(onSaveCountryError);
          }
          else {
              return DependentService.updateCountry($scope.country, modelId)
                  .then(onSaveCountrySuccess)
                  .catch(onSaveCountryError);
          }
      };

      function updateCountries(tempId, country) {
          var index = $scope.countries.map(function (e) { return e.id; }).indexOf(tempId);
          $scope.countries[index] = country;
      };

      $scope.view.cancelCountryChanges = function () {
          $scope.view.showEditCitizenshipCountry = false;
          if (isNewCountry($scope.country)) {
              removeCountryFromView($scope.country);
          }
          else {
              $scope.country = angular.copy(originalCountry);
          }
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

      $scope.view.onIsPrimaryChange = function () {
          $scope.$emit(ConstantsService.primaryCountryChangedEventName, $scope.country);
      }

      $scope.view.onEditCountryClick = function () {
          $scope.view.showEditCitizenshipCountry = true;
          var id = getCountryFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          };
          smoothScroll(getCountryFormDivElement(id), options);
      };

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

  });
