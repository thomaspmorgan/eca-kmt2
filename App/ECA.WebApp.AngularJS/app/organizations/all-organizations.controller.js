'use strict';

/**
 * @ngdoc function
* @name staticApp.controller:AllOrganizationsCtrl
 * @description
 * # AllOrganizationsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOrganizationsCtrl', function ($scope, $stateParams, $state, $log, $modal, OrganizationService, TableService, LookupService, NotificationService, StateService) {

      $scope.organizations = [];
      $scope.start = 0;
      $scope.end = 0;
      $scope.total = 0;

      $scope.organizationsLoading = false;
      $scope.selectedOrgType = {};
      $scope.selectedOrganizationRoles = [];

      $scope.onEditIconClick = function (org) {
          $state.go('organizations.edit', { organizationId: org.organizationId });
      }

      $scope.addOrganization = function () {
          var addOrganizationModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/organizations/add-organization-modal.html',
              controller: 'AddOrganizationModalCtrl',
              backdrop: 'static',
              size: 'lg'
          });
          addOrganizationModalInstance.result.then(function (addedOrganization) {
              $log.info('Finished adding organization.');
              addOrganizationModalInstance.close(addedOrganization);
              StateService.goToOrganizationState(addedOrganization.organizationId);
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      $scope.getOrganizations = function (tableState) {

          $scope.organizationsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          OrganizationService.getOrganizations(params)
            .then(function (data) {
                $scope.organizations = data.results;
                var limit = TableService.getLimit();
                var start = TableService.getStart();
                tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
                $scope.start = start + 1;
                $scope.end = start + data.results.length;
                $scope.total = data.total;
                $scope.organizationsLoading = false;
            });
      };

      var params = { start: 0, limit: 300 };
      LookupService.getOrganizationRoles(params)
        .then(function (result) {
            $scope.organizationRoles = result.data.results;
        });
  });