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
      $scope.view.isDeleting = false;
      
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
      var originalPointOfContact = angular.copy($scope.poc);
      
      $scope.view.savePointOfContact = function (event) {
          event.preventDefault();
          $scope.view.isSavingPointOfContact = true;

          if (isNewPoc($scope.poc)) {
              var tempId = angular.copy($scope.poc.id);
              return ContactsService.create($scope.poc)
                .then(function (response) {
                    $scope.view.isSavingPointOfContact = false;
                    $scope.view.showEditPoc = false;
                    return response.data;
                })
                .catch(function (response) {
                    $scope.view.isSavingPointOfContact = false;
                    $scope.view.showEditPoc = false;
                    var message = "Unable to save new contact.";
                    NotificationService.showErrorMessage(message);
                    $log.error(message);
                });
          }
          else {
              return ContactsService.update($scope.poc)
                  .then(function (response) {
                      $scope.view.isSavingPointOfContact = false;
                      $scope.view.showEditPoc = false;
                    return response.data;
                  })
                  .catch(function (response) {
                      $scope.view.isSavingPointOfContact = false;
                      $scope.view.showEditPoc = false;
                        var message = "Unable to update contact.";
                        NotificationService.showErrorMessage(message);
                        $log.error(message);
                  });
          }          
      }
      
      var likePointsOfContactByFullNameFilter = FilterService.add('points-of-contact-model-search-by-full-name-filter');
      function loadPointsOfContactByFullName(fullName) {
          if (fullName && fullName.length > 0) {
              likePointsOfContactByFullNameFilter.reset();
              likePointsOfContactByFullNameFilter = likePointsOfContactByFullNameFilter
                  .skip(0)
                  .take(1)
                  .equal('fullName', fullName);
              $scope.view.isLoadingPointsOfContactByFullName = true;
              var params = likePointsOfContactByFullNameFilter.toParams();
              return ContactsService.get(params)
              .then(function (response) {
                  $scope.view.likePointsOfContactByFullName = response.data.results;
                  $scope.view.isLoadingPointsOfContactByFullName = false;
                  $scope.view.likePointsOfContactByFullNameTotal = response.data.total;
                  return $scope.view.likePointsOfContactByFullName;
              })
              .catch(function (response) {
                  $scope.view.isLoadingPointsOfContactByFullName = false;
                  var mesage = "Unable to load like points of contact.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              });
          }
      }

      $scope.view.onEditPocClick = function () {
          $scope.view.showEditPoc = true;
          $scope.view.collapsePoc = false;
      };
      
      $scope.view.onFullNameChange = function (fullName) {
          if (fullName && fullName.length > 0) {
              return loadPointsOfContactByFullName(fullName);
          }
      }
      
      $scope.view.removePointsOfContact = function () {
          $scope.$parent.project.pointsOfContactIds = [];
      }
      
      function removePointsOfContactFromView(poc) {
          $scope.$emit(ConstantsService.removePointsOfContactEventName, poc);
      }
      
      $scope.view.onDeletePocClick = function (poc) {
          $scope.view.isDeleting = true;
          return ContactsService.delete(poc)
          .then(function (response) {
              $scope.view.isDeleting = false;
              $scope.view.showEditPoc = false;
              var index = $scope.view.selectedPointsOfContact.indexOf(poc);
              $scope.view.selectedPointsOfContact.splice(index, 1);
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
      
      $scope.view.cancelPointOfContactChanges = function (event) {
          event.preventDefault();
          $scope.view.showEditPoc = false;
          if (isNewPoc($scope.poc)) {
              removePointsOfContactFromView($scope.poc);
          }
          else {
              $scope.poc = angular.copy(originalPointOfContact);
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

      $scope.$on(ConstantsService.removePointsOfContactEventName, function (event, poc) {
          console.assert($scope.view, 'The scope must exist.  It should be set by the directive.');
          console.assert($scope.view.selectedPointsOfContact instanceof Array, 'The entity pocs is defined but must be an array.');

          var pocs = $scope.view.selectedPointsOfContact;
          var index = pocs.map(function (e) { return e.id }).indexOf(poc.id);
          if (index !== -1) {
              var removedItems = pocs.splice(index, 1);
              $scope.view.selectedPointsOfContact = pocs;
              $log.info('Removed poc at index ' + index);
          }
      });

  });