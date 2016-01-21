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
      $scope.view.title = 'Manage ' + itinerary.name + ' Participants';
      $scope.view.limit = 30;
      $scope.view.filteredParticipants = [];
      $scope.view.filteredParticipantsCount = 0;
      $scope.view.isLoadingFilteredParticipants = false;
      $scope.view.isLoadingTravelPeriodParticipants = false;
      $scope.view.travelPeriodParticipants = [];
      $scope.view.travelStopParticipants = [];
      $scope.view.itineraryStops = [];
      $scope.view.selectedItineraryStop = null;
      $scope.view.isLoadingItineraryStops = false;
      $scope.view.selectedProjectParticipants = [];
      $scope.view.participantFilter = '';
      $scope.view.itineraryStopParticipantSources = [];
      $scope.view.itineraryParticipantSourceIndex = 0;
      $scope.view.itineraryParticipantSourceName = "This Travel Period";
      $scope.view.selectedItineraryStopParticipantSource = null;
      $scope.view.sourceParticipants = [];

      //$scope.view.onSortTravelPeriodParticipants = function () {
      //    $scope.view.travelPeriodParticipants = orderByFilter($scope.view.travelPeriodParticipants, 'fullName');
      //}

      //$scope.view.onSortItineraryStopParticipants = function () {
      //    $scope.view.selectedItineraryStop.participants = orderByFilter($scope.view.selectedItineraryStop.participants, 'fullName');
      //}







      $scope.view.onClearItineraryParticipantsClick = function () {
          for (var i = $scope.view.travelPeriodParticipants.length - 1; i >= 0; i--) {
              var participant = $scope.view.travelPeriodParticipants[i];
              if (!participant.isItineraryStopParticipant) {
                  $scope.view.travelPeriodParticipants.splice(i, 1);
              }
          }

          return saveItineraryParticipants(project, itinerary, $scope.view.travelPeriodParticipants)
            .then(loadParticipants(project.id, $scope.view.projectFilterFilter));
      }

      $scope.view.onAddAllItineraryParticipants = function () {
          //debugger;
          //if ($scope.view.travelPeriodParticipants.length > 0) {
          //    angular.forEach($scope.view.travelPeriodParticipants, function (p, index) {
          //        $scope.view.selectedItineraryStop.participants.push(p);
          //    });
          //    return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants);
          //}

      }

      $scope.view.onClearItineraryStopParticipantsClick = function () {
          $scope.view.selectedItineraryStop.participants = [];
          return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants);
      }

      $scope.view.onSelectAvailableTravelPeriodParticipant = function (participant) {
          var selectedProjectParticipantIds = getParticipantIds($scope.view.selectedProjectParticipants);
          var participantIdIndex = selectedProjectParticipantIds.indexOf(participant.participantId);
          if (participant.isSelected) {
              if (participantIdIndex < 0) {
                  $scope.view.selectedProjectParticipants.push(participant);
              }
          }
          else {
              removeParticipant(participant, $scope.view.selectedProjectParticipants);
          }
      }

      $scope.view.onSelectAvailableTravelPeriodParticipantRow = function (participant) {
          participant.isSelected = !participant.isSelected;
          $scope.view.onSelectAvailableTravelPeriodParticipant(participant);
      }

      $scope.view.onAddSelectedProjectParticipantsToItinerary = function () {
          var allParticipants = [];
          angular.forEach($scope.view.selectedProjectParticipants, function (p, index) {
              allParticipants.push(p);
          });
          angular.forEach($scope.view.travelPeriodParticipants, function (p, index) {
              allParticipants.push(p);
          });
          return saveItineraryParticipants(project, itinerary, allParticipants)
              .then(function () {
                  return loadTravelPeriodParticipants(project, itinerary).then(function () {
                      $scope.view.selectedProjectParticipants = [];
                      return loadParticipants(project.id, $scope.view.participantFilter);
                  });
              });
      }

      $scope.view.onAddAllProjectParticipantsToItinerary = function () {
          if ($scope.view.filteredParticipants.length > 0) {
              angular.forEach($scope.view.filteredParticipants, function (p, index) {
                  $scope.view.selectedProjectParticipants.push(p);
              });
              return $scope.view.onAddSelectedProjectParticipantsToItinerary();
          }
      }

      $scope.view.onRemoveItineraryParticipantClick = function (participant) {
          removeParticipant(participant, $scope.view.travelPeriodParticipants);
          removeParticipant(participant, $scope.view.selectedProjectParticipants);
          return $scope.view.onAddSelectedProjectParticipantsToItinerary();
      }

      $scope.view.onParticipantFilterChange = function () {
          $scope.view.selectedProjectParticipants = [];
          return loadParticipants(project.id, $scope.view.participantFilter);
      }

      

      //$scope.view.onSelectItineraryParticipant = function($item, $model){
      //    return saveItineraryParticipants(project, itinerary, $scope.view.travelPeriodParticipants);
      //}

      //$scope.view.onRemoveItineraryParticipant = function ($item, $model) {
      //    return saveItineraryParticipants(project, itinerary, $scope.view.travelPeriodParticipants)
      //        .then(loadParticipants(project.id, null));
      //}

      //$scope.view.onSelectItineraryStopParticipant = function ($item, $model) {
      //    return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants)
      //}

      //$scope.view.onRemoveItineraryStopParticipant = function ($item, $model) {
      //    return $scope.view.onSelectItineraryStopParticipant($item, $model);
      //}

      //$scope.view.getFilteredParticipants = function (search) {
      //    return loadParticipants(project.id, search);
      //}

      $scope.view.onSelectSourceParticipant = function (participant) {
          participant.isSelected = !participant.isSelected;
          //$scope.view.onSelectAvailableTravelPeriodParticipant(participant);
      }

      $scope.view.onAddSelectedParticipantsToItineraryStop = function () {
          
          var selectedParticipants = [];
          var participantsToRemoveByIndex = [];
          angular.forEach($scope.view.sourceParticipants, function (p, index) {
              if (p.isSelected) {
                  selectedParticipants.push(p);
                  participantsToRemoveByIndex.push(index);
              }
          });
          if (selectedParticipants.length > 0) {
              
              return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, selectedParticipants)
              .then(function() {
                  angular.forEach(participantsToRemoveByIndex, function (participantIndex, index) {
                      $scope.view.sourceParticipants.splice(participantIndex, 1);
                  });
              });
          }
      }

      $scope.view.onSelectItineraryStopParticipantSource = function ($item, $model) {
          if ($item) {
              var itineraryStopParticipantIds = [];
              if ($scope.view.selectedItineraryStop !== null) {
                  itineraryStopParticipantIds = getParticipantIds($scope.view.selectedItineraryStop.participants);
              }
              angular.forEach($item.participants, function (p, index) {
                  if (!containsFilter(itineraryStopParticipantIds, p.participantId)) {
                      var participantCopy = angular.copy(p);
                      participantCopy.isSelected = false;
                      $scope.view.sourceParticipants.push(participantCopy);
                  }
              });
          }
      }

      $scope.view.onAddAllParticipantsToItineraryStop = function () {

      }

      $scope.view.onCloseClick = function () {
          $modalInstance.close($scope.view.travelPeriodParticipants);
      }

      $scope.view.onSelectItineraryStop = function ($item, $model) {
          if ($item) {
              copyTravelPeriodParticipants($item.participants);
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
          var travelStopParticipantIds = getParticipantIds($scope.view.travelStopParticipants);
          participant.isItineraryStopParticipant = containsFilter(travelStopParticipantIds, participant.participantId);
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
          debugger;
          var model = {
              participantIds: getParticipantIds(participants)
          }

          return ProjectService.updateItineraryStopParticipants(project.id, itinerary.id, itineraryStop.itineraryStopId, model)
          .then(function (response) {
              $scope.view.travelStopParticipants = getAllParticipants($scope.view.itineraryStops);
              setIsItineraryStopOnAllTravelPeriodParticipants();
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
          var alreadySelectedParticipantsById = getParticipantIds($scope.view.travelPeriodParticipants);
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

      function setIsItineraryStopOnAllTravelPeriodParticipants() {
          angular.forEach($scope.view.travelPeriodParticipants, function (p, index) {
              setIsItineraryStopParticipant(p);
          });
      }

      function loadTravelPeriodParticipants(project, itinerary) {
          $scope.view.isLoadingTravelPeriodParticipants = true;
          return ProjectService.getItineraryParticipants(project.id, itinerary.id)
          .then(function (response) {
              $scope.view.isLoadingTravelPeriodParticipants = false;
              $scope.view.travelPeriodParticipants = response.data;
              setParticipantSource($scope.view.itineraryParticipantSourceIndex, $scope.view.itineraryParticipantSourceName, $scope.view.travelPeriodParticipants);
              setIsItineraryStopOnAllTravelPeriodParticipants();
              return $scope.view.travelPeriodParticipants;
          })
          .catch(function (response) {
              $scope.view.isLoadingTravelPeriodParticipants = false;
          });
      }

      function loadItineraryStops(itinerary) {
          $scope.view.isLoadingItineraryStops = true;
          return ProjectService.getItineraryStops(itinerary.projectId, itinerary.id)
          .then(function (response) {

              $scope.view.travelStopParticipants = getAllParticipants(response.data);
              $scope.view.itineraryStops = response.data;
              angular.forEach($scope.view.itineraryStops, function (stop, index) {
                  stop.sourceIndex = $scope.view.itineraryStopParticipantSources.length;
                  setParticipantSource(stop.sourceIndex, stop.name, stop.participants);
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

      function setParticipantSource(index, name, participants) {
          $scope.view.itineraryStopParticipantSources[index] = {
              name: name,
              participants: participants
          };
      }

      function getAllParticipants(itineraryStops) {
          var participants = [];
          var isTravelStopParticipantAdded = function (participant) {
              for (var i = 0; i < participants.length; i++) {
                  var travelStopParticipant = participants[i];
                  if (travelStopParticipant.participantId === participant.participantId) {
                      return true;
                  }
              }
              return false;
          };

          angular.forEach(itineraryStops, function (stop, stopIndex) {
              angular.forEach(stop.participants, function (stopParticipant, stopParticipantIndex) {
                  if (!isTravelStopParticipantAdded(stopParticipant)) {
                      participants.push(stopParticipant)
                  }
              });
          });
          return participants;
      }

      loadItineraryStops(itinerary)
      .then(function () {
          loadTravelPeriodParticipants(project, itinerary)
            .then(function () {
                loadParticipants(project.id, null)
            })
      });



  });
