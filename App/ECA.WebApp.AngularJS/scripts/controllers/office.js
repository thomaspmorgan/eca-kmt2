'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OfficeCtrl', function ($scope,
      $stateParams,
      $q,
      $log,
      DragonBreath,
      ConstantsService,
      NotificationService,
      OfficeService,
      TableService,
      AuthService,
      LookupService,
      ProgramService) {

      var officeId = $stateParams.officeId;
      $scope.office = {};
      $scope.showFundingTab = true;
      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          partners: {
              title: 'Branches & Programs',
              path: 'branches',
              active: true,
              order: 2
          },
          participants: {
              title: 'Timeline',
              path: 'activity',
              active: true,
              order: 3
          },
          artifacts: {
              title: 'Attachments',
              path: 'artifacts',
              active: true,
              order: 4
          },
          moneyflows: {
              title: 'Funding',
              path: 'moneyflows',
              active: $scope.showFundingTab,
              order: 5
          }
      };

      $scope.data = {};
      $scope.data.loadedOfficePromise = $q.defer();

      function getOfficeById(id) {
          return OfficeService.get(id)
          .then(function (response) {
              $scope.office = response.data;
              $scope.data.loadedOfficePromise.resolve($scope.office);
          })
          .catch(function (response) {
              var message = "Unable to load office by id.";
              NotificationService.showErrorMessage(message);
              $log.error(message);

          })
      }

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.office.value, 'The constants service must have the project resource type value.');
          var resourceType = ConstantsService.resourceType.office.value;
          var config = {};
          config[ConstantsService.permission.viewOffice.value] = {
              hasPermission: function () {
                  $scope.showFundingTab = false;
                  $log.info('User has view office permission in office.js controller.');
              },
              notAuthorized: function () {
                  $scope.showFundingTab = false;
                  $log.info('User not authorized to view office in office.js controller.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, officeId, config)
            .then(function (result) {
                
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      }

      loadPermissions()
      .then(function () {
          return $q.all([getOfficeById(officeId)])
            .then(function (results) {
            })
            .catch(function (response) {
                var message = "Unable to load office by id.";
                NotificationService.showErrorMessage(message);
                $log.error(message);
                $scope.data.loadedOfficePromise.reject(message);
            });
      })
      .catch(function () {
          var message = "Unable to load permissions.";
          $log.error(message);
      });
  });
