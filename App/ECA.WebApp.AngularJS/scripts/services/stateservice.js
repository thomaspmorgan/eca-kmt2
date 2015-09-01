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

          goToProgramState: function (programId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.program, { programId: programId }, options) + '#top';
          },

          goToProjectState: function (projectId, options) {
              options = options || {};
              return $state.go(service.stateNames.overview.project, { projectId: projectId }, options) + '#top';
          }
      };
      return service;
  });