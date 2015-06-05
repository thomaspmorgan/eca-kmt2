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

      var moneyFlow;

      function getMoneyFlow(data) {
          if (data.results) {
              moneyFlow = data.results[0];
          } else {
              moneyFlow = data;
          }
      }

      return {
          get: function (id) {
              var defer = $q.defer();
              DragonBreath.get('moneyFlows', id)
                .success(function (data) {
                    getMoneyFlow(data);
                    defer.resolve(data);
                });
              return defer.promise;
          },
          getMoneyFlowsByProject: function (id, params) {
              var defer = $q.defer();
              var path = 'projects/' + id + '/moneyFlows'
              DragonBreath.get(params, path)
                .success(function (data) {
                    defer.resolve(data);
                });

              return defer.promise;
          },
          update: function (moneyFlow, id) {
              var defer = $q.defer();
              DragonBreath.save(moneyFlow, 'moneyFlows', id)
                .success(function (data) {
                    getProject(data);
                    defer.resolve(moneyFlow);
                });
              return defer.promise;
          },
          create: function (moneyFlow) {
              var defer = $q.defer();
              DragonBreath.create(moneyFlow, 'moneyFlows')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },
          copy: function (id) {
              var defer = $q.defer();
              DragonBreath.copy(id, 'moneyFlows')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          }
      };
  });
