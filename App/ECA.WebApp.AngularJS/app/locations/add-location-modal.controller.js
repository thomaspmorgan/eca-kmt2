﻿'use strict';


/**
 * @ngdoc function
 * @name staticApp.controller:AddLocationCtrl
 * @description The add location controller is used to control adding new locations.
 * # AddLocationCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')

  .controller('AddLocationCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $timeout,
        $compile,
        $modalInstance,
        allowedLocationTypeIds,
        LookupService,
        LocationService,
        ConstantsService,
        NotificationService,
        TableService,
        FilterService,
        containsFilter
        ) {
      if (allowedLocationTypeIds !== null) {
          if (containsFilter(allowedLocationTypeIds, ConstantsService.locationType.country.id)) {
              throw Error('The add location modal does not support adding countries.');
          }
          if (containsFilter(allowedLocationTypeIds, ConstantsService.locationType.division.id)) {
              throw Error('The add location modal does not support adding divisions.');
          }
          if (containsFilter(allowedLocationTypeIds, ConstantsService.locationType.region.id)) {
              throw Error('The add location modal does not support adding regions.');
          }
          if (containsFilter(allowedLocationTypeIds, ConstantsService.locationType.post.id)) {
              throw Error('The add location modal does not support adding posts.');
          }
          if (containsFilter(allowedLocationTypeIds, ConstantsService.locationType.address.id)) {
              throw Error('The add location modal does not support adding posts.');
          }
      }

      $scope.view = {};
      $scope.view.locationTypes = [];
      $scope.view.regions = [];
      $scope.view.countries = [];
      $scope.view.divisions = [];
      $scope.view.cities = [];
      $scope.view.countryIso = [];
      $scope.view.googledLocation = {};
      $scope.view.isGeocoding = false;
      $scope.view.isLoadingPredictions = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isLoadingCities = false;
      $scope.view.isLoadingDivisions = false;
      $scope.view.isSavingNewLocation = false;
      $scope.view.isMapIdle = false;
      $scope.view.search = '';
      $scope.view.locationExists = false;
      $scope.view.isTransformingLocation = false;
      $scope.view.isLongitudeRequired = false;
      $scope.view.isLatitudeRequired = false;
      $scope.view.matchingLocations = [];
      $scope.view.totalMatchingLocations = 0;

      $scope.view.mapOptions = {
          center: new google.maps.LatLng(38.9071, -77.0368),
          zoom: 0,
          mapTypeId: google.maps.MapTypeId.ROADMAP
      };
      $scope.view.newLocation = getNewLocation();

      $scope.view.onCountryChange = function () {
          loadDivisions();
          setRegionByCountryId($scope.view.newLocation.countryId);
          clearCity();
          clearLatAndLong();
          return checkNewLocationExistence();
      }

      function setRegionByCountryId(countryId) {
          angular.forEach($scope.view.countries, function (country, index) {
              if (country.id === countryId) {
                  $scope.view.newLocation.regionId = country.regionId;
              }
          });
      }

      $scope.view.onSelectCityBlur = function ($event) {
          if ($scope.view.newLocation.city === '') {
              clearCity();
              clearDivision();
              clearCountry();
              clearLatAndLong();
          }
      };

      $scope.view.onSelectCity = function ($item, $model, $label) {
          $scope.view.newLocation.city = $item.name;
          $scope.view.newLocation.cityId = $item.id;
          if ($item.region && !$scope.view.newLocation.regionId) {
              $log.info('Auto populating region to location.');
              $scope.view.newLocation.regionId = $item.regionId;
          }
          if ($item.country && $item.countryId) {
              $log.info('Auto populating country to location.');
              $scope.view.newLocation.countryId = $item.countryId;
          }
          if ($item.division && $item.divisionId) {
              $log.info('Auto populating division to location.');
              $scope.view.newLocation.divisionId = $item.divisionId;

              return $q.all(loadDivisions(), checkNewLocationExistence())
              .then(function () {
              })
              .catch(function () {
                  $log.error('Error when selecting city.');
              });
          } else {
              clearDivision();
          }
      }

      $scope.view.onGetGooglePredictions = function (search) {
          return LocationService.getLocationSuggestions(search)
          .then(function (predictions) {
              return predictions;
          })
          .catch(function (response) {
              $log.error(response);
          });
      }

      $scope.view.onDivisionChange = function () {
          clearCity();
          clearLatAndLong();
          return checkNewLocationExistence();
      }

      $scope.view.onLongitudeChange = function () {
          centerLongitudeAndLatitude();
      };

      $scope.view.onLatitudeChange = function () {
          return centerLongitudeAndLatitude();
      };

      $scope.view.onNameChange = function () {
          return checkNewLocationExistence();
      }

      var fixedRedrawIssue = false;
      $scope.view.onMapIdle = function () {
          //this fixes a google map issue when this modal is closed and then reopened
          //the map would just show a grey box
          if (!fixedRedrawIssue) {
              var map = getNewLocationMap();
              google.maps.event.trigger(map, 'resize');
              map.setCenter($scope.view.mapOptions.center);
              fixedRedrawIssue = true;
          }
          $scope.view.isMapIdle = true;
      }

      $scope.view.loadCities = function (search) {
          return loadCities(search);
      }

      $scope.view.onSaveClick = function () {
          return saveNewLocation()
              .then(function (loc) {
                  $modalInstance.close(loc);
              });
      }

      $scope.view.onCloseClick = function () {
          $modalInstance.dismiss('cancel');
      }

      $scope.view.onLocationTypeChange = function () {
          if ($scope.view.newLocation.locationTypeId === ConstantsService.locationType.city.id) {
              $scope.view.newLocation.city = '';
              delete $scope.view.newLocation.city;
              delete $scope.view.newLocation.cityId;
          }
          if ($scope.view.newLocation.locationTypeId === ConstantsService.locationType.place.id
              || $scope.view.newLocation.locationTypeId === ConstantsService.locationType.building.id) {
              $scope.view.isLatitudeRequired = true;
              $scope.view.isLongitudeRequired = true;
          }
          else {
              $scope.view.isLatitudeRequired = false;
              $scope.view.isLongitudeRequired = false;
          }
          return checkNewLocationExistence();
      }

      $scope.view.onSearchCityClick = function () {
          return doGeocode($scope.view.search);
      }

      $scope.view.onLatitudeMapClick = function () {
          var map = getNewLocationMap();
          var center = map.getCenter();
          $scope.view.newLocation.latitude = center.lat();
          $scope.view.newLocation.longitude = center.lng();
      }

      $scope.view.onGooglePredictionSelect = function ($item, $model, $label) {
          var map = getNewLocationMap();
          return getPlaceById($item.place_id, map);
      }

      var markers = [];
      function doGeocode(address) {
          clearMapMarkers();
          $scope.view.isGeocoding = true;
          return LocationService.geocode(address)
          .then(function (results) {
              if (results.length > 0) {
                  var map = getNewLocationMap();
                  var bounds = new google.maps.LatLngBounds();
                  angular.forEach(results, function (result, index) {
                      bounds.extend(result.geometry.location);
                      window.setTimeout(function () {
                          var marker = new google.maps.Marker({
                              position: result.geometry.location,
                              map: map,
                              animation: google.maps.Animation.DROP,
                          });
                          marker.addListener('click', function () {
                              onMapMarkerClick(marker, result);
                          });
                          markers.push(marker);
                      }, index * 100);
                  });
                  map.fitBounds(bounds);
              }
              else {
                  NotificationService.showWarningMessage('No results found for the search.');
              }
              $scope.view.isGeocoding = false;
          })
          .catch(function (response) {
              $scope.view.isGeocoding = false;
              $log.error('Unable to perform search.');
              NotificationService.showErrorMessage("Unable to perform search.");
          });
      }

      function getPlaceById(googlePlaceId, map) {
          var dfd = $q.defer();
          var service = new google.maps.places.PlacesService(map);          
          service.getDetails({ placeId: googlePlaceId }, function (place, status) {
              if (status == google.maps.places.PlacesServiceStatus.OK) {
                  var bounds = new google.maps.LatLngBounds();
                  bounds.extend(place.geometry.location);
                  var marker = new google.maps.Marker({
                      position: place.geometry.location,
                      map: map,
                      animation: google.maps.Animation.DROP,
                      title: place.name
                  });
                  marker.addListener('click', function () {
                      onMapMarkerClick(marker, place);
                  });
                  map.fitBounds(bounds);
                  markers.push(marker);
                  dfd.resolve(marker);
              }
              else {
                  dfd.reject('Unable to load google place by id.');
              }
          });
          return dfd.promise;
      }

      function clearLatAndLong() {
          delete $scope.view.newLocation.longitude;
          delete $scope.view.newLocation.latitude;
      }

      function clearCity() {
          delete $scope.view.newLocation.cityId;
          delete $scope.view.newLocation.city;
      }

      function clearDivision() {
          delete $scope.view.newLocation.divisionId;
      }

      function clearCountry() {
          delete $scope.view.newLocation.countryId;
      }

      function clearMapMarkers() {
          angular.forEach(markers, function (marker, index) {
              marker.setMap(null);
          });
          markers = [];
      }

      var infoWindow = null;
      function onMapMarkerClick(marker, geocodeResult) {
          $scope.view.isTransformingLocation = true;
          var map = getNewLocationMap();

          $scope.geocodeResult = geocodeResult;
          var content = '<div><ng-include src="\'app/locations/marker.html\'"></ng-include></div>';
          var compiled = $compile(content)($scope);
          if (infoWindow) {
              infoWindow.close();
          }
          infoWindow = new google.maps.InfoWindow();
          infoWindow.setContent(compiled[0]);
          infoWindow.open(map, marker);
          return LocationService.transformGeocodedLocation(geocodeResult)
          .then(function (transformedLocation) {
              if (transformedLocation.cityId && transformedLocation.locationTypeId !== ConstantsService.locationType.city.id) {
                  transformedLocation.city = transformedLocation.cityShortName;
              }
              if (!transformedLocation.name && transformedLocation.locationTypeId !== ConstantsService.locationType.city.id) {
                  transformedLocation.name = geocodeResult.name;
              }
              $scope.view.newLocation = transformedLocation;
              var locationLoadPromises = [loadCountries(), loadDivisions(), checkNewLocationExistence()];
              if (transformedLocation.locationTypeId !== ConstantsService.locationType.city.id) {
                  locationLoadPromises.push(loadCities(transformedLocation.cityShortName));
              }
              return $q.all(locationLoadPromises)
              .then(function () {
                  $scope.view.isTransformingLocation = false;
              })
              .catch(function () {
                  var message = 'Unable to perform lookups necessary for selecting a google location.';
                  $log.error(message);
                  NotificationService.showErrorMessage(message);
                  $scope.view.isTransformingLocation = false;
              });
          })
          .catch(function () {
              var message = "Unable to transform google geocoded location.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
              $scope.view.isTransformingLocation = false;
          });
      }

      function getNewLocationMap() {
          return $scope.view.newLocationMap;
      }

      function centerLongitudeAndLatitude() {
          if ($scope.view.newLocation.longitude && $scope.view.newLocation.latitude) {
              var map = getNewLocationMap();
              map.setCenter({ lat: $scope.view.newLocation.latitude, lng: $scope.view.newLocation.longitude });
          }
      }

      var countriesFilter = FilterService.add('addlocation_countries');
      var countriesParams = countriesFilter
          .skip(0)
          .take(300)
          .equal('locationTypeId', ConstantsService.locationType.country.id)
          .isTrue('isActive')
          .sortBy('name')
          .toParams();
      function loadCountries() {
          return LocationService.get(countriesParams)
          .then(function (response) {
              $scope.view.countries = response.results;
          })
          .catch(function () {
              var message = "Unable to load countries.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var regionsFilter = FilterService.add('addlocation_regions');
      var regionsParams = regionsFilter
          .skip(0)
          .take(25)
          .equal('locationTypeId', ConstantsService.locationType.region.id)
          .sortBy('name')
          .toParams();
      function loadRegions() {
          return LocationService.get(regionsParams)
          .then(function (response) {
              $scope.view.regions = response.results;
          })
          .catch(function () {
              var message = "Unable to load regions.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var divisionsFilter = FilterService.add('addlocation_divisions');
      function loadDivisions() {
          $scope.view.isLoadingDivisions = true;
          var countryId = $scope.view.newLocation.countryId;
          divisionsFilter.reset();
          var divisionsParams = divisionsFilter
              .skip(0)
              .take(300)
              .equal('locationTypeId', ConstantsService.locationType.division.id)
              .equal('countryId', countryId)
              .sortBy('name')
              .toParams();
          return LocationService.get(divisionsParams)
          .then(function (response) {
              $scope.view.divisions = response.results;
              $scope.view.isLoadingDivisions = false;
          })
          .catch(function () {
              $scope.view.isLoadingDivisions = false;
              var message = "Unable to load divisions.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var citiesFilter = FilterService.add('addlocation_cities');
      function loadCities(search) {
          citiesFilter.reset();
          citiesFilter = citiesFilter
              .skip(0)
              .take(300)
              .equal('locationTypeId', ConstantsService.locationType.city.id)
              .isNotNull('name')
              .sortBy('name');
          if ($scope.view.newLocation.countryId) {
              citiesFilter = citiesFilter.equal('countryId', $scope.view.newLocation.countryId);
          }
          if ($scope.view.newLocation.divisionId) {
              citiesFilter = citiesFilter.equal('divisionId', $scope.view.newLocation.divisionId);
          }
          if (search) {
              citiesFilter = citiesFilter.like('name', search);
          }
          var cityParams = citiesFilter.toParams();
          $scope.view.isLoadingCities = true;
          return LocationService.get(cityParams)
          .then(function (response) {
              $scope.view.cities = response.results;
              $scope.view.isLoadingCities = false;
              return response.results;
          })
          .catch(function () {
              var message = "Unable to load cities.";
              $scope.view.isLoadingCities = false;
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var locationTypeIds = allowedLocationTypeIds || [ConstantsService.locationType.city.id, ConstantsService.locationType.building.id, ConstantsService.locationType.place.id];
      var locationTypesFilter = FilterService.add('addlocation_locationTypes');
      var locationTypesParams = locationTypesFilter
          .skip(0)
          .take(300)
          .in('id', locationTypeIds)
          .sortBy('value')
          .toParams();
      function getLocationTypes() {
          return LookupService.getLocationTypes(locationTypesParams)
          .then(function (response) {
              $scope.view.locationTypes = response.data.results;
          })
          .catch(function (response) {
              var message = "Unable to load location types.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function getNewLocation() {
          return {
          };
      }

      function saveNewLocation() {
          $scope.view.isSavingNewLocation = true;
          return LocationService.create($scope.view.newLocation)
          .then(function (response) {
              NotificationService.showSuccessMessage("Successfully saved location.");
              $scope.view.isSavingNewLocation = false;
              var savedLocation = response.data;
              return savedLocation;
          })
          .catch(function (response) {
              var message = "Unable to save location.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
              $scope.view.isSavingNewLocation = false;
          });
      }

      var existenceFilter = FilterService.add('addlocation_existencefilter');
      function checkNewLocationExistence() {
          $scope.view.locationExists = false;
          $scope.view.matchingLocations = [];
          existenceFilter.reset();
          existenceFilter = existenceFilter
              .skip(0)
              .take(10)
              .sortBy('name');
          if ($scope.view.newLocation.locationTypeId) {
              existenceFilter = existenceFilter.equal('locationTypeId', $scope.view.newLocation.locationTypeId);
          }
          if ($scope.view.newLocation.name && $scope.view.newLocation.name.length > 0) {
              existenceFilter = existenceFilter.like('name', $scope.view.newLocation.name)
          }
          if ($scope.view.newLocation.countryId) {
              existenceFilter = existenceFilter.equal('countryId', $scope.view.newLocation.countryId);
          }
          if ($scope.view.newLocation.divisionId) {
              existenceFilter = existenceFilter.equal('divisionId', $scope.view.newLocation.divisionId);
          }
          return LocationService.get(existenceFilter.toParams())
          .then(function (response) {
              $scope.view.locationExists = response.total > 0;
              $scope.view.matchingLocations = response.results;
              $scope.view.totalMatchingLocations = response.total;
          })
          .catch(function (response) {
              var message = "Unable to validate location.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
          });

      }

      $scope.view.isLoadingRequiredData = true;
      $q.all([getLocationTypes(), loadCountries(), loadRegions()])
        .then(function () {
            $scope.view.isLoadingRequiredData = false;
            var element = angular.element('#locationName');
            $timeout(function () {
                element.focus();
            }, 800);
        })
        .catch(function () {
            $scope.view.isLoadingRequiredData = false;
        });
  });
