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
        project,
        itinerary,
        containsFilter,
        NotificationService,
        ConstantsService,
        LocationService,
        ParticipantService,
        ProjectService,
        FilterService) {

      $scope.view = {};
      $scope.view.limit = 30;
      $scope.view.filteredParticipants = [];
      $scope.view.filteredParticipantsCount = 0;
      $scope.view.isLoadingFilteredParticipants = false;
      $scope.view.isLoadingTravelPeriodParticipants = false;
      $scope.view.showConfirmCancel = false;
      $scope.view.travelPeriodParticipants = [];
      $scope.view.travelStopParticipants = [];
      $scope.view.itineraryStops = [];
      $scope.view.isLoadingItineraryStops = false;

      $scope.view.onSaveClick = function () {
          
      }

      $scope.view.onCancelClick = function () {
          if ($scope.form.itineraryForm.$dirty) {
              $scope.view.showConfirmCancel = true;
          }
          else {
              $modalInstance.dismiss('cancel');
          }
      }

      $scope.view.onYesCancelChangesClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onNoDoNotCancelChangesClick = function () {
          $scope.view.showConfirmCancel = false;
      }

      $scope.view.onSelectItineraryParticipant = function($item, $model){
          return saveItineraryParticipants(project, itinerary, $scope.view.travelPeriodParticipants);
      }

      $scope.view.onRemoveItineraryParticipant = function ($item, $model) {
          return saveItineraryParticipants(project, itinerary, $scope.view.travelPeriodParticipants).then(loadParticipants(project.id, null));
      }

      $scope.view.getFilteredParticipants = function(search){
          return loadParticipants(project.id, search);
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
          })
      }

      var personParticipantsFilter = FilterService.add('manageparticipantscontroller_personparticipants');
      function loadParticipants(projectId, search) {
          $scope.view.isLoadingFilteredParticipants = true;
          personParticipantsFilter.reset();
          personParticipantsFilter = personParticipantsFilter.isNotNull('personId').take($scope.view.limit);
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

      function loadTravelPeriodParticipants(project, itinerary){
          $scope.view.isLoadingTravelPeriodParticipants = true;
          return ProjectService.getItineraryParticipants(project.id, itinerary.id)
          .then(function (response) {
              $scope.view.isLoadingTravelPeriodParticipants = false;
              $scope.view.travelPeriodParticipants = response.data;
              setIsItineraryStopParticipant($scope.view.travelPeriodParticipants, $scope.view.travelStopParticipants);
              return $scope.view.travelPeriodParticipants;
          })
          .catch(function(response){
              $scope.view.isLoadingTravelPeriodParticipants = false;
          });
      }

      function setIsItineraryStopParticipant(participants, travelStopParticipants) {
          var travelStopParticipantIds = getParticipantIds(travelStopParticipants);
          angular.forEach(participants, function (p, index) {
              p.isItineraryStopParticipant = containsFilter(travelStopParticipantIds, p.participantId);
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


      loadItineraryStops(itinerary)
      .then(function () {
          loadTravelPeriodParticipants(project, itinerary);
      });
      
      
  });
