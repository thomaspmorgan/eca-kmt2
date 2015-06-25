angular.module('staticApp')
  .controller('ProgramCollaboratorsCtrl', function ($scope, $stateParams, $q, ProgramService, AuthService, ConstantsService) {

      function loadCollaborators(params) {
          var programId = $stateParams.programId;
          var url = '/programs/' + programId + '/collaborators';
          //isLoading(true);
          return $q.when(AuthService.getPrincipalResourceAuthorizations(ConstantsService.resourceType.program.value, programId, url, params))
          .then(updateCollaborators, showLoadCollaboratorsError)
          .then(function () {
              //isLoading(false);
          });
      }

      function updateCollaborators(resourceAuthorizations) {
          console.log(resourceAuthorizations);
          $scope.collaborators = resourceAuthorizations;
      }

      function showLoadCollaboratorsError() {
          NotificationService.showErrorMessage('There was an error loading collaborators.');
      }

      $q.all([loadCollaborators({start: 0, limit: 300})]);

  });