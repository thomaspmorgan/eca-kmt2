'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller: ViewDependentModalCtrl
 * # ViewDependentModalCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ViewDependentModalCtrl', function ($scope, $modalInstance, DependentService, DateTimeService,
                                                    ConstantsService, $stateParams, $q, dependent) {
      
      $scope.dependent = loadDependent(dependent.id);
      $scope.dependentLoading = true;
      $scope.selectedCountriesOfCitizenship = [];

      function loadDependent(dependentId) {
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
             });
      };

      $scope.onCloseViewDependentClick = function () {
          $modalInstance.dismiss('cancel');
      }
  });