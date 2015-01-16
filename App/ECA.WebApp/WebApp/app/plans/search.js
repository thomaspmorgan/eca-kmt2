define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var router = require('plugins/router');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function SearchViewModel() {
        var self = this;

        // Search criteria
        self.textQuery = ko.observable();
        self.countryCode = ko.observable();
        self.audienceId = ko.observable();
        self.themeId = ko.observable();

        // Available values
        self.countries = ko.observableArray([]);
        self.themes = ko.observableArray([]);
        self.audiences = ko.observableArray([]);
        self.audiencesFilteredByYear = ko.observableArray();
        self.themesFilteredByYear = ko.observableArray();

        self.filterByYear = function (fiscalYear) {
            self.audiencesFilteredByYear.removeAll();
            if (self.audiences().length > 0) {
                ko.utils.arrayForEach(self.audiences(), function (audience) {
                    if (!fiscalYear || audience.fiscalYear == fiscalYear) {
                        self.audiencesFilteredByYear.push(audience);
                    }
                });
            }
            self.themesFilteredByYear.removeAll();
            if (self.themes().length > 0) {
                ko.utils.arrayForEach(self.themes(), function (theme) {
                    if (!fiscalYear || theme.fiscalYear == fiscalYear) {
                        self.themesFilteredByYear.push(theme);
                    }
                });
            }
        };

        self.loadData = function () {
            var c = ajax.get('countries').done(function (countries) {
                self.countries(countries);
            });
            var a = ajax.get('audiences').done(function (audiences) {
                ko.utils.arrayForEach(audiences, function (fiscalYear) {
                    ko.utils.arrayForEach(fiscalYear.categories, function (category) {
                        self.audiences.push(category);
                    });
                });
            });
            var t = ajax.get('themes').done(function (themes) {
                ko.utils.arrayForEach(themes, function (fiscalYear) {
                    ko.utils.arrayForEach(fiscalYear.categories, function (category) {
                        self.themes.push(category);
                    });
                });
            });
            return $.when(c, a, t);
        };

        self.activate = function () {
            self.fySubscription = app.on('fiscalYear:change', function (fiscalYear) {
                self.filterByYear(fiscalYear);
            });
            if (self.dataLoaded) {
                self.savedAudienceId = self.audienceId();
                self.savedThemeId = self.themeId();
                self.filterByYear(globals.fiscalYear);
                return true;
            }
            return self.loadData().then(function () {
                self.dataLoaded = true;
                self.filterByYear(globals.fiscalYear);
            })
        };

        self.attached = function () {
            self.audienceId(self.savedAudienceId);
            self.themeId(self.savedThemeId);
        };

        self.deactivate = function () {
            self.fySubscription.off();
        };

        self.doClear = function () {
            self.textQuery(null);
            self.countryCode(null);
            self.audienceId(null);
            self.themeId(null);
            self.doSearch();
        };

        self.doSearch = function () {
            var criteria = {};
            if (self.textQuery()) {
                criteria.q = self.textQuery().trim();
            }
            if (self.countryCode()) {
                criteria.c = self.countryCode();
            }
            if (self.audienceId()) {
                criteria.a = [parseInt(self.audienceId())];
            }
            if (self.themeId()) {
                criteria.t = [parseInt(self.themeId())];
            }
            var url = '#';
            var param = $.param(criteria, true);
            if (param) {
                url = url + '?' + param;
            }
            router.navigate(url);
        };
    }

    return new SearchViewModel();
});
