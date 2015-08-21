'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressesCtrl
 * @description The memberships controller is used to control the list of memberships.
 * # AddressesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('MembershipsCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        ConstantsService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapseMemberships = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadMembershipsPromise = $q.defer();

      $scope.view.onAddMembershipClick = function (entityMemberships, personId) {
          console.assert(entityMemberships, 'The entity memberships is not defined.');
          console.assert(entityMemberships instanceof Array, 'The entity memberships is defined but must be an array.');
          var name = "";
          var newMembership = {
              id: --tempId,
              personId: personId,
              name: name
          };
          entityMemberships.splice(0, 0, newMembership);
          $scope.view.collapseMemberships = false;
      }

      $scope.$on(ConstantsService.removeNewMembershipEventName, function (event, newMembership) {
          console.assert($scope.model, 'The scope person must exist.  It should be set by the directive.');
          console.assert($scope.model.memberships instanceof Array, 'The entity memberships is defined but must be an array.');

          var memberships = $scope.model.memberships;
          var index = memberships.indexOf(newMembership);
          var removedItems = memberships.splice(index, 1);
          $log.info('Removed one new membership at index ' + index);
      });

  });
