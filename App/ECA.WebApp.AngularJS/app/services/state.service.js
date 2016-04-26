'use strict';

/**
 * @ngdoc service
 * @name staticApp.StateService
 * @description
 * # authService
 * Factory for handling angularjs states.
 */
angular.module('staticApp')
  .factory('StateService', function ($rootScope, $log, $http, $state, ConstantsService, ParticipantService, DragonBreath) {

      var projectPrefix = 'projects';
      var programPrefix = 'programs';
      var officePrefix = 'offices';
      var organizationPrefix = 'organizations';
      var fundingPrefix = 'funding'
      var personPrefix = 'people';
      var personalInformationPrefix = 'personalinformation';

      var participantProjectPrefix = projectPrefix + '.participants';

      var service = {
          stateNames: {
              prefixes: {
                  project: projectPrefix,
                  program: programPrefix,
                  office: officePrefix,
                  organization: organizationPrefix,
                  funding: fundingPrefix,
                  person: personPrefix
              },
              overview: {
                  project: projectPrefix + '.overview',
                  program: programPrefix + '.overview',
                  office: officePrefix + '.overview',
                  organization: organizationPrefix + '.overview',
                  funding: fundingPrefix + '.overview',
                  person: personPrefix + '.personalinformation'
              },
              people: {
                  personalInfomation: personPrefix + '.' + personalInformationPrefix,
                  section: {
                      general: personPrefix + '.' + personalInformationPrefix + '.' + 'general',
                      pii: personPrefix + '.' + personalInformationPrefix + '.' + 'pii',
                      contact: personPrefix + '.' + personalInformationPrefix + '.' + 'contact',
                  }
              },
              edit: {
                  project: projectPrefix + '.edit',
                  program: programPrefix + '.edit',
                  office: officePrefix + '.edit'
              },
              moneyflow: {
                  organization: organizationPrefix + '.moneyflows',
                  person: personPrefix + '.moneyflows',
                  office: officePrefix + '.moneyflows',
                  project: projectPrefix + '.moneyflows',
                  program: programPrefix + '.moneyflows'
              },
              participant: {
                  sevis: participantProjectPrefix + '.sevis'
              }
          },

          isStateAvailableByMoneyFlowSourceRecipientTypeId: function (moneyFlowSourceReceipientTypeId) {
              if (moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id
                  || moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id
                  || moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id
                  || moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id
                  || moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
                  return true;
              }
              else {
                  return false;
              }
          },

          getStateByMoneyFlowSourceRecipientType: function (entityId, moneyFlowSourceReceipientTypeId) {
              console.assert(service.isStateAvailableByMoneyFlowSourceRecipientTypeId(moneyFlowSourceReceipientTypeId), "The state is not supported for the money flow source recipient type.");
              if (moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
                  return service.getOrganizationState(entityId);
              }
              if (moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
                  return service.getProgramState(entityId);
              }
              if (moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
                  return service.getProjectState(entityId);
              }
              if (moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
                  return service.getOfficeState(entityId);
              }
              if (moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.participant.id) {
                  return service.getParticipantState(entityId);
              }
              throw Error('The money flow source recipient type is not supported.');
          },

          getProjectState: function (projectId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.project, { projectId: projectId }, options);
          },

          getProgramState: function (programId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.program, { programId: programId }, options);
          },

          getOfficeState: function (officeId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.office, { officeId: officeId }, options);
          },

          getOrganizationState: function (organizationId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.organization, { organizationId: organizationId }, options);
          },


          getFundingState: function (organizationId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.funding, { organizationId: organizationId }, options);
          },

          getPersonState: function (personId, options) {
              options = options || {};
              return $state.href(service.stateNames.people.personalInfomation, { personId: personId }, options);
          },

          getPiiState: function (personId, options) {
              options = options || {};
              return $state.href(service.stateNames.people.section.pii, { personId: personId }, options);
          },

          getPersonGeneralInformationState: function (personId, options) {
              options = options || {};
              return $state.href(service.stateNames.people.section.general, { personId: personId }, options);
          },

          getPersonContactInformationState: function (personId, options) {
              options = options || {};
              return $state.href(service.stateNames.people.section.contact, { personId: personId }, options);
          },

          getParticipantState: function (participantId, options) {
              options = options || {};
              return ParticipantService.getParticipantById(participantId)
              .then(function (data) {
                  if (data.personId) {
                      return service.getPersonState(data.personId, options);
                  }
                  else {
                      return service.getOrganizationState(data.organizationId, options);
                  }
              });
          },

          getParticipantSevisState: function (projectId, participantId, options) {
              options = options || {};
              return $state.href(service.stateNames.participant.sevis, { participantId: participantId, projectId: projectId }, options);
          },

          goToParticipantSevisState: function (projectId, participantId, options) {
              options = options || {};
              return $state.go(service.stateNames.participant.sevis, { participantId: participantId, projectId: projectId }, options);
          },

          goToOfficeState: function (officeId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.office, { officeId: officeId }, options);
          },

          goToOrganizationState: function (organizationId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.organization, { organizationId: organizationId }, options);
          },

          goToFundingState: function (organizationId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.funding, { organizationId: organizationId }, options);
          },

          goToPersonState: function (personId, options) {
              options = options || {};
              return $state.go(service.stateNames.people.personalInfomation, { personId: personId }, options);
          },

          goToPiiState: function (personId, options) {
              options = options || {};
              return $state.go(service.stateNames.people.section.pii, { personId: personId }, options);
          },

          goToPersonGeneralInformationState: function (personId, options) {
              options = options || {};
              return $state.go(service.stateNames.people.section.general, { personId: personId }, options);
          },

          goToPersonContactInformationState: function (personId, options) {
              options = options || {};
              return $state.go(service.stateNames.people.section.contact, { personId: personId }, options);
          },

          goToProgramState: function (programId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.program, { programId: programId }, options);
          },

          goToProjectState: function (projectId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.project, { projectId: projectId }, options);
          },

          goToParticipantState: function (participantId, options) {
              options = options || {};
              return ParticipantService.getParticipantById(participantId)
              .then(function (data) {
                  if (data.personId) {
                      return service.goToPersonState(data.personId, options);
                  }
                  else {
                      return service.goToOrganizationState(data.organizationId, options);
                  }
              });
          },

          goToForbiddenState: function () {
              return $state.go('forbidden');
          },

          goToErrorState: function () {
              return $state.go('error');
          },

          goToEditProgramState: function (programId, options) {
              options = options || {};
              return $state.go(service.stateNames.edit.program, { programId: programId }, options);
          },

          isProjectState: function (stateName) {
              console.assert(stateName, 'The stateName must be defined.');
              return stateName.indexOf(service.stateNames.prefixes.project) == 0;
          },

          isProgramState: function (stateName) {
              console.assert(stateName, 'The stateName must be defined.');
              return stateName.indexOf(service.stateNames.prefixes.program) == 0;
          },

          isOfficeState: function (stateName) {
              console.assert(stateName, 'The stateName must be defined.');
              return stateName.indexOf(service.stateNames.prefixes.office) == 0;
          },

          isOrganizationState: function (stateName) {
              console.assert(stateName, 'The stateName must be defined.');
              return stateName.indexOf(service.stateNames.prefixes.organization) == 0;
          },

          isPersonState: function (stateName) {
              console.assert(stateName, 'The stateName must be defined.');
              return stateName.indexOf(service.stateNames.prefixes.person) == 0;
          }
      };
      return service;
  });