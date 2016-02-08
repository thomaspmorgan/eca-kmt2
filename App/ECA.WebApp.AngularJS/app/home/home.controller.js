'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:HomeCtrl
 * @description
 * # HomeCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('HomeCtrl', function ($rootScope, $scope, $state, $modal, MessageBox, AuthService, BookmarkService, NotificationService, StateService, BrowserService) {

      BrowserService.setHomeDocumentTitle('Your Shortcuts');
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
          if (bookmark.type === 'Office') {
              return StateService.getOfficeState(bookmark.officeId);
          } else if (bookmark.type === 'Program') {
              return StateService.getProgramState(bookmark.programId);
          } else if (bookmark.type === 'Project') {
              return StateService.getProjectState(bookmark.projectId);
          } else if (bookmark.type === 'Person') {
              return StateService.getPersonState(bookmark.personId);
          } else if (bookmark.type === 'Organization') {
              return StateService.getOrganizationState(bookmark.organizationId);
          }
          else {
              throw Error('The bookmark type is not supported.');
          }
      }

      $scope.deleteBookmark = function (bookmark) {
          MessageBox.confirm({
              title: 'Confirm',
              message: 'Are you sure you wish to delete the bookmark?',
              okText: 'Yes',
              cancelText: 'No',
              okCallback: function () {
                  deleteBookmark(bookmark);
              }
          });
      }

      function deleteBookmark(bookmark) {
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
