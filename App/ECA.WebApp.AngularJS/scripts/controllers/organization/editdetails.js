'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationEditCtrl
 * @description The edit controller is used on the edit view of an organization.
 * # OrganizationOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OrganizationEditDetailsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OrganizationService,
        LookupService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.organizationTypes = [];
      $scope.view.isLoadingParentOrganizations = false;
      $scope.view.searchAvailableOrganizationsLimit = 10;
      

      $scope.view.getAvailableParentOrganizations = function (search) {
          return loadAvailableOrganizations(search);
      }

      $scope.view.onSelectAvailableParentOrganization = function ($item, $model, $label) {
          $scope.organization.parentOrganizationId = $item.organizationId;
          $scope.organization.parentOrganizationName = $item.name;
      }

      function loadAvailableOrganizations(search) {
          var params = {
              start: 0,
              limit: $scope.view.searchAvailableOrganizationsLimit,
          };
          if (search) {
              params.keyword = search
          }
          $scope.view.isLoadingParentOrganizations = true;

          return OrganizationService.getOrganizations(params)
          .then(function (response) {
              var data = null;
              var total = null;
              if (response.data) {
                  data = response.data.results;
                  total = response.data.total;
              }
              else {
                  data = response.results;
                  total = response.total;
              }
              $scope.view.isLoadingParentOrganizations = false;
              return data;
          })
          .catch(function () {
              $scope.view.isLoadingParentOrganizations = false;
              $log.error('Unable to load available organizations.');
              NotificationService.showErrorMessage('Unable to load available organizations.');
          });
      }

      var orgTypesParams = {
          start: 0,
          limit: 300
      };

      function onOrgTypesLoad (orgTypes) {
          if (orgTypes.data.total > orgTypesParams.limit) {
              var message = 'There are more org types than can be loaded.  Not all org types will be visible.'
              NotificationService.showErrorMessage(message);
              $log.error(message);
          }
          $scope.view.organizationTypes = orgTypes.data.results;
      }


      $q.all([LookupService.getOrganizationTypes(orgTypesParams)])
      .then(function (results) {
          //results is an array
          var orgTypes = results[0];
          onOrgTypesLoad(orgTypes);

      })
        .catch(function () {
            $log.error('Unable to load organization.');
        });
  });
