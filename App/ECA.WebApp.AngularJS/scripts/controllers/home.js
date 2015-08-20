'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:HomeCtrl
 * @description
 * # HomeCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('HomeCtrl', function ($rootScope, $scope, $state, $window, AuthService, BookmarkService, NotificationService) {

      $scope.loadingBookmarks = true;

      $scope.tabs = {
          shortcuts: {
              title: 'Your Shortcuts',
              path: 'shortcuts',
              active: true,
              order: 1
          },
          notfications: {
              title: 'Notifications & Timeline',
              path: 'notifications',
              active: true,
              order: 2
          },
          news: {
              title: 'News (3)',
              path: 'news',
              active: true,
              order: 3
          }
      };

      $scope.getHref = function (bookmark) {

          var href;

          if (bookmark.type === 'Office') {
              href = $state.href('offices.overview', { officeId: bookmark.officeId });
          } else if (bookmark.type === 'Program') {
              href = $state.href('programs.overview', { programId: bookmark.programId });
          } else if (bookmark.type === 'Project') {
              href = $state.href('projects.overview', { officeId: bookmark.officeId, programId: bookmark.programId, projectId: bookmark.projectId });
          } else if (bookmark.type === 'Person') {
              href = $state.href('people.personalinformation', { personId: bookmark.personId });
          } else if (bookmark.type === 'Organization') {
              href = $state.href('organizations.overview', { organizationId: bookmark.organizationId });
          }

          return href;
      }

      $scope.deleteBookmark = function (bookmark) {
          BookmarkService.deleteBookmark(bookmark)
          .then(function () {
            NotificationService.showSuccessMessage('The bookmark was successfully removed.');
          }, function () {
            NotificationService.showErrorMessage('There was an error removing the bookmark.');
          })
          .finally(function () {
              getBookmarks();
          });
      }

      function getBookmarks() {

          var params = { limit: 300 };

          BookmarkService.getBookmarks(params)
            .then(function (data) {
                $scope.bookmarks = data.data.results;
            }, function () {
                NotificationService.showErrorMessage('There was an error loading the bookmarks.');
            })
            .finally(function () {
                $scope.loadingBookmarks = false;
            });
      }

      getBookmarks();
      
});
