'use strict';

/**
 * @ngdoc service
 * @name staticApp.SearchService
 * @description
 * # SearchService
 * Service to retrieve search results.
 */
angular.module('staticApp')
  .factory('SearchService', function() {

      var dataItems = [
          {
              "id": 1,
              "title": "doc one",
              "subtitle": "doc one subtitle",
              "type": "archive"
          },
          {
              "id": 2,
              "title": "doc two",
              "subtitle": "doc two subtitle",
              "type": "archive"
          },
          {
              "id": 3,
              "title": "doc three",
              "subtitle": "doc three subtitle",
              "type": "archive"
          },
          {
              "id": 4,
              "title": "doc four",
              "subtitle": "doc four subtitle",
              "type": "archive"
          },
          {
              "id": 5,
              "title": "doc five",
              "subtitle": "doc five subtitle",
              "type": "archive"
          },
          {
              "id": 6,
              "title": "doc six",
              "subtitle": "doc six subtitle",
              "type": "archive"
          },
          {
              "id": 7,
              "title": "doc seven",
              "subtitle": "doc seven subtitle",
              "type": "archive"
          },
          {
              "id": 8,
              "title": "doc eight",
              "subtitle": "doc eight subtitle",
              "type": "archive"
          },
          {
              "id": 9,
              "title": "doc nine",
              "subtitle": "doc nine subtitle",
              "type": "archive"
          },
          {
              "id": 10,
              "title": "doc ten",
              "subtitle": "doc ten subtitle",
              "type": "archive"
          },
          {
              "id": 11,
              "title": "doc eleven",
              "subtitle": "doc eleven subtitle",
              "type": "archive"
          },
          {
              "id": 12,
              "title": "doc twelve",
              "subtitle": "doc twelve subtitle",
              "type": "archive"
          },
          {
              "id": 13,
              "title": "doc thirteen",
              "subtitle": "doc two thirteen",
              "type": "archive"
          }
      ];
      var mFac = {};

      mFac.search = function (query, facets, filters, limit) {
          var arrayToReturn = [];
          for (var i = 0; i < dataItems.length; i++) {
              if (dataItems[i].title.toLocaleLowerCase().indexOf(query.toLocaleLowerCase()) >= 0) {
                  arrayToReturn.push(dataItems[i]);
              }
          }
          return arrayToReturn;
      };
      

      return {
          search: function (query, facets, filters, limit) {
              return mFac.search(query, facets, filters, limit);
          }
      }
      
  });


