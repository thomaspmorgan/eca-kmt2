'use strict';

/**
 * @ngdoc service
 * @name staticApp.person
 * @description
 * # person
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('PersonService', function ($q, DragonBreath) {

    var person;

    function getPerson(data) {
      if (data.results) {
           person = data.results[0];
        } else {
            person = data;
        }
        delete person._id;
        delete person._rev;
       if (!person.participants) {
           person.participants = [];  
       }
       if (!person.tabs) {
            person.tabs = {
                activies: {
                    title: 'Activities',
                    path: 'activity',
                    active: true,
                    order: 1
                },
                personalinformation: {
                    title: 'Personal Information',
                    path: 'personalinformation',
                    active: true,
                    order: 2
                },
                relatedreport: {
                    title: 'Related Reports',
                    path: 'relatedreports',
                    active: true,
                    order: 3
                },
                impact: {
                    title: 'Impact & Stories',
                    path: 'impact',
                    active: true,
                    order: 4
                }
            };
       }
    }

    return {
      get: function (id) {
        var defer = $q.defer();
        DragonBreath.get('people', id)
          .success(function (data) {
            getPerson(data);
             defer.resolve(person);
          });
        return defer.promise;
      },
      create: function (person) {
        var defer = $q.defer();
        DragonBreath.create(person, 'people')
          .success(function (data) {
            getPerson(data);
            defer.resolve(person);
          });
        return defer.promise;
      }
    };
  });
