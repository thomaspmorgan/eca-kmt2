'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # moneyFlow
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('MoneyFlowService', function (DragonBreath, $q) {
      return {          
          getMoneyFlowsByProgram: function (id, params) {
              var path = 'programs/' + id + '/moneyFlows'
              return DragonBreath.get(params, path);
          },
          getMoneyFlowsByProject: function (id, params) {
              var path = 'projects/' + id + '/moneyFlows'
              return DragonBreath.get(params, path);
          },
          update: function (moneyFlow, id) {
              return DragonBreath.save(moneyFlow, 'moneyFlows', id);
          },
          create: function (moneyFlow) {
              return DragonBreath.create(moneyFlow, 'moneyFlows');
          }
      };
  });
