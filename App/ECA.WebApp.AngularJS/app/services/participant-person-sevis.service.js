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
            processParticipantSevisBatchLog: processParticipantSevisBatchLog,
            verifyExchangeVisitor: verifyExchangeVisitor
        };

        return service;

        function getParticipantPersonsSevisById(projectId, id) {
            return DragonBreath.get('project/' + projectId + '/participantPersonsSevis', id)
            .then(function (response) {
                if (response.data.sevisValidationResult) {
                    response.data.sevisValidationResult = angular.fromJson(response.data.sevisValidationResult);
                }
                if (response.data.sevisBatchResult) {
                    response.data.sevisBatchResult = angular.fromJson(response.data.sevisBatchResult);
                }
                return response;
            });
        };

        function updateParticipantPersonsSevis(projectId, sevisInfo) {
            var path = 'project/' + projectId + '/participantPersonsSevis';
            return DragonBreath.save(sevisInfo, path);
        };

        function sendToSevis(projectId, participantIds) {
            return DragonBreath.create(participantIds, 'project/' + projectId + '/participantPersonsSevis/sendToSevis');
        };

        function verifyExchangeVisitor(projectId, participantId) {
            return DragonBreath.create({}, 'project/' + projectId + '/participant/' + participantId + '/verify');
        }
        
        // update participant sevis batch status
        function processParticipantSevisBatchLog(id) {
            var path = 'ParticipantPersonsSevis/UpdateSevisBatchStatus';
            return DragonBreath.get(path, id);
        };
        
    }
})();