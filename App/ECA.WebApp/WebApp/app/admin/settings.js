define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');

    function SettingViewModel(setting) {
        var self = this;
        self.key = ko.observable(setting.key);
        self.value = ko.observable(setting.value);
    }

    function SettingListViewModel() {
        var self = this;

        self.settings = ko.observableArray([]);

        self.selectedItem = ko.observable();

        self.value = ko.observable('');

        self.activate = function () {
            self.view = 'settings';

            self.settings.removeAll();
            return ajax.get('settings').done(function (settings) {
                ko.utils.arrayForEach(settings, function (setting) {
                    self.settings.push(new SettingViewModel(setting));
                });
            }).fail(function (message) {
                app.showMessage(message, "Settings");
            });
        };

        self.doEdit = function (item) {
            self.selectedItem(item);
            self.value(item.value());
            $('#setting-edit-dialog').modal();
        };

        self.doEditSubmit = function () {
            return ajax.put('settings/{0}'.format(self.selectedItem().key()), {
                value: self.value()
            }).done(function (setting) {
                self.selectedItem().key(setting.key);
                self.selectedItem().value(setting.value);
            }).fail(function (message) {
                app.showMessage(message, self.selectedItem().key());
            });
        };

    }

    return new SettingListViewModel();
});

