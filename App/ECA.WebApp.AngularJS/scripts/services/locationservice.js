'use strict';

/**
 * @ngdoc service
 * @name staticApp.LocationService
 * @description
 * # LocationService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('LocationService', function (DragonBreath, $q) {
    return {
      get: function (params) {
        var defer = $q.defer();
        DragonBreath.get(params, 'locations')
          .success(function (data) {
             defer.resolve(data);
          });
        return defer.promise;
      },
    };
  });
