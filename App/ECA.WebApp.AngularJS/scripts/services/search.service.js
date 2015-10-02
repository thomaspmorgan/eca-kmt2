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
          // (query, facets, filters, limit)
          getAll: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'Search')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          }
      }
      
  });


