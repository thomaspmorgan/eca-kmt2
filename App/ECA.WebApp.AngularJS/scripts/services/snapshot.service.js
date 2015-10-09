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
          getProgramSnapshot: function (id) {
              return DragonBreath.get('ProgramSnapshot', id);
          }
      }

  });


