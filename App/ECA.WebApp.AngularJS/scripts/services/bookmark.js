'use strict';

/**
 * @ngdoc service
 * @name staticApp.BookmarkService
 * @description
 * # BookmarkService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('BookmarkService', function (DragonBreath) {

      return {

          getBookmarks: function (params)  {
              return DragonBreath.get(params, 'bookmarks');
          },

          createBookmark: function (bookmark) {
              return DragonBreath.create(bookmark, 'bookmarks');
          },

          deleteBookmark: function (id) {
              return DragonBreath.delete(id, 'bookmarks');
          }
      };
  });