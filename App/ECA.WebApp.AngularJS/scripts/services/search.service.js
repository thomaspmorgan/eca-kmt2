'use strict';

/**
 * @ngdoc service
 * @name staticApp.SearchService
 * @description
 * # SearchService
 * Service to retrieve search results.
 */
angular.module('staticApp')
  .factory('SearchService', function (DragonBreath, $q) {

      return {
          getAll: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'Search')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          get: function (id) {
              var defer = $q.defer();
              DragonBreath.get('Documents', id)
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          }
      }
      
  });


