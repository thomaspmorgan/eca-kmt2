'use strict';

/**
 * @ngdoc service
 * @name staticApp.participantPerson
 * @description
 * # participantPerson
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ParticipantPersonsService', function ($q, DragonBreath) {

      return {
          getParticipantPersons: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'participantPersons')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getParticipantsByProject: function (id, params) {
              var defer = $q.defer();
              var path = 'projects/' + id + "/participantPersons";
              DragonBreath.get(params, path)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getParticipantPersonsById: function (id) {
              return DragonBreath.get('participantPersons', id);
          }
      };
  });
