'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:BranchesAndProgramsCtrl
 * @description
 * # BranchesAndProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('BranchesAndProgramsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        StateService,
        TableService,
        OfficeService,
        BrowserService,
        ConstantsService,
        NotificationService) {

      var officeId = $stateParams.officeId;
      $scope.view = {};
      $scope.view.isOfficeLoading = false;
      $scope.view.office = {};

      $scope.view.branches = [];
      $scope.view.isLoadingBranches = true;
      $scope.view.header = 'Branches & Programs';

      $scope.view.onCreateProgramClick = function () {
          var addProgramModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/programs/add-program-modal.html',
              controller: 'AddProgramModalCtrl',
              backdrop: 'static',
              size: 'lg',
              resolve: {
                  office: function () {
                      return $scope.view.office;
                  },
                  parentProgram: function () {
                      return null;
                  }
              }
          });
          addProgramModalInstance.result.then(function (addedProgram) {
              $log.info('Finished adding program.');
              addProgramModalInstance.close(addedProgram);
              StateService.goToProgramState(addedProgram.id);

          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      }

      function getIds(element) {
          return element.id;
      };

      function setFundingTabEnabled(isEnabled) {
          $scope.isFundingTabEnabled = isEnabled;
      }

      function updateHeader() {
          if ($scope.view.branches.length === 0) {
              $scope.view.header = "Programs";
          }
      }

      function loadChildOffices(officesId) {
          $scope.view.isLoadingBranches = true;
          return OfficeService.getChildOffices(officesId)
              .then(function (response, status, headers, config) {
                  $scope.view.isLoadingBranches = false;
                  $scope.view.branches = response.data;
                  updateHeader();
              })
          .catch(function (response) {
              $scope.view.isLoadingBranches = false;
              var message = "Unable to load child offices and branches.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          })
      }

      isLoadingOffice(true);
      $scope.data.loadedOfficePromise.promise
      .then(function (office) {
          BrowserService.setDocumentTitleByOffice(office, 'Branches and Programs');
          $scope.view.office = office;
          var params = {
              start: 0,
              limit: 300
          };
          return $q.all([loadChildOffices(officeId)])
          .then(function () {
              $log.info('Loaded office specific data.');
              isLoadingOffice(false);
          });
      });

      function isLoadingOffice(isLoading) {
          $scope.view.isOfficeLoading = isLoading;
      }

  });
