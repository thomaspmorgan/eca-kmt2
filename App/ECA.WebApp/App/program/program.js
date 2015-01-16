/// <reference path="~/Scripts/jquery-1.10.2.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/Scripts/durandal/app.js" />
/// <reference path="~/Scripts/jquery-1.10.2.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/Scripts/durandal/app.js" />

define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var router = require('plugins/router');

    var childRouter = router
         .createChildRouter()
         .makeRelative({ moduleId: 'program', fromParent: true, dynamicHash: ':id' })
        .map([
            { route: ['overview', ''], moduleId: 'overview', title: 'Overview', nav: true },
            { route: 'branchesprojects', moduleId: 'branchesprojects', title: 'Branches & Projects', nav: true },
            { route: 'activity', moduleId: 'activity', title: 'Activity', nav: true },
            { route: 'artifacts', moduleId: 'artifacts', title: 'Artifacts', nav: true },
            { route: 'impact', moduleId: 'impact', title: 'Impact', nav: true }

        ]).buildNavigationModel();

    function ProgramViewModel() {
        var self = this;

        self.router = childRouter;
        //self.programs = ko.observableArray([]);
        self.programName = ko.observable('');
        self.theProgram;

        self.activate = function (programId) {
            self.view = 'program';

            return ajax.get('programs/' + programId).done(function (program) {
                self.theProgram = program;
                self.programName = ko.observable(program.name);
            }).fail(function (message) {
                app.showMessage(message, "Program");
            });
        };
    }

    return new ProgramViewModel();
    //return {
    //    router: childRouter,
    //    title: ko.observable(),

    //    activate: function (context) {
    //        return ajax.get('programs/' + context).done(function (program) {
    //            title = ko.observable(program.name);
    //        }).fail(function (message) {
    //            app.showMessage(message, "Program");
    //        })
    //    }
    //};

});