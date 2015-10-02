'use strict';

/**
 * @ngdoc service
 * @name staticApp.languageProficiency
 * @description
 * # languageProficiency
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('LanguageProficiencyService', function ($q, DragonBreath) {

      return {
          addLanguageProficiency: function (languageProficiency, personId) {
              return DragonBreath.create(languageProficiency, 'people/' + personId + '/languageProficiency');
          },
          updateLanguageProficiency: function (languageProficiency, personId) {
              return DragonBreath.save(languageProficiency, 'people/' + personId + '/languageProficiency');
          },
          deleteLanguageProficiency: function (languageProficiency, personId) {
              return DragonBreath.delete(languageProficiency, 'people/' + personId + '/languageProficiency/' + languageProficiency.languageId);
          }
      };
  });