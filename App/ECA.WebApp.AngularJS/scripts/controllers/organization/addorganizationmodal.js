'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddOrganizationModalCtrl
 * @description
 * # AddOrganizationModalCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AddOrganizationModalCtrl', function ($q, $scope, $modalInstance, $log, FilterService, OrganizationService, ConstantsService, LookupService, NotificationService) {

      $scope.view = {};
      $scope.view.maxNameLength = 500;
      $scope.view.maxDescriptionLength = 3000;
      $scope.view.showConfirmCancel = false;
      $scope.view.isSavingOrganization = false;
      $scope.view.isLoadingLikeOrganizations = false;
      $scope.view.matchingOrganizationsByName = [];
      $scope.view.doesOrganizationExist = false;
      $scope.view.organization = {
          name: '',
          description: '',
          organizationRoles: [],
          pointsOfContact: []
      };

      $scope.view.onSaveClick = function () {
          saveOrganization();
      }

      function saveOrganization() {
          $scope.view.isSavingOrganization = true;
          console.log($scope.view.organization);
          OrganizationService.create($scope.view.organization)
            .then(function (response) {
                $scope.view.isSavingOrganization = false;
                $modalInstance.close(response.data);
            })
          .catch(function (response) {
              $scope.view.isSavingOrganization = false;
              var message = 'Unable to save organization.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.organizationForm.$dirty) {
              $scope.view.showConfirmCancel = true;
          }
          else {
              $modalInstance.dismiss('cancel');
          }
      }

      $scope.view.onYesCancelChangesClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onNoDoNotCancelChangesClick = function () {
          $scope.view.showConfirmCancel = false;
      }

      var organizationsWithSameNameFilter = FilterService.add('addorganizationmodal_organizationswithsamename');
      $scope.view.onOrganizationNameChange = function () {
          var organizationName = $scope.view.organization.name;
          if (organizationName && organizationName.length > 0) {
              organizationsWithSameNameFilter.reset();
              organizationsWithSameNameFilter = organizationsWithSameNameFilter
                  .skip(0)
                  .take(1)
                  .equal('name', organizationName);
              $scope.view.isLoadingLikeOrganizations = true;
              console.log(organizationsWithSameNameFilter.toParams());
              return OrganizationService.getOrganizations(organizationsWithSameNameFilter.toParams())
                  .then(function (response) {
                      $scope.view.matchingOrganizationsByName = response.results;
                      $scope.view.doesOrganizationExist = response.total > 0;
                      $scope.view.isLoadingLikeOrganizations = false;
                  })
                  .catch(function (response) {
                      $scope.view.isLoadingLikeOrganizations = false;
                      var message = "Unable to load matching organizations.";
                      $log.error(message);
                      NotificationService.showErrorMessage(message);
                  });
          }
      }

      function loadOrganizationTypes() {
          return OrganizationService.getTypes({
              limit: 300,
              filter: {
                  comparison: ConstantsService.notInComparisonType,
                  property: 'id',
                  value: [ConstantsService.organizationType.office.id, ConstantsService.organizationType.branch.id, ConstantsService.organizationType.division.id]
              }
          })
            .then(function (data) {
                $scope.view.organizationTypes = data.data.results;
            });
      }

      function loadOrganizationRoles() {
          return LookupService.getOrganizationRoles({ limit: 300 })
            .then(function (data) {
                $scope.view.organizationRoles = data.data.results;
            });
      }

      function loadPointsOfContact() {
          return LookupService.getAllContacts({ limit: 300 })
            .then(function (data) {
                $scope.view.pointsOfContact = data.results;
            });
      }

      $q.all([loadOrganizationTypes(), loadOrganizationRoles(), loadPointsOfContact()]);
      
  });
