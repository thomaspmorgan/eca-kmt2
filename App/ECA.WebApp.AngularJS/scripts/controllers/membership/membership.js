'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressCtrl
 * @description The address control is use to control a single address.
 * # AddressCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('MembershipCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        smoothScroll,
        LookupService,
        MembershipService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.showEditMembership = false;
      $scope.view.isSavingChanges = false;

      var originalMembership = angular.copy($scope.membership);

      $scope.view.saveMembershipChanges = function () {
          $scope.view.isSavingChanges = true;

          if (isNewMembership($scope.membership)) {
              var tempId = angular.copy($scope.membership.id);
              return MembershipService.addMembership($scope.membership, $scope.view.params.personId)
                .then(onSaveMembershipSuccess)
                .then(function () {
                    updateMembershipFormDivId(tempId);
                    updateMemberships(tempId, $scope.membership);
                })
                .catch(onSaveMembershipError);
          }
          else {
              return MembershipService.updateMembership($scope.membership, $scope.view.params.personId)
                  .then(onSaveMembershipSuccess)
                  .catch(onSaveMembershipError);
          }
      };

      function updateMemberships(tempId, membership) {
          var index = $scope.model.memberships.map(function (e) { return e.id }).indexOf(tempId);
          $scope.model.memberships[index] = membership;
      };

      $scope.view.cancelMembershipChanges = function (form) {
          $scope.view.showEditMembership = false;
          if (isNewMembership($scope.membership)) {
              removeMembershipFromView($scope.membership);
          }
          else {
              $scope.membership = angular.copy(originalMembership);
          }
      };

      $scope.view.onDeleteMembershipClick = function () {
          if (isNewMembership($scope.membership)) {
              removeMembershipFromView($scope.membership);
          }
          else {
              $scope.view.isDeletingMembership = true;
              return MembershipService.deleteMembership($scope.membership, $scope.view.params.personId)
              .then(function () {
                  NotificationService.showSuccessMessage("Successfully deleted membership.");
                  $scope.view.isDeletingMembership = false;
                  removeMembershipFromView($scope.membership);
              })
              .catch(function () {
                  var message = "Unable to delete membership.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
      }

      $scope.view.onEditMembershipClick = function () {
          $scope.view.showEditMembership = true;
          var id = getMembershipFormDivId();
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 200,
              callbackBefore: function (element) {},
              callbackAfter: function (element) { }
          }
          smoothScroll(getMembershipFormDivElement(id), options);
      };

      function removeMembershipFromView(membership) {
          $scope.$emit(ConstantsService.removeNewMembershipEventName, membership);
      }

      function getMembershipFormDivIdPrefix(){
          return 'membershipForm';
      }

      function getMembershipFormDivId() {
          return getMembershipFormDivIdPrefix() + $scope.membership.id;
      }
      
      function updateMembershipFormDivId(tempId) {
          var id = getMembershipFormDivIdPrefix() + tempId;
          var e = getMembershipFormDivElement(id);
          e.id = getMembershipFormDivIdPrefix() + $scope.membership.id;
      }

      function getMembershipFormDivElement(id) {
          return document.getElementById(id);
      }

      function onSaveMembershipSuccess(response) {
          $scope.membership = response.data;
          originalMembership = angular.copy($scope.membership);
          NotificationService.showSuccessMessage("Successfully saved changes to membership.");
          $scope.view.showEditMembership = false;
          $scope.view.isSavingChanges = false;
      }

      function onSaveMembershipError() {
          var message = "Failed to save membership changes.";
          NotificationService.showErrorMessage(message);
          $log.error(message);
          $scope.view.isSavingChanges = false;
      }

      function isNewMembership(membership) {
          return membership.personId;
      }
  });
