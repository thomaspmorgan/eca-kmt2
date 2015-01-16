define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var router = require('plugins/router');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function ViewModel() {
        var self = this;

        self.from = ko.observable();
        self.body = ko.observable();
        self.confirmation = ko.observable();

        self.activate = function () {
            self.from(globals.user.alternateEmail);
            self.body('');
            self.confirmation('');
        };

        self.sendFeedback = function () {
            ajax.post('feedback', {
                from: self.from(),
                subject: 'PD Tracker Feedback',
                body: self.body()
            }).done(function () {
                self.confirmation('Success. Email Sent. Thank you for your feedback.')
            }).fail(function (message) {
                self.confirmation(message);
            });
        };

        self.doDone = function () {
            router.navigateBack();
        };
    }

    return new ViewModel();
});