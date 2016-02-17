(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ParticipantPersonsSevisService', participantPersonsSevisService);

    participantPersonsSevisService.$inject = ['$q','DragonBreath'];

    function participantPersonsSevisService($q, DragonBreath) {
        var service = {
            getParticipantPersonsSevisById: getParticipantPersonsSevisById,
            updateParticipantPersonsSevis: updateParticipantPersonsSevis,
            sendToSevis: sendToSevis,
            validateParticipantPersonsCreateSevis: validateParticipantPersonsCreateSevis,
            validateParticipantPersonsUpdateSevis: validateParticipantPersonsUpdateSevis,
            createParticipantSevisCommStatus: createParticipantSevisCommStatus,
        };

        return service;

        function getParticipantPersonsSevisById(projectId, id) {
            return DragonBreath.get('project/' + projectId + '/participantPersonsSevis', id);
        };

        function updateParticipantPersonsSevis(projectId, sevisInfo) {
            var path = 'project/' + projectId + '/participantPersonsSevis';
            return DragonBreath.save(sevisInfo, path);
        };

        function sendToSevis(projectId, participantIds) {
            return DragonBreath.create(participantIds, 'project/' + projectId + '/participantPersonsSevis/sendToSevis');
        };
        
        // validate a sevis create object
        function validateParticipantPersonsCreateSevis(projectId, id) {
            var path = 'project/' + projectId + '/ParticipantPersonsSevis/ValidateCreateSevis';
            return DragonBreath.get(path, id);
        };

        // validate a sevis update object
        function validateParticipantPersonsUpdateSevis(projectId, id) {
            var path = 'project/' + projectId + '/ParticipantPersonsSevis/ValidateUpdateSevis';
            return DragonBreath.get(path, id);
        };

        // create participant sevis status
        function createParticipantSevisCommStatus(projectId, id, params) {
            var path = 'project/' + projectId +'/ParticipantPersonsSevis/' + id + '/CreateSevisCommStatus';
            return DragonBreath.create(params, path);
        };
        
    }
})();