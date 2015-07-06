'use strict';

/**
 * @ngdoc service
 * @name staticApp.program
 * @description
 * # program
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ReportService', function (DragonBreath, $q) {

      return {
          get: function (id) {
              return DragonBreath.get('reports', id);
          },          
          update: function (program, id) {
              return DragonBreath.save(report, 'reports', id);
          },
          create: function (program) {
              return DragonBreath.create(report, 'reports');
          },
          getAll: function (params) {
              return DragonBreath.get(params, 'reports');
          },
          getProjectAwards: function(programId, countryId) {
              return "report/ProjectAwards?programId=" + programId + "&countryId=" + countryId;
          },
          getRegionAwards:  function(programId)
          {
              return "report/RegionAwards?programId=" + programId;
          },
          getPostAwards:  function(programId)
          {
              return "report/PostAwards?programId=" + programId;
          },
          getFocusAwards: function(programId)
          {
              return "report/FocusAwards?programId=" + programId;
          },
          getFocusCategoryAwards: function(programId)
          {
              return "report/FocusCategoryAwards?programId=" + programId;
          },
          getCountryAwards: function(programId)
          {
              return "report/CountryAwards?programId=" + programId;
          },
          getObjectiveAwards: function(programId, objectiveId)
          {
              return "report/ObjectiveAwards?programId=" + programId + "&objectiveId=" + objectiveId;
          }
      };
  });
