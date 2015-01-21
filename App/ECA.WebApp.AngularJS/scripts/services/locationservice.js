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
      get: function (type) {
        var defer = $q.defer();
        DragonBreath.get({type: type}, 'locations')
          .success(function (data) {
             defer.resolve(data);
          });
        return defer.promise;
      },
    };
  });
