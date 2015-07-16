'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ToolbarCtrl
 * @description
 * # ToolbarCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ToolbarCtrl', function ($scope, $modal, ConstantsService) {

      $scope.openCollaboratorModal = function(resourceType, foreignResourceId) {

          var modalInstance = $modal.open({
              animation: $scope.animationsEnabled,
              templateUrl: '/views/partials/collaborators.html',
              controller: 'CollaboratorCtrl',
              size: 'lg',
              resolve: {
                  parameters: function () {
                      return {
                          resourceType: resourceType,
                          foreignResourceId: foreignResourceId
                      }
                  }
              }
          });
      };
  });
