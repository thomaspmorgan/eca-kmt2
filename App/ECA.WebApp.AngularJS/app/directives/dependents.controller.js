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
        ConstantsService,
        DependentService,
        LocationService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseDependents = false;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadDependentsPromise = $q.defer();

      $scope.view.onAddDependentClick = function () {
          $scope.dependentLoading = false;
          var addDependentModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/people/add-dependent-modal.html',
              controller: 'AddDependentModalCtrl',
              size: 'md',
              resolve: {}
          });
          addDependentModalInstance.result.then(function (addedDependent) {
              $log.info('Finished adding dependent.');
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      };

      $scope.view.onEditDependentClick = function (dependent) {
          $scope.dependentLoading = false;
          dependent.original = angular.copy(dependent);
          dependent.currentlyEditing = true;

          var editDependentModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/people/edit-dependent-modal.html',
              controller: 'EditDependentModalCtrl',
              size: 'md',
              resolve: {
                  dependent: function () {
                      return dependent;
                  }
              }
          });
          editDependentModalInstance.result.then(function (updatedDependent) {
              $log.info('Finished updating dependent.');
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      };
      
      function loadCities(search) {
          if (search || $scope.dependent) {
              var params = {
                  limit: 30,
                  filter: [
                    { property: 'locationTypeId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.locationType.city.id }
                  ]
              };
              if (search) {
                  params.filter.push({ property: 'name', comparison: ConstantsService.likeComparisonType, value: search });
              }
              else if ($scope.dependent.cityOfBirthId) {
                  params.filter.push({ property: 'id', comparison: ConstantsService.equalComparisonType, value: $scope.dependent.cityOfBirthId });
              }
              return LocationService.get(params)
                .then(function (data) {
                    $scope.cities = data.results;
                    return $scope.cities;
                });
          }
      }

      $scope.$on(ConstantsService.removeNewDependentEventName, function (event, newDependent) {
          console.assert($scope.model, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.model.dependents instanceof Array, 'The entity dependents is defined but must be an array.');

          var dependents = $scope.model.dependents;
          var index = dependents.indexOf(newDependent);
          var removedItems = dependents.splice(index, 1);
          $log.info('Removed one new dependent at index ' + index);
      });
  });
