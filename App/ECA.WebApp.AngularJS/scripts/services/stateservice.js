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
              return $state.href('projects.overview', { projectId: projectId });
          },

          getProgramState: function (programId) {
              return $state.href('programs.overview', { programId: programId });
          },

          getOfficeState: function (officeId) {
              return $state.href('offices.overview', { officeId: officeId });
          },

          getOrganizationState: function (organizationId) {
              return $state.href('organizations.overview', { organizationId: organizationId });
          },

          getPersonState: function (personId) {
              return $state.href('people.personalinformation', { personId: personId });
          }
      };
      return service;
  });