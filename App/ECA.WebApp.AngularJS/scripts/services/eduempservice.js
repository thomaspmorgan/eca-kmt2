'use strict';

/**
 * @ngdoc service
 * @name staticApp.eduemp
 * @description
 * # education and employment service
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('EduEmpService', function ($q, DragonBreath) {

      return {
          addEducation: function (eduemp, personId) {
              return DragonBreath.create(eduemp, 'people/' + personId + '/education');
          },
          addEmployment: function (eduemp, personId) {
              return DragonBreath.create(eduemp, 'people/' + personId + '/employment');
          },
          updateEducation: function (eduemp, personId) {
              return DragonBreath.save(eduemp, 'people/' + personId + '/education');
          },
          updateEmployment: function (eduemp, personId) {
              return DragonBreath.save(eduemp, 'people/' + personId + '/employment');
          },
          deleteEducation: function (eduemp, personId) {
              return DragonBreath.delete(eduemp, 'people/' + personId + '/education/' + eduemp.id);
          },
          deleteEmployment: function (eduemp, personId) {
              return DragonBreath.delete(eduemp, 'people/' + personId + '/employment/' + eduemp.id);
          }
      };
  });