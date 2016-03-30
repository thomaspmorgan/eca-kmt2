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
          update: function (dependent) {
              return DragonBreath.save('people/dependent', dependent);
          },
          create: function (dependent) {
              return DragonBreath.create('person/dependent', dependent);
          }
      };
  });
