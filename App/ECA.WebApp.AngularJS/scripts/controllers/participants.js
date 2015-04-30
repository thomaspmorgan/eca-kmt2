'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ParticipantCtrl
 * @description
 * # ParticipantCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ParticipantCtrl', function ($scope, ParticipantService, PersonService, LookupService, LocationService, ConstantsService, $stateParams) {

      $scope.tabs = {
          personalInformation: {
              title: 'Personal Information',
              path: 'personalinformation',
              active: true,
              order: 1
          },
          activity: {
              title: 'Activities',
              path: 'activity',
              active: true,
              order: 2
          },
          relatedReports: {
              title: 'Related Reports',
              path: 'relatedreports',
              active: true,
              order: 3
          },
          impact: {
              title: 'Impact',
              path: 'impact',
              active: true,
              order: 4
          },
      };

      $scope.showPii = true;
      $scope.showContact = true;

      $scope.editPii = false;

      $scope.selectedCountriesOfCitizenship = [];

      $scope.activityImageSet = [
          'images/placeholders/participant/activities1.png',
          'images/placeholders/participant/activities2.png'
      ];

      $scope.updateGender = function () {
          $scope.pii.gender = getObjectById($scope.pii.genderId, $scope.genders).value;
          console.log($scope.pii);
      };

      $scope.updateCountryOfBirth = function () {
          $scope.pii.countryOfBirth = getObjectById($scope.pii.countryOfBirthId, $scope.countries).name;
          getCitiesByCountryId($scope.pii.countryOfBirthId);
          clearCityOfBirth();
      }

      function clearCityOfBirth() {
          delete $scope.pii.cityOfBirth;
          delete $scope.pii.cityOfBirthId;
      }

      $scope.updateCityOfBirth = function () {
          $scope.pii.cityOfBirth = getObjectById($scope.pii.cityOfBirthId, $scope.cities).name;
      }

      $scope.updateMaritalStatus = function () {
          $scope.pii.maritalStatus = getObjectById($scope.pii.maritalStatusId, $scope.maritalStatuses).value;
      }

      $scope.updateAddressCountry = function (id) {
          var address = getObjectById(id, $scope.pii.homeAddresses);
          address.country = getObjectById(address.countryId, $scope.countries).name;
          getAddressCitiesByCountryId(address.countryId);
          delete address.cityId;
          delete address.city;
      }

      $scope.updateAddressCity = function (id) {
          var address = getObjectById(id, $scope.pii.homeAddresses);
          address.city = getObjectById(address.cityId, $scope.addressCities).name;
      }

      function getObjectById(id, array) {
          for (var i = 0; i < array.length; i++) {
              if(array[i].id === id) {
                  return array[i];
              }
          }
          return null;
      };

    ParticipantService.getParticipantById($stateParams.participantId)
      .then(function (data) {
          $scope.participant = data;
          PersonService.getPiiById(data.personId)
            .then(function (data) {
                $scope.pii = data;
                $scope.selectedCountriesOfCitizenship = $scope.pii.countriesOfCitizenship.map(function (obj) {
                    var location = {};
                    location.id = obj.id;
                    location.name = obj.value;
                    return location;
                });
                if ($scope.pii.countryOfBirthId !== undefined) {
                    getCitiesByCountryId($scope.pii.countryOfBirthId);
                }
                if ($scope.pii.homeAddresses[0].countryId !== undefined) {
                    getAddressCitiesByCountryId($scope.pii.homeAddresses[0].countryId);
                }
            });
          PersonService.getContactInfoById(data.personId)
            .then(function (data) {
                $scope.contactInfo = data;
            });
      });

    function getCitiesByCountryId(countryId) {
        LocationService.get({
            limit: 300,
            filter: [{ property: 'countryId', comparison: 'eq', value: countryId },
                     { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id }]
        }).then(function (data) {
            $scope.cities = data.results;
        });
    };

    function getAddressCitiesByCountryId(countryId) {
        LocationService.get({
            limit: 300,
            filter: [{ property: 'countryId', comparison: 'eq', value: countryId },
                     { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.city.id }]
        }).then(function (data) {
            $scope.addressCities = data.results;
        });
    };

    LookupService.getAllGenders({ limit: 300 })
       .then(function (data) {
           $scope.genders = data.results;
       });

    LookupService.getAllMaritalStatuses({ limit: 300 })
      .then(function (data) {
          $scope.maritalStatuses = data.results;
      });

    LocationService.get({ limit: 300, filter: { property: 'locationTypeId', comparison: 'eq', value: ConstantsService.locationType.country.id } })
      .then(function (data) {
        $scope.countries = data.results;
      });

    $scope.cancelEditPii = function () {
        $scope.editPii = false;

        PersonService.getPiiById($scope.participant.personId)
            .then(function (data) {
                $scope.pii = data;
                $scope.selectedCountriesOfCitizenship = $scope.pii.countriesOfCitizenship.map(function (obj) {
                    var location = {};
                    location.id = obj.id;
                    location.name = obj.value;
                    return location;
                });
                if ($scope.pii.countryOfBirthId !== undefined) {
                    getCitiesByCountryId($scope.pii.countryOfBirthId);
                }
                if ($scope.pii.homeAddresses[0].countryId !== undefined) {
                    getAddressCitiesByCountryId($scope.pii.homeAddresses[0].countryId);
                }
            });
    };
    
  });
