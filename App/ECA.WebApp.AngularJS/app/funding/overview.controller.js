'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:FundingOverviewCtrl
 * @description The overview controller is used on the overview tab of a funding source.
 * # FundingOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('FundingOverviewCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OrganizationService,
        BrowserService,
        ConstantsService,
        NavigationService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.showEditDetails = false;

      $scope.view.saveDetailsEdit = function () {
          saveOrganization();
      };

      $scope.view.cancelDetailsEdit = function () {
          $scope.view.showEditDetails = false;
      };

      isOrganizationLoading(true);
      $scope.data.loadedOrganizationPromise.promise
      .then(function (org) {
          BrowserService.setDocumentTitleByOrganization(org, 'Overview');
          isOrganizationLoading(false);
      });

      function isOrganizationLoading(isLoading) {
          $scope.view.isOrganizationLoading = isLoading;
      }

      function saveOrganization() {
          var org = $scope.organization;
          isOrganizationLoading(true);
          return OrganizationService.update(org)
          .then(function (results) {
              $scope.organization = results.data;
              $scope.view.showEditDetails = false;
              isOrganizationLoading(false);
              NavigationService.updateBreadcrumbs();
              NotificationService.showSuccessMessage('Successfully updated the organization.');
          })
          .catch(function () {
              isOrganizationLoading(false);
              var message = 'Unable to save organization changes.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
  });
