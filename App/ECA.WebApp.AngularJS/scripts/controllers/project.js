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
      ProgramService, ParticipantService, LocationService, MoneyFlowService, AuthService,
      TableService, ConstantsService, LookupService, orderByFilter) {

      $scope.project = {};

      $scope.newParticipant = {};

      $scope.modal = {};

      $scope.genders = {};
      $scope.currencyTypes = {};
      $scope.moneyFlowTypes = [];
      $scope.moneyFlowStati = [];

      $scope.moneyFlows = [];

      $scope.editingMoneyFlows = [];
      $scope.dateFormat = 'dd-MMMM-yyyy';

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
          artifacts: {title: 'Artifacts',path: 'artifacts',active: true,order: 4 },
          moneyflows: {title: 'Money Flow',path: 'moneyflows',active: true,order: 5},
          impact: {title: 'Impact',path: 'impact',active: false,order: 6},
          activity: {title: 'Activities',path: 'activity',active: true,order: 7},
          itinerary: {title: 'Itineraries',path: 'itineraries',active: true,order: 8}
      };

      function enabledProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = true;
      }

      function disableProjectStatusButton() {
          $scope.isProjectStatusButtonEnabled = false;
      }

      ProjectService.get($stateParams.projectId)
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

      $scope.participantsLoading = false;

      $scope.getParticipants = function (tableState) {

          $scope.participantsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          ParticipantService.getParticipantsByProject($stateParams.projectId, params)
            .then(function (data) {
                $scope.project.participants = data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
                $scope.participantsLoading = false;
            });
      };
      
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

      LookupService.getAllGenders({ limit: 300 })
        .then(function (data) {
            $scope.genders = data.results;
        });

      LookupService.getAllMoneyFlowStati({ limit: 300 })
        .then(function (data) {
            $scope.moneyFlowStati = data.results;
        });

      LookupService.getAllMoneyFlowTypes({ limit: 300 })
        .then(function (data) {
            $scope.moneyFlowTypes = data.results;
        });

      LocationService.get({ limit: 300, filter: {property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id}})
        .then(function (data) {
            $scope.countries = data.results;
        });

      $scope.birthCountrySelected = function (data) {
          LocationService.get({
              limit: 300,
              filter: [{ property: 'countryId', comparison: 'eq', value: data.id },
                       { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id}]
          }).then(function (data) {
                $scope.cities = data.results;
            });
      }

      $scope.addParticipant = function () {

          setupNewParticipant();

          console.log($scope.newParticipant);

          PersonService.create($scope.newParticipant)
            .then(function () {
                $scope.participantsLoading = true;
                var params = {
                    start: TableService.getStart(),
                    limit: TableService.getLimit(),
                };
                ParticipantService.getParticipantsByProject($stateParams.projectId, params)
                    .then(function (data) {
                        $scope.project.participants = data.results;
                        $scope.participantsLoading = false;
                    });
                displaySuccess();
            }, function (error) {
                if (error.status == 400) {
                    displayError(error.data);
                }
            });
          $scope.modalClose();
      };
      
      function setupNewParticipant() {
          delete $scope.newParticipant.countryOfBirth;
          $scope.newParticipant.projectId = $scope.project.id;
          $scope.newParticipant.gender = $scope.newParticipant.gender[0].id;
          $scope.newParticipant.cityOfBirth = $scope.newParticipant.cityOfBirth[0].id;
          $scope.newParticipant.countriesOfCitizenship =
               $scope.newParticipant.countriesOfCitizenship.map(function (obj) {
                   return obj.id;
               });
      };

      function displaySuccess() {
          $scope.modal.addParticipantResult = true;
          $scope.result = {};
          $scope.result.title = "Person Created";
          $scope.result.subtitle = "The person was created successfully!";
      };

      function displayError(error) {
          $scope.modal.addParticipantResult = true;
          $scope.result = {};
          $scope.result.title = "Error Creating Person";
          $scope.result.subtitle = "There was an error creating the new person.";
          $scope.result.error = error;
      };

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

      $scope.modalClose = function () {
          $scope.modal.addParticipant = false;
      };

      $scope.modalClear = function () {
          $scope.modal.addParticipant = false;

          angular.forEach($scope.newParticipant, function (value, key) {
              $scope.newParticipant[key] = '';
          });

          angular.forEach($scope.genders, function (value, key) {
              if ($scope.genders[key].ticked === undefined) {
                  $scope.genders[key].ticked = false;
              } else {
                  delete $scope.genders[key].ticked;
              }
          });
          angular.forEach($scope.countries, function (value, key) {
              if ($scope.countries[key].ticked === undefined) {
                  $scope.countries[key].ticked = false;
              } else {
                  delete $scope.countries[key].ticked;
              }
          });
          $scope.cities = [];
      };
      
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

      $scope.moneyFlowsLoading = false;

      /* money flow manipulation */
      $scope.getMoneyFlows = function (tableState) {

          $scope.moneyFlowsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          MoneyFlowService.getMoneyFlowsByProject($stateParams.projectId, params)
             .then(function (data) {
                 $scope.moneyFlows = data.results;
                 var limit = TableService.getLimit();
                 tableState.pagination.numberOfPages = Math.ceil(data.total / limit);

                 angular.forEach($scope.moneyFlows, function (value) {
                     $scope.editingMoneyFlows[value.id] = false;
                 });

                 $scope.moneyFlowsLoading = false;
             });
      };

      $scope.editMoneyFlow = function (moneyFlowId) {
          $scope.editingMoneyFlows[moneyFlowId] = true;
          $scope.currentlyEditing = true;
      };
      
      $scope.saveMoneyFlow = function (moneyFlowId) {
          $("#transactionDate" + moneyFlowId).css("opacity", "0");
          $("#changesSaved" + moneyFlowId).css("display", "inline");

          $scope.editingMoneyFlows[moneyFlowId] = false;
          $scope.currentlyEditing = false;

          // animate the 'saved' label
          $("#changesSaved"+moneyFlowId).animate({
              opacity: 0,
          }, 3000, function () {
              $("#transactionDate" + moneyFlowId).css("opacity", "100");
              $("#changesSaved" + moneyFlowId).css({"display": "none", "opacity" : "100"});

          });
      };

      $scope.cancelMoneyFlowEdit = function (moneyFlowId) {
          $scope.editingMoneyFlows[moneyFlowId] = false;
          $scope.currentlyEditing = false;
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
