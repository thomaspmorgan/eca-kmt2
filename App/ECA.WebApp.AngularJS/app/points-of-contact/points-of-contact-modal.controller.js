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

      $scope.view = {};
      $scope.view.pointOfContactFilter = '';
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingPointOfContact = false;

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
      $scope.view.likePointsOfContact = [];

      $scope.view.deleteEmailAddress = function ($index) {
          $scope.view.newPointOfContact.emailAddresses.splice($index, 1);
          if ($scope.view.newPointOfContact.emailAddresses.length === 1) {
              var firstEmail = $scope.view.newPointOfContact.emailAddresses[0];
              if (firstEmail.address.length === 0) {
                  firstEmail.isDefault = true;
              }
          }
      }
      $scope.view.addEmailAddress = function () {
          $scope.view.newPointOfContact.emailAddresses.push({
              emailAddressTypeId: ConstantsService.emailAddressType.home.id,
              address: '',
              isPrimary: false
          });
      }

      $scope.view.onEmailAddressChange = function ($index) {
          var email = $scope.view.newPointOfContact.emailAddresses[$index];
          if ($index === 0) {
              if (email.address && email.address.length > 0) {
                  delete email.default;
              }
              else {
                  email.default = true;
              }
          }
          if (email.address && email.address.length > 0) {
              return loadPointsOfContactByEmail(email.address);
          }
      }

      $scope.view.deletePhoneNumber = function ($index) {
          $scope.view.newPointOfContact.phoneNumbers.splice($index, 1);
          if ($scope.view.newPointOfContact.phoneNumbers.length === 1) {
              var firstNumber = $scope.view.newPointOfContact.phoneNumbers[0];
              if (firstNumber.number.length === 0) {
                  firstNumber.isDefault = true;
              }
          }
      }
      $scope.view.addPhoneNumber = function () {
          $scope.view.newPointOfContact.phoneNumbers.push({
              phoneNumberTypeId: ConstantsService.phoneNumberType.home.id,
              number: '',
              isPrimary: false
          });
      }

      $scope.view.onSaveClick = function () {
          return savePointOfContact($scope.view.newPointOfContact);
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

      var likePointsOfContactByEmailFilter = FilterService.add('points-of-contact-model-search-by-email-filter');
      function loadPointsOfContactByEmail(emailAddress) {
          likePointsOfContactByEmailFilter.reset();
          likePointsOfContactByEmailFilter = likePointsOfContactByEmailFilter
              .skip(0)
              .take(1)
              .containsAny('emailAddressValues', [emailAddress]);
          var params = likePointsOfContactByEmailFilter.toParams();
          return ContactsService.get(params)
          .then(function (response) {
              $scope.view.likePointsOfContact = response.data.results;
              return $scope.view.likePointsOfContact;
          })
          .catch(function (response) {
              var mesage = "Unable to load like points of contact.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      //var emailRegex = new RegExp(ConstantsService.emailRegex);
      //var pointsOfContactFilter = FilterService.add('points-of-contact');
      //function loadPointsOfContact(search) {
      //    pointsOfContactFilter.reset();
      //    pointsOfContactFilter = pointsOfContactFilter
      //        .skip(0)
      //        .take(100);
      //    if (search) {
      //        if (emailRegex.test(search)) {
      //            pointsOfContactFilter = pointsOfContactFilter.containsAny('emailAddressValues', [search]);
      //        }
      //        else {
      //            pointsOfContactFilter = pointsOfContactFilter.like('fullName', search);
      //        }
      //    }
      //    var params = pointsOfContactFilter.toParams();
      //    return ContactsService.get(params)
      //    .then(function (response) {
      //        $scope.view.contacts = response.data.results;
      //        return $scope.view.contacts;
      //    })
      //    .catch(function (response) {
      //        var mesage = "Unable to load points of contact.";
      //        NotificationService.showErrorMessage(message);
      //        $log.error(message);
      //    });
      //}

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

      var newPointOfContactId = -1;
      function createNewPointOfContact() {
          var newPointOfContact = {
              id: newPointOfContactId,
              emailAddresses: [{
                  address: '',
                  emailAddressTypeId: ConstantsService.emailAddressType.home.id,
                  isDefault: true,
                  isPrimary: true
              }],
              phoneNumbers: [{
                  phoneNumberTypeId: ConstantsService.phoneNumberType.home.id,
                  number: '',
                  isDefault: true,
                  isPrimary: true
              }]
          };
          return newPointOfContact;
      }

      function savePointOfContact(pointOfContact) {
          $scope.view.isSavingPointOfContact = true;
          return ContactsService.create(pointOfContact)
          .then(function (response) {
              $scope.view.isSavingPointOfContact = false;
              pointOfContact = response.data;
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
