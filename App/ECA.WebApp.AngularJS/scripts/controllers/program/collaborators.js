angular.module('staticApp')
  .controller('ProgramCollaboratorsCtrl', function ($scope, $stateParams, $q, ProgramService, AuthService, ConstantsService) {

      $scope.listCount = {
          start: 0,
          total: 0
      }

      $scope.collaboratorsLoading = false;

      function loadCollaborators(params) {
          var programId = $stateParams.programId;
          var url = '/programs/' + programId + '/collaborators';
          $scope.collaboratorsLoading = true;
          return $q.when(AuthService.getPrincipalResourceAuthorizations(ConstantsService.resourceType.program.value, programId, url, params))
          .then(updateCollaborators, showLoadCollaboratorsError)
          .then(function () {
              if ($scope.collaborators.length > 0) {
                  $scope.listCount.start = 1;
              }
              $scope.listCount.total = $scope.collaborators.length;
              $scope.collaboratorsLoading = false;
          });
      }

      function updateCollaborators(resourceAuthorizations) {
          $scope.collaborators = resourceAuthorizations;
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

      $q.all([loadCollaborators({start: 0, limit: 300})]);

  });