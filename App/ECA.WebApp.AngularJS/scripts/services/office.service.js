﻿'use strict';

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
          getAll: function (params) {
              return DragonBreath.get(params, 'offices');
          },
          getSettings: function (officeId) {
              var path = 'offices/' + officeId + '/Settings';
              return DragonBreath.get(path);
          },
          getCategories: function (officeId, params) {
              var path = 'offices/' + officeId + '/categories';
              return DragonBreath.get(params, path);
          },
          getObjectives: function (officeId, params) {
              var path = 'offices/' + officeId + '/objectives';
              return DragonBreath.get(params, path);
          },
          addPermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'offices/collaborator/add');
          },
          revokePermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'offices/collaborator/revoke');
          },
          removePermission: function (permissionModel) {
              return DragonBreath.create(permissionModel, 'offices/collaborator/remove');
          }
      };
  });