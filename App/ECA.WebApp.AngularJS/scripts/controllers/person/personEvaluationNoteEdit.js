'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: personEvaluationNoteEditCtrl
 * # personEvaluationNoteEditCtrl
 * Controller for person evaluation notes
 */
angular.module('staticApp')
  .controller('personEvaluationNoteEditCtrl', function ($scope,
      $stateParams,
      $q,
      $log,
      PersonService,
      EvaluationService,
      NotificationService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.personId = parseInt($stateParams.personId);
      $scope.view.isSavingChanges = false;
      $scope.view.maxDescriptionLength = 255;
      $scope.view.evaluations = [];

      $scope.showEditEvaluation = false;
      $scope.EvaluationLoading = false;

      var originalEvaluation = angular.copy($scope.evaluation);

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEvaluationNotes(personId);
      });

      function loadEvaluationNotes(personId) {
          $scope.EvaluationLoading = true;
          PersonService.getEvaluationNotesById(personId)
          .then(function (data) {
              $scope.view.evaluations = data;
          });
          $scope.EvaluationLoading = false;
      };
      
      $scope.view.saveEvaluationChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewEvaluation($scope.evaluation)) {
              //var tempId = angular.copy($scope.evaluation.evaluationNoteId);
              return EvaluationService.addEvaluationNote($scope.evaluation, $scope.view.personId)
                .then(onSaveEvaluationSuccess)
                //.then(function () {
                //    updateEvaluationFormDivId(tempId);
                //    updateEvaluations(tempId, $scope.evaluation);
                //})
                .catch(onSaveEvaluationError);
          }
          else {
              return EvaluationService.updateEvaluationNote($scope.evaluation, $scope.view.personId)
                  .then(onSaveEvaluationSuccess)
                  .catch(onSaveEvaluationError);
          }
      };

      //function updateEvaluations(tempId, evaluation) {
      //    var index = $scope.view.evaluations.map(function (e) { return e.evaluationNoteId }).indexOf(tempId);
      //    $scope.view.evaluations[index] = evaluation;
      //};

      $scope.view.onEditEvaluationClick = function () {
          $scope.view.showEditEvaluation = true;
      };

      $scope.view.cancelEditEvaluation = function () {
          $scope.edit.Evaluation = false;
          //if (isNewEvaluation($scope.evaluation)) {
          //    removeEvaluationFromView($scope.evaluation);
          //}
          //else {
          //    $scope.evaluation = angular.copy(originalEvaluation);
          //}
      };

      $scope.view.cancelEvaluationChanges = function () {
          $scope.view.showEditEvaluation = false;
          if (isNewEvaluation($scope.evaluation)) {
              removeEvaluationFromView($scope.evaluation);
          }
          else {
              $scope.evaluation = angular.copy(originalEvaluation);
          }
      };

      $scope.view.onAddEvaluationClick = function (entityEvaluations) {
          console.assert(entityEvaluations, 'The evaluation entity is not defined.');
          console.assert(entityEvaluations instanceof Array, 'The evaluation entity is defined but must be an array.');
          var note = "";
          var newEvaluation = {
              EvaluationNoteId: null,
              EvaluationNote: note,
              personId: $scope.view.personId,
              isNew: true
          };
          entityEvaluations.splice(0, 0, newEvaluation);
          $scope.showEvalNotes = false;
      };

      $scope.view.onDeleteEvaluationClick = function () {
          if (isNewEvaluation($scope.evaluation)) {
              removeEvaluationFromView($scope.evaluation);
          }
          else {
              $scope.view.isDeletingEvaluation = true;
              return EvaluationService.deleteEvaluationNote($scope.evaluation, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted evaluation.");
                  $scope.view.isDeletingEvaluation = false;
                  removeEvaluationFromView($scope.evaluation);
              })
              .catch(function () {
                  var message = "Unable to delete evaluation.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      }

      function onSaveEvaluationSuccess(response) {
          $scope.evaluation = response.data;
          originalEvaluation = angular.copy($scope.evaluation);
          NotificationService.showSuccessMessage("Successfully saved changes to evaluation.");
          $scope.view.showEditEvaluation = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveEvaluationError() {
          var message = "Failed to save evaluation changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewEvaluation(evalnote) {
          if (evalnote.isNew) {
              return true;
          }
          else {
              return false;
          }
      }

      function removeEvaluationFromView(evaluation) {
          $scope.$emit(ConstantsService.removeNewEvaluationEventName, evaluation);
      }

      function getEvaluationFormDivIdPrefix() {
          return 'evaluationForm';
      }

      function getEvaluationFormDivId() {
          return getEvaluationFormDivIdPrefix() + $scope.evaluation.evaluationNoteId;
      }

      function updateEvaluationFormDivId(tempId) {
          var id = getEvaluationFormDivIdPrefix() + tempId;
          var e = getEvaluationFormDivElement(id);
          e.id = getEvaluationFormDivIdPrefix() + $scope.evaluation.evaluationNoteId;
      }

      function getEvaluationFormDivElement(id) {
          return document.getElementById(id);
      }

  });