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
        $modal,
        $modalInstance,
        filterFilter,
        ContactsService,
        LookupService,
        NotificationService,
        ConstantsService,
        FilterService) {
            
      var emailRegex = new RegExp(ConstantsService.emailRegex);

      $scope.view = {};
      $scope.view.pointOfContactFilter = '';
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingPointOfContact = false;
      $scope.view.isLoadingPointsOfContactByFullName = false;

      $scope.view.maxNameLength = 100;
      $scope.view.searchLimit = 30;
      $scope.view.maxEmailAddresses = 10;
      $scope.view.maxPhoneNumbers = 10;
      $scope.view.contacts = [];
      $scope.view.newPointOfContact = createNewPointOfContact();
      $scope.view.likePointsOfContactByFullNameTotal = 0;
            
      $scope.view.onFullNameChange = function () {
          if ($scope.view.newPointOfContact.fullName && $scope.view.newPointOfContact.fullName.length > 0) {
              return loadPointsOfContactByFullName($scope.view.newPointOfContact.fullName);
          }
      }

      $scope.view.onSaveClick = function () {
          return savePointOfContact($scope.view.newPointOfContact)
          .then(function (pointOfContact) {
              $modalInstance.close(pointOfContact);
          });
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.pointOfContactForm.$dirty) {
              $scope.view.showConfirmCancel = true;
          }
          else {
              $modalInstance.dismiss('cancel');
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

      function createNewPointOfContact() {
          var newPointOfContact = {
              emailAddresses: [],
              phoneNumbers: []
          };
          return newPointOfContact;
      }

      function savePointOfContact(pointOfContact) {
          $scope.view.isSavingPointOfContact = true;
          return ContactsService.create(pointOfContact)
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

  });