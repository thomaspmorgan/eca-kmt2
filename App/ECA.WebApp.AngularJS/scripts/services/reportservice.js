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
          }
      };
  });
