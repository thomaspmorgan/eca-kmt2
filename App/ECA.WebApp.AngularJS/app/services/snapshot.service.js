'use strict';

/**
 * @ngdoc service
 * @name staticApp.SnapshotService
 * @description
 * # SnapshotService
 * Service to retrieve search results.
 */
angular.module('staticApp')
  .factory('SnapshotService', function (DragonBreath) {

      return {
          GetProgramCounts: function (id) {
              return DragonBreath.get('ProgramSnapshotCounts', id);
          },
          GetProgramBudgetByYear: function (id) {
              return DragonBreath.get('ProgramBudgetByYear', id);
          },
          GetProgramMostFundedCountries: function (id) {
              return DragonBreath.get('ProgramMostFundedCountries', id);
          },
          GetProgramTopThemes: function (id) {
              return DragonBreath.get('ProgramTopThemes', id);
          },
          GetProgramParticipantsByLocation: function (id) {
              return DragonBreath.get('ProgramParticipantsByLocation', id);
          },
          GetProgramParticipantsByYear: function (id) {
              return DragonBreath.get('ProgramParticipantsByYear', id);
          },
          GetProgramParticipantGender: function (id) {
              return DragonBreath.get('ProgramParticipantGender', id);
          },
          GetProgramParticipantAge: function (id) {
              return DragonBreath.get('ProgramParticipantAge', id);
          },
          GetProgramParticipantEducation: function (id) {
              return DragonBreath.get('ProgramParticipantEducation', id);
          }
      }

  });


