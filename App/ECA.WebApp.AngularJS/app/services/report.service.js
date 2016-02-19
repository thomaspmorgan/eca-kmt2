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
          getReport: function(parameters) {
              switch (parameters.report.Report) {
                  case "ProjectAwards":
                      return this.getProjectAwards(parameters.program.programId, parameters.country.id, parameters.selectedFormat.key);
                      break;
                  case "RegionAwards":
                      return this.getRegionAwards(parameters.program.programId, parameters.selectedFormat.key);
                      break;
                  case "PostAwards":
                      return this.getPostAwards(parameters.program.programId, parameters.selectedFormat.key);
                      break;
                  case "FocusAwards":
                      return this.getFocusAwards(parameters.program.programId, parameters.selectedFormat.key);
                      break;
                  case "FocusCategoryAwards":
                      return this.getFocusCategoryAwards(parameters.program.programId, parameters.selectedFormat.key);
                      break;
                  case "CountryAwards":
                      return this.getCountryAwards(parameters.program.programId, parameters.selectedFormat.key);
                      break;
                  case "ObjectiveAwards":
                      return this.getObjectiveAwards(parameters.program.programId, parameters.objective.id, parameters.selectedFormat.key);
                      break;
                  case "YearAwards":
                      return this.getYearAwards(parameters.program.programId, parameters.selectedFormat.key);
                      break;
                  case "ProjectsWithGrantNumber":
                      return this.getProjectsWithGrantNumber(parameters.program.programId, parameters.selectedFormat.key);
                      break;
              }
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
