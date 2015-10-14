'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:LanguageProficienciesCtrl
 * @description The LanguageProficiencies controller is used to control the list of languageProficiencies for a person.
 * # LanguageProficienciesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('LanguageProficienciesCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        LookupService,
        ConstantsService,
        NotificationService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseLanguageProficiencies = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadLanguagesPromise = $q.defer();
      $scope.data.languages = [];
      
      $scope.view.onAddLanguageProficiencyClick = function (entityLanguageProficiencies, personId) {
          console.assert(entityLanguageProficiencies, 'The entity language proficiencies is not defined.');
          console.assert(entityLanguageProficiencies instanceof Array, 'The entity language proficiencies is defined but must be an array.');
          var newLanguageProficiency = {
              languageId: null,
              personId: personId,
              isNativeLanguage: false,
              speakingProficiency: 0,
              readingProficiency: 0,
              comprehensionProficiency: 0,
              isNew: true
          };
          entityLanguageProficiencies.splice(0, 0, newLanguageProficiency);
          $scope.view.collapseLanguageProficiencies = false;
      }

      $scope.$on(ConstantsService.removeNewLanguageProficiencyEventName, function (event, newLanguageProficiency) {
          console.assert($scope.model, 'The scope person must exist. It should be set by the directive.');
          console.assert($scope.model.languageProficiencies instanceof Array, 'The entity language proficiencies is defined but must be an array.');

          var languageProficiencies = $scope.model.languageProficiencies;
          var index = languageProficiencies.indexOf(newLanguageProficiency);
          var removedItems = languageProficiencies.splice(index, 1);
          $log.info('Removed one new languageProficiencies at index ' + index);
      });

      $scope.$on(ConstantsService.primaryLanguageProficiencyChangedEventName, function (event, primaryLanguageProficiency) {
          console.assert($scope.model, 'The scope model property must exist. It should be set by the directive.');
          console.assert($scope.model.languageProficiencies instanceof Array, 'The entity language proficiencies is defined but must be an array.');

          var primaryLanguageProficiencyIndex = $scope.model.languageProficiencies.map(function (e) { return e.languageId }).indexOf(primaryLanguageProficiency.languageId);
          angular.forEach($scope.model.languageProficiencies, function (languageProficiency, index) {
              if (primaryLanguageProficiencyIndex !== index) {
                  languageProficiency.isNativeLanguage = false;
              }
          });
      });

      function getLanguages() {
          var params = {
              start: 0,
              limit: 300
          };
          return LookupService.getAllLanguages(params)
          .then(function (response) {
              if (response.data.total > params.limit) {
                  var message = "There are more languages than could be loaded.  Not all languages types will be shown.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              }
              $log.info('Loaded all languages.');
              var languages = response.data.results;
              $scope.data.loadLanguagesPromise.resolve(languages);
              $scope.data.languages = languages;
              return languages;
          })
          .catch(function () {
              var message = 'Unable to load languages.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
      getLanguages();
  });
