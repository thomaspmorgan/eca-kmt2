﻿'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:PhoneNumbersCtrl
 * @description The email addresses controller is used to control the list of phone numbers.
 * # PhoneNumbersCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('PhoneNumbersCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        ConstantsService,
        NotificationService,
        LookupService
        ) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.collapsePhoneNumbers = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadPhoneNumberTypesPromise = $q.defer();
      $scope.data.phoneNumberTypes = [];

      $scope.view.onAddPhoneNumberClick = function (phoneNumberableType, entityPhoneNumbers, phoneNumberableId) {
          console.assert(entityPhoneNumbers, 'The entity phone numbers is not defined.');
          console.assert(entityPhoneNumbers instanceof Array, 'The entity phone numbers is defined but must be an array.');
          var newPhoneNumber = {
              id: --tempId,
              phoneNumberableId: phoneNumberableId,
              phoneNumberableType: phoneNumberableType,
              phoneNumberType: ConstantsService.phoneNumberType.home.value,
              phoneNumberTypeId: ConstantsService.phoneNumberType.home.id,
              isNew: true,
              number: ""
          };
          entityPhoneNumbers.splice(0, 0, newPhoneNumber);
          $scope.view.collapsePhoneNumbers = false;
      };

      $scope.$on(ConstantsService.removeNewPhoneNumberEventName, function (event, newPhoneNumber) {
          console.assert($scope.phoneNumberable, 'The scope phoneNumberable property must exist.  It should be set by the directive.');
          console.assert($scope.phoneNumberable.phoneNumbers instanceof Array, 'The entity phoneNumbers is defined but must be an array.');

          var phoneNumbers = $scope.phoneNumberable.phoneNumbers;
          var index = phoneNumbers.indexOf(newPhoneNumber);
          var removedItems = phoneNumbers.splice(index, 1);
          $log.info('Removed one new phone number at index ' + index);
      });

      function getPhoneNumberTypes() {
          var params = {
              start: 0,
              limit: 300
          };
          return LookupService.getPhoneNumberTypes(params)
          .then(function (response) {
              if (response.data.total > params.limit) {
                  var message = "There are more phone number types than could be loaded.  Not all phone number types will be shown.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              }
              $log.info('Loaded all phone number types.');
              var phoneNumberTypes = response.data.results;
              $scope.data.loadPhoneNumberTypesPromise.resolve(phoneNumberTypes);
              $scope.data.phoneNumberTypes = phoneNumberTypes;
              return phoneNumberTypes;
          })
          .catch(function () {
              var message = 'Unable to load phone number types.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
      getPhoneNumberTypes();
  });