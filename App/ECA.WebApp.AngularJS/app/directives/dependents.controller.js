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
              resolve: {
              }
          });
          addDependentModalInstance.result.then(function (dependent) {
              var added = {
                  id: dependent.dependentId,
                  value: dependent.lastName + ', ' + dependent.firstName
              };
              $scope.model.dependents.splice(0, 0, added);
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
          editDependentModalInstance.result.then(function (dependent) {
              var index = $scope.model.dependents.map(function (e) { return e.id }).indexOf(dependent.dependentId);
              var updated = {
                  id: dependent.dependentId,
                  value: dependent.lastName + ', ' + dependent.firstName
              };
              $scope.model.dependents[index] = updated;
              $log.info('Finished updating dependent.');
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      };
      
      $scope.view.onDeleteDependentClick = function (index) {
          $scope.view.isDeletingDependent = true;
          var obj = $scope.model.dependents[index];
          var deleted = {};
          $scope.dependent = {};
          return DependentService.getDependentById(obj.id)
             .then(function (data) {
                 $scope.dependent = data;
                 if ($scope.dependent.sevisId) {
                 $scope.dependent.isDeleted = true;
                 deleteEditDependent($scope.dependent);
                     deleted = {
                         id: $scope.dependent.dependentId,
                         value: $scope.dependent.lastName + ', ' + $scope.dependent.firstName
                     };
                 } else {
                     return DependentService.delete(obj.id)
                     .then(function () {
                         deleted = {
                     id: $scope.dependent.dependentId,
                     value: $scope.dependent.lastName + ', ' + $scope.dependent.firstName
                 };
                    });
                 }                 
                 removeDependentFromView(deleted);
                 $scope.view.isDeletingDependent = false;
          })
          .catch(function () {
              var message = "Unable to delete dependent.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });          
      };

      function deleteEditDependent(dependent) {
          return DependentService.update(dependent)
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

      function removeDependentFromView(dependent) {
          $scope.$emit(ConstantsService.removeNewDependentEventName, dependent);
      }

      $scope.$on(ConstantsService.removeNewDependentEventName, function (event, dependent) {
          console.assert($scope.view, 'The scope must exist.  It should be set by the directive.');
          console.assert($scope.model.dependents instanceof Array, 'The entity dependents is defined but must be an array.');

          var dependents = $scope.model.dependents;
          var index = dependents.map(function (e) { return e.id }).indexOf(dependent.id);
          if (index !== -1) {
              var removedItems = dependents.splice(index, 1);
              $log.info('Removed dependent at index ' + index);
          } else {
              $log.info('Could not remove dependent');
          }
          
      });

  });
