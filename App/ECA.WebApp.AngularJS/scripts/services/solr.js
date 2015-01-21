'use strict';

/**
 * @ngdoc service
 * @name staticApp.solr
 * @description
 * # solr
 * Service in the staticApp.
 */
angular.module('staticApp')
	.constant('SEARCH_PREFIX', '/search/')
	.constant('SEARCH_ENDPOINT', '')
  .factory('solr', function($http, SEARCH_ENDPOINT, SEARCH_PREFIX) {

    var get = function(query, facets, filters, limit){
      query = query || '*';
      limit = limit || 10;
      var path = SEARCH_ENDPOINT+SEARCH_PREFIX;
      var conf = {
        params:{
          facets:['doctype','endDate'],
          query:query,
          limit:limit
        }
      };

      return $http.get(path,conf);
    };

    return {
      get: get
    };
    // AngularJS will instantiate a singleton by calling "new" on this function
  });
