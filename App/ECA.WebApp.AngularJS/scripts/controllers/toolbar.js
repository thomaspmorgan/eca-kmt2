'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ToolbarCtrl
 * @description
 * # ToolbarCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ToolbarCtrl', function ($scope, ConstantsService) {

      $scope.openCollaboratorModal = function(resourceType, foreignResourceId) {
          console.log(resourceType);
          console.log(foreignResourceId);
      };
  });
