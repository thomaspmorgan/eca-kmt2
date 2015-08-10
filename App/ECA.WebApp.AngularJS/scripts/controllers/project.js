'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectCtrl', function ($scope, $state, $stateParams, $log, $q, ProjectService, PersonService,
      ProgramService, ParticipantService, LocationService, AuthService,OrganizationService,
      TableService, ConstantsService, LookupService, orderByFilter) {

      $scope.project = {};
      $scope.modalForm = {};
      $scope.currencyTypes = {};
      $scope.currentProjectId = $stateParams.projectId;

      $scope.currentlyEditing = false;

      $scope.isProjectEditCancelButtonVisible = false;
      $scope.showProjectEditCancelButton = false;
      $scope.isProjectStatusButtonInEditMode = false;
      $scope.isInEditViewState = false;
      $scope.projectStatusButtonText = "...";
      $scope.isTransactionDatePickerOpen = false;

      $scope.tabs = {
          overview: {title: 'Overview', path: 'overview', active: true, order: 1 },
          partners: {title: 'Partners', path: 'partners', active: false,order: 3 },
          participants: {title: 'Participants',path: 'participants',active: true,order: 2 },
          artifacts: {title: 'Attachments',path: 'artifacts',active: true,order: 4 },
          moneyflows: {title: 'Funding',path: 'moneyflows',active: true,order: 5},
          impact: {title: 'Impact',path: 'impact',active: false,order: 6},
          activity: {title: 'Timeline',path: 'activity',active: true,order: 7},
          itinerary: {title: 'Travel',path: 'itineraries',active: true,order: 8}
      };

      function enabledProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = true;
      }

      function disableProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = false;
      }

      ProjectService.getById($stateParams.projectId)
        .then(function (data) {            
            $scope.project = data.data;
            if (angular.isArray($scope.project.participants)) {
                $scope.tabs.participants.active = true;
            }
            if (angular.isArray($scope.project.agreements)) {
                $scope.tabs.partners.active = true;
            }
            if (angular.isArray($scope.project.moneyFlows)) {
                $scope.tabs.moneyflows.active = true;
            }
        });

      var editStateName = 'projects.edit';

      $scope.showProjectEditCancelButton = function() {
          $scope.isProjectEditCancelButtonVisible = true;
      }

      $scope.hideProjectEditCancelButton = function() {
          $scope.isProjectEditCancelButtonVisible = false;
      }
      $scope.isInEditViewState = $state.current.name === editStateName;
      if ($scope.isInEditViewState) {
          $scope.showProjectEditCancelButton();
      }
      else {
          $scope.hideProjectEditCancelButton();
      }
      $scope.onProjectStatusButtonClick = function ($event) {
          if ($state.current.name === editStateName) {
              $scope.$broadcast(ConstantsService.saveProjectEventName);
          }
          else {              
              $state.go(editStateName);
              $scope.showProjectEditCancelButton();
          }
      };

      $scope.onCancelButtonClick = function ($event) {
          $scope.$broadcast(ConstantsService.cancelProjectEventName);
      }

      $scope.params = $stateParams;

      ProgramService.get($stateParams.programId)
        .then(function (data) {
            $scope.program = data;
        });

      $scope.updateProject = function () {
          saveProject();
      };

      $scope.addTab = function () {
          if ($scope.tabs.participants.active && !$scope.project.participants) {
              $scope.project.participants = [];
          }
          if ($scope.tabs.partners.active && !$scope.project.agreements) {
              $scope.project.agreements = [];
          }
          if ($scope.tabs.artifacts.active && !$scope.project.artifactReferences) {
              $scope.project.artifactReferences = [];
          }
          if ($scope.tabs.moneyflows.active && !$scope.project.moneyFlows) {
              $scope.project.moneyFlows = [];
          }
          if ($scope.tabs.impact.active && !$scope.project.impacts) {
              $scope.project.impacts = [];
          }
          console.log($scope.tabs);
          saveProject();
      }

      $scope.saveProject = function () {
          var project = {
              id: Date.now().toString(),
              name: $scope.newProject.title,
              description: $scope.newProject.description,
              branch: $scope.newProject.branch[0].name,
              parentProgram: {
                  name: $scope.program.name,
                  id: $scope.program.id
              },
              parentOffice: {
                  name: $scope.program.owner.longName,
                  id: $scope.program.owner.organizationId
              }
          };

          ProjectService.create(project)
              .then(function (project) {
                  console.log($scope.program.id);
                  $state.go('projects.overview', { officeId: $scope.program.owner.organizationId, projectId: project.id, programId: $scope.program.id });
              });

          if (!$scope.program.projectReferences) {
              $scope.program.projectReferences = [];
          }
          $scope.program.projectReferences.push({ name: project.name, id: project.id });
          saveProgram();
      };


      function saveProject() {
          ProjectService.update($scope.project, $stateParams.projectId)
            .then(function (project) {
                $scope.project = project;
            });
      }
      
      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var projectId = $stateParams.projectId;
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.editProject.value] = {
              hasPermission: function () {
                  enabledProjectStatusButton();
                  $log.info('User has edit project permission in project.js controller.');
              },
              notAuthorized: function () {
                  disableProjectStatusButton();
                  $log.info('User not authorized to edit project  in project.js controller.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      }


      // DIALOG BOX FUNCTIONS

      $scope.confirmDeleteYes = function () {
          executeDeleteMoneyFlow();
          $scope.confirmDelete = false;
          $scope.moneyFlowConfirmMessage = "deleted";
          $scope.confirmSuccess = true;
      }

      $scope.closeConfirm = function () {
          $scope.confirmSuccess = false;
      }
      
      // calendar popup for startDate
      $scope.transactionCalendarOpen = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();

          $scope.transactionCalendarOpened = true;

      };

      $scope.createModalCancel = function () {
          $scope.checkFormStatus();
      };

      $scope.confirmClose = function (closeModal) {
          $scope.showConfirmClose = false;

          if (closeModal)
          {
              $scope.showCreateMoneyFlow = false;
              $scope.modalClear();
          }
      };

      $scope.confirmCloseSuccess = function () {
          $scope.confirmSuccess = false;

          $scope.currentlyEditing = false;
          $scope.getMoneyFlows();

      };

      $scope.checkFormStatus = function () {
          if ($scope.modalForm.moneyFlowForm.$dirty) {
              $scope.showConfirmClose = true;
          }
          else {
              $scope.showCreateMoneyFlow = false;
          }
      };

      $scope.confirmCancel = function () {
          // simply close the confirmation modal dialog
          $scope.confirmDelete = false;
      };

      $scope.openTransactionDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.isTransactionDatePickerOpen = true;
      };

      $q.all([loadPermissions()])
      .then(function () {

      }, function () {

      });
  });
