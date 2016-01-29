'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationImpactCtrl
 * @description The impact controller is used on the impact view of an organization.
 * # OrganizationImpactCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OrganizationImpactCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OrganizationService,
        BrowserService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      isOrganizationLoading(true);

      $scope.data.loadedOrganizationPromise.promise
      .then(function (org) {
          $log.info('impact here.');
          BrowserService.setDocumentTitleByOrganization(org, 'Impact');
          isOrganizationLoading(false);
      });

      function isOrganizationLoading(isLoading) {
          $scope.view.isOrganizationLoading = isLoading;
      }
  });
