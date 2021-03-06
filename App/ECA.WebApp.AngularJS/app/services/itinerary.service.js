﻿'use strict';

/**
 * @ngdoc service
 * @name staticApp.itinerary
 * @description
 * # itinerary
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('ItineraryService', function (DragonBreath, $q, DateTimeService) {
      var colorIndex = 0;
      var colors = ["#d35304", "#8e9900", "#689e0c", "#640096", "#dd067d", "#028287", "#ea007d", "#024c60", "#0da514", "#e216af", "#9e0910", "#0b6293", "#51a508", "#1c9e0e", "#dbb702", "#078435", "#8c0c21", "#460f9e", "#319e03", "#064260", "#bc0b6f", "#0da591", "#0f0666", "#2c930d", "#054175", "#c405a1", "#9e0803", "#02426b", "#6e9601", "#576d00", "#099620", "#0c7287", "#89a309", "#e05d06", "#98a004", "#027f5c", "#0e8c5d", "#2a7a00", "#071f8c", "#e50d80", "#017756", "#cc6a14", "#007a78", "#056482", "#007267", "#af0e08", "#076677", "#9b1d07", "#350d84", "#87011e", "#0b5570", "#e56814", "#8c0c35", "#69960f", "#646d02", "#4a820a", "#af001d", "#ad1608", "#068934", "#023e70", "#efbc02", "#ce0ca1", "#e008ae", "#ef09ef"];

      return {
          getItineraries: function (projectId) {
              return DragonBreath.get({}, 'projects/' + projectId + '/itineraries');
          },
          getItineraryParticipants: function (projectId, itineraryId) {
              return DragonBreath.get({}, 'projects/' + projectId + '/itinerary/' + itineraryId + '/participants');
          },
          updateItineraryParticipants: function (projectId, itineraryId, participantIds) {
              return DragonBreath.create(participantIds, 'projects/' + projectId + '/itinerary/' + itineraryId + '/participants');
          },
          addItinerary: function (itinerary, projectId) {
              return DragonBreath.create(itinerary, 'projects/' + projectId + '/itinerary');
          },
          updateItinerary: function (itinerary, projectId) {
              return DragonBreath.save(itinerary, 'projects/' + projectId + '/itinerary');
          },
          getItineraryStops: function (projectId, itineraryId) {
              var me = this;
              colorIndex = 0;
              return DragonBreath.get({}, 'projects/' + projectId + '/itinerary/' + itineraryId + '/stops')
              .then(function (response) {
                  angular.forEach(response.data, function (stop, index) {
                      me.initializeItineraryStopModel(stop);
                      stop.setArrivalDateFromString(stop.arrivalDate);
                      stop.setDepartureDateFromString(stop.departureDate);
                  });
                  return response;
              });
          },
          addItineraryStop: function (itineraryStop, projectId, itineraryId) {
              var me = this;
              return DragonBreath.create(itineraryStop, 'projects/' + projectId + '/itinerary/' + itineraryId + '/stops')
              .then(function (response) {
                  var stop = response.data;
                  me.initializeItineraryStopModel(stop);
                  stop.setArrivalDateFromString(stop.arrivalDate);
                  stop.setDepartureDateFromString(stop.departureDate);
                  return response;
              });
          },
          updateItineraryStop: function (itineraryStop, projectId, itineraryId) {
              var me = this;
              return DragonBreath.save(itineraryStop, 'projects/' + projectId + '/itinerary/' + itineraryId + '/stops')
              .then(function (response) {
                  var stop = response.data;
                  me.initializeItineraryStopModel(stop, itineraryStop.colorIndex);
                  stop.setArrivalDateFromString(stop.arrivalDate);
                  stop.setDepartureDateFromString(stop.departureDate);
                  return response;
              });
          },
          updateItineraryStopParticipants: function (projectId, itineraryId, itineraryStopId, participantIds) {
              return DragonBreath.create(participantIds, 'projects/' + projectId + '/itinerary/' + itineraryId + '/stop/' + itineraryStopId + '/participants');
          },

          initializeItineraryStopModel: function (itineraryStop, itineraryStopColorIndex) {
              var dateFormat = "MMM D, YYYY";
              var timeFormat = "h:mm a";

              var cIndex = itineraryStopColorIndex || colorIndex++;
              itineraryStop.colorIndex = cIndex;
              itineraryStop.color = colors[cIndex % colors.length];

              if (itineraryStop.timezoneId) {
                  itineraryStop.timezone = moment.tz.zone(itineraryStop.timezoneId);
              }
              else {
                  itineraryStop.timezone = null;
              }


              if (itineraryStop.destinationLocation) {
                  itineraryStop.destinationLocationId = itineraryStop.destinationLocation.id;
              }
              else {
                  itineraryStop.destinationLocationId = null;
              }

              itineraryStop.setArrivalDateFromString = function (dateAsString) {
                  console.assert(dateAsString, 'The date as string must be defined.');
                  console.assert(this.timezoneId, 'The timezone must be known.');
                  var m = moment(dateAsString);
                  m.tz(this.timezoneId);
                  this.setArrivalDate(m);
              }

              itineraryStop.setArrivalDateFromDateAndTime = function (d, time) {
                  console.assert(this.timezoneId, 'The timezone must be known.');
                  console.assert(d, 'Date must be known');
                  console.assert(time, 'Time must be known.');
                  var m = DateTimeService.getDateAndTimeInTimezoneAsMoment(d, time, this.timezoneId);
                  this.setArrivalDate(m);
              }

              itineraryStop.setDepartureDateFromString = function (dateAsString) {
                  console.assert(dateAsString, 'The date as string must be defined.');
                  console.assert(this.timezoneId, 'The timezone must be known.');
                  var m = moment(dateAsString);
                  m.tz(this.timezoneId);
                  this.setDepartureDate(m);
              }

              itineraryStop.setDepartureDateFromDateAndTime = function (d, time) {
                  console.assert(this.timezoneId, 'The timezone must be known.');
                  console.assert(d, 'Date must be known');
                  console.assert(time, 'Time must be known.');
                  var m = DateTimeService.getDateAndTimeInTimezoneAsMoment(d, time, this.timezoneId);
                  this.setDepartureDate(m);
              }

              itineraryStop.setArrivalDate = function (m) {
                  console.assert(m, 'm must be defined.');
                  this.arrivalDate = m.format();

                  this.destinationArrivalMoment = m;
                  this.destinationArrivalDateAsString = m.format(dateFormat);
                  this.destinationArrivalTimeAsString = m.format(timeFormat);

                  this.localArrivalMoment = m.local();
                  this.localArrivalDateAsString = this.localArrivalMoment.format(dateFormat);
                  this.localArrivalTimeAsString = this.localArrivalMoment.format(timeFormat);
              }

              itineraryStop.setDepartureDate = function (m) {
                  console.assert(m, 'm must be defined.');
                  this.departureDate = m.format();

                  this.destinationDepartureMoment = m;
                  this.destinationDepartureDateAsString = m.format(dateFormat);
                  this.destinationDepartureTimeAsString = m.format(timeFormat);

                  this.localDepartureMoment = m.local();
                  this.localDepartureDateAsString = this.localDepartureMoment.format(dateFormat);
                  this.localDepartureTimeAsString = this.localDepartureMoment.format(timeFormat);
              }

              itineraryStop.getEvent = function () {
                  var start = moment(this.arrivalDate);
                  var end = moment(this.departureDate);
                  return {
                      title: this.name,
                      start: start,
                      end: end,
                      allDay: false,
                      itineraryStopId: this.itineraryStopId,
                      color: this.color
                  };
              }
          }
      };
  });
