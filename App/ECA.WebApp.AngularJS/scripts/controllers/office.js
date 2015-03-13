'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OfficeCtrl', function ($scope, $stateParams, $q, DragonBreath, OfficeService) {

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
              title: 'Activity',
              path: 'activity',
              active: true,
              order: 3
          },
          artifacts: {
              title: 'Artifacts',
              path: 'artifacts',
              active: true,
              order: 4
          },
          moneyflows: {
              title: 'Money Flows',
              path: 'moneyflows',
              active: true,
              order: 5
          }
      };

      $scope.office = {};
      $scope.isLoading = true;
      $scope.officeExists = true;
      $scope.serverErrorOccurred = false;

      var officeId = $stateParams.officeId;

      function reset() {
          $scope.officeExists = true;
          $scope.serverErrorOccurred = false;
      }

      function setBusy() {
          $scope.isLoading = true;
      }

      function setIdle() {
          $scope.isLoading = false;
      }

      function showNotFound() {
          $scope.officeExists = false;
      }

      function showServerError() {
          $scope.serverErrorOccurred = true;
      }

      function getOfficeById(id) {
          var dfd = $q.defer();
          reset();
          OfficeService.get(id)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);
                  
              });
          return dfd.promise;
      }

      setBusy();
      getOfficeById(officeId)
          .then(function (data) {
              setIdle();
              $scope.office = data;
              
          }, function (errorCode) {
              setIdle();
              if (errorCode === 404) {
                  showNotFound();
              }
              else {
                  showServerError();
              }
          });
  });
