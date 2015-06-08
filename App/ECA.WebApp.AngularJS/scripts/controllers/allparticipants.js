'use strict';

/**
 * @ngdoc function
* @name staticApp.controller:AllParticipantsCtrl
 * @description
 * # AllParticipantsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllParticipantsCtrl', function ($scope, $stateParams, $state, $log, ParticipantService, TableService, ConstantsService, NotificationService) {

      $scope.participants = [];
      $scope.start = 0;
      $scope.end = 0;
      $scope.total = 0;

      $scope.participantsLoading = false;

      $scope.onParticipantClick = function (participant) {
          
          if (participant.personId) {
              $log.info('Navigating the individual state.');
              $state.go('participants.personalinformation', { participantId: participant.participantId });
          }
          else if(participant.organizationId){
              $log.info('Navigating to organization overview state.');
              $state.go('organizations.overview', { organizationId: participant.organizationId });
          }
          else {
              NotificationService.showErrorMessage('The participant is neither an organization nor a person.');
          }
      };

      $scope.getParticipants = function (tableState) {

          $scope.participantsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()

          };

          ParticipantService.getParticipants(params)
            .then(function (data) {
                $scope.participants = data.results;
                var limit = TableService.getLimit();
                var start = TableService.getStart();
                tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
                $scope.start = start + 1;
                $scope.end = start + data.results.length;
                $scope.total = data.total;
                $scope.participantsLoading = false;
            });
      };

  });
