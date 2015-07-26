'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ToolbarCtrl
 * @description
 * # ToolbarCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ToolbarCtrl', function ($scope, $state, $stateParams, $modal, ConstantsService, AuthService) {

      var params = getParamsFromState();

      $scope.isOwner = false;

      AuthService.getResourcePermissions(params.resourceType.value, params.foreignResourceId)
        .then(function (result) {
            var permissions = result.data;
            for (var i = 0; i < permissions.length; i++) {
                if (permissions[i].permissionId === params.ownerPermissionId) {
                    $scope.isOwner = true;
                }
            }
        });

      function getParamsFromState() {
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
          }

          return { resourceType: resourceType, foreignResourceId: foreignResourceId, ownerPermissionId: ownerPermissionId};
      }

      $scope.openCollaboratorModal = function() {
          var modalInstance = $modal.open({
              animation: $scope.animationsEnabled,
              templateUrl: '/views/partials/collaborators.html',
              controller: 'CollaboratorCtrl',
              size: 'lg',
              resolve: {
                  parameters: function () {
                      return {
                          resourceType: params.resourceType,
                          foreignResourceId: params.foreignResourceId
                      }
                  }
              }
          });
      };
  });
