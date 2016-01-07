(function () {
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
            updateParticipantPersonsSevis: updateParticipantPersonsSevis,
            sendToSevis: sendToSevis,
            validateParticipantPersonsCreateSevis: validateParticipantPersonsCreateSevis,
            validateParticipantPersonsUpdateSevis: validateParticipantPersonsUpdateSevis
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
            return DragonBreath.save(sevisInfo, path);
        };

        function sendToSevis(participantIds) {
            return DragonBreath.create(participantIds, 'participantPersonsSevis/sendToSevis');
        }
        
        // validate a sevis create object
        function validateParticipantPersonsCreateSevis(id) {
            var path = 'ParticipantPersonsSevis/ValidateCreateSevis';
            return DragonBreath.get(path, id);
        };

        // validate a sevis update object
        function validateParticipantPersonsUpdateSevis(id) {
            var path = 'ParticipantPersonsSevis/ValidateUpdateSevis';
            return DragonBreath.get(path, id);
        };

        // update participant sevis status
        function updateParticipantSevisCommStatus(id, params) {
            var path = 'ParticipantPersonsSevis/UpdateSevisCommStatus/' + id;
            return DragonBreath.get(params, path);
        };

    }
})();