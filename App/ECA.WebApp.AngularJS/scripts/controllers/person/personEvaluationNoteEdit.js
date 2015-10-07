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
      AuthService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.personId = parseInt($stateParams.personId);
      $scope.view.isSavingChanges = false;
      $scope.view.maxDescriptionLength = 255;
      $scope.view.ecaUserId = null;
      $scope.view.evaluations = [];
      var tempEvalId = 0;
      $scope.showEditEvaluation = false;
      $scope.EvaluationLoading = false;

      var originalEvaluation = angular.copy($scope.evaluation);

      function loadUserId() {
          return AuthService.getUserInfo()
              .then(function (response) {
                  $scope.view.ecaUserId = response.data.ecaUserId;
                  return response.data.ecaUserId;
              })
              .catch(function () {
                  $log.error('Unable to load user info.');
              });
      }

      $scope.personIdDeferred.promise
      .then(function (personId) {
          loadEvaluationNotes(personId);
          loadUserId();
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
          $scope.evaluation.personId = $scope.view.personId;
          if (isNewEvaluation($scope.evaluation)) {
              tempEvalId = angular.copy($scope.evaluation.evaluationNoteId);
              return EvaluationService.addEvaluationNote($scope.evaluation, $scope.view.personId)
                .then(onSaveEvaluationSuccess)
                .then(function () {
                    updateEvaluationFormDivId(tempEvalId);
                    updateEvaluations(tempEvalId, $scope.evaluation);
                })
                .catch(onSaveEvaluationError);
          }
          else {
              return EvaluationService.updateEvaluationNote($scope.evaluation, $scope.view.personId)
                  .then(onSaveEvaluationSuccess)
                  .catch(onSaveEvaluationError);
          }
      };

      function updateEvaluations(tempId, evaluation) {
          var index = $scope.view.evaluations.map(function (e) { return e.evaluationNoteId }).indexOf(tempId);
          $scope.view.evaluations[index] = evaluation;
      };

      $scope.view.onEditEvaluationClick = function () {
          $scope.view.showEditEvaluation = true;
      };

      $scope.view.cancelEditEvaluation = function () {
          $scope.edit.Evaluation = false;
      };

      $scope.view.cancelEvaluationChanges = function (form) {
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
              evaluationNoteId: --tempEvalId,
              evaluationNote: note,
              personId: $scope.view.personId,
              userId: null,
              userName: null,
              addedOn: null,
              revisedOn: null,
              emailAddress: null,
              isNew: true
          };
          entityEvaluations.splice(0, 0, newEvaluation);
          $scope.showEvalNotes = false;
      };

      $scope.view.onDeleteEvaluationClick = function (index) {
          if (isNewEvaluation($scope.evaluation)) {
              removeEvaluationFromView(index);
          } else {
              $scope.view.isDeletingEvaluation = true;
              return EvaluationService.deleteEvaluationNote($scope.evaluation, $scope.view.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted evaluation.");
                  $scope.view.isDeletingEvaluation = false;
                  removeEvaluationFromView(index);
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
          return evalnote.isNew;
      }

      function removeEvaluationFromView(index) {
          $scope.$emit(ConstantsService.removeNewEvaluationEventName, index);
      }

      function getEvaluationFormDivIdPrefix() {
          return 'evaluationForm';
      }

      function updateEvaluationFormDivId(tempId) {
          var id = getEvaluationFormDivIdPrefix() + tempId;
          var e = getEvaluationFormDivElement(id);
          e.id = getEvaluationFormDivIdPrefix() + $scope.evaluation.evaluationNoteId.toString();
      }

      function getEvaluationFormDivElement(id) {
          return document.getElementById(id);
      }
      
      $scope.$on(ConstantsService.removeNewEvaluationEventName, function (event, index) {
          console.assert($scope.view, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.view.evaluations instanceof Array, 'The entity evaluation is defined but must be an array.');

          $scope.view.evaluations.splice(index, 1);
          $log.info('Removed one new evaluation at index ' + index);
      });

  });