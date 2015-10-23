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
      $scope.view.showLanguageAlreadyUsedError = false;

      $scope.view.proficiencyOptions = [
          { id: 0, name: '0' },
          { id: 1, name: '1' },
          { id: 2, name: '2' },
          { id: 3, name: '3' },
          { id: 4, name: '4' },
          { id: 5, name: '5' }
      ];

      var originalLanguageProficiency = angular.copy($scope.languageProficiency);
      var originalProficiencyLanguages = angular.copy($scope.model.languageProficiencies);

      $scope.view.saveLanguageProficiencyChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewLanguageProficiency($scope.languageProficiency)) {
              var tempId = angular.copy($scope.languageProficiency.languageId);
              return LanguageProficiencyService.addLanguageProficiency($scope.languageProficiency, $scope.view.params.personId)
              .then(onSaveLanguageProficiencySuccess)
              .then(function () {
                  updateLanguageProficiencyFormDivId(tempId);
                  updateLanguageProficiencies(tempId, $scope.languageProficiency);
              })
              .catch(onSaveLanguageProficiencyError);
          }
          else {
              return LanguageProficiencyService.updateLanguageProficiency($scope.languageProficiency, $scope.view.params.personId)
                  .then(onSaveLanguageProficiencySuccess)
                  .catch(onSaveLanguageProficiencyError);
          }
      };

      function updateLanguageProficiencies(tempId, languageProficiency) {
          var index = $scope.model.languageProficiencies.map(function (e) { return e.languageId }).indexOf(tempId);
          $scope.model.languageProficiencies[index] = languageProficiency;
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
                  NotificationService.showSuccessMessage("Successfully deleted language proficiency.");
                  $scope.view.isDeletingLanguageProficiency = false;
                  removeLanguageProficiencyFromView($scope.languageProficiency);
              })
              .catch(function () {
                  var message = "Unable to delete language proficiency.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      }

      $scope.view.onEditLanguageProficiencyClick = function (proficiency) {
          $scope.view.showEditLanguageProficiency = true;
          //isLanguageExists(proficiency);
          //var id = getLanguageProficiencyFormDivId();
          //var options = {
          //    duration: 500,
          //    easing: 'easeIn',
          //    offset: 200,
          //    callbackBefore: function (element) {},
          //    callbackAfter: function (element) { }
          //}
          //smoothScroll(getLanguageProficiencyFormDivElement(id), options);
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
          return document.getElementById(id);
      }

      function onSaveLanguageProficiencySuccess(response) {
          $scope.languageProficiency = response.data;
          originalLanguageProficiency = angular.copy($scope.languageProficiency);
          originalProficiencyLanguages = angular.copy($scope.model.languageProficiencies);
          NotificationService.showSuccessMessage("Successfully saved changes to language proficiency.");
          $scope.view.showEditLanguageProficiency = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveLanguageProficiencyError() {
          var message = "Failed to save language proficiency changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewLanguageProficiency(languageProficiency) {
          return languageProficiency.isNew;
      }

      //function isLanguageExists(language) {
      //    if (language) {
      //        var editIndex = $scope.data.languages.map(function (e) { return e.languageId }).indexOf(language.languageId);

      //        if (editIndex === -1) {
      //            $scope.data.languages.push({
      //                id: language.languageId,
      //                name: language.languageName
      //            });
      //        }

      //        $scope.languageProficiency.languageId = language.languageId;
      //    }
      //}

      //$scope.view.onIsNativeLanguageChange = function () {
      //    $scope.$emit(ConstantsService.primaryLanguageProficiencyChangedEventName, $scope.languageProficiency);
      //}
  })
.filter("filterExisting", function () {
    return function (languages, proficiencies, scope) {
        var filtered = [];

        if (proficiencies) {
            for (var i = 0; i < languages.length; i++) {
                var lindex = proficiencies.map(function (e) { return e.languageId }).indexOf(languages[i].id);

                if (lindex === -1 || scope.languageProficiency.languageId === languages[i].id) {
                    filtered.push(languages[i]);
                }
            }
        }
        return filtered;
    };
});
