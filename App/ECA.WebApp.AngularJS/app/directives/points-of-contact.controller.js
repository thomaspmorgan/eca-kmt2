'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:PointsOfContactCtrl
 * @description The controller is used to control the list of points of contact of a project.
 * # PointsOfContactCtrl
 * Controller of the project points of contact directive
 */
angular.module('staticApp')
  .controller('PointsOfContactCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        filterFilter,
        ContactsService,
        LookupService,
        NotificationService,
        ConstantsService,
        FilterService) {
            
      $scope.view = {};
      $scope.view.pointOfContactFilter = '';
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingPointOfContact = false;
      $scope.view.isLoadingPointsOfContactByFullName = false;
      var tempId = 0;

      $scope.view.maxNameLength = 100;
      $scope.view.searchLimit = 30;
      $scope.view.maxEmailAddresses = 10;
      $scope.view.maxPhoneNumbers = 10;
      $scope.view.collapsePocs = true;
      $scope.view.collapsePoc = true;
      $scope.view.showEditPoc = false;
      $scope.view.pointsOfContact = [];
      $scope.view.selectedPointsOfContact = [];
      $scope.view.newPointOfContact = createNewPointOfContact();
      $scope.view.likePointsOfContactByFullNameTotal = 0;
      $scope.view.showConfirmDelete = false;
      
      $scope.view.onAddPointOfContactClick = function (pointsOfContact) {
          console.assert(pointsOfContact, 'The entity points of contact is not defined.');
          console.assert(pointsOfContact instanceof Array, 'The entity points of contact is defined but must be an array.');
          var newPointOfContact = {
              id: --tempId,
              isNew: true,
              emailAddresses: [],
              phoneNumbers: []
          };
          pointsOfContact.splice(0, 0, newPointOfContact);
          $scope.view.collapsePoc = false;
      }
            
      function createNewPointOfContact() {
          var newPointOfContact = {
              emailAddresses: [],
              phoneNumbers: []
          };
          return newPointOfContact;
      }
      
  });