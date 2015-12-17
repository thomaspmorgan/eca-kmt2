'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ItineraryCtrl
 * @description
 * # ItineraryCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ItineraryCtrl', function (
      $scope,
      $state,
      $stateParams,
      $log,
      $q,
      FilterService,
      ProjectService,
      LocationService,
      StateService,
      NotificationService,
      ConstantsService) {
      
      $scope.view = {};
      $scope.view.itinerary = $scope.$parent.itinerary;
      $scope.view.project = null;
      $scope.view.isInEditMode = false;
      $scope.view.arrivalLocations = [];
      $scope.view.arrivalLocationsCount = 0;
      $scope.view.departureLocations = [];
      $scope.view.departureLocationsCount = 0;
      $scope.view.searchLimit = 30;
      $scope.view.isStartDateDatePickerOpen = false;
      $scope.view.isEndDateDatePickerOpen = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSaving = false;

      var itineraryCopy = angular.copy($scope.view.itinerary);

      $scope.view.onEditClick = function (itinerary) {
          $log.info('edit');
          $scope.view.isInEditMode = true;
      }

      $scope.view.onCommentClick = function (itinerary) {
          $log.info('comment');
      }

      $scope.view.onDeleteClick = function (itinerary) {
          $log.info('delete');
      }

      $scope.view.isLoadingRequiredData = true;
      $scope.$parent.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.project = project;
          $scope.view.isLoadingRequiredData = false;
      });

      $scope.view.onCancelClick = function (itinerary) {
          $log.info('cancel');
          $scope.view.itinerary = itineraryCopy;
          $scope.view.isInEditMode = false;
      }

      $scope.view.onSaveClick = function (itinerary) {
          $scope.view.isInEditMode = false;
          $scope.view.isSaving = true;
          
          return ProjectService.updateItinerary(itinerary, $scope.view.project.id)
          .then(function (response) {
              $scope.view.isSaving = false;
              NotificationService.showSuccessMessage("Successfully updated the traveling period.");
              var updateditinerary = response.data;
              initializeItinerary(updateditinerary);
              itineraryCopy = angular.copy(updateditinerary);
              $scope.view.itinerary = updateditinerary;
          })
          .catch(function (response) {
              $scope.view.isSaving = false;
              var message = "Unable to save updated itinerary.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      $scope.view.onDepartureLocationSelect = function ($item, $model) {
          $scope.view.itinerary.departureLocation = $model;
          $scope.view.itinerary.departureLocationId = $model.id;
      }

      $scope.view.onArrivalLocationSelect = function ($item, $model) {
          $scope.view.itinerary.arrivalLocation = $model;
          $scope.view.itinerary.arrivalLocationId = $model.id;
      }

      $scope.view.openStartDateDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isStartDateDatePickerOpen = true;
      }

      $scope.view.openEndDateDatePicker = function ($event) {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.view.isEndDateDatePickerOpen = true;
      }

      var arrivalFilter = FilterService.add('itinerary_arrivallocations');
      $scope.view.getArrivalLocations = function ($search) {
          var params = getSearchParams(arrivalFilter, $search, [ConstantsService.locationType.city.id]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.arrivalLocations = data.results;
              $scope.view.arrivalLocationsCount = data.total;
              return data.results;
          });
      }

      var departureFilter = FilterService.add('itinerary_departurelocations');
      $scope.view.getDepartureLocations = function ($search) {
          var params = getSearchParams(arrivalFilter, $search, [
              ConstantsService.locationType.city.id,
              ConstantsService.locationType.division.id,
              ConstantsService.locationType.country.id,
              ConstantsService.locationType.region.id
          ]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.departureLocations = data.results;
              $scope.view.departureLocationsCount = data.total;
              return data.results;
          });
      }

      function getSearchParams(filter, search, locationTypesById) {
          if (!angular.isArray(locationTypesById)) {
              throw Error('locationTypesById must be an array.');
          }
          filter.reset();
          filter = filter
              .skip(0)
              .take($scope.view.searchLimit);
              
          if (search) {
              filter = filter.like('name', search);
          }
          if (locationTypesById.length > 1) {
              filter = filter.in('locationTypeId', locationTypesById);
          }
          else if (locationTypesById.length === 1) {
              filter = filter.equal('locationTypeId', locationTypesById[0]);
          }
          return filter.toParams();
      }
      
      function loadLocations(params) {
          return LocationService.get(params)
          .then(function (response) {
              return response;
          })
          .catch(function (response) {
              $log.error('Unable to load locations.')
          })
      }

      function initializeItinerary(itinerary) {
          toDate(itinerary, 'startDate');
          toDate(itinerary, 'endDate');
          toDate(itinerary, 'lastRevisedOn');
          if (itinerary.arrivalLocation) {
              itinerary.arrivalLocationId = itinerary.arrivalLocation.id;
          }
          if (itinerary.departureLocation) {
              itinerary.departureLocationId = itinerary.departureLocation.id;
          }
      }

      function toDate(itinerary, datePropertyName) {
          var date = new Date(itinerary[datePropertyName]);
          if (!isNaN(date.getTime())) {
              itinerary[datePropertyName] = date;
          }
      }
      initializeItinerary($scope.view.itinerary);
  });
