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
      $scope.view.selectedPointsOfContact = [];
      $scope.view.showEditDetails = true;
      $scope.view.selectedOrganizationRoles = [];

      $scope.view.searchPointsOfContact = function (data) {
          loadPointsOfContact(data);
      }

      $scope.view.getAvailableParentOrganizations = function (search) {
          return loadAvailableOrganizations(search);
      }

      $scope.view.onSelectAvailableParentOrganization = function ($item, $model, $label) {
          $scope.organization.parentOrganizationId = $item.organizationId;
          $scope.organization.parentOrganizationName = $item.name;
      }

      $scope.view.onSelectParentOrganizationBlur = function ($event) {
          if ($scope.organization.parentOrganizationName === '') {
              delete $scope.organization.parentOrganizationName;
              delete $scope.organization.parentOrganizationId;
          }
      }

      $scope.data.loadedOrganizationPromise.promise
        .then(function (org) {
            setSelectedPointsOfContact();
        });

      $scope.view.onSelectPointsOfContactChange = function () {
          updatePointsOfContactIds();
      }

      $scope.view.onSelectOrganizationRolesChange = function () {
          updateOrganizationRoleIds();
      }

      function setSelectedPointsOfContact() {
          setSelectedItems('contacts', 'selectedPointsOfContact');
      }

      function setSelectedItems(projectPropertyName, viewSelectedPropertyName) {
          console.assert($scope.organization.hasOwnProperty(projectPropertyName), "The organization property " + projectPropertyName + " does not exist.");
          console.assert($scope.view.hasOwnProperty(viewSelectedPropertyName), "The view " + viewSelectedPropertyName + " property does not exist.");
          console.assert(Array.isArray($scope.view[viewSelectedPropertyName]), "The view " + viewSelectedPropertyName + " property must be an array.");

          var orgItems = $scope.organization[projectPropertyName];
          $scope.view[viewSelectedPropertyName] = $scope.view[viewSelectedPropertyName].splice(0, $scope.view[viewSelectedPropertyName].length);
          for (var i = 0; i < orgItems.length; i++) {
              var orgItem = orgItems[i];
              $scope.view[viewSelectedPropertyName].push(orgItem);
          }
      }

      function updatePointsOfContactIds() {
          var propertyName = "pointsOfContactIds";
          $scope.organization[propertyName] = $scope.organization[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedPointsOfContact');
      }

      function updateOrganizationRoleIds() {
          var propertyName = "organizationRoleIds";
          $scope.organization[propertyName] = $scope.organization[propertyName] || [];
          updateRelationshipIds(propertyName, 'selectedOrganizationRoles');
      }

      function updateRelationshipIds(idsPropertyName, viewiewSelectedPropertyName) {
          console.assert($scope.organization.hasOwnProperty(idsPropertyName), "The organization must have the property named " + idsPropertyName);
          console.assert($scope.view.hasOwnProperty(viewiewSelectedPropertyName), "The view must have the property named " + viewiewSelectedPropertyName);
          $scope.organization[idsPropertyName] = [];
          $scope.organization[idsPropertyName] = $scope.view[viewiewSelectedPropertyName].map(function (c) {
              return c.id;
          });
      }

      var maxLimit = 300;
      function loadPointsOfContact(search) {
          var params = {
              start: 0,
              limit: maxLimit,
              filter: []
          };
          if ($scope.view.selectedPointsOfContact.length > 0) {
              var idsToRemove = $scope.view.selectedPointsOfContact.map(function (c) { return c.id; });
              params.filter.push({
                  comparison: ConstantsService.notInComparisonType,
                  property: 'id',
                  value: idsToRemove
              });
          }
          if (search) {
              params.filter.push(
                  {
                      comparison: ConstantsService.likeComparisonType,
                      property: 'fullName',
                      value: search
                  });
          }
          return LookupService.getAllContacts(params)
              .then(function (response) {
                  if (response.total > maxLimit) {
                      $log.error('There are more contacts in the system then are currently loaded, an issue could occur in the UI not showing all possible values.');
                  }
                  for (var i = 0; i < response.results.length; i++) {
                      var position = "";
                      if (response.results[i].position) {
                          position = " (" + response.results[i].position + ")";
                      }
                      response.results[i].value = response.results[i].fullName + position;
                  }
                  $scope.view.pointsOfContact = response.results;
                  return $scope.view.pointsOfContact;
              });
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
          limit: maxLimit,
          filter: {
              comparison: ConstantsService.notInComparisonType,
              property: 'id',
              value: [ConstantsService.organizationType.office.id, ConstantsService.organizationType.branch.id, ConstantsService.organizationType.division.id]
          }
      };

      var orgRolesParams = {
          start: 0,
          limit: maxLimit
      };

      function onOrgTypesLoad(orgTypes) {
          if (orgTypes.data.total > orgTypesParams.limit) {
              var message = 'There are more org types than can be loaded.  Not all org types will be visible.'
              NotificationService.showErrorMessage(message);
              $log.error(message);
          }
          $scope.view.organizationTypes = orgTypes.data.results;
      }

      function onOrgRolesLoad(orgRoles) {
          if (orgRoles.data.total > orgRolesParams.limit) {
              var message = 'There are more org roles than can be loaded.  Not all org roles will be visible.'
              NotificationService.showErrorMessage(message);
              $log.error(message);
          }
          $scope.view.organizationRoles = orgRoles.data.results;
          for (var i = 0; i < $scope.organization.organizationRoles.length; i++) {
              var role = $scope.view.organizationRoles.filter(function (role) { return role.id === $scope.organization.organizationRoles[i].id })[0];
              $scope.view.selectedOrganizationRoles.push(role);
          }
      }

      $q.all([LookupService.getOrganizationTypes(orgTypesParams), LookupService.getOrganizationRoles(orgRolesParams)])
      .then(function (results) {
          //results is an array
          var orgTypes = results[0];
          onOrgTypesLoad(orgTypes);
          var orgRoles = results[1];
          onOrgRolesLoad(orgRoles);
      })
        .catch(function () {
            $log.error('Unable to load organization.');
        });
  });
