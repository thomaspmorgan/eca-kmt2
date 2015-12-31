'use strict';
var onGoogleReady = function () {
    if (console && console.log) {
        console.log('Google maps api initialized.');
    }
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
          geoCodingConstants: {
              division: 'administrative_area_level_1',
              country: 'country',
              city: 'locality',
              route: 'route',
              display: 'formatted_address',
              pointOfInterest: 'point_of_interest',
              streetAddress: 'street_address'
          },

          getLocationTypeConstant: function (geocodeKey) {
              if (geocodeKey === service.geoCodingConstants.city) {
                  return ConstantsService.locationType.city;
              }
              if (geocodeKey === service.geoCodingConstants.streetAddress) {
                  return ConstantsService.locationType.building;
              }
              if (geocodeKey === service.geoCodingConstants.pointOfInterest
                  || geocodeKey === service.geoCodingConstants.route) {
                  return ConstantsService.locationType.place;
              }
              return null;
          },

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

          getCountryLocationParams: function (transformedLocation) {
              countriesFilter.reset();
              return countriesFilter
                  .skip(0)
                  .take(300)
                  .equal('locationTypeId', ConstantsService.locationType.country.id)
                  .equal('locationIso2', transformedLocation.countryShortName)
                  .sortBy('name')
                  .toParams();
          },

          getDivisionLocationParams: function (transformedLocation) {
              divisionsFilter.reset();
              
              divisionsFilter = divisionsFilter
                .skip(0)
                .take(300)
                .equal('locationTypeId', ConstantsService.locationType.division.id)
                .equal('countryId', transformedLocation.countryId)
                .equal('locationIso', transformedLocation.divisionShortName)
                .sortBy('name');
              return divisionsFilter.toParams();
          },

          getCityLocationParams: function (transformedLocation) {
              citiesFilter.reset();
              return citiesFilter
                .skip(0)
                .take(300)
                .equal('locationTypeId', ConstantsService.locationType.city.id)
                .equal('countryId', transformedLocation.countryId)
                .equal('divisionId', transformedLocation.divisionId)
                .like('name', transformedLocation.cityShortName)
                .sortBy('name')
                .toParams();
          },


          transformGeocodedLocation: function (result) {
              console.assert(result, 'The result must defined.');
              var transformedLocation = {};
              angular.forEach(result.address_components, function (comp, index) {
                  if (comp.types.indexOf(service.geoCodingConstants.city) >= 0) {
                      transformedLocation.cityLongName = comp.long_name;
                      transformedLocation.cityShortName = comp.short_name;
                  }
                  if (comp.types.indexOf(service.geoCodingConstants.country) >= 0) {
                      transformedLocation.countryLongName = comp.long_name;
                      transformedLocation.countryShortName = comp.short_name;
                  }
                  if (comp.types.indexOf(service.geoCodingConstants.division) >= 0) {
                      transformedLocation.divisionLongName = comp.long_name;
                      transformedLocation.divisionShortName = comp.short_name;
                  }
                  if (comp.types.indexOf(service.geoCodingConstants.pointOfInterest) >= 0) {
                      transformedLocation.name = comp.long_name;
                  }
              });
              for (var i = 0; i < result.types.length; i++) {
                  var constant = service.getLocationTypeConstant(result.types[i]);
                  if (constant !== null) {
                      
                      transformedLocation.locationTypeId = constant.id;
                      transformedLocation.locationTypeName = constant.value;
                      if (transformedLocation.locationTypeId === ConstantsService.locationType.city.id) {
                          transformedLocation.name = transformedLocation.cityLongName;
                      }
                      break;
                  }
              }
              transformedLocation.locationDisplay = result[service.geoCodingConstants.display];
              transformedLocation.latitude = result.geometry.location.lat();
              transformedLocation.longitude = result.geometry.location.lng();
              return service.get(service.getCountryLocationParams(transformedLocation))
              .then(function (resultCountries) {
                  if (resultCountries.total === 1) {
                      var countryId = resultCountries.results[0].id;
                      var regionId = resultCountries.results[0].regionId;
                      var region = resultCountries.results[0].region;
                      $log.info('Successfully located country id [' + countryId + '] for geocoded country short name + [' + transformedLocation.countryShortName + '].');
                      transformedLocation.countryId = countryId;
                      transformedLocation.regionId = regionId;
                      transformedLocation.region = region;

                      return service.get(service.getDivisionLocationParams(transformedLocation))
                      .then(function (resultDivisions) {
                          if (resultDivisions.total === 1) {
                              var divisionId = resultDivisions.results[0].id
                              $log.info('Successfully located division id [' + divisionId + '] for geocoded division short name + [' + transformedLocation.divisionShortName + '].');
                              transformedLocation.divisionId = divisionId;

                              return service.get(service.getCityLocationParams(transformedLocation))
                              .then(function (resultCities) {
                                  if (resultCities.total === 1) {
                                      var cityId = resultCities.results[0].id;
                                      transformedLocation.cityId = cityId;
                                      $log.info('Successfully located city id [' + cityId + '] for geocoded city short name + [' + transformedLocation.cityShortName + '].');
                                  }
                                  return transformedLocation;
                              })
                              .catch(function () {
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
