define(function (require) {

    var ko = require('knockout');
    var app = require('durandal/app');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function ReportViewModel() {
        var self = this;

        self.reports = ko.observableArray([]);
        self.exportLink = ko.observable();

        self.groupPlans = function (plans) {
            var report = null;
            ko.utils.arrayForEach(plans, function (plan) {
                if (report === null || report.fiscalYear !== plan.fiscalYear) {
                    if (report !== null) {
                        self.reports.push(report);
                    }
                    report = {
                        fiscalYear: plan.fiscalYear,
                        importedPlans: [],
                        inProgressPlans: [],
                        publishedPlans: []
                    };
                }
                if (plan.currentStatus == 'Imported') {
                    report.importedPlans.push(plan);
                } else if (plan.currentStatus == 'Published') {
                    report.publishedPlans.push(plan);
                } else {
                    report.inProgressPlans.push(plan);
                }
            });
            if (report !== null) {
                self.reports.push(report);
            }
        };

        self.activate = function () {
            self.fySubscription = app.on('fiscalYear:change', function (fiscalYear) {
                self.refresh();
            });
            return self.refresh();
        };

        self.refresh = function () {
            var url;
            if (globals.fiscalYear > 0) {
                url = 'reports/annual/' + globals.fiscalYear;
            } else {
                url = 'reports/annual';
            }
            self.exportLink('../api/' + url + '?format=csv&ticket=' + globals.ticket);
            return ajax.get(url).done(function (plans) {
                self.reports.removeAll();
                self.groupPlans(plans);
            });
        };

        self.deactivate = function () {
            self.fySubscription.off();
        };
    }

    return new ReportViewModel();
});
