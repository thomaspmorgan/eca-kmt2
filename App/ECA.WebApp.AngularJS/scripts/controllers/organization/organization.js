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
          artifacts: { title: 'Attachments', path: 'artifacts', active: true, order: 2 },
          impact: { title: 'Impact', path: 'impact', active: true, order: 3 },
          activity: { title: 'Timeline', path: 'activities', active: true, order: 4 }
      };

      $scope.view.organizationId = $stateParams.organizationId;

      $scope.data = {};
      $scope.data.loadedOrganizationPromise = $q.defer();
      $scope.data.loadedOrganizationPromise.promise.then(function (org) {
          $scope.organization = org;
      });

      function loadOrganization(organizationId) {
          return OrganizationService.getById(organizationId)
          .then(function (response) {
              var org = response.data;
              $scope.data.loadedOrganizationPromise.resolve(org);
          })
          .catch(function () {
              $log.error('Unable to load organization.');
              NotificationService.showErrorMessage('Unable to load organization.');
          });
      }
      $q.all([loadOrganization($scope.view.organizationId)])
      .then(function (results) {
          //results is an array

      })
    .catch(function () {
        $log.error('Unable to load organization.');
    });
  });
