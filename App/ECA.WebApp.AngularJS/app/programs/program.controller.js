'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramCtrl', function (
        $scope,
        $stateParams,
        $state,
        $log,
        $q,
        ConstantsService,
        OfficeService,
        AuthService,
        StateService,
        NotificationService,
        ProgramService,
        DataPointConfigurationService) {

      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          projects: {
              title: 'Sub-Programs & Projects',
              path: 'projects',
              active: true,
              order: 2
          },
          activity: {
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
          impact: {
              title: 'Impact',
              path: 'impact',
              active: true,
              order: 5
          },
          moneyflows: {
              title: 'Funding',
              path: 'moneyflows',
              active: true,
              order: 6
          }
      };
      var programId = $stateParams.programId;

      $scope.data = {};
      $scope.data.loadProgramPromise = $q.defer();
      $scope.data.loadOfficeSettingsPromise = $q.defer();
      $scope.data.loadDataPointConfigurationsPromise = $q.defer();
      $scope.view = {};
      $scope.view.isLoadingProgram = false;
      $scope.view.permalink = '';
      $scope.view.isEditButtonEnabled = false;
      $scope.view.showEditProgramButton = false;
      $scope.view.showCancelEditProgramButton = false;
      $scope.view.editProgramStateName = StateService.stateNames.edit.program;

      $scope.view.state = $state;
      $scope.view.onEditProgramClick = function ($event) {
          if ($state.current.name === $scope.view.editProgramStateName) {
              $scope.$broadcast(ConstantsService.saveProgramEventName);
          }
          else {
              StateService.goToEditProgramState(programId, {});
          }
      }

      $scope.view.onCancelEditProgramClick = function ($event) {
          $scope.$broadcast(ConstantsService.cancelProgramChangesEventName);
      }

      function isInEditState() {
          return $scope.view.state.current.name === view.editProgramStateName;
      }

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.program.value, 'The constants service must have the program resource type value.');
          var resourceType = ConstantsService.resourceType.program.value;
          var config = {};
          config[ConstantsService.permission.editProgram.value] = {
              hasPermission: function () {
                  $log.info('User has edit program permission in edit.js controller.');
                  if ($state.current.name !== StateService.stateNames.edit.program) {
                      $scope.view.isEditButtonEnabled = true;
                  }
                  
                  $scope.view.showEditProgramButton = true;
              },
              notAuthorized: function () {
                  $scope.view.showEditProgramButton = false;
              }
          };
          config[ConstantsService.permission.viewProgram.value] = {
              hasPermission: function () {
                  $scope.tabs.moneyflows.active = true;
                  $log.info('User has view program permission in program.controller.js controller.');
              },
              notAuthorized: function () {
                  $scope.tabs.moneyflows.active = false;
                  $log.info('User not authorized to view program in program.controller.js controller.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, programId, config)
            .then(function (result) {

            }, function () {
                $log.error('Unable to load user permissions.');
            });
      }

      function loadProgramById(id) {
          $scope.view.isLoadingProgram = true;
          return ProgramService.get(id)
            .then(function (data) {
                $scope.view.isLoadingProgram = false;
                $scope.view.permalink = StateService.getProgramState(id, { absolute: true });
                $scope.program = data;

                var startDate = new Date($scope.program.startDate);
                if (!isNaN(startDate.getTime())) {
                    $scope.program.startDate = startDate;
                }
                var endDate = new Date($scope.program.endDate);
                if (!isNaN(endDate.getTime())) {
                    $scope.program.endDate = endDate;
                }
                else {
                    $scope.program.endDate = null;
                }
                $scope.data.loadProgramPromise.resolve($scope.program);
            })
            .catch(function (response) {
                $scope.view.isLoadingProgram = false;
                var message = 'Unable to load program by id.';
                $log.error(message);
                NotificationService.showErrorMessage(message);
            });
      }

      function loadOfficeSettings(officeId) {
          return OfficeService.getSettings(officeId)
              .then(function (response) {
                  var data = response.data;
                  $scope.data.loadOfficeSettingsPromise.resolve(data);
              })
          .catch(function () {
              $log.error('Unable to load office settings.');
              NotificationService.showErrorMessage('Unable to load office settings.');
          });
      }

      function loadDataPointConfigurations() {
          var params = { programId: programId };
          return DataPointConfigurationService.getDataPointConfigurations(params)
           .then(function (response) {
               var data = response.data;
               $scope.data.loadDataPointConfigurationsPromise.resolve(data);
           }, function () {
               NotificationService.showErrorMessage('Unable to load data point configurations for id = ' + programId + ".");
           });
      }

      $q.all([loadPermissions(), loadDataPointConfigurations()])
      .then(function (results) {
          return loadProgramById(programId)
              .then(function () {
                  var officeId = $scope.program.ownerOrganizationId;
                  return loadOfficeSettings(officeId)
                  .then(function () {

                  });
              });
      })
      .catch(function () {
          var message = "Unable to load program required data.";
          $log.error(message);
          NotificationService.showErrorMessage(message);
      })
  });
