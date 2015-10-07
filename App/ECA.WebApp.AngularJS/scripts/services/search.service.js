'use strict';

/**
 * @ngdoc service
 * @name staticApp.SearchService
 * @description
 * # SearchService
 * Service to retrieve search results.
 */
angular.module('staticApp')
  .factory('SearchService', function (DragonBreath) {

      return {
          postSearch: function (params) {
              return DragonBreath.create(params, 'Search');
          },
          getDocInfo: function (id) {
              return DragonBreath.get('Documents', id);
          }
      }
      
  });


