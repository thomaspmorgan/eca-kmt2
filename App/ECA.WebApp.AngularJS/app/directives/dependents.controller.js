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
        LocationService,
        NotificationService,
        DateTimeService
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
      
      function saveEditDependent(dependent) {
          return DependentService.update(dependent, dependent.personId)
              .then(function (response) {
                  NotificationService.showSuccessMessage("The dependent delete was successful.");
              },
              function (error) {
                  if (error.status == 400) {
                      if (error.data.message && error.data.modelState) {
                          for (var key in error.data.modelState) {
                              NotificationService.showErrorMessage(error.data.modelState[key][0]);
                          }
                      }
                      else if (error.data.Message && error.data.ValidationErrors) {
                          for (var key in error.data.ValidationErrors) {
                              NotificationService.showErrorMessage(error.data.ValidationErrors[key]);
                          }
                      } else {
                          NotificationService.showErrorMessage(error.data);
                      }
                  } else {
                      if (error) {
                          NotificationService.showErrorMessage(error.status + ': ' + error.statusText);
                      }
                  }
              });
      };
      
      $scope.view.onDeleteDependentClick = function (index) {
          $scope.view.isDeletingDependent = true;
          var obj = $scope.model.dependents[index];
          $scope.dependent = {};
          return DependentService.getDependentById(obj.id)
             .then(function (data) {
                 $scope.dependent = data;
                 $scope.dependent.isDeleted = true;
                 saveEditDependent($scope.dependent);
                 removeDependentFromView(index);
                 $scope.view.isDeletingDependent = false;
          })
          .catch(function () {
              var message = "Unable to delete dependent.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
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
              else if ($scope.dependent.placeOfBirth_LocationId) {
                  params.filter.push({ property: 'id', comparison: ConstantsService.equalComparisonType, value: $scope.dependent.placeOfBirth_LocationId });
              }
              return LocationService.get(params)
                .then(function (data) {
                    $scope.cities = data.results;
                    return $scope.cities;
                });
          }
      }

      function removeDependentFromView(index) {
          $scope.$emit(ConstantsService.removeNewDependentEventName, index);
      }

      $scope.$on(ConstantsService.removeNewDependentEventName, function (event, index) {
          console.assert($scope.view, 'The scope must exist.  It should be set by the directive.');
          console.assert($scope.model.dependents instanceof Array, 'The entity dependents is defined but must be an array.');
          $scope.model.dependents.splice(index, 1);
          $log.info('Removed dependent at index ' + index);
      });
  });
