define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function UserViewModel(user) {
        var self = this;
        self.userId = user.userId;
        self.userName = user.userName;
        self.displayName = user.displayName;
        self.missionCodes = ko.observable(user.missionCodes.join(', '));
        self.groupName = ko.observable(user.groupName);
        self.jobTitle = ko.observable(user.jobTitle);
        self.alternateEmail = ko.observable(user.alternateEmail);
    }

    function ViewModel() {
        var self = this;

        self.user = globals.user;

        self.users = ko.observableArray([]);
        self.groups = ko.observableArray([]);
        self.loaded = ko.observable(false);
        self.selectedItem = ko.observable();

        self.newUserName = ko.observable('');
        self.newDisplayName = ko.observable('');
        self.newMissionCodes = ko.observable('');
        self.newGroupName = ko.observable('');
        self.newJobTitle = ko.observable('');
        self.newAlternateEmail = ko.observable('');

        self.importSuccess = ko.observable('');
        self.importError = ko.observable('');

        self.doAdd = function () {
            $('#add-user-dialog').modal();
        };

        self.doAddSubmit = function () {
            return ajax.post('users', {
                userName: self.newUserName(),
                displayName: self.newDisplayName(),
                groupName: self.newGroupName(),
                missionCodes: self.newMissionCodes(),
                jobTitle: self.newJobTitle(),
                alternateEmail: self.newAlternateEmail()
            }).done(function (user) {
                app.showMessage('User Added');
            });
        };

        self.doImport = function () {
            self.importSuccess('');
            self.importError('');
            $('#import-users-dialog').modal();
        };

        self.doImportSubmit = function () {
            $('#wrap').mask('Importing Users...');
            $("#import-users-form").ajaxSubmit({
                method: 'POST',
                url: '../api/users/bulk',
                dataType: 'text',
                contentType: 'application/json',
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
                    }
                },
                error: function () {
                    $('#wrap').unmask();
                    self.importError("Error importing file.");
                }
            });
        };

        self.doEdit = function (item) {
            self.selectedItem(item);
            $('#edit-user-dialog').modal();
        };

        self.doEditSubmit = function () {
            var user = self.selectedItem();
            ajax.put('users/{0}'.format(user.userId), {
                groupName: user.groupName(),
                missionCodes: user.missionCodes(),
                jobTitle: user.jobTitle(),
                alternateEmail: user.alternateEmail()
            }).done(function () {
                app.showMessage('Updated user information.', user.displayName);
            });
        };

        self.activate = function () {
            self.view = 'userRoles';
            self.user = globals.user;
            self.users.removeAll();
            self.groups.removeAll();
            self.loaded(false);

            ajax.get('users').done(function (users) {
                ko.utils.arrayForEach(users, function (user) {
                    self.users.push(new UserViewModel(user));
                });
                self.users.sort(function (a, b) {
                    if (a.displayName < b.displayName) return -1;
                    if (a.displayName > b.displayName) return 1;
                    return 0
                });
                self.loaded(true);
            });

            ajax.get('groups').done(function (groups) {
                ko.utils.arrayForEach(groups, function (group) {
                    self.groups.push(group);
                });
            });
        };
    };

    return new ViewModel();
});