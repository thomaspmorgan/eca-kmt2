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
          getProjectAwards: function(programId, countryId, format) {
              return "report/ProjectAwards?programId=" + programId + "&countryId=" + countryId + "&format=" + format;
          },
          getRegionAwards:  function(programId, format)
          {
              return "report/RegionAwards?programId=" + programId + "&format=" + format;
          },
          getPostAwards:  function(programId, format)
          {
              return "report/PostAwards?programId=" + programId + "&format=" + format;
          },
          getFocusAwards: function(programId, format)
          {
              return "report/FocusAwards?programId=" + programId + "&format=" + format;
          },
          getFocusCategoryAwards: function(programId, format)
          {
              return "report/FocusCategoryAwards?programId=" + programId + "&format=" + format;
          },
          getCountryAwards: function(programId, format)
          {
              return "report/CountryAwards?programId=" + programId + "&format=" + format;
          },
          getObjectiveAwards: function(programId, objectiveId, format)
          {
              return "report/ObjectiveAwards?programId=" + programId + "&objectiveId=" + objectiveId + "&format=" + format;
          },
          getYearAwards: function(programId, format)
          {
              return "report/YearAwards?programId=" + programId + "&format=" + format;
          },
          getProjectsWithGrantNumber: function(programId, format)
          {
              return "report/ProjectsWithGrantNumber?programId=" + programId + "&format=" + format;
          }
      };
  });
