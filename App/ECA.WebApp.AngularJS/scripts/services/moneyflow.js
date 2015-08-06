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
              var path = 'programs/' + id + '/moneyFlows'
              return DragonBreath.get(params, path);
          },
          getMoneyFlowsByProject: function (id, params) {
              var path = 'projects/' + id + '/moneyFlows'
              return DragonBreath.get(params, path);
          },
          update: function (moneyFlow, entityId) {
              var path = '';
              if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.project.id) {
                  path = 'projects/' + entityId + '/moneyflows';
              }
              else if (moneyFlow.entityTypeId === ConstantsService.moneyFlowSourceRecipientType.program.id) {
                  path = 'programs/' + entityId + '/moneyflows';
              }
              else {
                  throw Error('The money flow source recipient type is not yet recognized.');
              }
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
              else {
                  throw Error('The money flow source recipient type is not yet supported.');
              }
              path += '/MoneyFlows';
              return DragonBreath.create(moneyFlow, path);
          }
      };
  });
