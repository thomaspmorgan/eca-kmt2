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

      $scope.view.onExpandItineraryStopClick = function (itineraryStop) {
          $scope.view.isItineraryStopExpanded = true;
      }

      $scope.view.onCollapseItineraryStopClick = function (itineraryStop) {
          $scope.view.isItineraryStopExpanded = false;
      }

      $scope.view.onEditClick = function (itineraryStop) {
          $log.info('edit');
          //$scope.view.isInEditMode = true;
      }

      $scope.view.onCommentClick = function (itineraryStop) {
          $log.info('comment');
      }

      $scope.view.onDeleteClick = function (itineraryStop) {
          $log.info('delete');
      }
  });
