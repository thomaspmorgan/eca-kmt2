﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectParticipantInfoCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $state,
        $filter,
        orderByFilter,
		FilterService,
		LookupService,
        OrganizationService,
        PersonService,
        StateService,
        ConstantsService,
        AuthService,
        ProjectService,
        MessageBox,
        NotificationService,
        ParticipantService,
        ParticipantPersonsService
        )  {

      $scope.view = {};
      $scope.view.participantTypes = [];
      $scope.view.participantStatii = [];
      $scope.view.homeInstitutions = [];
      $scope.view.hostInstitutions = [];
      $scope.view.placementOrganizations = [];
      $scope.view.homeInstitutionAddresses = [];
      $scope.view.hostInstitutionAddresses = [];
      $scope.view.placementOrganizationAddresses = [];
      $scope.view.selectedHomeInstitutionAddresses = [];
      $scope.view.selectedHostInstitutionAddresses = [];
      $scope.view.selectedPlacementOrganizationAddresses = [];
      $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      $scope.view.isLoadingInfo = false;
      $scope.view.isSavingUpdate = false;
      $scope.view.participantPerson = null;
      $scope.view.isInfoTabInEditMode = false;
      $scope.view.editLocked = true;

      var notifyStatuses = ConstantsService.sevisStatuChangeAlertIds.split(',');

      $scope.view.loadHomeInstitutions = function ($search) {
          return loadHomeInstitutions($search);
      }

      $scope.view.loadHostInstitutions = function ($search) {
          return loadHostInstitutions($search);
      }

      $scope.view.loadPlacementOrganizations = function ($search) {
          return loadPlacementOrganizations($search);
      }

      $scope.view.onSelectHostInstitution = function ($item, $model) {
          $scope.view.selectedHostInstitutionAddresses = [];
          $scope.view.participantPerson.hostInstitutionAddressId = null;
          if ($model) {
              $scope.view.participantPerson.hostInstitutionId = $model;
              return loadOrganizationById($model)
              .then(function (org) {
                  $scope.view.hostInstitutionAddresses = org.addresses;
              });
          }
          else {
              $scope.view.participantPerson.hostInstitutionId = null;
          }
      }

      $scope.view.onSelectHostInstitutionAddress = function ($item, $model) {
          $scope.view.participantPerson.hostInstitutionAddressId = $model;
          $scope.view.selectedHostInstitutionAddresses = [$model];
      }

      $scope.view.onSelectHomeInstitution = function ($item, $model) {
          $scope.view.selectedHomeInstitutionAddresses = [];
          $scope.view.participantPerson.homeInstitutionAddressId = null;
          if ($model) {
              $scope.view.participantPerson.homeInstitutionId = $model;
              return loadOrganizationById($model)
              .then(function (org) {
                  $scope.view.homeInstitutionAddresses = org.addresses;
              });
          }
          else {
              $scope.view.participantPerson.homeInstitutionId = null;
          }
      }

      $scope.view.onSelectHomeInstitutionAddress = function ($item, $model) {
          $scope.view.participantPerson.homeInstitutionAddressId = $model;
          $scope.view.selectedHomeInstitutionAddresses = [$model];
      }

      $scope.view.onSelectPlacementOrganization = function ($item, $model) {
          $scope.view.selectedPlacementOrganizationAddresses = [];
          $scope.view.participantPerson.placementOrganizationAddressId = null;
          if ($model) {
              $scope.view.participantPerson.placementOrganizationId = $model;
              return loadOrganizationById($model)
              .then(function (org) {
                  $scope.view.placementOrganizationAddresses = org.addresses;
              });
          }
          else {
              $scope.view.participantPerson.placementOrganizationId = null;
          }
      }

      $scope.view.onSelectPlacementOrganizationAddress = function ($item, $model) {
          $scope.view.participantPerson.placementOrganizationAddressId = $model;
          $scope.view.selectedPlacementOrganizationAddresses = [$model];
      }

      $scope.view.onRemoveHostInstitutionAddress = function ($item, $model) {
          $scope.view.participantPerson.hostInstitutionAddressId = null;
      }

      $scope.view.onRemoveHomeInstitutionAddress = function ($item, $model) {
          $scope.view.participantPerson.homeInstitutionAddressId = null;
      }

      $scope.view.onRemovePlacementOrganizationAddress = function ($item, $model) {
          $scope.view.participantPerson.placementOrganizationAddressId = null;
      }

      $scope.view.onCancelButtonClick = function () {
          return loadParticipantInfo(projectId, $scope.participantid)
              .then(function (response) {
                  $scope.view.isInfoTabInEditMode = false;
              });
      }

      $scope.$watch("personid", function (personId) {
          ParticipantPersonsService.getIsParticipantPersonLocked(personId)
          .then(function (response) {
              $scope.view.editLocked = response.data;
          });
          $scope.view.isInfoTabInEditMode = false;
      });

      $scope.editGeneral = function () {
          if (!$scope.view.editLocked) {
              $scope.view.isInfoTabInEditMode = true;
          }
      }

      $scope.view.onSaveButtonClick = function () {
          $scope.view.isSavingUpdate = true;
          return saveParticipantPerson($scope.view.participantPerson)
            .then(function (response) {
                return loadParticipantInfo(projectId, $scope.participantid)
                .then(function (response) {
                    $scope.view.isSavingUpdate = false;
                    $scope.view.isInfoTabInEditMode = false;
                    updateParentTableParticipantSevisStatus(response);
                    NotificationService.showSuccessMessage('Successfully updated the participant personal information.');
                    showSevisUpdateAlert(response.participantStatusId, $scope.$parent.sevisinfo);
                });
            })
            .catch(function (response) {
                $scope.view.isSavingUpdate = false;
                var message = "Unable to update participant person.";
                $log.error(message);
                NotificationService.showErrorMessage(message);
            });
      }

      function showSevisUpdateAlert(statusId, sevisInfo) {
          if (notifyStatuses.indexOf(statusId.toString()) !== -1 && sevisInfo) {
              var defer = $q.defer();

              MessageBox.confirm({
                  title: 'SEVIS Alert',
                  message: 'Remember to manually cancel the user in the SEVIS RTI interface',
                  okText: 'OK',
                  cancelText: 'Cancel',
                  hideCancel: true,
                  okCallback: function () {
                      defer.resolve();
                  }
              });

              return defer.promise;
          } else {
              return false;
          }
      };

      $scope.$watch('participantid', function () {
          loadParticipantInfo(projectId, $scope.participantid)
              .then(function (results) {
                  $scope.view.isLoadingEditParticipantInfoRequiredData = false;
              })
              .catch(function () {
                  $scope.view.isLoadingEditParticipantInfoRequiredData = false;
              });
      });

      function updateParentTableParticipantSevisStatus(participant) {
          if ($scope.onparticipantupdated) {
              $scope.onparticipantupdated()(participant);
          }
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
              return loadOrganizations(homeInstitutionFilter)
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
              return loadOrganizations(hostInstitutionFilter)
              .then(function (data) {
                  $scope.view.hostInstitutions = data.results;
                  return $scope.view.hostInstitutions;
              });
          }
      }

      var placementOrganizationFilter = FilterService.add('project-participant-editinfo-placementorganizationfilter');
      function loadPlacementOrganizations(search) {
          placementOrganizationFilter.reset();
          placementOrganizationFilter = placementOrganizationFilter
                  .skip(0)
                  .take(10);

          if (search && search.length > 0) {
              placementOrganizationFilter = placementOrganizationFilter.like('name', search);
              return loadOrganizations(placementOrganizationFilter)
              .then(function (data) {
                  $scope.view.placementOrganizations = data.results;
                  return $scope.view.placementOrganizations;
              });
          }
      }

      function saveParticipantPerson(participantPerson) {
          return ParticipantPersonsService.updateParticipantPerson(projectId, participantPerson);
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
          $scope.view.homeInstitutions = [];
          $scope.view.homeInstitutionAddresses = [];
          $scope.view.selectedHomeInstitutionAddresses = [];

          var homeInstitution = $scope.view.participantPerson.homeInstitution;
          if (homeInstitution) {
              homeInstitution.id = homeInstitution.organizationId;
              return loadOrganizationById(homeInstitution.organizationId)
              .then(function (org) {
                  $scope.view.homeInstitutions.push(org);
                  org.addresses = org.addresses || [];
                  org.addresses = orderByFilter(org.addresses, '-isPrimary')
                  angular.forEach(org.addresses, function (orgAddress, index) {
                      $scope.view.homeInstitutionAddresses.push(orgAddress);
                  });
                  if ($scope.view.participantPerson.homeInstitutionAddressId) {
                      $scope.view.selectedHomeInstitutionAddresses.push($scope.view.participantPerson.homeInstitutionAddressId);
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
          $scope.view.hostInstitutions = [];
          $scope.view.hostInstitutionAddresses = [];
          $scope.view.selectedHostInstitutionAddresses = [];

          var hostInstitution = $scope.view.participantPerson.hostInstitution;
          if (hostInstitution) {
              hostInstitution.id = hostInstitution.organizationId;
              return loadOrganizationById(hostInstitution.organizationId)
              .then(function (org) {
                  $scope.view.hostInstitutions.push(org);
                  org.addresses = org.addresses || [];
                  org.addresses = orderByFilter(org.addresses, '-isPrimary')
                  angular.forEach(org.addresses, function (orgAddress, index) {
                      $scope.view.hostInstitutionAddresses.push(orgAddress);
                  });
                  if ($scope.view.participantPerson.hostInstitutionAddressId) {
                      $scope.view.selectedHostInstitutionAddresses.push($scope.view.participantPerson.hostInstitutionAddressId);
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

      function initializePlacementOrganization(organization) {
          $scope.view.placementOrganizations = [];
          $scope.view.placementOrganizationAddresses = [];
          $scope.view.selectedPlacementOrganizationAddresses = [];

          var placementOrganization = $scope.view.participantPerson.placementOrganization;
          if (placementOrganization) {
              placementOrganization.id = placementOrganization.organizationId;
              return loadOrganizationById(placementOrganization.organizationId)
              .then(function (org) {
                  $scope.view.placementOrganizations.push(org);
                  org.addresses = org.addresses || [];
                  org.addresses = orderByFilter(org.addresses, '-isPrimary')
                  angular.forEach(org.addresses, function (orgAddress, index) {
                      $scope.view.placementOrganizationAddresses.push(orgAddress);
                  });
                  if ($scope.view.participantPerson.placementOrganizationAddressId) {
                      $scope.view.selectedPlacementOrganizationAddresses.push($scope.view.participantPerson.placementOrganizationAddressId);
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

      function getNewParticipantPerson(participantId, participantStatusId, participantTypeId) {
          return {
              participantId: participantId,
              participantStatusId: participantStatusId,
              participantTypeId: participantTypeId,
              projectId: projectId
          };
      }

      function initializePersonInfo(personInfo) {
          return $q.all([initializeHomeInstitution(personInfo.homeInstitution), initializeHostInstitution(personInfo.hostInstitution), (initializePlacementOrganization(personInfo.placementOrganization))])
          .then(function (results) {

          });
      }

      function getPreferredAddress(institution, participantPersonInstitutionAddressId) {
          var address = null;
          if (institution && institution.addresses && institution.addresses.length > 0) {
              for (var i = 0; i < institution.addresses.length; i++) {
                  var institutionAddress = institution.addresses[i];
                  if (institutionAddress.addressId === participantPersonInstitutionAddressId) {
                      address = institutionAddress;
                      break;
                  }
              }
          }
          return address;
      }

      function loadParticipantInfo(projectId, participantId) {
          $scope.view.isLoadingInfo = true;
          return ParticipantPersonsService.getParticipantPersonsById(projectId, participantId)
          .then(function (response) {
              if (response.data.homeInstitution) {
                  response.data.homeInstitutionId = response.data.homeInstitution.organizationId;
                  response.data.homeInstitution.href = StateService.getOrganizationState(response.data.homeInstitution.organizationId);
                  response.data.homeInstitutionAddress = getPreferredAddress(response.data.homeInstitution, response.data.homeInstitutionAddressId);
              }
              if (response.data.hostInstitution) {
                  response.data.hostInstitutionId = response.data.hostInstitution.organizationId;
                  response.data.hostInstitution.href = StateService.getOrganizationState(response.data.hostInstitution.organizationId);
                  response.data.hostInstitutionAddress = getPreferredAddress(response.data.hostInstitution, response.data.hostInstitutionAddressId);
              }
              if (response.data.placementOrganization) {
                  response.data.placementOrganizationId = response.data.placementOrganization.organizationId;
                  response.data.placementOrganization.href = StateService.getOrganizationState(response.data.placementOrganization.organizationId);
                  response.data.placementOrganizationAddress = getPreferredAddress(response.data.placementOrganization, response.data.placementOrganizationAddressId);
              }
              $scope.view.participantPerson = response.data;
              return initializePersonInfo(response.data)
              .then(function (response) {
                  $scope.view.isLoadingInfo = false;
                  return $scope.view.participantPerson;
              });
          })
          .catch(function (response) {
              $scope.view.isLoadingInfo = false;
              $log.error('Unable to load participant info for ' + participantId + '.');
              NotificationService.showErrorMessage('Unable to load participant info for ' + participantId + '.');
          });
      };

      $scope.view.isLoadingEditParticipantInfoRequiredData = true;
      $q.all([
          loadParticipantTypes(),
          loadParticipantStatii(),
          loadParticipantInfo(projectId, $scope.participantid)])
      .then(function (results) {
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      })
      .catch(function () {
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      });
  });
