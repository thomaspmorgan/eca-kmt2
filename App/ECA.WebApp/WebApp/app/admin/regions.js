define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function RegionViewModel(region) {
        var self = this;
        self.regionCode = ko.observable(region.regionCode);
        self.regionName = ko.observable(region.regionName);
        self.isActive = ko.observable(region.isActive);
        self.isActiveText = ko.computed(function () {
            return self.isActive() ? 'Yes' : 'No';
        }, self);
    }

    function RegionListViewModel() {
        var self = this;

        self.regions = ko.observableArray([]);

        self.newRegionCode = ko.observable('');
        self.newRegionName = ko.observable('');

        self.selectedItem = ko.observable();

        self.regionCode = ko.observable('');
        self.regionName = ko.observable('');
        self.isActive = ko.observable(false);

        self.activate = function () {
            self.view = 'regions';
            self.user = globals.user;

            self.regions.removeAll();
            return ajax.get('regions').done(function (regions) {
                ko.utils.arrayForEach(regions, function (region) {
                    self.regions.push(new RegionViewModel(region));
                });
            }).fail(function (message) {
                app.showMessage(message, 'Regions');
            });
        };

        self.doAdd = function () {
            return ajax.post('regions', {
                regionCode: self.newRegionCode().toUpperCase(),
                regionName: self.newRegionName(),
                isActive: true
            }).done(function (region) {
                self.regions.unshift(new RegionViewModel(region));
                self.newRegionCode('');
                self.newRegionName('');
            }).fail(function (message) {
                app.showMessage(message, self.newRegionName());
            });
        };

        self.doEdit = function (item) {
            self.selectedItem(item);
            self.regionCode(item.regionCode());
            self.regionName(item.regionName());
            self.isActive(item.isActive());
            $('#region-edit-dialog').modal();
        };

        self.doEditSubmit = function () {
            self.regionCode(self.regionCode().toUpperCase());
            return ajax.put('regions/{0}'.format(self.selectedItem().regionCode()), {
                regionName: self.regionName(),
                isActive: self.isActive()
            }).done(function () {
                self.selectedItem().regionName(self.regionName());
                self.selectedItem().isActive(self.isActive());
            }).fail(function (message) {
                app.showMessage(message, self.regionName());
            });
        };

        self.doDelete = function (item) {
            self.selectedItem(item);
            $('#region-delete-confirm').modal();
        };

        self.doDeleteConfirm = function () {
            ajax.del('regions/{0}'.format(self.selectedItem().regionCode())).done(function () {
                self.regions.remove(self.selectedItem());
            }).fail(function (message) {
                app.showMessage(message, self.selectedItem().regionName());
            });
        };
    }

    return new RegionListViewModel();
});
