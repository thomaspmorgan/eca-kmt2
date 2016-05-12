'use strict';

/**
 * @ngdoc service
 * @name staticApp.contacts
 * @description
 * # contacts
 * Factory in the staticApp.
 */
angular.module('staticApp')
    .factory('ContactsService', function ($q, DragonBreath) {

      return {

        create: function (contact) {
            return DragonBreath.create(contact, 'contacts');
        },
        update: function (contact) {
            return DragonBreath.save(contact, 'contacts');
        },
        get: function (params) {
            return DragonBreath.get(params, 'contacts');
        }

    };
  });