'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:FundingActivitiesCtrl
 * @description The artifacts controller is used on the activites view of a funding source.
 * # FundingActivitiesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('FundingActivitiesCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OrganizationService,
        ConstantsService,
        BrowserService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      isOrganizationLoading(true);

      $scope.data.loadedOrganizationPromise.promise
      .then(function (org) {
          $log.info('activities here.');
          BrowserService.setDocumentTitleByOrganization(org, 'Timeline');
          isOrganizationLoading(false);
      });

      function isOrganizationLoading(isLoading) {
          $scope.view.isOrganizationLoading = isLoading;
      }
  });
