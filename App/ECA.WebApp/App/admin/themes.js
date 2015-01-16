define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');

    function fiscalYears(n) {
        n = n || 5;
        var years = [];
        var current = new Date().getFullYear();
        for (var i = 0; i < 5; i++) {
            years.push(current + i);
        }
        return years;
    }

    function ThemeViewModel(audience) {
        var self = this;
        self.id = 0;
        self.fiscalYear = ko.observable(2000);
        self.category = ko.observable('');
        self.name = ko.observable('');
        self.sortKey = ko.observable(9999);
        self.updateFrom = function (audience) {
            self.id = audience.id;
            self.fiscalYear(audience.fiscalYear);
            self.category(audience.category);
            self.name(audience.name);
            self.sortKey(audience.sortKey);
        };
        if (audience) {
            self.updateFrom(audience);
        }
    }

    function ThemeListViewModel() {
        var self = this;

        self.themes = ko.observableArray([]);
        self.years = fiscalYears();
        self.selectedItem = ko.observable(new ThemeViewModel());
        self.modifiedItem = ko.observable(new ThemeViewModel());
        self.addedItem = ko.observable(new ThemeViewModel());

        self.activate = function () {
            self.view = 'themes';

            return ajax.get('themes').done(function (themes) {
                self.refresh(themes);
            }).fail(function (message) {
                app.showMessage(message, "Themes");
            });
        };

        self.refresh = function (themes) {
            self.themes.removeAll();
            for (var i = 0; i < themes.length; i++) {
                self.themes.push(themes[i]);
            }
        };

        self.doAdd = function () {
            var item = self.addedItem();
            return ajax.post('themes', {
                fiscalYear: item.fiscalYear(),
                category: item.category(),
                name: item.name(),
                sortKey: item.sortKey()
            }).done(function (themes) {
                self.addedItem(new ThemeViewModel()); // clear form
                self.refresh(themes);
            }).fail(function (message) {
                app.showMessage(message, item.name);
            });
        };

        self.doEdit = function (item) {
            self.selectedItem(new ThemeViewModel(item));
            self.modifiedItem(new ThemeViewModel(item));
            $('#theme-edit-dialog').modal();
        };

        self.doSaveChanges = function () {
            var item = self.modifiedItem();
            return ajax.put('themes/{0}'.format(item.id), {
                id: item.id,
                fiscalYear: item.fiscalYear(),
                category: item.category(),
                name: item.name(),
                sortKey: item.sortKey()
            }).done(function (themes) {
                self.refresh(themes);
            }).fail(function (message) {
                app.showMessage(message, item.name());
            });
        };

        self.doDelete = function (item) {
            self.selectedItem(new ThemeViewModel(item));
            $('#theme-delete-confirm').modal();
        };

        self.doDeleteConfirm = function () {
            ajax.del('themes/{0}'.format(self.selectedItem().id))
                .done(function (themes) {
                    self.refresh(themes);
                }).fail(function (message) {
                    app.showMessage(message, self.selectedItem().name());
                });
        };
    }

    return new ThemeListViewModel();
});
