'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationOverviewCtrl
 * @description The overview controller is used on the overview tab of an organization.
 * # OrganizationOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OrganizationOverviewCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        OrganizationService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;


      //$scope.view.isLoading = true;
      //$q.all([loadProject(), loadOfficeSettings()])
      //.then(function (results) {
      //    //results is an array

      //}, function (errorResponse) {
      //    $log.error('Failed initial loading of project view.');
      //})
      //.then(function () {
      //    $scope.view.isLoading = false;
      //});
  });
