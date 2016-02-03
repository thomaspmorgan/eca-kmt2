(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ContactsService', contactsService);

    contactsService.$inject = ['$q', 'DragonBreath'];

    function contactsService($q, DragonBreath) {
        var service = {
            create: create,
            get: get,
        };

        return service;

        function create(contact) {
            return DragonBreath.create(contact, 'contacts');
        };

        function get(params) {
            return DragonBreath.get(params, 'contacts');
        };

    }
})();