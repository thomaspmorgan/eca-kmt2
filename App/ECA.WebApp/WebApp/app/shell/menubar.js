define(function (require) {

    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var auth = require('pdtracker/auth');
    var globals = require('pdtracker/globals');

    return {
        user: globals.user,
        years: ko.observableArray([]),
        fiscalYear: ko.observable(),

        activate: function () {
            var self = this;
            //return ajax.get('fiscalYears').done(function (years) {
            //    self.years(years);
            //    globals.fiscalYear = self.years.length > 0 ? self.years[0] : undefined;
            //    self.fiscalYear(globals.fiscalYear);
            //    self.fiscalYear.subscribe(function (value) {
            //        globals.fiscalYear = value;
            //        app.trigger('fiscalYear:change', value);
            //    });
            //    return true;
            //});
            globals.fiscalYear = '14';
            return true;
        },

        attached: function () {
            $('#myaccount').click(function () {
                $('#myaccount-dropdown').slideToggle();
                return false;
            });
            $(document).click(function () {
                $('#myaccount-dropdown').slideUp();
            });
        },

        doSignOut: function () {
            auth.signOut();
        }
    };
});
