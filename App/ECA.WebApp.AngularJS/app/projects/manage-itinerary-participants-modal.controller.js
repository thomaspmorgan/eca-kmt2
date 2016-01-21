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
      $scope.view.title = 'Manage ' + itinerary.name + ' Participants';
      $scope.view.limit = 30;
      $scope.view.filteredParticipants = [];
      $scope.view.filteredParticipantsCount = 0;
      $scope.view.isLoadingFilteredParticipants = false;
      $scope.view.isLoadingTravelPeriodParticipants = false;
      $scope.view.showConfirmCancel = false;
      $scope.view.travelPeriodParticipants = [];
      $scope.view.travelPeriodParticipantsCopy = [];
      $scope.view.travelStopParticipants = [];
      $scope.view.itineraryStops = [];
      $scope.view.selectedItineraryStop = null;
      $scope.view.isLoadingItineraryStops = false;

      $scope.view.onSortTravelPeriodParticipants = function () {
          $scope.view.travelPeriodParticipants = orderByFilter($scope.view.travelPeriodParticipants, 'fullName');
      }

      $scope.view.onSortItineraryStopParticipants = function () {
          $scope.view.selectedItineraryStop.participants = orderByFilter($scope.view.selectedItineraryStop.participants, 'fullName');
      }


      $scope.view.onAddAllItineraryParticipants = function () {
          var addedParticipants = false;
          var alreadyAddedParticipantIds = getParticipantIds($scope.view.selectedItineraryStop.participants);
          angular.forEach($scope.view.travelPeriodParticipants, function (p, index) {
              if (!containsFilter(alreadyAddedParticipantIds, p.participantId)) {
                  addedParticipants = true;
                  $scope.view.selectedItineraryStop.participants.push(p);
              }
          });
          if (addedParticipants) {
              return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants);
          }
      }

      $scope.view.onClearItineraryStopParticipantsClick = function () {
          $scope.view.selectedItineraryStop.participants = [];
          return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants);
      }

      $scope.view.onSelectItineraryParticipant = function($item, $model){
          return saveItineraryParticipants(project, itinerary, $scope.view.travelPeriodParticipants);
      }

      $scope.view.onRemoveItineraryParticipant = function ($item, $model) {
          return saveItineraryParticipants(project, itinerary, $scope.view.travelPeriodParticipants)
              .then(loadParticipants(project.id, null));
      }

      $scope.view.onSelectItineraryStopParticipant = function ($item, $model) {
          return saveItineraryStopParticipants(project, itinerary, $scope.view.selectedItineraryStop, $scope.view.selectedItineraryStop.participants);
      }

      $scope.view.onRemoveItineraryStopParticipant = function ($item, $model) {
          return $scope.view.onSelectItineraryStopParticipant($item, $model);
      }

      $scope.view.getFilteredParticipants = function(search){
          return loadParticipants(project.id, search);
      }

      $scope.view.onCloseClick = function () {
          $modalInstance.close($scope.view.travelPeriodParticipants);
      }

      $scope.view.onSelectItineraryStop = function ($item, $model) {
          if ($item) {
              copyTravelPeriodParticipants($item.participants);
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
              copyTravelPeriodParticipants([]);
              var message = "Successfully set travel period participants.";
              NotificationService.showSuccessMessage(message);
          })
          .catch(function (response) {
              var message = "Unable to set travel period participants.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          })
      }

      function saveItineraryStopParticipants(project, itinerary, itineraryStop, participants) {
          var model = {
              participantIds: getParticipantIds(participants)
          }
          
          return ProjectService.updateItineraryStopParticipants(project.id, itinerary.id, itineraryStop.itineraryStopId, model)
          .then(function (response) {
              $scope.view.travelStopParticipants = getAllParticipants($scope.view.itineraryStops);              
              copyTravelPeriodParticipants(participants);
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
          if (search) {
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

      function loadTravelPeriodParticipants(project, itinerary){
          $scope.view.isLoadingTravelPeriodParticipants = true;
          return ProjectService.getItineraryParticipants(project.id, itinerary.id)
          .then(function (response) {
              $scope.view.isLoadingTravelPeriodParticipants = false;
              $scope.view.travelPeriodParticipants = response.data;
              copyTravelPeriodParticipants([]);
              setIsItineraryStopOnAllTravelPeriodParticipants();
              return $scope.view.travelPeriodParticipants;
          })
          .catch(function(response){
              $scope.view.isLoadingTravelPeriodParticipants = false;
          });
      }

      function loadItineraryStops(itinerary) {
          $scope.view.isLoadingItineraryStops = true;
          return ProjectService.getItineraryStops(itinerary.projectId, itinerary.id)
          .then(function (response) {

              $scope.view.travelStopParticipants = getAllParticipants(response.data);
              $scope.view.itineraryStops = response.data;
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

      function copyTravelPeriodParticipants(excludedParticipants) {
          $scope.view.travelPeriodParticipantsCopy = [];
          excludedParticipants = excludedParticipants || [];
          
          var excludedParticipantIds = getParticipantIds(excludedParticipants);
          angular.forEach($scope.view.travelPeriodParticipants, function (p, index) {
              if (!containsFilter(excludedParticipantIds, p.participantId)) {
                  $scope.view.travelPeriodParticipantsCopy.push({
                      fullName: p.fullName,
                      participantId: p.participantId,
                      personId: p.personId
                  });
              }
              
          });
      }

      loadItineraryStops(itinerary)
      .then(function () {
          loadTravelPeriodParticipants(project, itinerary);
      });
      
      
  });
