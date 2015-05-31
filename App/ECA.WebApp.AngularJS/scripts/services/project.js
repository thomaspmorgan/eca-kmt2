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
          get: function (id) {
              return DragonBreath.get('projects', id)
          },
          getProjectsByProgram: function (id, params) {
              var path = 'programs/' + id + '/projects'
              return DragonBreath.get(params, path);
          },
          update: function (project, id) {
              return DragonBreath.save(project, 'projects')
          },
          create: function (project) {
              return DragonBreath.create(project, 'projects');
          },
          getCollaboratorInfo: function(projectId) {
              return DragonBreath.get('projects/' + projectId + '/collaborators/details');
          },
          getCollaborators: function (projectId, params) {
              var path = 'projects/' + projectId + '/collaborators';
              return DragonBreath.get(params, path);
          },
          updatePermission: function (isAllowed, principalId, projectId, permissionId) {
              var path = '';
              var permissionModel = {
                  principalId: principalId,
                  projectId: projectId,
                  permissionId: permissionId
              };
              if (isAllowed) {
                  path = 'projects/collaborator/add';
              }
              else {
                  path = 'projects/collaborator/revoke';
              }
              return DragonBreath.create(permissionModel, path);
          },
          removePermission: function (principalId, projectId, permissionId) {
              var permissionModel = {
                  principalId: principalId,
                  projectId: projectId,
                  permissionId: permissionId
              };
              return DragonBreath.create(permissionModel, 'projects/collaborator/remove');
          }

      };
  });
