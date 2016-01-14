'use strict';

/**
 * @ngdoc service
 * @name staticApp.DateTimeService
 * @description
 * # DateTimeService
 * Factory in the staticApp.
 */
angular.module('staticApp')
  .factory('DateTimeService', function (DragonBreath, $q, $log, ConstantsService, FilterService) {

      var service = {
          momentDisplayFormat: "MMMM D, YYYY h:mm a",

          getDateAsLocalDisplayMoment: function(d){
              var m = moment(d);
              return moment([m.year(), m.month(), m.date(), m.hours(), m.minutes(), m.seconds()]);
          },

          getDateAndTimeInTimezoneAsMoment: function (date, time, timezoneId) {
              var dateMoment = moment(date);
              dateMoment.tz(timezoneId);
              dateMoment = dateMoment.startOf('day');

              var timeMoment = moment(time);              
              dateMoment.hours(timeMoment.hours());
              dateMoment.minutes(timeMoment.minutes());
              dateMoment.seconds(timeMoment.seconds());
              dateMoment.milliseconds(0);
              return dateMoment;
          }
      };
      return service;
  });