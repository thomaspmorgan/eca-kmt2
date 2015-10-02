'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # project
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ProjectService', function (DragonBreath, $q, AuthService, ConstantsService) {


      return {
          get: function(params){
              return DragonBreath.get(params, '/projects');
          },
          getById: function (id) {
              return DragonBreath.get('projects', id);
          },
          getProjectsByProgram: function (id, params) {
              var path = 'programs/' + id + '/projects';
              return DragonBreath.get(params, path);
          },
          getProjectsByPersonId: function (id, params) {
              var path = 'people/' + id + '/projects';
              return DragonBreath.get(params, path);
          },
          update: function (project, id) {
              return DragonBreath.save(project, 'projects')
          },
          create: function (project) {
              return DragonBreath.create(project, 'projects');
          },
          getCategories: function (programId, params) {
              var path = 'programs/' + programId + '/categories';
              return DragonBreath.get(params, path);
          },
          getObjectives: function (programId, params) {
              var path = 'programs/' + programId + '/objectives';
              return DragonBreath.get(params, path);
          },
          getCollaboratorInfo: function(projectId) {
              return DragonBreath.get('projects/' + projectId + '/collaborators/details');
          },
          getCollaborators: function (projectId, params) {
              var path = 'projects/' + projectId + '/collaborators';
              return DragonBreath.get(params, path);
          },
          addPermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'projects/collaborator/add');
          },
          revokePermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'projects/collaborator/revoke');
          },
          removePermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'projects/collaborator/remove');
          },
          addPersonParticipant: function (params) {
              return DragonBreath.create(params, 'projects/participants/person/add');
          },
          addOrganizationParticipant: function (params) {
              return DragonBreath.create(params, 'projects/participants/organization/add');
          }

      };
  });
