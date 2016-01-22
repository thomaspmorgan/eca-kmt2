'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AddProgramCtrl
 * @description
 * # AddProgramCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ManageItineraryParticipantsModalCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $modal,
        $modalInstance,
        $timeout,
        project,
        itinerary,
        containsFilter,
        orderByFilter,
        NotificationService,
        ConstantsService,
        LocationService,
        ParticipantService,
        ProjectService,
        FilterService) {

      $scope.view = {};
      $scope.view.itinerary = itinerary;
      $scope.view.itineraryTabKey = 'itineraryTab';
      $scope.view.itineraryStopTabKey = 'itineraryStopTab';
      $scope.view.currentTab = $scope.view.itineraryTabKey;

      $scope.view.title = 'Manage ' + itinerary.name + ' Participants';
      $scope.view.limit = 30;
      $scope.view.filteredParticipants = [];
      $scope.view.filteredParticipantsCount = 0;
      $scope.view.isLoadingFilteredParticipants = false;
      $scope.view.isLoadingItineraryParticipants = false;
      $scope.view.itineraryParticipants = [];
      $scope.view.itineraryStopParticipants = [];
      $scope.view.itineraryStops = [];
      $scope.view.selectedItineraryStop = null;
      $scope.view.isLoadingItineraryStops = false;
      $scope.view.participantFilter = '';
      $scope.view.itineraryStopParticipantSources = [];
      $scope.view.selectedItineraryStopParticipantSource = null;
      $scope.view.sourceParticipants = [];

      var itineraryGroupKey = 'Travel Period';
      var itineraryStopGroupKey = 'City Stop';

      $scope.view.toggleTab = function (key) {
          $scope.view.currentTab = key;
      }

      $scope.view.onClearItineraryParticipantsClick = function () {
          var canClearParticipants = false;
          for (var i = $scope.view.itineraryParticipants.length - 1; i >= 0; i--) {
              var participant = $scope.view.itineraryParticipants[i];
              if (!participant.isItineraryStopParticipant) {
                  $scope.view.itineraryParticipants.splice(i, 1);
                  canClearParticipants = true;
              }
          }
          if (canClearParticipants) {
              return saveItineraryParticipants(project, itinerary, $scope.view.itineraryParticipants)
                .then(loadParticipants(project.id, $scope.view.projectFilterFilter));
          }
      }

      $scope.view.onClearItineraryStopParticipantsClick = function () {
          $scope.view.selectedItineraryStop.participants = [];
          return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants);
      }

      $scope.view.onSelectAvailableItineraryParticipantRow = function (participant) {
          participant.isSelected = !participant.isSelected;
      }

      $scope.view.onAddSelectedProjectParticipantsToItinerary = function () {
          var allParticipants = [];
          angular.forEach($scope.view.filteredParticipants, function (p, index) {
              if (p.isSelected) {
                  allParticipants.push(p);
              }

          });
          angular.forEach($scope.view.itineraryParticipants, function (p, index) {
              allParticipants.push(p);
          });
          return saveItineraryParticipants(project, itinerary, allParticipants)
              .then(function () {
                  return loadItineraryParticipants(project, itinerary).then(function () {
                      return loadParticipants(project.id, $scope.view.participantFilter);
                  });
              });
      }

      $scope.view.onAddAllProjectParticipantsToItinerary = function () {
          if ($scope.view.filteredParticipants.length > 0) {
              angular.forEach($scope.view.filteredParticipants, function (p, index) {
                  p.isSelected = true;
              });
              return $scope.view.onAddSelectedProjectParticipantsToItinerary();
          }
      }

      $scope.view.onRemoveItineraryParticipantClick = function (participant) {
          removeParticipant(participant, $scope.view.itineraryParticipants);
          return $scope.view.onAddSelectedProjectParticipantsToItinerary();
      }

      $scope.view.onParticipantFilterChange = function () {
          return loadParticipants(project.id, $scope.view.participantFilter);
      }

      $scope.view.onSelectItineraryStop = function ($item, $model) {
          if ($item) {
              setSourceParticipants($scope.view.selectedItineraryStopParticipantSource, $item);
          }
      }

      $scope.view.onRemoveItinerarStopParticipant = function (participant) {
          if (participant) {
              removeParticipant(participant, $scope.view.selectedItineraryStop.participants);
              participant.isSelected = false;
              return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants)
              .then(function () {
                  setSourceParticipants($scope.view.selectedItineraryStopParticipantSource, $scope.view.selectedItineraryStop);
              });
          }
      }

      $scope.view.onSelectSourceParticipantRow = function (participant) {
          participant.isSelected = !participant.isSelected;
      }

      $scope.view.onAddSelectedParticipantsToItineraryStop = function () {
          var selectedParticipants = [];
          angular.forEach($scope.view.sourceParticipants, function (p, index) {
              if (p.isSelected) {
                  selectedParticipants.push(p);
              }
          });
          if (selectedParticipants.length > 0) {
              angular.forEach($scope.view.selectedItineraryStop.participants, function (p, index) {
                  selectedParticipants.push(p);
              });
              return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, selectedParticipants)
              .then(function () {
                  angular.forEach(selectedParticipants, function (selectedParticipant, selectedParticipantIndex) {
                      removeParticipant(selectedParticipant, $scope.view.sourceParticipants);
                  });
                  $scope.view.selectedItineraryStop.participants = selectedParticipants;
                  $scope.view.selectedItineraryStop.participants = orderByFilter($scope.view.selectedItineraryStop.participants, 'fullName');

              });
          }
      }

      $scope.view.onSelectItineraryStopParticipantSource = function ($item, $model) {
          if ($item) {
              setSourceParticipants($item, $scope.view.selectedItineraryStop);
          }
      }

      $scope.view.onAddAllParticipantsToItineraryStop = function () {
          angular.forEach($scope.view.sourceParticipants, function (p, index) {
              p.isSelected = true;
          });
          $scope.view.onAddSelectedParticipantsToItineraryStop();
      }

      $scope.view.onCloseClick = function () {
          $modalInstance.close($scope.view.itineraryParticipants);
      }

      function setSourceParticipants(participantSource, itineraryStop) {
          $scope.view.sourceParticipants = [];
          if (participantSource) {
              var participants = participantSource.participants;
              var itineraryStopParticipants = [];
              if (itineraryStop) {
                  itineraryStopParticipants = itineraryStop.participants;
              }

              var itineraryStopParticipantIds = getParticipantIds(itineraryStopParticipants);
              angular.forEach(participants, function (participant, index) {
                  if (!containsFilter(itineraryStopParticipantIds, participant.participantId)) {
                      $scope.view.sourceParticipants.push(participant);
                  }
              });
              $scope.view.sourceParticipants = orderByFilter($scope.view.sourceParticipants, 'fullName');
          }
      }

      function removeParticipant(participantToRemove, participants) {
          var participantIds = getParticipantIds(participants);
          var participantIdIndex = participantIds.indexOf(participantToRemove.participantId);
          if (participantIdIndex >= 0) {
              participants.splice(participantIdIndex, 1);
          }
      }

      function setIsItineraryStopParticipant(participant) {
          var itineraryStopParticipantIds = getParticipantIds($scope.view.itineraryStopParticipants);
          participant.isItineraryStopParticipant = containsFilter(itineraryStopParticipantIds, participant.participantId);
      }

      function getParticipantIds(participants) {
          return participants.map(function (p) { return p.participantId; });
      }

      function saveItineraryParticipants(project, itinerary, participants) {
          var model = {
              participantIds: getParticipantIds(participants)
          }
          return ProjectService.updateItineraryParticipants(project.id, itinerary.id, model)
          .then(function (response) {
              var message = "Successfully set travel period participants.";
              NotificationService.showSuccessMessage(message);
          })
          .catch(function (response) {
              var message = "Unable to set travel period participants.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function saveItineraryStopParticipants(project, itinerary, itineraryStop, participants) {
          var model = {
              participantIds: getParticipantIds(participants)
          }

          return ProjectService.updateItineraryStopParticipants(project.id, itinerary.id, itineraryStop.itineraryStopId, model)
          .then(function (response) {
              $scope.view.itineraryStopParticipants = getAllParticipants($scope.view.itineraryStops);
              setIsItineraryStopOnAllItineraryParticipants();
              var message = "Successfully set city stop participants.";
              NotificationService.showSuccessMessage(message);

          })
          .catch(function (response) {
              var message = "Unable to set city stop participants.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          })
      }

      var personParticipantsFilter = FilterService.add('manageparticipantscontroller_personparticipants');
      function loadParticipants(projectId, search) {
          $scope.view.isLoadingFilteredParticipants = true;
          personParticipantsFilter.reset();
          personParticipantsFilter = personParticipantsFilter.isTrue('isPersonParticipantType').take($scope.view.limit);
          if (search && search.length > 0) {
              personParticipantsFilter = personParticipantsFilter.like('name', search);
          }
          var alreadySelectedParticipantsById = getParticipantIds($scope.view.itineraryParticipants);
          if (alreadySelectedParticipantsById && alreadySelectedParticipantsById.length > 0) {
              personParticipantsFilter = personParticipantsFilter.notIn('participantId', alreadySelectedParticipantsById);
          }
          return ParticipantService.getParticipantsByProject(project.id, personParticipantsFilter.toParams())
          .then(function (results) {
              $scope.view.isLoadingFilteredParticipants = false;
              $scope.view.filteredParticipants = results.results;
              $scope.view.filteredParticipantsCount = results.total;
              angular.forEach($scope.view.filteredParticipants, function (p, index) {
                  p.fullName = p.name;
              })
              return $scope.view.filteredParticipants;
          })
          .catch(function () {
              var message = "Unable to load participants on the project.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
              $scope.view.isLoadingFilteredParticipants = false;
          })
      }

      function setIsItineraryStopOnAllItineraryParticipants() {
          angular.forEach($scope.view.itineraryParticipants, function (p, index) {
              setIsItineraryStopParticipant(p);
          });
      }

      function loadItineraryParticipants(project, itinerary) {
          $scope.view.isLoadingItineraryParticipants = true;
          return ProjectService.getItineraryParticipants(project.id, itinerary.id)
          .then(function (response) {
              var itinerarySourceId = -1;
              $scope.view.isLoadingItineraryParticipants = false;
              $scope.view.itineraryParticipants = response.data;
              addParticipantSource(itineraryGroupKey, itinerary.name, $scope.view.itineraryParticipants, itinerarySourceId);
              setIsItineraryStopOnAllItineraryParticipants();
              return $scope.view.itineraryParticipants;
          })
          .catch(function (response) {
              $scope.view.isLoadingItineraryParticipants = false;
          });
      }

      function loadItineraryStops(itinerary) {
          $scope.view.isLoadingItineraryStops = true;
          return ProjectService.getItineraryStops(itinerary.projectId, itinerary.id)
          .then(function (response) {

              $scope.view.itineraryStopParticipants = getAllParticipants(response.data);
              $scope.view.itineraryStops = response.data;
              angular.forEach($scope.view.itineraryStops, function (stop, index) {
                  addParticipantSource(itineraryStopGroupKey, stop.name, stop.participants, stop.itineraryStopId);
              });
              $scope.view.isLoadingItineraryStops = false;
              return $scope.view.itineraryStops;
          })
          .catch(function (response) {
              $scope.view.isLoadingItineraryStops = false;
              var message = "Unable to load city stops.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });
      }

      function addParticipantSource(group, name, participants, id) {
          var sourceIds = $scope.view.itineraryStopParticipantSources.map(function (x) { return x.id; });
          var index = sourceIds.indexOf(id);
          if (index >= 0) {
              $scope.view.itineraryStopParticipantSources.splice(index, 1);
          }

          $scope.view.itineraryStopParticipantSources.push({
              id: id,
              name: name,
              participants: participants,
              group: group
          });
          $scope.view.itineraryStopParticipantSources = orderByFilter($scope.view.itineraryStopParticipantSources, ['-group', 'name']);
      }

      function getAllParticipants(itineraryStops) {
          var participants = [];
          var isItineraryStopParticipantAdded = function (participant) {
              for (var i = 0; i < participants.length; i++) {
                  var itineraryStopParticipant = participants[i];
                  if (itineraryStopParticipant.participantId === participant.participantId) {
                      return true;
                  }
              }
              return false;
          };

          angular.forEach(itineraryStops, function (stop, stopIndex) {
              angular.forEach(stop.participants, function (stopParticipant, stopParticipantIndex) {
                  if (!isItineraryStopParticipantAdded(stopParticipant)) {
                      participants.push(stopParticipant)
                  }
              });
          });
          return participants;
      }

      loadItineraryStops(itinerary)
      .then(function () {
          loadItineraryParticipants(project, itinerary)
            .then(function () {
                loadParticipants(project.id, null)
            })
      });
  });
