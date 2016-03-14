(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('DefaultExchangeVisitorFundingService', defaultExchangeVisitorFundingService);

    defaultExchangeVisitorFundingService.$inject = ['$q', 'DragonBreath'];

    function defaultExchangeVisitorFundingService($q, DragonBreath) {
        var service = {
            getDefaultExchangeVisitorFundingById: getDefaultExchangeVisitorFundingById,
        };

        return service;

        function getDefaultExchangeVisitorFundingById(projectId, params) {
            var path = 'Project/' + projectId + '/DefaultExchangeVisitorFunding';
            return DragonBreath.get(params,  path);
        };
    }
})();
