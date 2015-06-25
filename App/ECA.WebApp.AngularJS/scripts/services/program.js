'use strict';

/**
 * @ngdoc service
 * @name staticApp.program
 * @description
 * # program
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ProgramService', function (DragonBreath, $q) {
      var newProgram;
      function getProgram(data) {
          if (data.results) {
              newProgram = data.results[0];
          } else {
              newProgram = data;
          }
      }

      return {
          get: function (id) {
              var defer = $q.defer();
              DragonBreath.get('programs', id)
                .success(function (data) {
                    getProgram(data);
                    defer.resolve(newProgram);
                });
              return defer.promise;
          },
          getAllProgramsAlpha: function (params) {
              var defer = $q.defer();
              var path = 'programs/Alpha';
              DragonBreath.get(params, path)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getCategories: function (officeId, params) {
              var path = 'offices/' + officeId + '/categories';
              return DragonBreath.get(params, path);
          },
          getObjectives: function (officeId, params) {
              var path = 'offices/' + officeId + '/objectives';
              return DragonBreath.get(params, path);
          },
          getCollaborators: function (programId, params) {
              var path = 'programs/' + programId + '/collaborators';
              return DragonBreath.get(params, path);
          },
          getAllProgramsHierarchy: function (params) {
              var defer = $q.defer();
              var path = 'programs/Hierarchy';
              DragonBreath.get(params,path)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          update: function (program, id) {
              var defer = $q.defer();
              DragonBreath.save(program, 'programs', id)
                .success(function (data) {
                    getProgram(data);
                    defer.resolve(newProgram);
                })
                .error(function (data) {
                    getProgram(data);
                    defer.resolve(newProgram);
                });
              return defer.promise;
          },
          create: function (program) {
              var defer = $q.defer();
              DragonBreath.create(program, 'programs')
                .success(function (data) {
                    getProgram(data);
                    defer.resolve(newProgram);
                })
                .error(function (data) {
                    getProgram(data);
                    defer.resolve(newProgram);
                });
              return defer.promise;
          }
      };
  });
