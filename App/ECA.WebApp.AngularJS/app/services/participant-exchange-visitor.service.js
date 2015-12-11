(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ParticipantExchangeVisitorService', participantExchangeVisitorService);

    participantExchangeVisitorService.$inject = ['$q', 'DragonBreath'];

    function participantExchangeVisitorService($q, DragonBreath) {
        var service = {
            getParticipantExchangeVisitors: getParticipantExchangeVisitors,
            getParticipantExchangeVisitorsByProject: getParticipantExchangeVisitorsByProject,
            getParticipantExchangeVisitorById: getParticipantExchangeVisitorById,
            updateParticipantExchangeVisitor: updateParticipantExchangeVisitor
        };

        return service;

        function getParticipantExchangeVisitors(params) {
            var defer = $q.defer();
            DragonBreath.get(params, 'ParticipantExchangeVisitors')
              .success(function (data) {
                  defer.resolve(data);
              });
            return defer.promise;
        };

        function getParticipantExchangeVisitorsByProject(id, params) {
            var defer = $q.defer();
            var path = 'projects/' + id + "/ParticipantExchangeVisitors";
            DragonBreath.get(params, path)
              .success(function (data) {
                  defer.resolve(data);
              });
            return defer.promise;
        };

        function getParticipantExchangeVisitorById(id) {
            return DragonBreath.get('ParticipantExchangeVisitors', id);
        };


        function updateParticipantExchangeVisitor(exchangeVisitorInfo) {
            var path = 'ParticipantExchangeVisitors';
            return DragonBreath.save(exchangeVisitorInfo, path);
        };
    }
})();