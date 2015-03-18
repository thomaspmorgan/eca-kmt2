'use strict';

/**
 * @ngdoc service
 * @name staticApp.person
 * @description
 * # participant
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ParticipantService', function ($q, DragonBreath) {

    return {
      getParticipants: function (params) {
        var defer = $q.defer();
        DragonBreath.get(params, 'participants')
          .success(function (data) {
             defer.resolve(data);
          });
        return defer.promise;
      },
      getParticipantsByProject: function (id, params) {
        var defer = $q.defer();
        var path = 'projects/' + id + "/participants";
        DragonBreath.get(params, path)
          .success(function (data) {
              defer.resolve(data);
          });
        return defer.promise;
      },
      getParticipantById: function (id) {
          var defer = $q.defer();
          DragonBreath.get('participants', id)
            .success(function (data) {
                defer.resolve(data);
            })
          return defer.promise;
      }
    };
  });
