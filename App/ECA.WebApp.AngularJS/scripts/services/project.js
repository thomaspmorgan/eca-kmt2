'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # project
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ProjectService', function (DragonBreath, $q) {


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
          getCollaborators: function (projectId, params) {
              var path = 'projects/' + projectId + '/collaborators';
              return DragonBreath.get(params, path);
          }
      };
  });
