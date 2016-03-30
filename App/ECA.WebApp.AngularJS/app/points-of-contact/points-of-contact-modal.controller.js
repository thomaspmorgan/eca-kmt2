'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:PointsOfContactCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('PointsOfContactModalCtrl', function (
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
      $scope.view.showGeneral = true;
      $scope.view.showEmails = false;
      $scope.view.showPhoneNumbers = false;
      $scope.view.contacts = [];
      $scope.view.newPointOfContact = createNewPointOfContact();
      $scope.view.emailAddressTypes = [];
      $scope.view.phoneNumberTypes = [];
      $scope.view.likePointsOfContactByEmail = [];
      $scope.view.likePointsOfContactByFullName = [];
      $scope.view.likePointsOfContactByFullNameTotal = 0;

      $scope.view.deleteEmailAddress = function ($index) {
          $scope.view.newPointOfContact.emailAddresses.splice($index, 1);
      }

      $scope.view.addEmailAddress = function () {
          $scope.view.showEmails = true;
          $scope.view.newPointOfContact.emailAddresses.push({
              emailAddressTypeId: ConstantsService.emailAddressType.home.id,
              address: ''
          });
      }

      $scope.view.onFullNameChange = function () {
          if ($scope.view.newPointOfContact.fullName && $scope.view.newPointOfContact.fullName.length > 0) {
              return loadPointsOfContactByFullName($scope.view.newPointOfContact.fullName);
          }
      }

      $scope.view.isValidEmail = function ($value, $index) {
          return emailRegex.test($value);
      }

      $scope.view.deletePhoneNumber = function ($index) {
          $scope.view.newPointOfContact.phoneNumbers.splice($index, 1);
      }
      $scope.view.addPhoneNumber = function () {
          $scope.view.showPhoneNumbers = true;
          $scope.view.newPointOfContact.phoneNumbers.push({
              phoneNumberTypeId: ConstantsService.phoneNumberType.home.id,
              number: ''
          });
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

      $scope.view.onYesCancelChangesClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onNoDoNotCancelChangesClick = function () {
          $scope.view.showConfirmCancel = false;
      }

      $scope.view.onIsPrimaryAddressChange = function ($index) {
          var email = $scope.view.newPointOfContact.emailAddresses[$index];
          if (email.isPrimary) {
              angular.forEach($scope.view.newPointOfContact.emailAddresses, function (address, index) {
                  if (index !== $index) {
                      address.isPrimary = false;
                  }
              });
          }
      }

      $scope.view.onIsPrimaryPhoneNumberChange = function ($index) {
          var number = $scope.view.newPointOfContact.phoneNumbers[$index];
          if (number.isPrimary) {
              angular.forEach($scope.view.newPointOfContact.phoneNumbers, function (number, index) {
                  if (index !== $index) {
                      number.isPrimary = false;
                  }
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

      var likePointsOfContactByEmailFilter = FilterService.add('points-of-contact-model-search-by-email-filter');
      function loadPointsOfContactByEmail(emailAddress) {
          if (emailAddress && emailAddress.length > 0) {
              likePointsOfContactByEmailFilter.reset();
              likePointsOfContactByEmailFilter = likePointsOfContactByEmailFilter
                  .skip(0)
                  .take(1)
                  .containsAny('emailAddressValues', [emailAddress]);
              var params = likePointsOfContactByEmailFilter.toParams();
              return ContactsService.get(params)
              .then(function (response) {
                  $scope.view.likePointsOfContactByEmail = response.data.results;
                  return $scope.view.likePointsOfContactByEmail;
              })
              .catch(function (response) {
                  var mesage = "Unable to load like points of contact.";
                  NotificationService.showErrorMessage(message);
                  $log.error(message);
              });
          }
      }

      function loadEmailAddressTypes() {
          return LookupService.getEmailAddressTypes({ start: 0, limit: 300 })
          .then(function (response) {
              $scope.view.emailAddressTypes = response.data.results;
              return $scope.view.emailAddressTypes;
          })
          .catch(function (response) {
              var message = "Unable to load email address types.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          })
      }

      function loadPhoneNumberTypes() {
          return LookupService.getPhoneNumberTypes({ start: 0, limit: 300 })
          .then(function (response) {
              $scope.view.phoneNumberTypes = response.data.results;
              return $scope.view.phoneNumberTypes;
          })
          .catch(function (response) {
              var message = "Unable to load phone number types.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          })
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

      $scope.view.isLoadingRequiredData = true;
      $q.all([loadEmailAddressTypes(), loadPhoneNumberTypes()])
      .then(function () {
          $scope.view.isLoadingRequiredData = false;
      })
      .catch(function () {
          $scope.view.isLoadingRequiredData = false;
      })
  });
