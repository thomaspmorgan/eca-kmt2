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
          getOrganizationsWoFundingSource: function (params) {
              return DragonBreath.get(params, 'organizationsWOFundingSource');
          },
          getOrganizationsHierarchyByRoleId : function (params) {
              return DragonBreath.get(params, 'organizations/hierarchyByRoleId');
          },
          getById: function (organizationId) {
              return DragonBreath.get('organizations', organizationId);
          },
          getTypes: function (params) {
              return DragonBreath.get(params, 'organizations/types');
          },
          update: function (organization) {
              return DragonBreath.save(organization, 'organizations');
          },
          create: function (organization) {
              return DragonBreath.create(organization, 'organizations');
          },
          createParticipantOrganization: function (organization) {
              return DragonBreath.create(organization, 'participants/organizations');
          }
      };
  });
