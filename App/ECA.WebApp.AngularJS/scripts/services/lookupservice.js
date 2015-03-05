'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # project
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('LookupService', function (DragonBreath, $q) {

      return {
          getAllThemes: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'themes')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },

          getAllGoals: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'goals')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllRegions: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'regions')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllFocusAreas: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'focusAreas')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getAllContacts: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'contacts')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          }
  };
});
