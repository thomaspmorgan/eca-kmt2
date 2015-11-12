'use strict';

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
		FilterService,
		LookupService,
        OrganizationService,
        PersonService,
        StateService,
        ConstantsService,
        AuthService,
        ProjectService,
        NotificationService,
        ParticipantService,
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
      $scope.view.isLoadingInfo = false;
      $scope.view.isSavingUpdate = false;
      $scope.view.participantPerson = null;
      $scope.view.isInfoTabInEditMode = false;


      $scope.view.loadHomeInstitutions = function ($search) {
          return loadHomeInstitutions($search);
      }

      $scope.view.loadHostInstitutions = function ($search) {
          return loadHostInstitutions($search);
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
      }

      $scope.view.onRemoveHostInstitutionAddress = function ($item, $model) {
          $scope.view.participantPerson.hostInstitutionAddressId = null;
      }

      $scope.view.onRemoveHomeInstitutionAddress = function ($item, $model) {
          $scope.view.participantPerson.homeInstitutionAddressId = null;
      }

      $scope.view.onCancelButtonClick = function () {
          return loadParticipantInfo($scope.participantid)
              .then(function (response) {
                  $scope.view.isInfoTabInEditMode = false;
              });
      }

      $scope.view.onSaveButtonClick = function () {
          $scope.view.isSavingUpdate = true;
          return saveParticipantPerson($scope.view.participantPerson)
            .then(function (response) {
                return loadParticipantInfo($scope.participantid)
                .then(function (response) {
                    $scope.view.isSavingUpdate = false;
                    $scope.view.isInfoTabInEditMode = false;
                });
            })
            .catch(function (response) {
                $scope.view.isSavingUpdate = false;
                var message = "Unable to update participant person.";
                $log.error(message);
                NotificationService.showErrorMessage(message);
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

      function saveParticipantPerson(participantPerson) {
          return ParticipantPersonsService.updateParticipantPerson(participantPerson);
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

      function getNewParticipantPerson(participantId, participantStatusId, participantTypeId) {
          return {
              participantId: participantId,
              participantStatusId: participantStatusId,
              participantTypeId: participantTypeId,
              projectId: projectId
          };
      }

      function initializePersonInfo(personInfo) {
          return $q.all([initializeHomeInstitution(personInfo.homeInstitution), initializeHostInstitution(personInfo.hostInstitution)])
          .then(function (results) {

          });
      }

      function getPreferredAddress(institution, participantPersonInstitutionAddressId) {
          var address = null;
          if (institution && institution.addresses && institution.addresses.length > 0) {
              angular.forEach(institution.addresses, function (institutionAddress, index) {
                  if (institutionAddress.addressId === participantPersonInstitutionAddressId) {
                      address = institutionAddress;
                  }
              });
          }
          return address;
      }

      var hasAttemptedToSaveNewParticipantPersonCount = 0;
      var maxAttemptedSaveNewParticipantPersonCount = 5;
      function loadParticipantInfo(participantId) {
          $scope.view.isLoadingInfo = true;
          return ParticipantPersonsService.getParticipantPersonsById(participantId)
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
              $scope.view.participantPerson = response.data;
              return initializePersonInfo(response.data)
              .then(function (response) {
                  $scope.view.isLoadingInfo = false;
                  return $scope.view.participantPerson;
              });
          })
          .catch(function (response) {
              if (response.status === 404) {
                  $log.info('The participant person was not found, creating a new participant person automatically.');
                  $scope.view.isLoadingInfo = false;
                  return saveNewParticipantPerson(participantId);
              }
              else {
                  $scope.view.isLoadingInfo = false;
                  $log.error('Unable to load participant info for ' + participantId + '.');
                  NotificationService.showErrorMessage('Unable to load participant info for ' + participantId + '.');
              }
          });
      };

      function saveNewParticipantPerson(participantId) {
          $scope.view.isLoadingInfo = true;
          hasAttemptedToSaveNewParticipantPersonCount++;
          if (hasAttemptedToSaveNewParticipantPersonCount < maxAttemptedSaveNewParticipantPersonCount) {
              return ParticipantService.getParticipantById(participantId)
              .then(function (responseData) {
                  var newParticipantPerson = getNewParticipantPerson(participantId, responseData.statusId, responseData.participantTypeId);
                  return saveParticipantPerson(newParticipantPerson)
                      .then(function (response) {
                          $scope.view.isLoadingInfo = false;
                          return loadParticipantInfo(participantId);
                      })
                      .catch(function (response) {
                          $scope.view.isLoadingInfo = false;
                          var message = "Unable to save participant person.";
                          $log.error(message);
                          NotificationService.showErrorMessage(message);
                      });

              })
              .catch(function (response) {
                  $scope.view.isLoadingInfo = false;
                  var message = "Unable to load participant with id " + participantId;
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              });
          }
          else {
              var message = "Several attempts have been made to save a new participant person and they have failed.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          }
      }

      $scope.view.isLoadingEditParticipantInfoRequiredData = true;
      $q.all([
          loadParticipantTypes(),
          loadParticipantStatii(),
          loadParticipantInfo($scope.participantid)])
      .then(function (results) {
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      })
      .catch(function () {
          $scope.view.isLoadingEditParticipantInfoRequiredData = false;
      });
  });
