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

    function AudienceViewModel(audience) {
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

    function AudienceListViewModel() {
        var self = this;

        self.audiences = ko.observableArray([]);
        self.years = fiscalYears();
        self.selectedItem = ko.observable(new AudienceViewModel());
        self.modifiedItem = ko.observable(new AudienceViewModel());
        self.addedItem = ko.observable(new AudienceViewModel());

        self.activate = function () {
            self.view = 'audiences';

            self.audiences.removeAll();
            return ajax.get('audiences').done(function (audiences) {
                self.refresh(audiences);
            }).fail(function (message) {
                app.showMessage(message, "Audiences");
            });
        };

        self.refresh = function (audiences) {
            self.audiences.removeAll();
            for (var i = 0; i < audiences.length; i++) {
                self.audiences.push(audiences[i]);
            }
        };

        self.doAdd = function () {
            var item = self.addedItem();
            return ajax.post('audiences', {
                fiscalYear: item.fiscalYear(),
                category: item.category(),
                name: item.name(),
                sortKey: item.sortKey()
            }).done(function (audiences) {
                self.addedItem(new AudienceViewModel()); // clear form
                self.refresh(audiences);
            }).fail(function (message) {
                app.showMessage(message, item.name);
            });
        };

        self.doEdit = function (item) {
            self.selectedItem(new AudienceViewModel(item));
            self.modifiedItem(new AudienceViewModel(item));
            $('#audience-edit-dialog').modal();
        };

        self.doSaveChanges = function () {
            var item = self.modifiedItem();
            return ajax.put('audiences/{0}'.format(item.id), {
                id: item.id,
                fiscalYear: item.fiscalYear(),
                category: item.category(),
                name: item.name(),
                sortKey: item.sortKey()
            }).done(function (audiences) {
                self.refresh(audiences);
            }).fail(function (message) {
                app.showMessage(message, item.name());
            });
        };

        self.doDelete = function (item) {
            self.selectedItem(new AudienceViewModel(item));
            $('#audience-delete-confirm').modal();
        };

        self.doDeleteConfirm = function () {
            ajax.del('audiences/{0}'.format(self.selectedItem().id))
                .done(function (audiences) {
                    self.refresh(audiences);
                }).fail(function (message) {
                    app.showMessage(message, self.selectedItem().name());
                });
        };
    }

    return new AudienceListViewModel();
});
