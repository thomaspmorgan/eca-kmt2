'use strict';

/**
 * @ngdoc service
 * @name staticApp.project
 * @description
 * # project
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ProjectService', function (DragonBreath, $q) {

    var project;

    function getProject(data) {
      if (data.results) {
           project = data.results[0];
        } else {
            project = data;
        }
        delete project._id;
        delete project._rev;
       // if (!project.tabs) {
       //      project.tabs = {
       //          overview: {
       //              title: 'Overview',
       //              path: 'overview',
       //              active: true,
       //              order: 1
       //          },
       //          partners: {
       //              title: 'Partners',
       //              path: 'partners',
       //              active: false,
       //              order: 2
       //          },
       //          participants: {
       //              title: 'Participants',
       //              path: 'participants',
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
       //          },
       //          impact: {
       //              title: 'Impact',
       //              path: 'impact',
       //              active: false,
       //              order: 6
       //          },
       //          activity: {
       //              title: 'Activity',
       //              path: 'activity',
       //              active: false,
       //              order: 7
       //          }
       //      };
       // }
    }

    return {
      get: function (id) {
        var defer = $q.defer();
        DragonBreath.get('projects', id)
          .success(function (data) {
            getProject(data);
             defer.resolve(project);
          });
        return defer.promise;
      },
      getProjectsByProgram: function (id, params) {
        var defer = $q.defer();
        var path = 'programs/' + id + '/projects'
        DragonBreath.get(params, path)
          .success(function (data) {
              defer.resolve(data);
          });

        return defer.promise;
      },
      update: function (project, id) {
        var defer = $q.defer();
        DragonBreath.save(project, 'projects', id)
          .success(function (data) {
              getProject(data);
              defer.resolve(project);
          });
        return defer.promise;
      },
      create: function (project) {
        var defer = $q.defer();
        DragonBreath.create(project, 'projects')
          .success(function (data) {
            defer.resolve(data);
          });
        return defer.promise;
      }
    };
  });
