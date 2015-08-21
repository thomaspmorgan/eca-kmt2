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
          addMembership: function (languageProficiency, personId) {
              return DragonBreath.create(membership, 'people/' + personId + '/languageProficiency');
          },
          updateMembership: function (languageProficiency, personId) {
              return DragonBreath.save(membership, 'people/' + personId + '/languageProficiency');
          },
          deleteMembership: function (languageProficiency, personId) {
              return DragonBreath.delete(membership, 'people/' + personId + '/languageProficiency/' + languageProficiency.languageId);
          }
      };
  });