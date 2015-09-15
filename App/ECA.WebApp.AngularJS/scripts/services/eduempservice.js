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
          addEduEmp: function (eduemp, personId) {
              return DragonBreath.create(eduemp, 'people/' + personId + '/eduemp');
          },
          updateEduEmp: function (eduemp, personId) {
              return DragonBreath.save(eduemp, 'people/' + personId + '/eduemp');
          },
          deleteEduEmp: function (eduemp, personId) {
              return DragonBreath.delete(eduemp, 'people/' + personId + '/eduemp/' + eduemp.id);
          }
      };
  });