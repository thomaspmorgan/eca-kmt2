'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectParticipantCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        OrganizationService,
        PersonService,
        LocationService,
        LookupService,
        ConstantsService,
        AuthService,
        ProjectService,
        NotificationService,
        TableService,
        ParticipantService,
        ParticipantPersonsService) {

      $scope.newParticipant = {};
      $scope.genders = {};

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isCollaboratorExpanded = false;
      $scope.view.numberOfCollaborators = -1;
      $scope.view.collaboratorsLastUpdated = null;
      $scope.view.isCollaboratorsModalOpen = false;

      $scope.view.addPersonFilterValue = 'person';
      $scope.view.addOrganizationFilterValue = 'organization';
      $scope.view.addParticipantFilter = 'person';
      $scope.view.selectedExistingParticipant = null;
      $scope.view.addParticipantsLimit = 10;
      $scope.view.isLoadingAvailableParticipants = false;
      $scope.view.totalAvailableParticipants = 0;
      $scope.view.displayedAvailableParticipantsCount = 0;
      $scope.view.isAddingParticipant = false;

      $scope.permissions = {};
      $scope.permissions.isProjectOwner = false;
      var projectId = $stateParams.projectId;

      $scope.view.addCollaborator = function ($event) {
          $scope.view.isCollaboratorsModalOpen = true;
          var modalInstance = $modal.open({
              templateUrl: '/views/project/collaborators.html',
              controller: 'ProjectCollaboratorCtrl',
              backdrop: 'static',
              resolve: {},
              windowClass: 'modal-center-large'
          });
          modalInstance.result.then(function () {
              $log.info('Closing...');
          }, function () {
              $log.info('Dismiss add collaborator dialog...');
          })
          .then(function () {
              $scope.view.isCollaboratorsModalOpen = false;
          });
      };

      $scope.view.onAddParticipantSelect = function ($item, $model, $label) {
          var clientModel = {
              projectId: projectId,
              name: $model.name || $model.fullName
          }
          if ($item.personId) {
              clientModel.personId = $item.personId;
          }
          else {
              clientModel.organizationId = $item.organizationId;
          }
          addParticipant(clientModel);
      }

      function addParticipant(clientModel) {
          var modalInstance = $modal.open({
              templateUrl: '/views/project/selectparticipanttype.html',
              controller: 'SelectParticipantTypeCtrl',
              backdrop: 'static',
              resolve: {
                  clientModel: function () {
                      return clientModel;
                  }
              },
          });
          modalInstance.result.then(function (updatedClientModel) {
              console.assert(updatedClientModel.participantTypeId, "The participant type must be set.");
              doAddParticipant(updatedClientModel);
              $log.info('Closing...');
          }, function () {
              clearAddParticipantView();
              $log.info('Dismiss select participant type dialog...');
          })
          .then(function () {

          });
      };

      function doAddParticipant(clientModel) {
          $scope.view.isAddingParticipant = true;
          var dfd = null;
          if (clientModel.personId) {
              dfd = ProjectService.addPersonParticipant(clientModel)
          }
          else {
              dfd = ProjectService.addOrganizationParticipant(clientModel);
          }
          dfd.then(function () {
              NotificationService.showSuccessMessage('Successfully added ' + clientModel.name + ' as a project participant.');
          })
          .catch(function () {
              NotificationService.showErrorMessage('Unable to add project participant.');
          })
          .then(function () {
              $scope.view.isAddingParticipant = false;
              reloadParticipantTable();
          });
          return dfd;
      }
      
      $scope.view.getAvailableParticipants = function (search) {
          return loadAvailableParticipants(search, $scope.view.addParticipantFilter);
      }

      $scope.view.formatAddedParticipant = function (participant) {
          if (participant && participant.name && participant.name.length > 0) {
              return participant.name;
          }
          else if (participant && participant.fullName && participant.fullName.length > 0) {
              return participant.fullName;
          }
          else {
              return '';
          }
      }

      $scope.view.onRadioButtonChange = function (radioButtonValue) {
          clearAddParticipantView();
      }

      $scope.birthCountrySelected = function (data) {
          LocationService.get({
              limit: 300,
              filter: [{ property: 'countryId', comparison: 'eq', value: data.id },
                       { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id }]
          }).then(function (data) {
              $scope.cities = data.results;
          });
      }

      $scope.addParticipant = function () {
          setupNewParticipant();
          PersonService.create($scope.newParticipant)
            .then(function () {
                displaySuccess();
                reloadParticipantTable();
            }, function (error) {
                if (error.status == 400) {
                    displayError(error.data);
                }
            });
          $scope.addParticipantModalClose();
      };

      $scope.addParticipantModalClose = function () {
          $scope.modal.addParticipant = false;
      };

      $scope.addParticipantModalClear = function () {
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

      function reloadParticipantTable() {
          console.assert($scope.getParticipantsTableState, "The table state function must exist.");
          $scope.getParticipants($scope.getParticipantsTableState());
      }

      function clearAddParticipantView() {
          $scope.view.selectedExistingParticipant = null;
          $scope.view.displayedAvailableParticipantsCount = 0;
          $scope.view.totalAvailableParticipants = 0;
      }

      function loadGenders() {
          return LookupService.getAllGenders({ limit: 300 })
            .then(function (data) {
                $scope.genders = data.results;
            });
      }

      function loadCountries() {
          return LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id } })
            .then(function (data) {
                $scope.countries = data.results;
            });
      }

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

      function loadAvailableParticipants(search, participantType) {
          var params = {
              start: 0,
              limit: $scope.view.addParticipantsLimit,
          };
          if (search) {
              params.keyword = search
          }
          $scope.view.isLoadingAvailableParticipants = true;
          var dfd = null;
          

          if (participantType === $scope.view.addPersonFilterValue) {
              dfd = PersonService.getPeople(params);
          }
          else if (participantType === $scope.view.addOrganizationFilterValue) {
              dfd = OrganizationService.getOrganizations(params);
          }
          else {
              throw error('Unable to add participant type filter.');
          }

          return dfd
          .then(function (response) {
              var data = null;
              var total = null;
              if (response.data) {
                  data = response.data.results;
                  total = response.data.total;
              }
              else{
                  data = response.results;
                  total = response.total;
              }
              $scope.view.isLoadingAvailableParticipants = false;
              $scope.view.totalAvailableParticipants = total;
              $scope.view.displayedAvailableParticipantsCount = data.length;
              return data;
          })
          .catch(function () {
              $scope.view.isLoadingAvailableParticipants = false;
              $log.error('Unable to load available participants.');
              NotificationService.showErrorMessage('Unable to load available participants.');
          });
      }

      function loadPermissions() {
          console.assert(ConstantsService.resourceType.project.value, 'The constants service must have the project resource type value.');
          var resourceType = ConstantsService.resourceType.project.value;
          var config = {};
          config[ConstantsService.permission.projectOwner.value] = {
              hasPermission: function () {
                  $scope.permissions.isProjectOwner = true;
                  $log.info('User has project owner permission in collaborator.js controller.');
              },
              notAuthorized: function () {
                  $scope.permissions.isProjectOwner = false;
                  $log.info('User not authorized to manage project collaborators in collaborator.js controller.');
              }
          };
          return AuthService.getResourcePermissions(resourceType, projectId, config)
            .then(function (result) {
            }, function () {
                $log.error('Unable to load user permissions in project.js controller.');
            });
      };
      
      function loadCollaboratorDetails() {
          return ProjectService.getCollaboratorInfo(projectId)
          .then(function (response) {
              if (response.data !== null) {
                  $scope.view.numberOfCollaborators = response.data.allowedPrincipalsCount;
                  var lastRevisedDate = new Date(response.data.lastRevisedOn);
                  if (!isNaN(lastRevisedDate.getTime())) {
                      $scope.view.collaboratorsLastUpdated = lastRevisedDate;
                  }
              }
              else {
                  NotificationService.showWarningMessage('Unable to load collaborator details.');
              }
          }, function (error) {
              $log.error('Unable to load project collaborator details.');
              NotificationService.showErrorMessage('Unable to load project collaborator details.');
          })
          .catch(function () {
              $log.error('Unable to load project collaborator details.');
              NotificationService.showErrorMessage('Unable to load project collaborator details.');
          });
      };

      $scope.participantsLoading = false;
      $scope.getParticipants = function (tableState) {
          $scope.participantInfo = {};
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
                $scope.participants = data.results;
                var limit = TableService.getLimit();
                tableState.pagination.numberOfPages = Math.ceil(data.total / limit);
                $scope.participantsLoading = false;
            }, function (error) {
                $log.error('Unable to load project participants.');
                NotificationService.showErrorMessage('Unable to load project participants.');
            });
      };

      function loadParticipantInfo(participantId) {
          return ParticipantPersonsService.getParticipantPersonsById(participantId)
          .then(function (data) {
              $scope.participantInfo[participantId] = data.data;
              $scope.participantInfo[participantId].show = true;
          }, function (error) {
              if (error.status === 404) {
                  $scope.participantInfo[participantId] = {};
                  $scope.participantInfo[participantId].show = true;
              } else {
                  $log.error('Unable to load participant info for ' + participantId + '.')
                  NotificationService.showErrorMessage('Unable to load participant info for ' + participantId + '.');
              }
          });
      };

      $scope.participantInfo = {};
      $scope.toggleParticipantInfo = function (participantId) {
          if ($scope.participantInfo[participantId]) {
              if ($scope.participantInfo[participantId].show === true) {
                  $scope.participantInfo[participantId].show = false;
              } else {
                  $scope.participantInfo[participantId].show = true;
              }
          } else {
              loadParticipantInfo(participantId);
          }
      };

      $scope.view.isLoading = true;
      $q.all([loadPermissions(), loadCollaboratorDetails(), loadGenders(), loadCountries()])
      .then(function (results) {

      }, function (errorResponse) {
          $log.error('Failed initial loading of project view.');
      })
      .then(function () {
          $scope.view.isLoading = false;
      });
  });
