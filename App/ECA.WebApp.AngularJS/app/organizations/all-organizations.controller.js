'use strict';

/**
 * @ngdoc function
* @name staticApp.controller:AllOrganizationsCtrl
 * @description
 * # AllOrganizationsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOrganizationsCtrl', function ($scope, $stateParams, $state, $log, $modal, OrganizationService, TableService, LookupService, NotificationService, StateService, smoothScroll) {

      $scope.organizations = [];
      $scope.start = 0;
      $scope.end = 0;
      $scope.total = 0;

      $scope.organizationsLoading = false;
      $scope.listType = 'hierarchy';

      $scope.view = {};
      $scope.view.selectedOrganizationRoleId = 3;
      
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

      $scope.getOrganizationsHierarchy = function (tableState) {

          $scope.organizationsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              organizationRoleId: $scope.view.selectedOrganizationRoleId,
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          params.filter.push({ property: "parentOrganization_OrganizationId", comparison: "null" });

          OrganizationService.getOrganizationsHierarchyByRoleId(params)
         .then(function (result) {
             var data = result.data;
             $scope.organizations = data.results;
             var limit = TableService.getLimit();
             var start = TableService.getStart();
             tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
             $scope.start = start + 1;
             $scope.end = start + data.results.length;
             $scope.total = data.total;
             $scope.organizationsLoading = false;
         });
      }

      $scope.expandOrganization = function (organization) {
          organization.isExpanded = true;
          organization.loadingChildrenOrganizations = true;
          OrganizationService.getOrganizationsHierarchyByRoleId({ organizationRoleId: $scope.view.selectedOrganizationRoleId, limit: 300, filter: { property: "parentOrganization_OrganizationId", comparison: "eq", value: organization.organizationId } })
          .then(function (result) {
              organization.loadingChildrenOrganizations = false;
              var childOrganizations = result.data.results;
              var parentOrganizationIndex = $scope.organizations.indexOf(organization);
              angular.forEach(childOrganizations, function (childOrganization, childOrganizationIndex) {
                  $scope.organizations.splice(parentOrganizationIndex + 1 + childOrganizationIndex, 0, childOrganization);
                  childOrganization.parent = organization;
              });
              organization.children = childOrganizations;
          });
      }

      $scope.collapseOrganization = function (organization) {
          organization.isExpanded = false;
          removeChildrenOrganizations(organization);
      }

      function removeChildrenOrganizations(organization) {
          if (organization.children) {
              for (var i = 0; i < organization.children.length; i++) {
                  var childOrganization = organization.children[i];
                  var childOrganizationIndex = $scope.organizations.indexOf(childOrganization);
                  $scope.organizations.splice(childOrganizationIndex, 1);
                  removeChildrenOrganizations(childOrganization);
              }
              delete organization.children;
          }
      }

      $scope.organizationRoleChanged = function () {
          var tableState = $scope.getOrganizationsTableState();
          $scope.getOrganizationsHierarchy(tableState);
      }

      $scope.scrollToParent = function (organization) {
          if(organization.parent) {
              scrollToOrganization(organization.parent);
          }
      }

      function scrollToOrganization(organization) {
          var id = "organization-" + organization.organizationId;
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 70,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          }
          var element = document.getElementById(id)
          smoothScroll(element, options);
      }

      var params = { start: 0, limit: 300 };
      LookupService.getOrganizationRoles(params)
        .then(function (result) {
            $scope.organizationRoles = result.data.results;
        });
  });