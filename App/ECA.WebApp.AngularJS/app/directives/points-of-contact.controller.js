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
      $scope.view.likePointsOfContactByFullNameTotal = 0;
      $scope.view.showConfirmDelete = false;
            
      $scope.view.onAddPointOfContactClick = function (pointsOfContact) {
          console.assert(pointsOfContact, 'The entity points of contact is not defined.');
          console.assert(pointsOfContact instanceof Array, 'The entity points of contact is defined but must be an array.');
          var newPointOfContact = {
              id: --tempId,
              isNew: true,
              showEditPoc: true,
              emailAddresses: [],
              phoneNumbers: []
          };
          pointsOfContact.splice(0, 0, newPointOfContact);
          $scope.view.collapsePocs = false;
      }
      
      $scope.$on(ConstantsService.removePointsOfContactEventName, function (event, poc) {
          console.assert($scope.model, 'The scope must exist.  It should be set by the directive.');
          console.assert($scope.model.selectedPointsOfContact instanceof Array, 'The entity pocs is defined but must be an array.');

          var pocs = $scope.model.selectedPointsOfContact;
          var index = pocs.map(function (e) { return e.id }).indexOf(poc.id);
          if (index !== -1) {
              var removedItems = pocs.splice(index, 1);
              $scope.model.selectedPointsOfContact = pocs;
              $log.info('Removed poc at index ' + index);
          }
      });
            
  });