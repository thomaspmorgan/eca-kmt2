'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:DependentsCtrl
 * @description The dependents controller is used to control the list of dependents.
 * # DependentsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('DependentsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        ConstantsService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseDependents = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadDependentsPromise = $q.defer();

      $scope.view.onAddDependentClick = function () {
          var addDependentModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/people/dependent-modal.html',
              controller: 'DependentsCtrl',
              size: 'lg',
              resolve: {
                  
              }
          });
          addDependentModalInstance.result.then(function (addedDependent) {
              $log.info('Finished adding dependents.');
              $modalInstance.close([addedDependent]);
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      };

      $scope.$on(ConstantsService.removeNewDependentEventName, function (event, newDependent) {
          console.assert($scope.model, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.model.dependents instanceof Array, 'The entity dependents is defined but must be an array.');

          var dependents = $scope.model.dependents;
          var index = dependents.indexOf(newDependent);
          var removedItems = dependents.splice(index, 1);
          $log.info('Removed one new dependent at index ' + index);
      });
  });
