'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:FundingImpactCtrl
 * @description The impact controller is used on the impact view of a funding source.
 * # FundingImpactCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('FundingImpactCtrl', function (
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
