(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('NavigationService', navigationService);

    navigationService.$inject = ['$q', '$rootScope', 'ConstantsService'];

    function navigationService($q, $rootScope, ConstantsService) {
        var service = {
            updateBreadcrumbs: updateBreadcrumbs
        };

        function updateBreadcrumbs(){
            $rootScope.$broadcast(ConstantsService.updateBreadcrumbsEventName);
        }

        return service;

    }
})();