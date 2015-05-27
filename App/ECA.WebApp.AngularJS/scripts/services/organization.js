﻿'use strict';

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
          }
      };
  });