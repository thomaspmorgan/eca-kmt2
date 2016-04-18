(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ParticipantPersonsSevisService', participantPersonsSevisService);

    participantPersonsSevisService.$inject = ['$q', 'DragonBreath'];

    function participantPersonsSevisService($q, DragonBreath) {
        var service = {
            getSevisParticipantsByProjectId: getSevisParticipantsByProjectId,
            getParticipantPersonsSevisById: getParticipantPersonsSevisById,
            updateParticipantPersonsSevis: updateParticipantPersonsSevis,
            sendToSevis: sendToSevis,
            getSevisCommStatuses: getSevisCommStatuses,
            parseSevisProperties: parseSevisProperties,
            getBatchInfo: getBatchInfo
        };

        return service;

        function getBatchInfo(projectId, participantId, batchId) {
            return DragonBreath.get('project/' + projectId + '/participantpersonssevis/' + participantId + '/batch/' + batchId);
        };

        function getSevisParticipantsByProjectId(projectId, params) {
            return DragonBreath.get(params, 'project/' + projectId + '/sevisParticipants');
        };

        function getSevisCommStatuses(projectId, participantId, params) {
            return DragonBreath.get(params, 'project/' + projectId + '/participantPersonsSevis', participantId + '/CommStatuses');
        };

        function getParticipantPersonsSevisById(projectId, id) {
            return DragonBreath.get('project/' + projectId + '/participantPersonsSevis', id)
            .then(function (response) {
                return service.parseSevisProperties(response);
            });
        };

        function updateParticipantPersonsSevis(projectId, sevisInfo) {
            var path = 'project/' + projectId + '/participantPersonsSevis';
            return DragonBreath.save(sevisInfo, path)
            .then(function (response) {
                return service.parseSevisProperties(response);
            });
        };

        function sendToSevis(applicationId, projectId, participantIds, sevisUsername, sevisOrgId) {
            var model = {
                participantIds: participantIds,
                sevisUsername: sevisUsername,
                sevisOrgId: sevisOrgId
            };
            return DragonBreath.create(model, 'application/' + applicationId + '/project/' + projectId + '/participantPersonsSevis/sendToSevis');
        };

        function parseSevisProperties(response) {
            if (response.data.sevisValidationResult) {
                response.data.sevisValidationResult = angular.fromJson(response.data.sevisValidationResult);
            }
            if (response.data.sevisBatchResult) {
                response.data.sevisBatchResult = angular.fromJson(response.data.sevisBatchResult);
            }
            return response;
        };
    }
})();