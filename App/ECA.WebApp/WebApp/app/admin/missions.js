define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function MissionViewModel(mission) {
        var self = this;
        self.missionId = mission.missionId;
        self.missionCode = ko.observable(mission.missionCode);
        self.missionName = ko.observable(mission.missionName);
        self.countryCode = ko.observable(mission.countryCode);
        self.countryName = ko.observable(mission.countryName);
        self.regionCode = ko.observable(mission.regionCode);
        self.regionName = ko.observable(mission.regionName);
        self.isActive = ko.observable(mission.isActive);
        self.isActiveText = ko.computed(function () {
            return self.isActive() ? 'Yes' : 'No';
        }, self);
    }

    function MissionListViewModel() {
        var self = this;

        self.missions = ko.observableArray([]);
        self.countries = ko.observableArray([]);
        self.regions = ko.observableArray([]);

        self.newMissionCode = ko.observable('');
        self.newMissionName = ko.observable('');
        self.newCountryCode = ko.observable('');
        self.newRegionCode = ko.observable('');

        self.selectedItem = ko.observable();

        self.missionCode = ko.observable('');
        self.missionName = ko.observable('');
        self.countryCode = ko.observable('');
        self.regionCode = ko.observable('');
        self.isActive = ko.observable(false);

        self.importSuccess = ko.observable('');
        self.importError = ko.observable('');

        self.activate = function () {
            self.view = 'missions';
            self.user = globals.user;

            self.missions.removeAll();
            self.countries.removeAll();
            self.regions.removeAll();
            var m = ajax.get('missions');
            var c = ajax.get('countries', {
                active: true
            });
            var r = ajax.get('regions', {
                active: true
            });
            return $.when(m, c, r).done(function (m, c, r) {
                ko.utils.arrayForEach(m[0], function (mission) {
                    self.missions.push(new MissionViewModel(mission));
                });
                ko.utils.arrayForEach(c[0], function (country) {
                    self.countries.push(country);
                });
                ko.utils.arrayForEach(r[0], function (region) {
                    self.regions.push(region);
                });
            }).fail(function (message) {
                app.showMessage(message, "Missions");
            });
        };

        self.doAdd = function () {
            $('#add-mission-dialog').modal();
        };

        self.doAddSubmit = function () {
            self.newMissionCode(self.newMissionCode().trim().toUpperCase());
            return ajax.post('missions', {
                missionCode: self.newMissionCode(),
                missionName: self.newMissionName(),
                countryCode: self.newCountryCode(),
                regionCode: self.newRegionCode(),
                isActive: true
            }).done(function (mission) {
                self.missions.unshift(new MissionViewModel(mission));
                self.newMissionCode('');
                self.newMissionName('');
                self.newCountryCode('');
                self.newRegionCode('');
            }).fail(function (message) {
                app.showMessage(message, self.newMissionCode());
            });
        };

        self.doImport = function () {
            self.importSuccess('');
            self.importError('');
            $('#import-missions-dialog').modal();
        };

        self.doImportSubmit = function () {
            $('#wrap').mask('Importing Missions...');
            $("#import-missions-form").ajaxSubmit({
                method: 'POST',
                url: '../api/missions/import',
                dataType: 'text',
                contentType: 'application/csv',
                beforeSend: function () {
                    self.importSuccess('');
                    self.importError('');
                },
                complete: function (response) {
                    $('#wrap').unmask();
                    var text = response.responseText;
                    if (text.indexOf('ERROR: ') == 0) {
                        self.importSuccess('');
                        self.importError(text);
                    } else {
                        self.importSuccess(text);
                        self.importError('');
                        self.missions.removeAll();
                        ajax.get('missions').done(function (missions) {
                            ko.utils.arrayForEach(missions, function (mission) {
                                self.missions.push(new MissionViewModel(mission));
                            });
                        });
                    }
                },
                error: function () {
                    $('#wrap').unmask();
                    self.importSuccess('');
                    self.importError("Error importing file.");
                }
            });
        };

        self.doEdit = function (item) {
            self.selectedItem(item);
            self.missionCode(item.missionCode());
            self.missionName(item.missionName());
            self.countryCode(item.countryCode());
            self.regionCode(item.regionCode());
            self.isActive(item.isActive());
            $('#edit-mission-dialog').modal();
        };

        self.doEditSubmit = function () {
            self.missionCode(self.missionCode().trim().toUpperCase());
            return ajax.put('missions/{0}'.format(self.selectedItem().missionId), {
                missionCode: self.missionCode(),
                missionName: self.missionName(),
                countryCode: self.countryCode(),
                regionCode: self.regionCode(),
                isActive: self.isActive()
            }).done(function (mission) {
                self.selectedItem().missionCode(mission.missionCode);
                self.selectedItem().missionName(mission.missionName);
                self.selectedItem().countryName(mission.countryName);
                self.selectedItem().regionName(mission.regionName);
                self.selectedItem().isActive(mission.isActive);
            }).fail(function (message) {
                app.showMessage(message, self.missionCode());
            });
        };

        self.doDelete = function (item) {
            self.selectedItem(item);
            $('#delete-mission-dialog').modal();
        };

        self.doDeleteConfirm = function () {
            ajax.del('missions/{0}'.format(self.selectedItem().missionId)).done(function () {
                self.missions.remove(self.selectedItem());
            }).fail(function (message) {
                app.showMessage(message, self.selectedItem().missionCode());
            });
        };
    }

    return new MissionListViewModel();
});
