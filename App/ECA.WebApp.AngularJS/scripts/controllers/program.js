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
        AuthService,
        StateService,
        NotificationService,
        ProgramService) {

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
          funding: {
              title: 'Funding',
              path: 'moneyflows',
              active: true,
              order: 6
          }
      };
      var programId = $stateParams.programId;

      $scope.data = {};
      $scope.data.loadProgramPromise = $q.defer();
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
                  StateService.goToForbiddenState();
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

      $q.all([loadPermissions()])
      .then(function (results) {
          return $q.all([loadProgramById(programId)])
              .then(function () {

              });
      })
      .catch(function () {
          var message = "Unable to load program required data.";
          $log.error(message);
          NotificationService.showErrorMessage(message);
      });


      function loadSnapshots () {
          $scope.view.isSnapshotLoading = true;
          SnapshotService.getProgramSnapshot(programId)
          .then(function (response) {
              $scope.view.snapshot = response.data;
              $scope.view.isSnapshotLoading = false;
          })
          .catch(function () {
              var message = 'Unable to load shapshot.';
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.view.isSnapshotLoading = false;
          });
      };


  });
