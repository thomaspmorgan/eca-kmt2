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
      $scope.view.hostInstitutionsAddresses = [];
      $scope.view.isLoadingEditParticipantInfoRequiredData = false;

      $scope.originalPersonInfo = angular.copy($scope.personinfo);

      $scope.view.loadHomeInstitutions = function ($search) {
          return loadHomeInstitutions($search);
      }

      $scope.view.onHomeInstitutionSelect = function ($item, $model) {
          $scope.personinfo.homeInstitutionAddressId = null;
          $scope.view.getInstitutionAddresses($item.organizationId);
      }



      $scope.view.getInstitutionAddresses = function (institutionId) {
          return loadOrganizationById(institutionId)
          .then(function (org) {
              $scope.view.homeInstitutionAddresses = org.addresses;
              return $scope.view.homeInstitutionAddresses;
          });
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

      function initializeHomeInstitution() {
          var homeInstitution = $scope.personinfo.homeInstitution;
          if (homeInstitution) {
              homeInstitution.id = homeInstitution.organizationId;
              $scope.view.homeInstitutions.push(homeInstitution);
          }
      }


      $scope.view.isLoadingEditParticipantInfoRequiredData = true;
      $q.all([loadParticipantTypes(), loadParticipantStatii()])
      .then(function (results) {
          initializeHomeInstitution();
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      })
      .catch(function () {
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      });
  });
