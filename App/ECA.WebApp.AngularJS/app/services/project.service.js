'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # project
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ProjectService', function (DragonBreath, $q, AuthService, ConstantsService, DateTimeService) {
      var colorIndex = 0;
      var colors = ["#d35304", "#8e9900", "#689e0c", "#640096", "#dd067d", "#028287", "#ea007d", "#024c60", "#0da514", "#e216af", "#9e0910", "#0b6293", "#51a508", "#1c9e0e", "#dbb702", "#078435", "#8c0c21", "#460f9e", "#319e03", "#064260", "#bc0b6f", "#0da591", "#0f0666", "#2c930d", "#054175", "#c405a1", "#9e0803", "#02426b", "#6e9601", "#576d00", "#099620", "#0c7287", "#89a309", "#e05d06", "#98a004", "#027f5c", "#0e8c5d", "#2a7a00", "#071f8c", "#e50d80", "#017756", "#cc6a14", "#007a78", "#056482", "#007267", "#af0e08", "#076677", "#9b1d07", "#350d84", "#87011e", "#0b5570", "#e56814", "#8c0c35", "#69960f", "#646d02", "#4a820a", "#af001d", "#ad1608", "#068934", "#023e70", "#efbc02", "#ce0ca1", "#e008ae", "#ef09ef"];

      return {
          get: function (params) {
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
          getCollaboratorInfo: function (projectId) {
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
