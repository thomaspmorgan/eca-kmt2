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
              DragonBreath.get('people/dependent/' + dependentId)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          update: function (dependent) {
              return DragonBreath.save(dependent, 'people/dependent');
          },
          create: function (dependent) {
              return DragonBreath.create(dependent, 'dependent');
          },
          delete: function (dependentId) {
              return DragonBreath.delete(dependentId, 'people/dependent/' + dependentId);
          }
      };
  });
