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
              return DragonBreath.get('ProgramCounts', id);
          },
          GetProgramRelatedProjectsCount: function (id) {
              return DragonBreath.get('ProgramRelatedProjectsCount', id);
          },
          GetProgramParticipantCount: function (id) {
              return DragonBreath.get('ProgramParticipantCount', id);
          },
          GetProgramBudgetTotal: function (id) {
              return DragonBreath.get('ProgramBudgetTotal', id);
          },
          GetProgramFundingSourcesCount: function (id) {
              return DragonBreath.get('ProgramFundingSourcesCount', id);
          },
          GetProgramCountryCount: function (id) {
              return DragonBreath.get('ProgramCountryCount', id);
          },
          GetProgramBeneficiaryCount: function (id) {
              return DragonBreath.get('ProgramBeneficiaryCount', id);
          },
          GetProgramImpactStoryCount: function (id) {
              return DragonBreath.get('ProgramImpactStoryCount', id);
          },
          GetProgramAlumniCount: function (id) {
              return DragonBreath.get('ProgramAlumniCount', id);
          },
          GetProgramProminenceCount: function (id) {
              return DragonBreath.get('ProgramProminenceCount', id);
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


