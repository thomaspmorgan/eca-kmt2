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
          addProfessionEducation: function (eduemp, personId) {
              return DragonBreath.create(eduemp, 'eduemp/' + personId + '/professioneducation');
          },
          updateProfessionEducation: function (eduemp, personId) {
              return DragonBreath.save(eduemp, 'eduemp/' + personId + '/professioneducation');
          },
          deleteProfessionEducation: function (eduemp, personId) {
              return DragonBreath.delete(eduemp, 'eduemp/' + personId + '/professioneducation/' + eduemp.professionEducationId);
          }
      };
  });