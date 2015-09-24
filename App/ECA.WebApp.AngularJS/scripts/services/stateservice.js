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

      var service = {
          stateNames: {
              overview: {
                  project: 'projects.overview',
                  program: 'programs.overview',
                  office: 'offices.overview',
                  organization: 'organizations.overview',
                  person: 'people.personalinformation'
              },
              edit: {
                  project: 'projects.edit',
                  program: 'programs.edit'
              },
              moneyflow: {
                  organization: 'organizations.moneyflows',
                  person: 'people.moneyflows',
                  office: 'offices.moneyflows',
                  project: 'projects.moneyflows',
                  program: 'programs.moneyflows'

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
              return $state.href(service.stateNames.overview.project, { projectId: projectId }, options) + '#top';
          },

          getProgramState: function (programId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.program, { programId: programId }, options) + '#top';
          },

          getOfficeState: function (officeId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.office, { officeId: officeId }, options) + '#top';
          },

          getOrganizationState: function (organizationId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.organization, { organizationId: organizationId }, options) + '#top';
          },

          getPersonState: function (personId, options) {
              options = options || {};
              return $state.href(service.stateNames.overview.person, { personId: personId }, options) + '#top';
          },

          getParticipantState: function(participantId, options){
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

          goToOfficeState: function (officeId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.office, { officeId: officeId }, options);
          },

          goToOrganizationState: function (organizationId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.organization, { organizationId: organizationId }, options);
          },

          goToPersonState: function (personId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.person, { personId: personId }, options);
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
              return $state.go(service.stateNames.edit.program, { programId: programId }, options) + '#top';
          }
      };
      return service;
  });