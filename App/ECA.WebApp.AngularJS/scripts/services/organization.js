'use strict';

/**
 * @ngdoc service
 * @name staticApp.organization
 * @description
 * # organization
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('OrganizationService', function ($q, DragonBreath) {
      return {
          getOrganizations: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'organizations')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getById: function (organizationId) {
              return DragonBreath.get('organizations', organizationId)
          },
          getTypes: function (params) {
              return DragonBreath.get(params, 'organizations/types');
          },
          update: function (organization) {
              return DragonBreath.save(organization, 'organizations');
          },
          addAddress: function (address) {
              return DragonBreath.create(address, 'organizations/address');
          },
          addSocialMedia: function (socialMedia) {
              return DragonBreath.create(socialMedia, 'organizations/socialmedia');
          }
      };
  });
