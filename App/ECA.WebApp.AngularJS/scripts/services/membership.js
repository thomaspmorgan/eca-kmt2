'use strict';

/**
 * @ngdoc service
 * @name staticApp.membership
 * @description
 * # membership
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('MembershipService', function ($q, DragonBreath) {

      return {
          addMembership: function (membership, personId) {
                return DragonBreath.create(membership, 'people/' + personId + '/membership');
          },
          updateMembership: function (membership, personId) {
              return DragonBreath.save(membership, 'people/' + personId + '/membership');
          },
          deleteMembership: function (membership, personId) {
              return DragonBreath.delete(membership, 'people/' + personId + '/membership/' + membership.id);
          }
      };
  });