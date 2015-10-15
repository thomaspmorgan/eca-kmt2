'use strict';

/**
 * @ngdoc service
 * @name staticApp.SearchService
 * @description
 * # SearchService
 * Service to retrieve search results.
 */
angular.module('staticApp')
  .factory('SearchService', function (DragonBreath, ConstantsService) {

      var kmtId = ConstantsService.kmtApplicationResourceId;
      return {
          postSearch: function (params) {
              return DragonBreath.create(params, 'Search/' + kmtId);
          },
          getDocInfo: function (id) {
              return DragonBreath.get('Documents/' + kmtId + '/' , id);
          },
          getFieldNames: function () {
              return DragonBreath.get('Documents/' + kmtId + '/Fields');
          }
      }
  });


