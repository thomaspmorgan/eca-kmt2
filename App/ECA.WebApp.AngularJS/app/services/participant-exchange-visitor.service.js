(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ParticipantExchangeVisitorService', participantExchangeVisitorService);

    participantExchangeVisitorService.$inject = ['$q', 'DragonBreath'];

    function participantExchangeVisitorService($q, DragonBreath) {
        var service = {
            getParticipantExchangeVisitorById: getParticipantExchangeVisitorById,
            updateParticipantExchangeVisitor: updateParticipantExchangeVisitor
        };

        return service;

        function getParticipantExchangeVisitorById(projectId, id) {
            return DragonBreath.get('Project/' + projectId + '/ParticipantExchangeVisitors', id);
        };
        
        function updateParticipantExchangeVisitor(projectId, exchangeVisitorInfo) {
            var path = 'Project/' + projectId + '/ParticipantExchangeVisitors';
            return DragonBreath.save(exchangeVisitorInfo, path);
        };
    }
})();