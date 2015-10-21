'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # moneyFlow
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('MoneyFlowService', function (DragonBreath, ConstantsService, $q) {
      return {          
          getMoneyFlowsByProgram: function (id, params) {
              var path = 'programs/' + id + '/moneyFlows';
              return DragonBreath.get(params, path);
          },
          getMoneyFlowsByProject: function (id, params) {
              var path = 'projects/' + id + '/moneyFlows';
              return DragonBreath.get(params, path);
          },
          getMoneyFlowsByOffice: function (id, params) {
              var path = 'offices/' + id + '/moneyFlows';
              return DragonBreath.get(params, path);
          },
          getMoneyFlowsByOrganization: function (id, params) {
              var path = 'organizations/' + id + '/moneyFlows';
              return DragonBreath.get(params, path);
          },
          getMoneyFlowsByPersonId: function (id, params) {
              var path = 'people/' + id + '/moneyFlows';
              return DragonBreath.get(params, path);
          },
          getSourceMoneyFlowsByProjectId: function(id, params){
              var path = 'projects/' + id + '/moneyflows/sources';
              return DragonBreath.get(params, path);
          },
          getSourceMoneyFlowsByProgramId: function (id, params) {
              var path = 'programs/' + id + '/moneyflows/sources';
              return DragonBreath.get(params, path);
          },
          getSourceMoneyFlowsByOfficeId: function (id, params) {
              var path = 'offices/' + id + '/moneyflows/sources';
              return DragonBreath.get(params, path);
          },
          getSourceMoneyFlowsByOrganizationsId: function (id, params) {
              var path = 'organizations/' + id + '/moneyflows/sources';
              return DragonBreath.get(params, path);
          },
          remove: function(moneyFlow, entityId) {
              var path = '';
              if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
                  path = 'projects';
              }
              else if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
                  path = 'programs';
              }
              else if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
                  path = 'offices';
              }
              else if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
                  path = 'organizations';
              }
              else {
                  throw Error('The money flow source recipient type is not yet recognized.');
              }
              path += '/' + entityId + '/moneyflows/' + moneyFlow.id;
              return DragonBreath.delete(moneyFlow, path);
          },
          update: function (moneyFlow, entityId) {
              var path = '';
              if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
                  path = 'projects';
              }
              else if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
                  path = 'programs';
              }
              else if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
                  path = 'offices';
              }
              else if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
                  path = 'organizations';
              }
              else {
                  throw Error('The money flow source recipient type is not yet recognized.');
              }
              path += '/' + entityId + '/moneyflows';
              return DragonBreath.save(moneyFlow, path);
          },
          create: function (moneyFlow) {
              console.assert(moneyFlow.entityTypeId, 'The money flow to create must have an entity type id assigned.');
              var entityTypeId = moneyFlow.entityTypeId;
              var path = '';
              if (entityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
                  path += 'project';
              }
              else if (entityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
                  path += 'program';
              }
              else if (entityTypeId === ConstantsService.moneyFlowSourceRecipientType.office.id) {
                  path += 'office';
              }
              else if (entityTypeId === ConstantsService.moneyFlowSourceRecipientType.organization.id) {
                  path += 'organization';
              }
              else {
                  throw Error('The money flow source recipient type is not yet supported.');
              }
              path += '/MoneyFlows';
              return DragonBreath.create(moneyFlow, path);
          }
      };
  });
