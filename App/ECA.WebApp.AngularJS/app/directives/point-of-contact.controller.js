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
        LookupService,
        NotificationService,
        ConstantsService,
        FilterService) {
            
      $scope.view = {};
      $scope.view.pointOfContactFilter = '';
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingPointOfContact = false;
      $scope.view.isLoadingPointsOfContactByFullName = false;
      
      $scope.view.maxNameLength = 100;
      $scope.view.searchLimit = 30;
      $scope.view.maxPoces = 10;
      $scope.view.maxPhoneNumbers = 10;
      $scope.view.collapsePocs = true;
      $scope.view.collapsePoc = true;
      $scope.view.showEditPoc = false;
      $scope.view.pointsOfContact = [];
      $scope.view.selectedPointsOfContact = [];
      $scope.view.likePointsOfContactByFullNameTotal = 0;
      $scope.view.showConfirmDelete = false;
      var originalPointOfContact = angular.copy($scope.$parent.poc);
      
      $scope.view.savePointOfContact = function (event) {
          event.preventDefault();
          $scope.view.isSavingPointOfContact = true;

          if (isNewPoc($scope.$parent.poc)) {
              var tempId = angular.copy($scope.$parent.poc.id);
              return ContactsService.create($scope.$parent.poc)
                .then(function (response) {
                    $scope.view.isSavingPointOfContact = false;
                    pointOfContact = response.data;
                    return pointOfContact;
                })
                .catch(function (response) {
                    $scope.view.isSavingPointOfContact = false;
                    var message = "Unable to save new contact.";
                    NotificationService.showErrorMessage(message);
                    $log.error(message);
                });
          }
          else {
              return ContactsService.update($scope.$parent.poc)
                  .then(function (response) {
                    $scope.view.isSavingPointOfContact = false;
                    pointOfContact = response.data;
                    return pointOfContact;
                  })
                  .catch(function (response) {
                        $scope.view.isSavingPointOfContact = false;
                        var message = "Unable to update contact.";
                        NotificationService.showErrorMessage(message);
                        $log.error(message);
                  });
          }          
      }
      
      $scope.view.onEditPocClick = function () {
          $scope.view.showEditPoc = true;
          $scope.view.collapsePoc = false;
      };
      
      $scope.view.onFullNameChange = function () {
          if ($scope.view.newPointOfContact.fullName && $scope.view.newPointOfContact.fullName.length > 0) {
              return loadPointsOfContactByFullName($scope.view.newPointOfContact.fullName);
          }
      }
      
      $scope.view.removePointsOfContact = function () {
          $scope.$parent.project.pointsOfContactIds = [];
      }
      
      function removePointsOfContactFromView(poc) {
          $scope.$emit(ConstantsService.removePointsOfContactEventName, poc);
      }

      $scope.view.cancelPointOfContactChanges = function (event) {
          event.preventDefault();
          $scope.view.showEditPoc = false;
          if (isNewPoc($scope.$parent.poc)) {
              removePointsOfContactFromView($scope.$parent.poc);
          }
          else {
              $scope.$parent.poc = angular.copy(originalPointOfContact);
          }
      };

      $scope.view.isUniquePointOfContact = function ($value, $index) {
          var dfd = $q.defer();
          loadPointsOfContactByEmail($value)
          .then(function (likePointsOfContact) {
              if (likePointsOfContact.length !== 0) {
                  dfd.reject();
              }
              else {
                  dfd.resolve();
              }
          })
          .catch(function (response) {
              dfd.reject();
          });
          return dfd.promise;
      }
      
      function getPocFormDivIdPrefix() {
          return 'pocForm';
      }

      function getPocFormDivId() {
          return getPocFormDivIdPrefix() + $scope.$parent.poc.id;
      }
      
      function updatePocFormDivId(tempId) {
          var id = getPocFormDivIdPrefix() + tempId;
          var e = getPocFormDivElement(id);
          e.id = getPocFormDivIdPrefix() + $scope.$parent.poc.id.toString();
      }

      function getPocFormDivElement(id) {
          return document.getElementById(id);
      }
      
      function isNewPoc(poc) {
          return poc.isNew;
      }



  });