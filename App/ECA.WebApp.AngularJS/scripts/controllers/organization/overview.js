'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationOverviewCtrl
 * @description The overview controller is used on the overview tab of an organization.
 * # OrganizationOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OrganizationOverviewCtrl', function (
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
          $log.info('overview here.');
          isOrganizationLoading(false);
      });

      function isOrganizationLoading(isLoading) {
          $scope.view.isOrganizationLoading = isLoading;
      }
  });
