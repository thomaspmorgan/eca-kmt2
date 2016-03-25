(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('DefaultExchangeVisitorFundingService', defaultExchangeVisitorFundingService);

    defaultExchangeVisitorFundingService.$inject = ['$q', 'DragonBreath'];

    function defaultExchangeVisitorFundingService($q, DragonBreath) {
        var service = {
            getDefaultExchangeVisitorFundingById: getDefaultExchangeVisitorFundingById,
            updateDefaultExchangeVisitorFunding: updateDefaultExchangeVisitorFunding,
        };

        return service;

        function getDefaultExchangeVisitorFundingById(projectId, params) {
            var path = 'Project/' + projectId + '/DefaultExchangeVisitorFunding';
            return DragonBreath.get(params,  path);
        };

        function updateDefaultExchangeVisitorFunding(projectId, defaultExchangeVisitorFunding) {
            var path = 'Project/' + projectId + '/DefaultExchangeVisitorFunding';
            return DragonBreath.save(defaultExchangeVisitorFunding, path);
        };
    }
})();
