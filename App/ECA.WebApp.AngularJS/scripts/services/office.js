'use strict';

/**
 * @ngdoc service
 * @name staticApp.program
 * @description
 * # program
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('OfficeService', function (DragonBreath, $q) {

      return {
          get: function (id) {
              return DragonBreath.get('offices', id);
          },          
          update: function (program, id) {
              return DragonBreath.save(program, 'offices', id);
          },
          create: function (program) {
              return DragonBreath.create(program, 'offices');
          },
          getPrograms: function (params, officeId) {
              var path = 'offices/' + officeId + '/Programs';
              return DragonBreath.get(params, path)
          },
          getChildOffices: function (officeId) {
              var path = 'offices/' + officeId + '/ChildOffices';
              return DragonBreath.get(path)
          },
          getAll: function (filterParams) {
              return DragonBreath.get(filterParams, 'offices');
          }
      };
  });
