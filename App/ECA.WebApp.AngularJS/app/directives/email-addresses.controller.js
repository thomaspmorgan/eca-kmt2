'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddressesCtrl
 * @description The email addresses controller is used to control the list of email addresses.
 * # AddressesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('EmailAddressesCtrl', function (
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
      $scope.view.collapseEmailAddresses = true;
      var tempId = 0;

      $scope.data = {};
      $scope.data.loadEmailAddressTypesPromise = $q.defer();
      $scope.data.emailAddressTypes = [];

      $scope.view.onAddEmailAddressClick = function (emailAddressableType, entityEmailAddresses, emailAddressableId) {
          console.assert(entityEmailAddresses, 'The entity email addresses is not defined.');
          console.assert(entityEmailAddresses instanceof Array, 'The entity email addresses is defined but must be an array.');
          var url = '';
          var newEmailAddress = {
              id: --tempId,
              emailAddressableId: emailAddressableId,
              EmailAddressableType: emailAddressableType,
              emailAddressType: ConstantsService.emailAddressType.home.value,
              emailAddressTypeId: ConstantsService.emailAddressType.home.id,
              isNew: true,
              isPrimary: false,
              address: ""
          };
          entityEmailAddresses.splice(0, 0, newEmailAddress);
          $scope.view.collapseEmailAddresses = false;
      };

      $scope.$on(ConstantsService.removeNewEmailAddressEventName, function (event, newEmailAddress) {
          console.assert($scope.emailAddressable, 'The scope emailAddressable property must exist.  It should be set by the directive.');
          console.assert($scope.emailAddressable.emailAddresses instanceof Array, 'The entity emailAddresses is defined but must be an array.');

          var emailAddresses = $scope.emailAddressable.emailAddresses;
          var index = emailAddresses.indexOf(newEmailAddress);
          var removedItems = emailAddresses.splice(index, 1);
          $log.info('Removed one new email address at index ' + index);
      });

      function getEmailAddressTypes() {
          var params = {
              start: 0,
              limit: 300
          };
          return LookupService.getEmailAddressTypes(params)
          .then(function (response) {
              if (response.data.total > params.limit) {
                  var message = "There are more email address types than could be loaded.  Not all email address types will be shown.";
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              }
              $log.info('Loaded all email address types.');
              var emailAddressTypes = response.data.results;
              $scope.data.loadEmailAddressTypesPromise.resolve(emailAddressTypes);
              $scope.data.emailAddressTypes = emailAddressTypes;
              return emailAddressTypes;
          })
          .catch(function () {
              var message = 'Unable to load email address types.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }
      getEmailAddressTypes();
  });
