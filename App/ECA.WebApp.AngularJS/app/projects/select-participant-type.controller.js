'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProjectCollaboratorCtrl
 * @description The AddProjectCollaboratorCtrl is used to manage collaborators on a project.
 * # AddProjectCollaboratorCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('SelectParticipantTypeCtrl', function ($scope,
      $stateParams,
      $q,
      clientModel,
      $modalInstance,
      LookupService,
      ConstantsService) {

      var projectId = $stateParams.projectId;
      $scope.view = {};
      $scope.view.isLoading = false;
      $scope.view.participantTypes = [];
      $scope.view.clientModel = clientModel;
      $scope.view.onCancelClick = function () {
          $modalInstance.dismiss('cancel');
      }
     
      $scope.view.onOkClick = function () {
          $modalInstance.close($scope.view.clientModel);
      }

      function isLoading(isLoading) {
          $scope.view.isLoading = isLoading;
      }

      function isSaving(isSaving) {
          $scope.view.isSaving = isSaving;
      }

      function updateCollaborators(resourceAuthorizations) {
          $scope.view.collaborators = resourceAuthorizations;
      }

      function loadParticipantTypes(params) {
          return LookupService.getParticipantTypes(params)
          .then(function(response) {
              $scope.view.participantTypes = response.data.results;
          });
      }

      $scope.view.isLoading = true;
      var loadParticipantTypeParams = { start: 0, limit: 300, filter: [] };
      if (clientModel.personId) {
          loadParticipantTypeParams.filter.push({ property: 'isPerson', comparison: ConstantsService.equalComparisonType, value: true });
      } else {
          loadParticipantTypeParams.filter.push({ property: 'isPerson', comparison: ConstantsService.equalComparisonType, value: false });
      }
      $q.all([loadParticipantTypes(loadParticipantTypeParams)])
        .then(function () {
            $scope.view.isLoading = false;
        })
        .catch(function() {
            $log.error('Unable to load participant types.');
        });


  });
