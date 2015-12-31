'use strict';

/**
 * @ngdoc service
 * @name staticApp.program
 * @description
 * # program
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ProgramService', function (DragonBreath, ConstantsService, $q) {
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
          getValidParentPrograms: function (programId, params) {
              var path = 'programs/' + programId + '/ParentPrograms';
              return DragonBreath.get(params, path);
          },

          //if parentProgramId is defined, the first level children will be returned
          //if parentProgramId is not defined, then the root programs returned
          getAllProgramsHierarchy: function (params, parentProgramId) {
              var me = this;
              var path = 'programs/Hierarchy';
              params = params || {};
              params.filter = params.filter || [];

              if (parentProgramId) {
                  params.filter.push({
                      comparison: ConstantsService.equalComparisonType,
                      property: 'parentProgram_ProgramId',
                      value: parentProgramId
                  });
              }
              else {
                  params.filter.push({
                      comparison: ConstantsService.equalComparisonType,
                      property: 'programLevel',
                      value: 0
                  });
              }

              return DragonBreath.get(params, path)
                .success(function (data) {
                    angular.forEach(data.results, function (program, index) {
                        program.isRoot = program.programLevel === 0;
                    });
                    return data;
                });
          },

          setChildrenOfProgramHierarchy: function (programsAsFlatList) {
              for (var i = 0; i < programsAsFlatList.length; i++) {
                  var parent = programsAsFlatList[i];
                  if (!parent.parentProgram_ProgramId) {
                      parent.isRoot = true;
                  }
                  else {
                      parent.isRoot = false;
                  }
                  parent.children = parent.children || [];

                  for (var j = 0; j < programsAsFlatList.length; j++) {
                      var potentialChild = programsAsFlatList[j];
                      if (potentialChild.parentProgram_ProgramId && potentialChild.parentProgram_ProgramId === parent.programId) {
                          parent.children.push(potentialChild);
                          potentialChild.parent = parent;
                      }
                  }
              }
          },

          update: function (program, id) {
              return DragonBreath.save(program, 'programs');
          },
          create: function (program) {
              return DragonBreath.create(program, 'programs');
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
