'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationArtifactsCtrl
 * @description The artifacts controller is used on the artifacts view of an organization.
 * # OrganizationArtifactsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OrganizationArtifactsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OrganizationService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      isOrganizationLoading(true);

      $scope.data.loadedOrganizationPromise.promise
      .then(function (org) {
          $log.info('artificats here.');
          isOrganizationLoading(false);
      });

      function isOrganizationLoading(isLoading) {
          $scope.view.isOrganizationLoading = isLoading;
      }
  });
