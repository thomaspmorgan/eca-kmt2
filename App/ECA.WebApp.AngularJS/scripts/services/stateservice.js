'use strict';

/**
 * @ngdoc service
 * @name staticApp.StateService
 * @description
 * # authService
 * Factory for handling angularjs states.
 */
angular.module('staticApp')
  .factory('StateService', function ($rootScope, $log, $http, $state, ConstantsService, DragonBreath) {

      var service = {
          stateNames: {
              overview: {
                  project: 'projects.overview',
                  program: 'programs.overview',
                  office: 'offices.overview',
                  organization: 'organizations.overview',
                  person: 'people.personalinformation'
              }
          },

          isStateAvailableByMoneyFlowSourceRecipientTypeId: function (moneyFlowSourceReceipientTypeId) {
              if (moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id
                  || moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id
                  || moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id
                  || moneyFlowSourceReceipientTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
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
          },

          getProjectState: function (projectId) {
              return $state.href(service.stateNames.overview.project, { projectId: projectId }) + '#top';
          },

          getProgramState: function (programId) {
              return $state.href(service.stateNames.overview.program, { programId: programId }) + '#top';
          },

          getOfficeState: function (officeId) {
              return $state.href(service.stateNames.overview.office, { officeId: officeId }) + '#top';
          },

          getOrganizationState: function (organizationId) {
              return $state.href(service.stateNames.overview.organization, { organizationId: organizationId }) + '#top';
          },

          getPersonState: function (personId) {
              return $state.href(service.stateNames.overview.person, { personId: personId }) + '#top';
          },

          goToProgramState: function (programId) {
              return $state.go(service.stateNames.overview.program, { programId: programId }) + '#top';
          },

          goToProjectState: function (projectId) {
              return $state.go(service.stateNames.overview.project, { projectId: projectId }) + '#top';
          }
      };
      return service;
  });