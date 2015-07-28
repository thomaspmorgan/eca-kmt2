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
          getCategories: function (programId, params) {
              var path = 'programs/' + programId + '/categories';
              return DragonBreath.get(params, path);
          },
          getObjectives: function (programId, params) {
              var path = 'programs/' + programId + '/objectives';
              return DragonBreath.get(params, path);
          },
          getSubPrograms: function (programId, params) {
              var path = 'programs/' + programId + '/subprograms';
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
          },
          addPermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'programs/collaborator/add');
          },
          revokePermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'programs/collaborator/revoke');
          },
          removePermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'programs/collaborator/remove');
          }
      };
  });
