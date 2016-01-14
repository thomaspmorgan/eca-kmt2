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

          //Returns a date that may be in another timezone into a date that can be displayed in the
          //browser, this date should then be converted to a date in the destination timezone.
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
          },

          //getDateAsTimezoneMoment: function(d, timezoneId){
          //    var m = moment(d);
          //    m.tz(timezoneId);
          //    return m;
          //},

          //getDateAsLocalMoment: function(d, timezoneId){
          //    return service.getDateAsTimezoneMoment(d, timezoneId).local();
          //},

          //getDateAndTimeAsTimezoneString: function (date, time, timezoneId, momentFormat) {
          //    return service.getMomentInTimezone(date, time, timezoneId).format(momentFormat);
          //}
      };
      return service;
  });