define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function CountryViewModel(country) {
        var self = this;
        self.countryCode = ko.observable(country.countryCode);
        self.countryName = ko.observable(country.countryName);
        self.regionCode = ko.observable(country.regionCode);
        self.regionName = ko.observable(country.regionName);
        self.isActive = ko.observable(country.isActive);
        self.isActiveText = ko.computed(function () {
            return self.isActive() ? 'Yes' : 'No';
        }, self);
    }

    function CountryListViewModel() {
        var self = this;

        self.countries = ko.observableArray([]);
        self.regions = ko.observableArray([]);

        self.newCountryCode = ko.observable('');
        self.newCountryName = ko.observable('');
        self.newRegionCode = ko.observable('');

        self.selectedItem = ko.observable();

        self.countryCode = ko.observable('');
        self.countryName = ko.observable('');
        self.regionCode = ko.observable('');
        self.isActive = ko.observable(false);

        self.activate = function () {
            self.view = 'countries';
            self.user = globals.user;

            self.countries.removeAll();
            self.regions.removeAll();
            var c = ajax.get('countries');
            var r = ajax.get('regions', {
                active: true
            });
            return $.when(c, r).done(function (c, r) {
                ko.utils.arrayForEach(c[0], function (country) {
                    self.countries.push(new CountryViewModel(country));
                });
                ko.utils.arrayForEach(r[0], function (region) {
                    self.regions.push(region);
                });
            }).fail(function (message) {
                app.showMessage(message, "Countries");
            });
        };

        self.doAdd = function () {
            return ajax.post('countries', {
                countryCode: self.newCountryCode().toUpperCase(),
                countryName: self.newCountryName(),
                regionCode: self.newRegionCode(),
                isActive: true
            }).done(function (country) {
                self.countries.unshift(new CountryViewModel(country));
                self.newCountryCode('');
                self.newCountryName('');
                self.newRegionCode('');
            }).fail(function (message) {
                app.showMessage(message, self.newCountryName());
            });
        };

        self.doEdit = function (item) {
            self.selectedItem(item);
            self.countryCode(item.countryCode());
            self.countryName(item.countryName());
            self.regionCode(item.regionCode());
            self.isActive(item.isActive());
            $('#country-edit-dialog').modal();
        };

        self.doEditSubmit = function () {
            return ajax.put('countries/{0}'.format(self.selectedItem().countryCode()), {
                countryName: self.countryName(),
                regionCode: self.regionCode(),
                isActive: self.isActive()
            }).done(function (country) {
                self.selectedItem().countryName(country.countryName);
                self.selectedItem().regionName(country.regionName);
                self.selectedItem().isActive(country.isActive);
            }).fail(function (message) {
                app.showMessage(message, self.countryName());
            });
        };

        self.doDelete = function (item) {
            self.selectedItem(item);
            $('#country-delete-confirm').modal();
        };

        self.doDeleteConfirm = function () {
            ajax.del('countries/{0}'.format(self.selectedItem().countryCode())).done(function () {
                self.countries.remove(self.selectedItem());
            }).fail(function (message) {
                app.showMessage(message, self.selectedItem().countryName());
            });
        };
    }

    return new CountryListViewModel();
});

