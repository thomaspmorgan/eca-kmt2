'use strict';

/**
 * @ngdoc function
* @name staticApp.controller:AllOrganizationsCtrl
 * @description
 * # AllOrganizationsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOrganizationsCtrl', function ($scope, $stateParams, $state, $log, OrganizationService, TableService) {

      $scope.organizations = [];
      $scope.start = 0;
      $scope.end = 0;
      $scope.total = 0;

      $scope.organizationsLoading = false;
      $scope.selectedOrgType = {};

      $scope.onEditIconClick = function (org) {
          $state.go('organizations.edit', { organizationId: org.organizationId });
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
  });