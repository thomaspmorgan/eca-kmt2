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
      $scope.selectedCountriesOfCitizenship = [];
      $scope.view.edit = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadDependentsPromise = $q.defer();

      $scope.view.onViewDependentClick = function (dependent) {
          var viewDependentModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/people/dependent-modal.html',
              controller: 'ViewDependentModalCtrl',
              size: 'lg',
              resolve: {
                  dependent: function () {
                      return dependent;
                  }
              }
          });
      };

      $scope.view.onAddDependentClick = function (person) {
          $scope.dependentLoading = false;
          var addDependentModalInstance = $modal.open({
              animation: true,
              templateUrl: 'app/people/add-dependent-modal.html',
              controller: 'AddDependentModalCtrl',
              size: 'lg',
              resolve: {
                  person: function () {
                      return person;
                  }
              }
          });
          addDependentModalInstance.result.then(function (dependent) {
              if (dependent) {
              var added = {
                  id: dependent.dependentId,
                  value: dependent.lastName + ', ' + dependent.firstName + ' (' + dependent.dependentType + ')'
              };
              $scope.model.dependents.splice(0, 0, added);
              $log.info('Finished adding dependent.');
              } else {
                  $log.info('Failed to add dependent.');
              }              
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
              size: 'lg',
              resolve: {
                  dependent: function () {
                      return dependent;
                  }
              }
          });
          editDependentModalInstance.result.then(function (dependent) {
              if (dependent) {
              var index = $scope.model.dependents.map(function (e) { return e.id }).indexOf(dependent.dependentId);
              var updated = {
                  id: dependent.dependentId,
                  value: dependent.lastName + ', ' + dependent.firstName + ' (' + dependent.dependentType + ')'
              };
              $scope.model.dependents[index] = updated;
              $log.info('Finished updating dependent.');
              } else {
                  $log.info('Failed to update dependent.');
              }              
          }, function () {
              $log.info('Modal dismissed at: ' + new Date());
          });
      };
      
      $scope.view.onDeleteDependentClick = function (dependentId) {
          deleteDependent(dependentId);
      };

      function deleteDependent(dependentId) {
          $scope.isDependentLoading = true;
          return DependentService.getDependentById(dependentId)
             .then(function (data) {
                 $scope.dependent = data;
                 if ($scope.dependent.countriesOfCitizenship) {
                     $scope.selectedCountriesOfCitizenship = $scope.dependent.countriesOfCitizenship.map(function (obj) {
                         var location = {};
                         location.id = obj.id;
                         location.name = obj.value;
                         return location;
                    });
                 }                 
                 if ($scope.dependent.dateOfBirth) {
                     $scope.dependent.dateOfBirth = DateTimeService.getDateAsLocalDisplayMoment($scope.dependent.dateOfBirth).toDate();
                 }
                 updateDeletedDependent();
          });          
      };

      function updateDeletedDependent() {
          $scope.isSavingDependent = true;
          setupDependent();
          return DependentService.delete($scope.dependent)
              .then(function (response) {
                  var deleted = {
                      id: $scope.dependent.dependentId,
                      value: $scope.dependent.lastName + ', ' + $scope.dependent.firstName + ' (' + $scope.dependent.dependentType + ')'
                  };
                  removeDependentFromView(deleted);
              },
              function (error) {
                  $scope.isSavingDependent = false;
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

      function setupDependent() {
          $scope.dependent.countriesOfCitizenship = $scope.selectedCountriesOfCitizenship.map(function (obj) {
              return obj.id;
          });
          if ($scope.dependent.dateOfBirth) {
              $scope.dependent.dateOfBirth.setUTCHours(0, 0, 0, 0);
          }
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
              $scope.model.dependents = dependents;
              $log.info('Removed dependent at index ' + index);
          }          
      });

  });
