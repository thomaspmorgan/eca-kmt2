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
          getEvaluationNotesById: function (id) {
              var defer = $q.defer();
              DragonBreath.get('people/' + id + '/evaluationNotes')
                .success(function (data) {
                    defer.resolve(data);
                })
              return defer.promise;
          },
          getEducationsById: function (id) {
              var defer = $q.defer();
              DragonBreath.get('people/' + id + '/education')
                .success(function (data) {
                    defer.resolve(data);
                })
              return defer.promise;
          },
          getEmploymentsById: function (id) {
              var defer = $q.defer();
              DragonBreath.get('people/' + id + '/employment')
                .success(function (data) {
                    defer.resolve(data);
                })
              return defer.promise;
          },
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
          getPersonById: function (id) {
              var defer = $q.defer();
              DragonBreath.get('/person/' + id)
                .success(function (data) {
                    defer.resolve(data);
                })
              return defer.promise;
          },
          getPeople: function (params) {
              return DragonBreath.get(params, 'people');
          },
          updatePii: function (pii, id) {
              return DragonBreath.save(pii, 'people/pii')
          },
          create: function (person) {
              return DragonBreath.create(person, 'people');
          },
          addAddress: function (address) {
              return DragonBreath.create(address, 'people/address');
          }
      };
  });
