/// <reference path="~/Scripts/jquery-1.10.2.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/Scripts/durandal/app.js" />

define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var router = require('plugins/router');

    function ProgramViewModel(program) {
        var self = this;
        self.name = ko.observable(program.name);
        self.description = ko.observable(program.description);
        self.programId = ko.observable(program.programId);
        self.url = ko.computed(function () {
            return '#/program/' + self.programId();
        });
    }

    function ProgramListViewModel() {
        var self = this;

     
        self.programs = ko.observableArray([]);

        self.activate = function () {
            self.view = 'programs';

            self.programs.removeAll();
            return ajax.get('programslist').done(function (programs) {
                self.refresh(programs);
            }).fail(function (message) {
                app.showMessage(message, "Programs");
            });
        };

        self.refresh = function (programs) {
            self.programs.removeAll();
            for (var i = 0; i < programs.length; i++) {
                self.programs.push(new ProgramViewModel(programs[i]));
            }
        };
    }

    return new ProgramListViewModel();

});
