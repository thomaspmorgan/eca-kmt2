'use strict';

/**
 * @ngdoc service
 * @name staticApp.DragonBreath
 * @description
 * # DragonBreath
 * Factory in the staticApp.
 */
angular.module('staticApp')
	.constant('API_PREFIX', '/api/')
	.constant('API_ENDPOINT', '')
  .factory('DragonBreath', function ($http, API_ENDPOINT, API_PREFIX) {

  	function DragonPath (args, slicePos) {
  		this.path = API_ENDPOINT + API_PREFIX + Array.prototype.slice.call(args, slicePos).join('/');
  	}

    return {
      get: function (filter) {
      	var dPath;
      	if (typeof filter === 'string') {
          dPath = new DragonPath(arguments);
          return $http.get(dPath.path);
        } 
      	dPath = new DragonPath(arguments, 1);
      	return $http.get(dPath.path, {params: filter});
      },
      save: function (object) {
      	var dPath = new DragonPath(arguments, 1);
      	return $http.put(dPath.path, object);
      },
      create: function (object) {
      	var dPath = new DragonPath(arguments, 1);
       	return $http.post(dPath.path, object);
      },
      delete: function () {
      	var dPath = new DragonPath(arguments, 1);
      	return $http.delete(dPath.path);	
      }
    };
  });
