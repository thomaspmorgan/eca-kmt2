'use strict';

/**
 * @ngdoc service
 * @name staticApp.dependent
 * @description
 * # dependent
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('DependentService', function ($q, DragonBreath) {

      return {
          getDependentById: function (dependentId) {
              var defer = $q.defer();
              DragonBreath.get('person/' + dependentId + '/dependent')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getDependents: function (params) {
              return DragonBreath.get(params, 'dependent');
          },
          update: function (personId, dependent) {
              return DragonBreath.save(dependent, 'dependent');
          },
          create: function (dependent) {
              return DragonBreath.create(dependent, 'dependent');
          },
          delete: function (dependent) {
              return DragonBreath.create(dependent, 'dependent');
          }
      };
  });
