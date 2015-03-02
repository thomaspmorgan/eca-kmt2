'use strict';

/**
 * @ngdoc service
 * @name staticApp.program
 * @description
 * # program
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ProgramService', function (DragonBreath, $q) {
    var program;
    function getProgram(data) {
        if (data.results) {
           program = data.results[0];
        } else {
            program = data;
        }
        delete program._id;
        delete program._rev;
       // if (!program.tabs) {
       //      program.tabs = {
       //          overview: {
       //              title: 'Overview',
       //              path: 'overview',
       //              active: true,
       //              order: 1
       //          },
       //          projects: {
       //              title: 'Projects & Regions',
       //              path: 'projects',
       //              active: true,
       //              order: 2
       //          },
       //          timeline: {
       //              title: 'Timeline',
       //              path: 'timeline',
       //              active: false,
       //              order: 3
       //          },
       //          artifacts: {
       //              title: 'Artifacts',
       //              path: 'artifacts',
       //              active: false,
       //              order: 4
       //          },
       //          moneyflows: {
       //              title: 'Money Flows',
       //              path: 'moneyFlows',
       //              active: false,
       //              order: 5
       //          }
       //      };
       // }
    }

    return {
      get: function (id) {
        var defer = $q.defer();
        DragonBreath.get('programs', id)
          .success(function (data) {
            getProgram(data);
            defer.resolve(program);
          });
        return defer.promise;
      },
      getAllPrograms: function (params) {
          var defer = $q.defer();
          DragonBreath.get(params, 'programs')
            .success(function (data) {
                defer.resolve(data);
            });
          return defer.promise;
      },
      update: function (program, id) {
        var defer = $q.defer();
        DragonBreath.save(program, 'programs', id)
          .success(function (data) {
              getProgram(data);
              defer.resolve(program);
          });
        return defer.promise;
      },
      create: function (program) {
        var defer = $q.defer();
        DragonBreath.create(program, 'programs')
          .success(function (data) {
            getProgram(data);
            defer.resolve(program);
          });
          return defer.promise;
      }
    };
  });
