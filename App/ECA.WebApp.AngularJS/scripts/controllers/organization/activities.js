'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationActivitiesCtrl
 * @description The artifacts controller is used on the activites view of an organization.
 * # OrganizationActivitiesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OrganizationActivitiesCtrl', function (
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
          $log.info('activities here.');
          isOrganizationLoading(false);
      });

      function isOrganizationLoading(isLoading) {
          $scope.view.isOrganizationLoading = isLoading;
      }
  });
