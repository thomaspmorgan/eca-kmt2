'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationCtrl
 * @description The overview controller is used on the for general purpose organization management.
 * # OrganizationCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OrganizationCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OrganizationService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.tabs = {
          overview: { title: 'Overview', path: 'overview', active: true, order: 1 },
          artifacts: { title: 'Artifacts', path: 'artifacts', active: true, order: 2 },
          impact: { title: 'Impact', path: 'impact', active: true, order: 3 },
          activity: { title: 'Activities', path: 'activity', active: true, order: 74 }
      };


      $scope.view.isLoadingOrganization = false;
      $scope.view.organizationId = $stateParams.organizationId;


      function loadOrganization(organizationId) {
          return OrganizationService.getById(organizationId)
          .then(function (response) {
              $scope.organization = response.data;
          })
          .catch(function () {
              $log.error('Unable to load organization.');
              NotificationService.showErrorMessage('Unable to load organization.');
          });
      }

      $scope.view.isLoadingOrganization = true;
      $q.all([loadOrganization($scope.view.organizationId)])
      .then(function (results) {
          //results is an array

      }, function (errorResponse) {
          $log.error('Failed initial loading of organization view.');
      })
      .then(function () {
          $scope.view.isLoadingOrganization = false;
      });
  });
