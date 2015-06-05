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
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
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

      $scope.view.isLoading = true;
      $q.all([loadOrganization($scope.view.organizationId)])
      .then(function (results) {
          //results is an array

      }, function (errorResponse) {
          $log.error('Failed initial loading of organization view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });
  });
