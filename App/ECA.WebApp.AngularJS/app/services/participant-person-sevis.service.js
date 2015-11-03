(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ParticipantPersonsSevisService', participantPersonSevisService);

    participantPersonSevisService.$inject = ['$q','DragonBreath'];

    function participantPersonSevisService($q, DragonBreath) {
        var service = {
            getParticipantPersonsSevis: getParticipantPersonsSevis,
            getParticipantPersonsSevisByProject: getParticipantPersonsSevisByProject,
            getParticipantPersonsSevisById: getParticipantPersonsSevisById,
            getParticipantPersonsSevisCommStatusesById: getParticipantPersonsSevisCommStatusesById,
            sendToSevis: sendToSevis
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
            var path = 'participantPersonSevis/' + id + '/sevisCommStatuses';
            return DragonBreath.get(params, path);
        };

        function sendToSevis(participantIds) {
            return DragonBreath.create(participantIds, 'participantPersonsSevis/sendToSevis');
        }
    }
})();