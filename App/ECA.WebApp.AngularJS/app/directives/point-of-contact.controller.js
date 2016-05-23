'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:PointOfContactCtrl
 * @description The controller is used to control the list of points of contact of a project.
 * # PointOfContactCtrl
 * Controller of the project points of contact directive
 */
angular.module('staticApp')
  .controller('PointOfContactCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        filterFilter,
        ContactsService,
        orderByFilter,
        LookupService,
        NotificationService,
        ConstantsService,
        FilterService) {
            
      $scope.view = {};
      $scope.view.pointOfContactFilter = '';
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingPointOfContact = false;
      $scope.view.isLoadingPointsOfContactByFullName = false;
      $scope.view.isDeleting = false;
      
      $scope.view.maxNameLength = 100;
      $scope.view.searchLimit = 30;
      $scope.view.maxPocs = 10;
      $scope.view.maxPhoneNumbers = 10;
      $scope.view.selectedPointsOfContact = [];
      $scope.view.likePointsOfContactByFullNameTotal = 0;
      $scope.view.showConfirmDelete = false;
      var originalPointOfContact = angular.copy($scope.poc);
      
      $scope.view.savePointOfContact = function (event) {
          event.preventDefault();
          $scope.view.isSavingPointOfContact = true;

          if (isNewPoc($scope.poc)) {
              var tempId = angular.copy($scope.poc.id);
              return ContactsService.create($scope.poc)
                .then(function (response) {
                    $scope.view.isSavingPointOfContact = false;
                    $scope.view.collapsePocs = true;
                    $scope.poc.collapsePoc = true;
                    $scope.poc.showEditPoc = false;
                    var index = $scope.model.selectedPointsOfContact.map(function (e) { return e.isNew }).indexOf($scope.poc.isNew);
                    $scope.model.selectedPointsOfContact[index] = response.data;
                    $scope.model.selectedPointsOfContact = orderByFilter($scope.model.selectedPointsOfContact, '+fullName');
                    $scope.poc.isNew = false;
                    return response.data;
                })
                .catch(function (response) {
                    $scope.view.isSavingPointOfContact = false;
                    var message = "Unable to save new contact.";
                    NotificationService.showErrorMessage(message);
                    $log.error(message);
                });
          }
          else {
              return ContactsService.update($scope.poc)
                  .then(function (response) {
                      $scope.view.isSavingPointOfContact = false;
                      $scope.view.collapsePocs = true;
                      $scope.poc.collapsePoc = true;
                      $scope.poc.showEditPoc = false;
                      var index = $scope.model.selectedPointsOfContact.map(function (e) { return e.id }).indexOf($scope.poc.id);
                      $scope.model.selectedPointsOfContact[index] = response.data;
                      $scope.model.selectedPointsOfContact = orderByFilter($scope.model.selectedPointsOfContact, '+fullName');
                    return response.data;
                  })
                  .catch(function (response) {
                      $scope.view.isSavingPointOfContact = false;
                        var message = "Unable to update contact.";
                        NotificationService.showErrorMessage(message);
                        $log.error(message);
                  });
          }
      }
      
      $scope.view.getContacts = function ($viewValue) {
          var params = {
              start: 0,
              limit: 25,
              filter: [{
                  property: 'fullName',
                  comparison: ConstantsService.likeComparisonType,
                  value: $viewValue
              }]
          };
          return ContactsService.get(params)
              .then(function (response) {
                  return response.data.results;
              }, function (error) {
                  NotificationService.showErrorMessage('There was an error loading available contacts.');
              });
      }

      $scope.view.onSelectContact = function ($item) {
          $scope.poc = $item;
          $scope.poc.isNew = false;
          $scope.poc.showEditPoc = true;
          $scope.poc.collapsePoc = false;
          var pocs = $scope.model.selectedPointsOfContact;
          var index = pocs.map(function (e) { return e.isNew }).indexOf(true);
          if (index !== -1) {
              var removedItems = pocs.splice(index, 1);
              pocs.push($scope.poc);
              $scope.model.selectedPointsOfContact = orderByFilter(pocs, '+fullName');
          }
      }

      $scope.view.onEditPocClick = function (poc) {
          poc.showEditPoc = true;
          poc.collapsePoc = false;
      };
      
      $scope.view.removePointsOfContact = function () {
          $scope.model.pointsOfContactIds = [];
      }
      
      function removePointsOfContactFromView(poc) {
          $scope.$emit(ConstantsService.removePointsOfContactEventName, poc);
      }
      
      $scope.view.onDeletePocClick = function (poc) {
          $scope.view.isDeleting = true;
          return ContactsService.delete(poc)
          .then(function (response) {
              $scope.view.isDeleting = false;
              $scope.poc.showEditPoc = false;
              NotificationService.showSuccessMessage('Successfully deleted the contact.');
              removePointsOfContactFromView(poc);
          })
          .catch(function (response) {
              $scope.view.isDeleting = false;
              var message = 'Unable to remove the contact.';
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }
      
      $scope.view.cancelPointOfContactChanges = function (event, poc) {
          event.preventDefault();
          poc.showEditPoc = true;
          $scope.view.collapsePocs = true;
          $scope.poc.collapsePoc = true;
          $scope.poc.showEditPoc = false;
          if (isNewPoc($scope.poc)) {
              removePointsOfContactFromView($scope.poc);
          }
          var index = $scope.model.selectedPointsOfContact.map(function (e) { return e.id }).indexOf($scope.poc.id);
          $scope.model.selectedPointsOfContact[index] = poc;
      };

      function getPocFormDivIdPrefix() {
          return 'pocForm';
      }

      function getPocFormDivId() {
          return getPocFormDivIdPrefix() + $scope.poc.id;
      }
      
      function updatePocFormDivId(tempId) {
          var id = getPocFormDivIdPrefix() + tempId;
          var e = getPocFormDivElement(id);
          e.id = getPocFormDivIdPrefix() + $scope.poc.id.toString();
      }

      function getPocFormDivElement(id) {
          return document.getElementById(id);
      }
      
      function isNewPoc(poc) {
          return poc.isNew;
      }
      
  });