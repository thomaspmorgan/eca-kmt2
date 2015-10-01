'use strict';

/**
 * @ngdoc function
* @name staticApp.controller:AllOrganizationsCtrl
 * @description
 * # AllOrganizationsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOrganizationsCtrl', function ($scope, $stateParams, $state, $log, $modal, OrganizationService, TableService, LookupService) {

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
              templateUrl: 'views/organizations/addorganizationmodal.html',
              controller: 'AddOrganizationModalCtrl',
              backdrop: 'static',
              size: 'lg'
          });
          /*
          var addProgramModalInstance = $modal.open({
              animation: true,
              templateUrl: 'views/program/addprogrammodal.html',
              controller: 'AddProgramModalCtrl',
              backdrop: 'static',
              size: 'lg',
              resolve: {
                  office: function () {
                      return {
                          id: $scope.view.program.ownerOrganizationId,
                          name: $scope.view.program.ownerName
                      }
                  },
                  parentProgram: function () {
                      return $scope.view.program;
                  }
              }
          });
          addProgramModalInstance.result.then(function (addedProgram) {
              $log.info('Finished adding program.');
              addProgramModalInstance.close(addedProgram);
              StateService.goToProgramState(addedProgram.id);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
          */
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