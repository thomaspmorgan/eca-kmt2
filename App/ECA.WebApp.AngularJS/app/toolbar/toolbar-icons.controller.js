'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ToolbarCtrl
 * @description
 * # ToolbarCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ToolbarIconsCtrl', function ($scope, $state, $stateParams, $modal, ConstantsService, AuthService, BookmarkService, NotificationService) {

      $scope.isOwner = false;
      $scope.isBookmarked = false;

      var stateParams = getStateParams();
      var bookmark;
      isBookmarked();

      AuthService.getResourcePermissions(stateParams.resourceType.value, stateParams.foreignResourceId)
        .then(function (result) {
            var permissions = result.data;
            for (var i = 0; i < permissions.length; i++) {
                if (permissions[i].permissionId === stateParams.ownerPermissionId) {
                    $scope.isOwner = true;
                }
            }
        });

      function getStateParams() {
          var resourceType;
          var foreignResourceId;
          var ownerPermissionId;
          var stateName = $state.current.name;

          if (stateName.indexOf(ConstantsService.resourceType.office.value.toLowerCase()) > -1) {
              resourceType = ConstantsService.resourceType.office;
              foreignResourceId = $stateParams.officeId;
              ownerPermissionId = ConstantsService.permission.officeOwner.id;
          } else if (stateName.indexOf(ConstantsService.resourceType.program.value.toLowerCase()) > -1) {
              resourceType = ConstantsService.resourceType.program;
              foreignResourceId = $stateParams.programId;
              ownerPermissionId = ConstantsService.permission.programOwner.id;
          } else if (stateName.indexOf(ConstantsService.resourceType.project.value.toLowerCase()) > -1) {
              resourceType = ConstantsService.resourceType.project;
              foreignResourceId = $stateParams.projectId;
              ownerPermissionId = ConstantsService.permission.projectOwner.id;
          } else if (stateName.indexOf("people") > -1) {
              resourceType = { value: "Person" };
              foreignResourceId = $stateParams.personId;
          } else if (stateName.indexOf("organization") > -1) {
              resourceType = { value: "Organization" };
              foreignResourceId = $stateParams.organizationId;
          }

          return { resourceType: resourceType, foreignResourceId: parseInt(foreignResourceId), ownerPermissionId: ownerPermissionId};
      }

      function isBookmarked() {

          var params = {
              limit: 300,
              filter: getFilter()
          };


          BookmarkService.getBookmarks(params)
            .then(function (data) {
                if (data.data.total === 1) {
                    bookmark = data.data.results[0];
                    $scope.isBookmarked = true;
                } else {
                    $scope.isBookmarked = false;
                }
            }, function () {
                NotificationService.showErrorMessage('There was an error loading bookmarks.');
            });

      }

      function getFilter() {

          var property = getProperty();

          var filter = [{
              comparison: 'eq',
              value: stateParams.foreignResourceId,
              property: property
          }];

          if (property === "officeId") {
              filter.push({
                  comparison: 'null',
                  property: 'programId'
              });
              filter.push({
                  comparison: 'null',
                  property: 'projectId'
              })
          } else if (property == "programId") {
              filter.push({
                  comparison: 'null',
                  property: 'officeId'
              });
              filter.push({
                  comparison: 'null',
                  property: 'projectId'
              })
          }

          return filter;
      }

      function getProperty() {

          var property;

          if (stateParams.resourceType.value === ConstantsService.resourceType.office.value) {
              property = 'officeId';
          } else if (stateParams.resourceType.value === ConstantsService.resourceType.program.value) {
              property = 'programId';
          } else if (stateParams.resourceType.value === ConstantsService.resourceType.project.value) {
              property = 'projectId';
          } else if (stateParams.resourceType.value === "Person") {
              property = 'personId';
          } else if (stateParams.resourceType.value === "Organization") {
              property = 'organizationId';
          }

          return property;
      }

      $scope.openCollaboratorModal = function() {
          var modalInstance = $modal.open({
              animation: true,
              templateUrl: '/app/collaborators/collaborators.html',
              controller: 'CollaboratorCtrl',
              size: 'lg',
              resolve: {
                  parameters: function () {
                      return {
                          resourceType: stateParams.resourceType,
                          foreignResourceId: stateParams.foreignResourceId
                      }
                  }
              }
          });
      }

      $scope.toggleBookmark = function () {
          $scope.togglingBookmark = true;
          if ($scope.isBookmarked) {
              $scope.isBookmarked = false;
              BookmarkService.deleteBookmark(bookmark)
                .then(function () {
                    NotificationService.showSuccessMessage('The bookmark was successfully removed.');
                }, function () {
                    NotificationService.showErrorMessage('There was an error removing the bookmark.');
                })
                .finally(function () {
                    isBookmarked();
                    $scope.togglingBookmark = false;
                });
          } else {
              $scope.isBookmarked = true;
              var params = {
                  automatic: false
              };
              var property = getProperty();
              params[property] = stateParams.foreignResourceId;
              BookmarkService.createBookmark(params)
                .then(function () {
                    NotificationService.showSuccessMessage('The bookmark was successfully added.');
                }, function () {
                    NotificationService.showErrorMessage('There was an error adding the bookmark.');
                })
                .finally(function () {
                    isBookmarked();
                    $scope.togglingBookmark = false;
                });
          }
      }

  });
