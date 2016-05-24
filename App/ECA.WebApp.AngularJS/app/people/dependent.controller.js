'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: ViewDependentModalCtrl
 * # ViewDependentModalCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ViewDependentModalCtrl', function (
      $scope,
      $modalInstance,
      DependentService,
      DateTimeService,
      DownloadService,
      ConstantsService,
      $stateParams,
      $q,
      dependent) {
      
      $scope.dependent = loadDependent(dependent.id);
      $scope.dependentLoading = true;

      function loadDependent(dependentId) {
          $scope.isDependentLoading = true;
          return DependentService.getDependentById(dependentId)
             .then(function (data) {
                 $scope.dependent = data;
                 if ($scope.dependent.dateOfBirth) {
                     $scope.dependent.dateOfBirth = DateTimeService.getDateAsLocalDisplayMoment($scope.dependent.dateOfBirth).toDate();
                 }
             }), 
            function (error) {
                if (error.status == 400) {
                    if (error.data.message && error.data.modelState) {
                        for (var key in error.data.modelState) {
                            NotificationService.showErrorMessage(error.data.modelState[key][0]);
                        }
                    } else {
                        NotificationService.showErrorMessage(error.data);
                    }
                }
            };
      }

      $scope.downloadDS2019 = function () {
          var url = 'People/Dependent/' + dependent.id + '/DS2019File';
          DownloadService.get(url, 'application/pdf')
          .then(function () {

          }, function () {
              NotificationService.showErrorMessage('Unable to download file.');
          });
      }

      $scope.onCloseViewDependentClick = function () {
          $modalInstance.dismiss('cancel');
      }
  });