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
          getEducations: function (personId, params) {
                return DragonBreath.get(params, 'eduemp/' + personId + '/educations');
          },
          getEmployments: function (personId, params) {
                return DragonBreath.get(params, 'eduemp/' + personId + '/employments');
          },
          addEducation: function (eduemp, personId) {
              return DragonBreath.create(eduemp, 'eduemp/' + personId + '/education');
          },
          addEmployment: function (eduemp, personId) {
              return DragonBreath.create(eduemp, 'eduemp/' + personId + '/employment');
          },
          updateEducation: function (eduemp, personId) {
              return DragonBreath.save(eduemp, 'eduemp/' + personId + '/education');
          },
          updateEmployment: function (eduemp, personId) {
              return DragonBreath.save(eduemp, 'eduemp/' + personId + '/employment');
          },
          deleteEducation: function (eduemp, personId) {
              return DragonBreath.delete(eduemp, 'eduemp/' + personId + '/education/' + eduemp.professionEducationId);
          },
          deleteEmployment: function (eduemp, personId) {
              return DragonBreath.delete(eduemp, 'eduemp/' + personId + '/employment/' + eduemp.professionEducationId);
          }
      };
  });