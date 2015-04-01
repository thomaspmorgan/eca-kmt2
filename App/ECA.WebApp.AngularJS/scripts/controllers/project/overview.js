'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectOverviewCtrl', function ($scope, $stateParams, $q, $log, $timeout, ProjectService, ProgramService, TableService, LookupService, ConstantsService) {

      $scope.view = {};
      $scope.view.params = $stateParams;

      $scope.view.saveFailed = false;
      $scope.view.errorMessage = "";
      $scope.view.validations = [];
      $scope.view.notifications = [];
      $scope.view.areAlertsCollapsed = false;
      $scope.view.isLoading = false;
      $scope.view.isSaving = false;

      $scope.editView = {};
      $scope.editView.show = false;
      $scope.editView.foci = [];
      $scope.editView.projectStati = [];
      $scope.editView.pointsOfContact = [];

      $scope.editView.loadProjectStati = function () {
          LookupService.getAllProjectStati({ start: 0, limit: 300 })
          .then(function (response) {
              $scope.editView.projectStati = response.data.results;
          }, function (errorResponse) {

          })
          .then(function () {

          });
      }

      $scope.editView.loadFoci = function () {
          LookupService.getAllFocusAreas({ start: 0, limit: 300 })
          .then(function (response) {
              $scope.editView.foci = response.results;
          });
      }

      $scope.editView.removePointsOfContact = function () {
          $scope.$parent.project.pointsOfContactIds = [];
      }

      $scope.editView.searchPointsOfContact = function (data) {
          loadPointsOfContact(data);
      }

      $scope.view.confirmFailOk = function () {
          $scope.view.saveFailed = false;
      }

      $scope.view.onCancelClick = function () {
          $scope.editView.show = false;
          loadProject();
      }

      $scope.view.onSaveClick = function ($event) {
          saveProject($event);
      }

      $scope.view.updateFocusView = function () {
          $scope.$parent.project.focus = getFocusById($scope.editView.foci, $scope.$parent.project.focusId).name;
      }

      $scope.view.updateStatusView = function () {
          $scope.$parent.project.status = getStatusById($scope.editView.projectStati, $scope.$parent.project.projectStatusId).name;
      }

      var editProjectEventName = ConstantsService.editProjectEventName;
      $scope.$on(editProjectEventName, function () {
          $log.info('Handling event [' + editProjectEventName + '] in overview.js controller.');
          setTickedPointsOfContact();
          $scope.editView.show = true;
      });

      function loadPointsOfContact(search) {
          var params = {
              start: 0,
              limit: 300
          };
          if (search && search.keyword) {
              params.filter = {
                  comparison: 'like',
                  property: 'fullName',
                  value: search.keyword
              }
          }
          LookupService.getAllContacts(params)
          .then(function (response) {
              $scope.editView.pointsOfContact = response.results;
          });
      }

      loadPointsOfContact(null);


      function setTickedPointsOfContact() {
          var pointsOfContact = $scope.$parent.project.contacts;
          for (var i = 0; i < pointsOfContact.length; i++) {
              var projectPoc = pointsOfContact[i];
              for (var j = 0; j < $scope.editView.pointsOfContact.length; j++) {
                  var contact = $scope.editView.pointsOfContact[j];
                  if (projectPoc.id === contact.id) {
                      contact.ticked = true;
                  }
              }
          }
      }

      function getStatusById(stati, id) {
          return getLookupById(stati, id);
      }

      function getFocusById(foci, id) {
          return getLookupById(foci, id);
      }

      function getLookupById(lookups, id) {
          for (var i = 0; i < lookups.length; i++) {
              var l = lookups[i];
              if (l.id === id) {
                  return l;
              }
          }
          return null;
      }

      function loadProject() {
          $scope.view.isLoading = true;
          var projectId = $stateParams.projectId;
          ProjectService.get(projectId)
            .then(function (data) {
                $scope.$parent.project = data.data;
                $scope.$parent.project.startDate = new Date($scope.$parent.project.startDate);
                $scope.$parent.project.endDate = new Date($scope.$parent.project.endDate);
                $scope.$parent.project.countryIsos = $scope.$parent.project.countryIsos || [];
            }, function (errorResponse) {
                $log.error('Failed to load project with id ' + projectId);
            })
            .then(function () {
                $scope.view.isLoading = false;
            });
      }
      loadProject();

      function saveProject($event) {
          $scope.view.isSaving = true;
          $scope.view.saveFailed = false;
          $scope.$parent.project.goalIds = $scope.$parent.project.goalIds || [];
          $scope.$parent.project.themeIds = $scope.$parent.project.themeIds || [];
          $scope.$parent.project.pointsOfContactIds = $scope.$parent.project.pointsOfContactIds || [];


          if ($scope.$parent.project.pointsOfContact && $scope.$parent.project.pointsOfContact.length > 0) {
              $scope.$parent.project.pointsOfContactIds = $scope.$parent.project.pointsOfContact.map(function (pointOfContact) { return pointOfContact.id; });
          }

          ProjectService.update($scope.$parent.project, $stateParams.projectId)
            .then(function (response) {
                $scope.$parent.project = response.data;
                $scope.editView.show = false;
                showSaveSuccess();
            }, function (errorResponse) {
                $scope.view.saveFailed = true;
                $scope.view.errorMessage = "An error occurred while saving the project.";
                if (errorResponse.data && errorResponse.data.Message) {
                    $scope.view.errorMessage = errorResponse.data.Message;
                }
                if (errorResponse.data && errorResponse.data.ValidationErrors) {
                    for (var key in errorResponse.data.ValidationErrors) {
                        $scope.view.validations.push(errorResponse.data.ValidationErrors[key]);
                    }
                }
            })
            .then(function () {
                $scope.view.isSaving = false;
            });
      }

      var removeAlertTimeout = 1500;
      var hideAlertsTimeout = 3000;
      function showSaveSuccess() {
          $scope.view.areAlertsCollapsed = false;
          var index = $scope.view.notifications.push({ type: 'success', msg: 'Successfully saved project changes.' });
          $timeout(function () {
              $scope.view.areAlertsCollapsed = true;
              $timeout(function () {
                  removeAlert(index);
              }, removeAlertTimeout);
          }, hideAlertsTimeout);
      }

      function removeAlert(index) {
          $scope.view.notifications = $scope.view.notifications.splice(index, 1);
      }
  });
