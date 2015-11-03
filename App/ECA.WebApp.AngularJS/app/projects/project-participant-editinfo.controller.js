'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectParticipantEditInfoCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $state,
		FilterService,
		LookupService,
        OrganizationService,
        PersonService,
        ConstantsService,
        AuthService,
        ProjectService,
        NotificationService,
        ParticipantPersonsService
        ) {

      $scope.view = {};
      $scope.view.participantTypes = [];
      $scope.view.participantStatii = [];
      $scope.view.homeInstitutions = [];
      $scope.view.hostInstitutions = [];
      $scope.view.homeInstitutionAddresses = [];
      $scope.view.hostInstitutionAddresses = [];
      $scope.view.selectedHomeInstitutionAddresses = [];
      $scope.view.selectedHostInstitutionAddresses = [];
      $scope.view.isLoadingEditParticipantInfoRequiredData = false;

      $scope.view.originalPersonInfo = angular.copy($scope.personinfo);

      $scope.view.loadHomeInstitutions = function ($search) {
          return loadHomeInstitutions($search);
      }

      $scope.view.loadHostInstitutions = function ($search) {
          return loadHostInstitutions($search);
      }

      $scope.view.onHostInstitutionSelect = function ($item, $model) {
          $scope.view.selectedHostInstitutionAddresses = [];
          $scope.personinfo.hostInstitutionAddressId = null;
          if ($model) {
              $scope.personinfo.hostInstitutionId = $model;
              return loadOrganizationById($model)
              .then(function (org) {
                  $scope.view.hostInstitutionAddresses = org.addresses;
              });

          }
          else {
              $scope.personinfo.hostInstitutionId = null;
          }
      }

      $scope.view.onSelectHostInstitutionAddress = function ($item, $model) {
          $scope.personinfo.hostInstitutionAddressId = $model;
      }

      $scope.view.onHomeInstitutionSelect = function ($item, $model) {
          $scope.view.selectedHomeInstitutionAddresses = [];
          $scope.personinfo.homeInstitutionAddressId = null;
          if ($model) {
              $scope.personinfo.homeInstitutionId = $model;
              return loadOrganizationById($model)
              .then(function (org) {
                  $scope.view.homeInstitutionAddresses = org.addresses;
              });
          }
          else {
              $scope.personinfo.homeInstitutionId = null;
          }
      }

      $scope.view.onSelectHomeInstitutionAddress = function ($item, $model) {
          $scope.personinfo.homeInstitutionAddressId = $model;
      }

      $scope.view.onCancelButtonClick = function () {
          $scope.view.originalPersonInfo = false;
          $scope.personinfo = $scope.view.originalPersonInfo;
          //$scope.personinfo.isInfoTabInEditMode = false;                 
      }

      var projectId = $stateParams.projectId;
      var limit = 300;
      var searchLimit = 10;

      function loadOrganizationById(organizationId) {
          return OrganizationService.getById(organizationId)
          .then(function (response) {
              return response.data;
          })
          .catch(function (response) {
              var message = "Unable to load organization by id.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          })
      }

      var participantTypesFilter = FilterService.add('project-participant-editinfo-partipanttypes');
      participantTypesFilter = participantTypesFilter.isTrue('isPerson').skip(0).take(limit);
      function loadParticipantTypes() {
          return LookupService.getParticipantTypes(participantTypesFilter.toParams())
          .then(function (response) {
              if (response.data.total > limit) {
                  var message = "The number of participant types loaded is less than the total number.  Some participant types may not be shown."
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
              $scope.view.participantTypes = response.data.results;
              return $scope.view.participantTypes;
          })
          .catch(function (response) {
              var message = "Unable to load participant types.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      var participantStattiFilter = FilterService.add('project-participant-editinfo-partipantstatii');
      participantStattiFilter = participantStattiFilter.skip(0).take(limit);
      function loadParticipantStatii() {
          return LookupService.getParticipantStatii(participantStattiFilter.toParams())
          .then(function (response) {
              if (response.data.total > limit) {
                  var message = "The number of participant statii loaded is less than the total number.  Some participant statii may not be shown."
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              }
              $scope.view.participantStatii = response.data.results;
              return $scope.view.participantStatii;
          })
          .catch(function (response) {
              var message = "Unable to load participant statii.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      var homeInstitutionFilter = FilterService.add('project-participant-editinfo-homeinstitutionfilter');
      function loadHomeInstitutions(search) {
          homeInstitutionFilter.reset();
          homeInstitutionFilter = homeInstitutionFilter
                  .skip(0)
                  .take(10);

          if (search && search.length > 0) {
              homeInstitutionFilter = homeInstitutionFilter.like('name', search);
              loadOrganizations(homeInstitutionFilter)
              .then(function (data) {
                  $scope.view.homeInstitutions = data.results;
                  return $scope.view.homeInstitutions;
              });
          }
      }

      var hostInstitutionFilter = FilterService.add('project-participant-editinfo-hostinstitutionfilter');
      function loadHostInstitutions(search) {
          hostInstitutionFilter.reset();
          hostInstitutionFilter = hostInstitutionFilter
                  .skip(0)
                  .take(10);

          if (search && search.length > 0) {
              hostInstitutionFilter = hostInstitutionFilter.like('name', search);
              loadOrganizations(hostInstitutionFilter)
              .then(function (data) {
                  $scope.view.hostInstitutions = data.results;
                  return $scope.view.hostInstitutions;
              });
          }
      }

      function loadOrganizations(filter) {
          return OrganizationService.getOrganizations(filter.toParams())
                .then(function (data) {
                    return data;
                })
                .catch(function (response) {
                    var message = "Unable to load organizations.";
                    $log.error(message);
                    NotificationService.showErrorMessage(message);
                });
      }

      function initializeHomeInstitution(organization) {
          var homeInstitution = $scope.personinfo.homeInstitution;
          if (homeInstitution) {
              homeInstitution.id = homeInstitution.organizationId;
              return loadOrganizationById(homeInstitution.organizationId)
              .then(function (org) {
                  $scope.view.homeInstitutions.push(org);
                  angular.forEach(org.addresses, function (orgAddress, index) {
                      $scope.view.homeInstitutionAddresses.push(orgAddress);
                  });
                  if ($scope.personinfo.homeInstitutionAddressId) {
                      $scope.view.selectedHomeInstitutionAddresses.push($scope.personinfo.homeInstitutionAddressId);
                  }

                  return org;
              });
          }
          else {
              var dfd = $q.defer();
              dfd.resolve();
              return dfd.promise;
          }
      }

      function initializeHostInstitution(organization) {
          var hostInstitution = $scope.personinfo.hostInstitution;
          if (hostInstitution) {
              hostInstitution.id = hostInstitution.organizationId;
              return loadOrganizationById(hostInstitution.organizationId)
              .then(function (org) {
                  $scope.view.hostInstitutions.push(org);
                  angular.forEach(org.addresses, function (orgAddress, index) {
                      $scope.view.hostInstitutionAddresses.push(orgAddress);
                  });
                  if ($scope.personinfo.hostInstitutionAddressId) {
                      $scope.view.selectedHostInstitutionAddresses.push($scope.personinfo.hostInstitutionAddressId);
                  }

                  return org;
              });
          }
          else {
              var dfd = $q.defer();
              dfd.resolve();
              return dfd.promise;
          }
      }

      $scope.view.isLoadingEditParticipantInfoRequiredData = true;
      $q.all([
          loadParticipantTypes(),
          loadParticipantStatii(),
          initializeHomeInstitution($scope.personinfo.homeInstitution),
          initializeHostInstitution($scope.personinfo.hostInstitution)])
      .then(function (results) {
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      })
      .catch(function () {
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      });
  });
