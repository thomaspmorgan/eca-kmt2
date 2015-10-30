﻿(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ParticipantPersonsSevisService', participantPersonsSevisService);

    participantPersonsSevisService.$inject = ['$q','DragonBreath'];

    function participantPersonsSevisService($q, DragonBreath) {
        var service = {
            getParticipantPersonsSevis: getParticipantPersonsSevis,
            getParticipantPersonsSevisByProject: getParticipantPersonsSevisByProject,
            getParticipantPersonsSevisById: getParticipantPersonsSevisById,
            getParticipantPersonsSevisCommStatusesById: getParticipantPersonsSevisCommStatusesById,
            saveParticipantPersonSevis: updateParticipantPersonsSevis
        };

        return service;

        function getParticipantPersonsSevis(params) {
            var defer = $q.defer();
            DragonBreath.get(params, 'participantPersonsSevis')
              .success(function (data) {
                  defer.resolve(data);
              });
            return defer.promise;
        };

        function getParticipantPersonsSevisByProject(id, params) {
            var defer = $q.defer();
            var path = 'projects/' + id + "/participantPersonsSevis";
            DragonBreath.get(params, path)
              .success(function (data) {
                  defer.resolve(data);
              });
            return defer.promise;
        };

        function getParticipantPersonsSevisById(id) {
            return DragonBreath.get('participantPersonsSevis', id);
        };

        function getParticipantPersonsSevisCommStatusesById(id, params) {
            var path = 'participantPersonsSevis/' + id + '/sevisCommStatuses';
            return DragonBreath.get(params, path);
        };

        function updateParticipantPersonsSevis(sevisInfo) {
            var path = 'participantPersonsSevis';
            return DragonBreath.save(path, sevisInfo);
        };
    }
})();