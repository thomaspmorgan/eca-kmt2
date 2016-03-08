'use strict';

/**
 * @ngdoc service
 * @name staticApp.participantPersons
 * @description
 * # participantPersons
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ParticipantPersonsService', function ($q, DragonBreath) {

      return {
          updateParticipantPerson: function(projectId, params){
              return DragonBreath.save(params, 'project/' + projectId + '/ParticipantPersons');
          },          
          getParticipantsByProject: function (id, params) {
              var defer = $q.defer();
              var path = 'project/' + id + "/participantPersons";
              DragonBreath.get(params, path)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getParticipantPersonsById: function (projectId, id) {
              return DragonBreath.get('project/' + projectId, '/participantPersons', id);
          }
      };
  });
