'use strict';
var onGoogleReady = function() {
    console.log('Google maps api initialized.');
    angular.bootstrap(document.getElementById('map'), ['staticApp']);
}

/**
 * @ngdoc service
 * @name staticApp.LocationService
 * @description
 * # LocationService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('LocationService', function (DragonBreath, $q, $log, ConstantsService, FilterService) {

      var countriesFilter = FilterService.add('locationService_countriesFilter');
      var divisionsFilter = FilterService.add('locationService_divisionsFilter');
      var citiesFilter = FilterService.add('locationService_citiesFilter');

      var service = {
          get: function (params) {
              var defer = $q.defer();
              DragonBreath.get(params, 'locations')
                .success(function (data) {
                    defer.resolve(data);
                });
              return defer.promise;
          },

          create: function (location) {
              return DragonBreath.create(location, 'locations');
          },

          geocode: function (address) {
              var deferred = $q.defer();
              var geocoder = new google.maps.Geocoder();
              geocoder.geocode({ address: address }, function (results, status) {
                  var serviceResult = {
                      success: false,
                      transformedLocation: null,
                      geocodeResponse: null
                  };
                  if (status === google.maps.GeocoderStatus.OK) {                      
                      //$q.when(service.handleGeocodeResponse(results[0]))
                      //.then(function (transformedLocation) {
                      //    $log.info('Successfully geocoded a location.');
                      //    serviceResult.success = true;
                      //    serviceResult.transformedLocation = transformedLocation;
                      //    serviceResult.geocodeResponse = results[0];
                      //    deferred.resolve(serviceResult);
                      //});

                      deferred.resolve(results);

                  }
                  else if (status === google.maps.GeocoderStatus.ZERO_RESULTS) {
                      deferred.resolve(serviceResult);
                  }
                  else {
                      var message = 'Unable to geocode the given location.  Error returned from api [' + status.toString() + '].'
                      deferred.reject(message);
                  }
              });
              return deferred.promise;

          },

          transformGeocodedLocation: function (result) {
              console.assert(result, 'The result must defined.');
              var transformedLocation = {};
              var divisionKey = 'administrative_area_level_1';
              var countryKey = 'country';
              var cityKey = 'locality';
              var displayKey = 'formatted_address';
              var pointOfInterestKey = "point_of_interest";
              var streetAddressKey = "street_address";

              angular.forEach(result.address_components, function (comp, index) {
                  if (comp.types.indexOf(cityKey) >= 0) {
                      transformedLocation.cityLongName = comp.long_name;
                      transformedLocation.cityShortName = comp.short_name;
                  }

                  if (comp.types.indexOf(countryKey) >= 0) {
                      transformedLocation.countryLongName = comp.long_name;
                      transformedLocation.countryShortName = comp.short_name;

                  }
                  if (comp.types.indexOf(divisionKey) >= 0) {
                      transformedLocation.divisionLongName = comp.long_name;
                      transformedLocation.divisionShortName = comp.short_name;
                  }
                  if (comp.types.indexOf(pointOfInterestKey) >= 0) {
                      transformedLocation.name = comp.long_name;
                  }
              });
              var locationTypeConstant = null;
              angular.forEach(result.types, function (t, index) {
                  if (locationTypeConstant === null && t === cityKey) {
                      locationTypeConstant = ConstantsService.locationType.city;
                  }
                  if (locationTypeConstant === null && t === streetAddressKey) {
                      locationTypeConstant = ConstantsService.locationType.building;
                  }
                  if (locationTypeConstant === null && t === pointOfInterestKey) {
                      locationTypeConstant = ConstantsService.locationType.place;
                  }
              });
              if (locationTypeConstant !== null) {
                  transformedLocation.locationTypeId = locationTypeConstant.id;
                  transformedLocation.locationTypeName = locationTypeConstant.value;
                  if (transformedLocation.locationTypeId === ConstantsService.locationType.city.id) {
                      transformedLocation.name = transformedLocation.cityLongName;
                  }                  
              }
              transformedLocation.locationDisplay = result[displayKey];
              transformedLocation.latitude = result.geometry.location.G;
              transformedLocation.longitude = result.geometry.location.K;

              divisionsFilter.reset();
              countriesFilter.reset();
              var countriesParams = countriesFilter
                  .skip(0)
                  .take(300)
                  .equal('locationTypeId', ConstantsService.locationType.country.id)
                  .equal('locationIso2', transformedLocation.countryShortName)
                  .sortBy('name')
                  .toParams();
              return service.get(countriesParams)
              .then(function (resultCountries) {
                  if (resultCountries.total === 1) {
                      var countryId = resultCountries.results[0].id;
                      $log.info('Successfully located country id [' + countryId + '] for geocoded country short name + [' + transformedLocation.countryShortName + '].');
                      transformedLocation.countryId = countryId;
                      var divisionsParams = divisionsFilter
                          .skip(0)
                          .take(300)
                          .equal('locationTypeId', ConstantsService.locationType.division.id)
                          .equal('countryId', countryId)
                          .equal('locationIso', transformedLocation.divisionShortName)
                          .sortBy('name')
                          .toParams();
                      return service.get(divisionsParams)
                      .then(function (resultDivisions) {
                          transformedLocation.divisions = resultDivisions.results;
                          if (resultDivisions.total === 1) {
                              var divisionId = resultDivisions.results[0].id
                              $log.info('Successfully located division id [' + divisionId + '] for geocoded division short name + [' + transformedLocation.divisionShortName + '].');
                              transformedLocation.divisionId = divisionId;
                              citiesFilter.reset();
                              var citiesParams = citiesFilter
                                  .skip(0)
                                  .take(300)
                                  .equal('locationTypeId', ConstantsService.locationType.city.id)
                                  .equal('countryId', countryId)
                                  .like('name', transformedLocation.cityShortName)
                                  .sortBy('name')
                                  .toParams();
                              return service.get(citiesParams)
                              .then(function (resultCities) {
                                  if (resultCities.total === 1) {
                                      var cityId = resultCities.results[0].id;
                                      transformedLocation.cityId = cityId;
                                      $log.info('Successfully located city id [' + cityId + '] for geocoded city short name + [' + transformedLocation.divisionShortName + '].');
                                  }
                                  return transformedLocation;
                              })
                              .catch(function() {
                                  $log.error('Unable to load cities.');
                              })
                          }
                          return transformedLocation;
                      })
                    .catch(function (response) {
                        $log.error('Unable to load divisions.');
                    });
                  }
                  return transformedLocation;
              })
              .catch(function () {
                  $log.error('Unable to load countries for geocoded response.');
              });
          }
      };
      return service;
  });
