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
              DragonBreath.get('dependents/' + dependentId)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          update: function (dependent) {
              return DragonBreath.save(dependent, 'dependents');
          },
          create: function (dependent) {
              return DragonBreath.create(dependent, 'people/dependents');
          },
          delete: function (dependent) {
              return DragonBreath.save(dependent, 'dependents/' + dependent.dependentId);
          }
      };
  });
