'use strict';

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

          getTimezoneByLocation: function (location) {
              if (location.latitude && location.longitude) {
                  return service.getTimezoneByLatAndLong(location.latitude, location.longitude);
              }
              else {
                  var locationName = location.name;
                  if (location.division) {
                      locationName += ' ' + location.division;
                  }
                  if (location.country) {
                      locationName += ' ' + location.country
                  }
                  return service.getTimezoneByGeocoding(locationName);
              }
          },

          getTimezoneByLatAndLong: function (latitude, longitude) {
              var path = 'Location/Timezone';
              return DragonBreath.get({ latitude: latitude, longitude: longitude }, path)
              .then(function (response) {
                  if (response.data.result) {
                      return response.data.result;
                  }
              })
              .catch(function (response) {
                  $log.info('Unable to determine timezone by location.');
              })
          },

          getTimezoneByGeocoding: function (locationName) {
              return service.geocode(locationName)
              .then(function (response) {
                  $log.info('Found ' + response.length + ' geocoded responses for ' + locationName);
                  if (response.length === 1) {
                      var firstLocation = response[0];
                      var lat = firstLocation.geometry.location.lat();
                      var long = firstLocation.geometry.location.lng();
                      return service.getTimezoneByLatAndLong(lat, long);
                  }
                  else {
                      return null;
                  }
              })
              .catch(function (response) {
                  $log.info('Unable to determine timezone by location.');
              })
          },

          geocode: function (address, component) {
              var deferred = $q.defer();
              var geocoder = new google.maps.Geocoder();
              geocoder.geocode({ address: address }, function (results, status) {
                  if (status === google.maps.GeocoderStatus.OK) {
                      deferred.resolve(results);
                  }
                  else if (status === google.maps.GeocoderStatus.ZERO_RESULTS) {
                      deferred.resolve([]);
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

          getLocationSuggestions: function (search) {
              var dfd = $q.defer();
              var service = new google.maps.places.AutocompleteService();
              service.getQueryPredictions({ input: search }, function (predictions, status) {
                  if (status != google.maps.places.PlacesServiceStatus.OK) {
                      dfd.reject('Unable to load predictions.');
                  }
                  else {
                      dfd.resolve(predictions);
                  }
              });
              return dfd.promise;
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

                      if (transformedLocation.divisionShortName) {
                          return service.get(service.getDivisionLocationParams(transformedLocation))
                            .then(function (resultDivisions) {
                                if (resultDivisions.total === 1) {
                                    var divisionId = resultDivisions.results[0].id
                                    $log.info('Successfully located division id [' + divisionId + '] for geocoded division short name + [' + transformedLocation.divisionShortName + '].');
                                    transformedLocation.divisionId = divisionId;

                                    if (transformedLocation.cityShortName) {
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
                                    else {
                                        return transformedLocation;
                                    }
                                }
                                return transformedLocation;
                            })
                            .catch(function (response) {
                                $log.error('Unable to load divisions.');
                            });
                      }
                      else {
                          return transformedLocation;
                      }
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
