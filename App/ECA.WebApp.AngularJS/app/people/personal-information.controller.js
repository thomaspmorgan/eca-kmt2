'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
  .controller('PersonInformationCtrl', function ($scope, $stateParams, $state, $timeout, $q, $filter, $log,
                                                smoothScroll, MessageBox, StateService, ConstantsService, ParticipantPersonsService, NotificationService) {

      $scope.edit = {};
      $scope.edit.General = false;
      $scope.edit.Pii = false;
      $scope.edit.Contact = false;
      $scope.edit.EduEmp = false;
      $scope.edit.blockEdit = false;
      $scope.showEvalNotes = true;
      $scope.showEduEmp = true;
      $scope.showGeneral = true;
      $scope.showContact = true;
      $scope.sevisStatus = { statusName: "", statusNameId: 0 };

      var notifyStatuses = ConstantsService.sevisStatusIds.split(',');
      
      ParticipantPersonsService.getParticipantPersonById($stateParams.personId)
          .then(function (data) {
              $scope.sevisStatus.statusName = data.data.sevisStatus;
              $scope.sevisStatus.statusNameId = data.data.sevisStatusId;
              $scope.isDisabled();
          }, function (error) {
              $log.error('Unable to load participant info for ' + $stateParams.personId + '.');
              NotificationService.showErrorMessage('Unable to load participant info for ' + $stateParams.personId + '.');
          });
      
      $scope.isDisabled = function () {
          if (notifyStatuses.indexOf($scope.sevisStatus.statusNameId.toString()) !== -1) {
              $scope.edit.blockEdit = true;
          } else {
              $scope.edit.blockEdit = false;
          }
      }
      
  });
