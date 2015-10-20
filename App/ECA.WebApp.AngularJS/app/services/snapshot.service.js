'use strict';

/**
 * @ngdoc service
 * @name staticApp.SnapshotService
 * @description
 * # SnapshotService
 * Service to retrieve search results.
 */
angular.module('staticApp')
  .factory('SnapshotService', function (DragonBreath) {

      return {
          getProgramCountryCount: function (id) {
              return DragonBreath.get('ProgramCountryCount', id);
          },
          getProgramSnapshot: function (id) {
              return DragonBreath.get('ProgramSnapshot', id);
          }
      }

  });


