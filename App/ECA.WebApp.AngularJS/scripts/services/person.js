﻿'use strict';

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
          create: function (person) {
              var defer = $q.defer();
              DragonBreath.create(person, 'people')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          }
      };
  });
