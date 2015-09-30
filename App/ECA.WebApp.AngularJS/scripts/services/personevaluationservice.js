'use strict';

/**
 * @ngdoc service
 * @name staticApp.evaluation
 * @description
 * # person evaluation notes service
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('EvaluationService', function ($q, DragonBreath) {

      return {
          addEvaluationNote: function (evaluation, personId) {
              return DragonBreath.create(evaluation, 'people/' + personId + '/EvaluationNote');
          },
          updateEvaluationNote: function (evaluation, personId) {
              return DragonBreath.save(evaluation, 'people/' + personId + '/EvaluationNote');
          },
          deleteEvaluationNote: function (evaluation, personId) {
              return DragonBreath.delete(evaluation, 'people/' + personId + '/EvaluationNote/' + evaluation.evaluationNoteId);
          }
      }

  });