'use strict';


/**
 * @ngdoc function
 * @name staticApp.controller:AddLocationCtrl
 * @description The add location controller is used to control adding new locations.
 * # AddLocationCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')

  .controller('MarkerCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        LookupService,
        LocationService,
        ConstantsService,
        NotificationService,
        FilterService
        ) {

      var geocodeResult = $scope.geocodeResult;
      $scope.view = {};
      $scope.view.location = {};
      $scope.view.matchingLocations = [];
      $scope.view.totalMatchingLocations = 0;
      $scope.view.locationExists = false;
      $scope.view.isTransformingLocation = false;
      $scope.view.isCheckingLocationExistence = false;
      $scope.view.googleLocation = '';
      $scope.view.maxInfoWindowLocations = 3;
      
      function doTransformGeocodedLocations() {
          $scope.view.isTransformingLocation = true;
          return LocationService.transformGeocodedLocation(geocodeResult)
            .then(function (transformedLocation) {
                if (transformedLocation.cityId && transformedLocation.locationTypeId !== ConstantsService.locationType.city.id) {
                    transformedLocation.city = transformedLocation.cityShortName;
                }
                $scope.view.googleLocation = geocodeResult.formatted_address;
                $scope.view.isTransformingLocation = false;
                $scope.view.location = transformedLocation;
                return checkNewLocationExistence($scope.view.location);
            })
            .catch(function () {
                var message = "Unable to transform google geocoded location.";
                $log.error(message);
                NotificationService.showErrorMessage(message);
                $scope.view.isTransformingLocation = false;
            });
      }

      var existenceFilter = FilterService.add('addlocation_existencefilter');
      function checkNewLocationExistence(loc) {
          
          $scope.view.locationExists = false;
          if (loc.locationTypeId !== ConstantsService.locationType.building.id
              && loc.locationTypeId !== ConstantsService.locationType.place.id) {
              existenceFilter.reset();
              existenceFilter = existenceFilter
                  .skip(0)
                  .take(10)
                  .sortBy('name');
              if (loc.locationTypeId === ConstantsService.locationType.city.id) {
                  existenceFilter = existenceFilter.equal('locationTypeId', ConstantsService.locationType.city.id);
              }
              if (loc.name && loc.name.length > 0) {
                  existenceFilter = existenceFilter.like('name', loc.name)
              }
              if (loc.countryId) {
                  existenceFilter = existenceFilter.equal('countryId', loc.countryId);
              }
              if (loc.divisionId) {
                  existenceFilter = existenceFilter.equal('divisionId', loc.divisionId);
              }
              $scope.view.isCheckingLocationExistence = true;
              return LocationService.get(existenceFilter.toParams())
              .then(function (response) {
                  $scope.view.totalMatchingLocations = response.total;
                  $scope.view.matchingLocations = response.results;
                  $scope.view.isCheckingLocationExistence = false;
                  if (response.total > 0) {
                      $scope.view.locationExists = true;
                  }
                  else {
                      $scope.view.locationExists = false;
                  }
              })
              .catch(function (response) {
                  var message = "Unable to validate location.";
                  $scope.view.isCheckingLocationExistence = false;
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
              });
          }
          else {
              var dfd = $q.defer();
              dfd.resolve();
              return dfd.promise;
          }
      }
      doTransformGeocodedLocations();
  });
