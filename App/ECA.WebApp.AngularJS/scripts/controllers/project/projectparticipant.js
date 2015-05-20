'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectParticipantCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isCollaboratorExpanded = true;

      $scope.view.addCollaborator = function ($event) {
          var modalInstance = $modal.open({
              templateUrl: '/views/project/collaborators.html',
              controller: 'AddProjectCollaboratorCtrl',
              backdrop: 'static',
              resolve: {},
              size: 'lg'
          });
          modalInstance.result.then(function () {
              $log.info('Cancelling changes...');              
          }, function () {
              $log.info('Dismiss add collaborator dialog...');
          });
      };
      
      

      //$scope.view.isLoading = true;
      //$q.all([loadProject(), loadOfficeSettings()])
      //.then(function (results) {
      //    //results is an array

      //}, function (errorResponse) {
      //    $log.error('Failed initial loading of project view.');
      //})
      //.then(function () {
      //    $scope.view.isLoading = false;
      //});
  });
