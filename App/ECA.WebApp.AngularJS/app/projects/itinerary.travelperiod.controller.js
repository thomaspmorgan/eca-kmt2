'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:TravelPeriodCtrl
 * @description
 * # TravelPeriodCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('TravelPeriodCtrl', function (
      $scope,
      $state,
      $stateParams,
      $log,
      $q,
      FilterService,
      ProjectService,
      LocationService,
      AuthService,
      StateService,
      ConstantsService,
      orderByFilter) {

      $scope.view = {};
      $scope.view.travelPeriod = $scope.$parent.travelPeriod;
      $scope.view.project = null;
      $scope.view.isInEditMode = false;
      $scope.view.arrivalLocations = [];
      $scope.view.arrivalLocationsCount = 0;
      $scope.view.departureLocations = [];
      $scope.view.departureLocationsCount = 0;
      $scope.view.searchLimit = 30;

      var travelPeriodCopy = angular.copy($scope.view.travelPeriod);

      $scope.view.onEditClick = function (travelPeriod) {
          $log.info('edit');
          $scope.view.isInEditMode = true;
      }

      $scope.view.onCommentClick = function (travelPeriod) {
          $log.info('comment');
      }

      $scope.view.onDeleteClick = function (travelPeriod) {
          $log.info('delete');
      }

      $scope.$parent.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          $scope.view.project = project;
      });

      $scope.view.onCancelClick = function (travelPeriod) {
          $log.info('cancel');
          $scope.view.travelPeriod = travelPeriodCopy;
          $scope.view.isInEditMode = false;
      }

      $scope.view.onSaveClick = function (travelPeriod) {
          $scope.view.isInEditMode = false;
          travelPeriodCopy = angular.copy(travelPeriod);
          $log.info('save');
      }

      $scope.view.onDepartureLocationSelect = function ($item, $model) {
          $scope.view.travelPeriod.departureLocation = $model;
      }

      $scope.view.onArrivalLocationSelect = function ($item, $model) {
          $scope.view.travelPeriod.arrivalLocation = $model;
      }

      var arrivalFilter = FilterService.add('itinerary_travelperiod_arrivallocations');
      $scope.view.getArrivalLocations = function ($search) {
          var params = getSearchParams(arrivalFilter, $search, [ConstantsService.locationType.city.id]);
          return loadLocations(params)
          .then(function (data) {
              $scope.view.arrivalLocations = data.results;
              $scope.view.arrivalLocationsCount = data.total;
              return data.results;
          });
      }

      var departureFilter = FilterService.add('itinerary_travelperiod_departurelocations');
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
  });
