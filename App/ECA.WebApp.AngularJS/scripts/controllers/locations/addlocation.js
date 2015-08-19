'use strict';


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
        $modal,
        smoothScroll,
        LookupService,
        LocationService,
        ConstantsService,
        NotificationService,
        TableService,
        FilterService
        ) {
      $scope.view = {};
      $scope.view.locationTypes = [];
      $scope.view.regions = [];
      $scope.view.countries = [];
      $scope.view.divisions = [];
      $scope.view.cities = [];
      $scope.view.countryIso = [];
      $scope.view.googledLocation = {};
      $scope.view.isGeocoding = false;
      $scope.view.isLoadingRequiredData = false;
      $scope.view.isSavingNewLocation = false;
      $scope.view.isMapIdle = false;
      $scope.view.search = 'lincoln memorial';//'311 cobblestone landing mount juliet';
      $scope.view.mapOptions = {
          center: new google.maps.LatLng(38.9071, -77.0368),
          zoom: 5,
          mapTypeId: google.maps.MapTypeId.ROADMAP
      };
      $scope.view.newLocation = getNewLocation();

      $scope.view.onCountryChange = function () {
          loadDivisions();
          loadCities();
      }

      $scope.view.onDivisionChange = function () {
          return loadCities();
      }

      $scope.view.onLongitudeChange = function () {
          centerLongitudeAndLatitude();
      };

      $scope.view.onLatitudeChange = function () {
          centerLongitudeAndLatitude();
      };

      $scope.onMapIdle = function(){
          $scope.view.isMapIdle = true;
      }

      $scope.view.onSaveClick = function () {
          return saveNewLocation();
      }

      $scope.view.onLocationTypeChange = function () {
          if ($scope.view.newLocation.locationTypeId === ConstantsService.locationType.city.id) {
              delete $scope.view.newLocation.cityId;
          }
      }

      $scope.view.onSearchCityClick = function () {
          $scope.view.isGeocoding = true;
          return LocationService.geocode($scope.view.search, getNewLocationMap())
          .then(function (result) {
              $scope.view.isGeocoding = false;
              if (result.success && result.transformedLocation) {
                  if (result.transformedLocation.divisions && result.transformedLocation.divisions.length > 0) {
                      $scope.view.divisions = result.transformedLocation.divisions;
                  }
                  $scope.view.newLocation = angular.copy(result.transformedLocation);
              }
              else {
                  NotificationService.showWarningMessage('No results for found the search.');
              }
          })
          .catch(function (response) {
              $scope.view.isGeocoding = false;
              $log.error('Unable to perform search.');
              NotificationService.showErrorMessage("Unable to perform search.");
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
          .sortBy('name')
          .toParams();
      function loadContries() {
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
          })
          .catch(function () {
              var message = "Unable to load divisions.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var citiesFilter = FilterService.add('addlocation_cities');
      function loadCities() {
          
          citiesFilter.reset();
          citiesFilter = citiesFilter
              .skip(0)
              .take(300)
              .equal('locationTypeId', ConstantsService.locationType.city.id)
              .isNotNull('name')
              .sortBy('name');
          if ($scope.view.newLocation.divisionId) {
              citiesFilter = citiesFilter.equal('divisionId', $scope.view.newLocation.divisionId);
          }
          if ($scope.view.newLocation.countryId) {
              citiesFilter = citiesFilter.equal('countryId', $scope.view.newLocation.countryId);
          }

          var cityParams = citiesFilter.toParams();
          return LocationService.get(cityParams)
          .then(function (response) {
              $scope.view.cities = response.results;
          })
          .catch(function () {
              var message = "Unable to load cities.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var locationTypesFilter = FilterService.add('addlocation_locationTypes');
      var locationTypesParams = locationTypesFilter
          .skip(0)
          .take(300)
          .in('id', [ConstantsService.locationType.city.id, ConstantsService.locationType.building.id, ConstantsService.locationType.place.id])
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
          debugger;
          $scope.view.isSavingNewLocation = true;
          return LocationService.create($scope.view.newLocation)
          .then(function (response) {
              NotificationService.showSuccessMessage("Successfully saved location.");
              $scope.view.isSavingNewLocation = false;
          })
          .catch(function (response) {
              var message = "Unable to save location.";
              $log.error(message);
              NotificationService.showErrorMessage(message);
              $scope.view.isSavingNewLocation = false;
          });
      }



      //var map = new google.maps.Map(d3.select("#googlemap").node(), {
      //    zoom: 8,
      //    center: new google.maps.LatLng(37.76487, -122.41948),
      //    mapTypeId: google.maps.MapTypeId.TERRAIN
      //});

      //// Load the station data. When the data comes back, create an overlay.
      //d3.json("stations.json", function (data) {
      //    var overlay = new google.maps.OverlayView();

      //    // Add the container when the overlay is added to the map.
      //    overlay.onAdd = function () {
      //        var layer = d3.select(this.getPanes().overlayLayer).append("div")
      //            .attr("class", "stations");

      //        // Draw each marker as a separate SVG element.
      //        // We could use a single SVG, but what size would it have?
      //        overlay.draw = function () {
      //            var projection = this.getProjection(),
      //                padding = 10;

      //            var marker = layer.selectAll("svg")
      //                .data(d3.entries(data))
      //                .each(transform) // update existing markers
      //              .enter().append("svg:svg")
      //                .each(transform)
      //                .attr("class", "marker");

      //            // Add a circle.
      //            marker.append("svg:circle")
      //                .attr("r", 4.5)
      //                .attr("cx", padding)
      //                .attr("cy", padding);

      //            // Add a label.
      //            marker.append("svg:text")
      //                .attr("x", padding + 7)
      //                .attr("y", padding)
      //                .attr("dy", ".31em")
      //                .text(function (d) { return d.key; });

      //            function transform(d) {
      //                d = new google.maps.LatLng(d.value[1], d.value[0]);
      //                d = projection.fromLatLngToDivPixel(d);
      //                return d3.select(this)
      //                    .style("left", (d.x - padding) + "px")
      //                    .style("top", (d.y - padding) + "px");
      //            }
      //        };
      //    };

      //    // Bind our overlay to the map…
      //    overlay.setMap(map);
      //});



      $scope.view.isLoadingRequiredData = true;
      $q.all([getLocationTypes(), loadContries(), loadRegions()])
        .then(function () {
            $scope.view.isLoadingRequiredData = false;
        })
        .catch(function () {
            $scope.view.isLoadingRequiredData = false;
        });
  });
