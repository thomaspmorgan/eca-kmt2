'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ItineraryStopCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        NotificationService,
        ConstantsService,
        LocationService,
        MessageBox,
        ProjectService,
        FilterService,
        LookupService) {

      $scope.view = {};
      $scope.view.isInEditMode = false;
      $scope.view.itineraryStop = $scope.itineraryStop;
      $scope.view.itinerary = $scope.itinerary;
      $scope.view.isItineraryStopExpanded = false;
      $scope.view.isParticipantsExpanded = false;

      $scope.view.onExpandItineraryStopClick = function (itineraryStop) {
          $scope.view.isItineraryStopExpanded = true;
      }

      $scope.view.onCollapseItineraryStopClick = function (itineraryStop) {
          $scope.view.isItineraryStopExpanded = false;
      }

      $scope.view.onExpandParticipantsClick = function (itineraryStop) {
          $scope.view.isParticipantsExpanded = true;
      }

      $scope.view.onCollapseParticipantsClick = function (itineraryStop) {
          $scope.view.isParticipantsExpanded = false;
      }

      $scope.view.onExpandGroupClick = function (group) {
          group.isExpanded = true;
      }

      $scope.view.onCollapseGroupClick = function (group) {
          group.isExpanded = false;
      }

      $scope.view.onEditGroupClick = function (group) {
          $log.info('edit group click');
      }

      $scope.view.onCommentGroupClick = function (group) {
          $log.info('comment group');
      }

      $scope.view.onDeleteGroupClick = function (group) {
          $log.info('delete group');
      }

      $scope.view.onEditClick = function (itineraryStop) {
          $log.info('edit itinerary stop');
          //$scope.view.isInEditMode = true;
      }

      $scope.view.onCommentClick = function (itineraryStop) {
          $log.info('comment itinerary stop');
      }

      $scope.view.onDeleteClick = function (itineraryStop) {
          $log.info('delete itinerary stop');
      }


      angular.forEach($scope.view.itineraryStop.groups, function (group, index) {
          group.isExpanded = false;
      });
  });
