﻿'use strict';

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
          update: function (dependent, id) {
              return DragonBreath.save(dependent, 'people/dependent');
          },
          create: function (dependent) {
              return DragonBreath.create(dependent, 'people/dependent');
          },
          delete: function (personId, dependentId) {
              return DragonBreath.create(dependent, 'person/' + personId + '/dependent/' + dependentId);
          }
      };
  });