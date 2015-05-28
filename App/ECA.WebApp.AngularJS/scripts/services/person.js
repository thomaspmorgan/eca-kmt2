'use strict';

/**
 * @ngdoc service
 * @name staticApp.person
 * @description
 * # person
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('PersonService', function ($q, DragonBreath) {

      return {
          getGeneralById: function (id) {
              var defer = $q.defer();
              DragonBreath.get('people/' + id + '/general')
                .success(function (data) {
                    defer.resolve(data);
                })
              return defer.promise;
          },
          getPiiById: function (id) {
              var defer = $q.defer();
              DragonBreath.get('people/' + id + '/pii')
                .success(function (data) {
                    defer.resolve(data);
                })
              return defer.promise;
          },
          getContactInfoById: function (id) {
              var defer = $q.defer();
              DragonBreath.get('people/' + id + '/contactInfo')
                .success(function (data) {
                    defer.resolve(data);
                })
              return defer.promise;
          },
          updatePii: function (pii, id) {
              return DragonBreath.save(pii, 'people/pii')
          },
          create: function (person) {
              return DragonBreath.create(person, 'people');
          }
      };
  });
