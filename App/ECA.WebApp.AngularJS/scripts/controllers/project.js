'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectCtrl', function ($scope, $stateParams, ProjectService, ProgramService, ParticipantService, LocationService, MoneyFlowService) {

      $scope.project = {};

      $scope.newParticipant = {};

      $scope.modal = {};

      $scope.newMoneyFlow = {};

      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          partners: {
              title: 'Partners',
              path: 'partners',
              active: false,
              order: 3
          },
          participants: {
              title: 'Participants',
              path: 'participants',
              active: true,
              order: 2
          },
          artifacts: {
              title: 'Artifacts',
              path: 'artifacts',
              active: true,
              order: 4
          },
          moneyflows: {
              title: 'Money Flow',
              path: 'moneyflows',
              active: true,
              order: 5
          },
          impact: {
              title: 'Impact',
              path: 'impact',
              active: false,
              order: 6
          },
          activity: {
              title: 'Activities',
              path: 'activity',
              active: true,
              order: 7
          },
          itinerary: {
              title: 'Itineraries',
              path: 'itineraries',
              active: true,
              order: 8
          }
      };

      $scope.locations = {};

      LocationService.get('city')
        .then(function (data) {
            $scope.locations.cities = data.results;
        });


      LocationService.get('country')
        .then(function (data) {
            $scope.locations.countries = data.results;
        })

      ProjectService.get($stateParams.projectId)
        .then(function (data) {
            $scope.project = data;
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

      ParticipantService.getParticipantsByProject($stateParams.projectId)
        .then(function (data) {
            $scope.project.participants = data;
        });

      $scope.participantsCopy = [].concat($scope.project.participants);

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

      MoneyFlowService.getMoneyFlowsByProject($stateParams.projectId)
       .then(function (moneyFlows) {
           $scope.project.moneyFlows = moneyFlows;
       });

      $scope.participants = {
          gender: [{ name: 'Male' }, { name: 'Female' }, { name: 'Other' }]
      };

      $scope.addParticipant = function () {
          $scope.newParticipant.id = Date.now().toString();
          console.log('project', $scope.project);
          $scope.newParticipant.projectReferences = [{ projectId: $scope.project.id, projectName: $scope.project.name }];
          $scope.newParticipant.programReferences = [{ programId: $scope.project.parentProgram.programId, programName: $scope.project.parentProgram.displayName }];
          var projectPersonRef = {
              personId: $scope.newParticipant.id,
              personName: $scope.newParticipant.firstName + " " + $scope.newParticipant.lastName
          };
          $scope.project.participants.push(projectPersonRef);
          $scope.newParticipant.names = [
            {
                type: 'givenName',
                value: $scope.newParticipant.firstName
            },
            {
                type: 'surname',
                value: $scope.newParticipant.lastName
            }
          ];
          $scope.newParticipant.gender = $scope.newParticipant.gender[0].name;
          $scope.newParticipant.placeOfBirth = {
              displayName: $scope.newParticipant.birthCity.city,
              locationId: $scope.newParticipant.birthCity.id
          };
          $scope.newParticipant.countriesOfCitizenships = [
            {
                displayName: $scope.newParticipant.countryOfCitizenship
            }
          ];
          PersonService.create(angular.copy($scope.newParticipant), 'people');
          $scope.modalClear();
          saveProject();
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

      $scope.modalClear = function () {
          $scope.modal.addParticipant = false;
          angular.forEach($scope.newParticipant, function (value, key) {
              $scope.newParticipant[key] = '';
          });
      };

  });
