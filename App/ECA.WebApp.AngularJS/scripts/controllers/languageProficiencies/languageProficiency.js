'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:LanguageProficiencyCtrl
 * @description The LanguageProficiency control is use to control a single LanguageProficiency.
 * # LanguageProficiencyCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('LanguageProficiencyCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        LookupService,
        LanguageProficiencyService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.showEditLanguageProficiency = false;
      $scope.view.isSavingChanges = false;

      $scope.view.proficiencyOptions = [
          { id: 0, name: '0'},
          { id: 1, name: '1' },
          { id: 2, name: '2' },
          { id: 3, name: '3' },
          { id: 4, name: '4' },
          { id: 5, name: '5' }
      ];

      var originalLanguageProficiency = angular.copy($scope.languageProficiency);


      $scope.view.saveLanguageProficiencyChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewLanguageProficiency($scope.languageProficiency)) {
              var tempId = angular.copy($scope.languageProficiency.languageId);
              return LanguageProficiencyService.addLanguageProficiency($scope.languageProficiency, $scope.view.params.personId)
                .then(onSaveLanguageProficiencySuccess)
                .then(function () {
                    updateLanguageProficiencyFormDivId(tempId);
                })
                .catch(onSaveLanguageProficiencyError);
          }
          else {
              return LanguageProficiencyService.updateLanguageProficiency($scope.languageProficiency, $scope.view.params.personId)
                  .then(onSaveLanguageProficiencySuccess)
                  .catch(onSaveLanguageProficiencyError);
          }
      };

      $scope.view.cancelLanguageProficiencyChanges = function (form) {
          $scope.view.showEditLanguageProficiency = false;
          if (isNewLanguageProficiency($scope.languageProficiency)) {
              removeLanguageProficiencyFromView($scope.languageProficiency);
          }
          else {
              $scope.languageProficiency = angular.copy(originalLanguageProficiency);
          }
      };

      $scope.view.onDeleteLanguageProficiencyClick = function () {
          if (isNewLanguageProficiency($scope.languageProficiency)) {
              removeLanguageProficiencyFromView($scope.languageProficiency);
          }
          else {
              $scope.view.isDeletingLanguageProficiency = true;
              return LanguageProficiencyService.deleteLanguageProficiency($scope.languageProficiency, $scope.view.params.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted languageProficiency.");
                  $scope.view.isDeletingLanguageProficiency = false;
                  removeLanguageProficiencyFromView($scope.languageProficiency);
              })
              .catch(function () {
                  var message = "Unable to delete languageProficiency.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      }

      $scope.view.onEditLanguageProficiencyClick = function () {
          $scope.view.showEditLanguageProficiency = true;
          var id = getLanguageProficiencyFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) {},
              callbackAfter: function (element) { }
          }
          smoothScroll(getLanguageProficiencyFormDivElement(id), options);
      };

      function removeLanguageProficiencyFromView(languageProficiency) {
          $scope.$emit(ConstantsService.removeNewLanguageProficiencyEventName, languageProficiency);
      }

      function getLanguageProficiencyFormDivIdPrefix() {
          return 'languageProficiencyForm';
      }

      function getLanguageProficiencyFormDivId() {
          return getLanguageProficiencyFormDivIdPrefix() + $scope.languageProficiency.languageId;
      }
      
      function updateLanguageProficiencyFormDivId(tempId) {
          var id = getLanguageProficiencyFormDivIdPrefix() + tempId;
          var e = getLanguageProficiencyFormDivElement(id);
          e.id = getLanguageProficiencyFormDivIdPrefix() + $scope.languageProficiency.languageId;
      }

      function getLanguageProficiencyFormDivElement(id) {
          return document.getElementById(id)
      }

      function onSaveLanguageProficiencySuccess(response) {
          $scope.languageProficiency = response.data;
          originalLanguageProficiency = angular.copy($scope.languageProficiency);
          NotificationService.showSuccessMessage("Successfully saved changes to languageProficiency.");
          $scope.view.showEditLanguageProficiency = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveLanguageProficiencyError() {
          var message = "Failed to save languageProficiency changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewLanguageProficiency(languageProficiency) {
          if (languageProficiency.isNew) {
              return languageProficiency.isNew == true;
          }
          else {
              return false;
          }
      }
  });
