'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationOverviewCtrl
 * @description The overview controller is used on the overview tab of an organization.
 * # OrganizationOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OfficeOverviewCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OfficeService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.isOfficeLoading = false;
      $scope.view.params = $stateParams;
      $scope.view.office = {};

      isLoadingOffice(true);
      $scope.data.loadedOfficePromise.promise
      .then(function (office) {
          $scope.view.office = office;
          isLoadingOffice(false);
      });

      function isLoadingOffice(isLoading) {
          $scope.view.isOfficeLoading = isLoading;
      }

  });
