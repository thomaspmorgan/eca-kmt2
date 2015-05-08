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
          getAll: function (params) {
              return DragonBreath.get(params, 'offices');
          },
          getSettings: function (officeId) {
              var path = 'offices/' + officeId + '/Settings';
              return DragonBreath.get(path);
          },
          getSettingsValue: function (allSettings, settingName) {
              for (var i = 0; i < allSettings.length; i++) {
                  var setting = allSettings[i];
                  if (setting.name === settingName) {
                      return setting.value;
                  }
              }
              return null;
          }

      };
  });
