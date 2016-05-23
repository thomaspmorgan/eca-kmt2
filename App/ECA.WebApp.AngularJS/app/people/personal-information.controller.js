'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
  .controller('PersonInformationCtrl', function ($scope, $stateParams, $state, $timeout, $q, $filter, $log,
                                                smoothScroll, MessageBox, StateService, ConstantsService, ParticipantPersonsService, NotificationService) {

      $scope.edit = {};
      $scope.edit.General = false;
      $scope.edit.EduEmp = false;
      $scope.showEvalNotes = true;
      $scope.showEduEmp = true;
      $scope.showGeneral = false;
      $scope.showContact = true;
      $scope.showPii = true;

      $scope.personId = $stateParams.personId;

      $scope.updatePiiCallback = function () {
          $scope.$parent.onPersonPiiUpdated();
      }
      
  });
