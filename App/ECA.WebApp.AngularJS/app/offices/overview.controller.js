'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:OrganizationOverviewCtrl
 * @description The overview controller is used on the overview tab of an organization.
 * # OrganizationOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OfficeOverviewCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $filter,
        DataPointConfigurationService,
        ConstantsService,
        BrowserService,
        NotificationService) {

      $scope.view = {};
      $scope.view.isOfficeLoading = false;
      $scope.view.params = $stateParams;
      $scope.view.office = {};
      $scope.view.dataPointConfigurations = {};

      isLoadingOffice(true);
      $scope.data.loadedOfficePromise.promise
      .then(function (office) {
          BrowserService.setDocumentTitleByOffice(office, 'Overview');
          $scope.view.office = office;
          var params = { officeId: office.id };
          DataPointConfigurationService.getDataPointConfigurations(params)
                .then(function (response) {
                    var array = $filter('filter')(response.data, { categoryId: ConstantsService.dataPointCategory.office.id });
                    for (var i = 0; i < array.length; i++) {
                        $scope.view.dataPointConfigurations[array[i].propertyId] = array[i].isRequired;
                    }
                }, function () {
                    NotificationService.showErrorMessage('Unable to load data point configurations for office with id = ' + parameters.foreignResourceId + ".");
                });
          isLoadingOffice(false);
      });

      function isLoadingOffice(isLoading) {
          $scope.view.isOfficeLoading = isLoading;
      }
  });
