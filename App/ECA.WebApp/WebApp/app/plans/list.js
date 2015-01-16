define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var router = require('plugins/router');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function containsPlan(plans, planId) {
        if (!planId) {
            return false;
        }
        for (var i = 0, len = plans.length; i < len; i++) {
            if (plans[i].planId == planId) {
                return true;
            }
        }
        return false;
    }

    function PlanListViewModel() {
        var self = this;

        self.user = globals.user;
        self.userHasMissions = self.user.missionCodes && self.user.missionCodes.length > 0;
        self.fiscalYear = ko.observable(globals.fiscalYear);
        self.showAll = ko.observable(self.user.isGlobalAdministrator && !self.userHasMissions);
        self.workingDrafts = ko.observableArray();
        self.completedPlans = ko.observableArray();
        self.planId = ko.observable(0);
        self.html = ko.observable();
        self.actions = ko.observableArray();
        self.action = ko.observable({ name: '', label: '' });
        self.history = ko.observableArray();
        self.historyEnabled = ko.observable(false);

        self.currentVersion = ko.computed(function () {
            var history = self.history();
            var planId = self.planId();
            for (var i = 0, len = history.length; i < len; i++) {
                if (history[i].planId == planId) {
                    return history[i];
                }
            }
            return null;
        }, self);

        self.loadSummaries = function () {
            self.html('');
            self.actions([]);
            self.history([]);
            self.workingDrafts.removeAll();
            self.completedPlans.removeAll();

            var url;
            if (self.showAll() && self.user.isGlobalAdministrator) {
                url = 'allplans';
            } else {
                url = 'myplans';
            }

            var query;
            if (self.fiscalYear()) {
                query = {
                    fiscalYear: self.fiscalYear()
                };
            } else {
                query = null;
            }

            return ajax.get(url, query).then(function (versions) {
                ko.utils.arrayForEach(versions.workingDrafts, function (version) {
                    self.workingDrafts.push(version);
                });
                ko.utils.arrayForEach(versions.completedPlans, function (version) {
                    self.completedPlans.push(version);
                });
            });
        }

        self.isValidPlanId = function (planId) {
            return planId && (self.planId() == planId ||
                containsPlan(self.completedPlans(), planId) ||
                containsPlan(self.workingDrafts(), planId) ||
                containsPlan(self.history(), planId));
        };

        self.loadPlan = function (planId) {
            self.planId(planId);
            var isValid = self.isValidPlanId(planId);
            self.html('');
            self.actions.removeAll();
            self.history.removeAll();
            if (!isValid) {
                return;
            }
            var p1 = ajax.getHtml('plans/' + planId);
            var p2 = ajax.get('plans/' + planId + '/history');
            var p3 = ajax.get('plans/' + planId + '/actions');
            $.when(p1, p2, p3).done(function (a1, a2, a3) {
                self.html(a1[0]);
                self.history(a2[0]);
                ko.utils.arrayForEach(a3[0], function (action) {
                    var label;
                    var confirm;
                    switch (action.actionName) {
                        case 'Check Out':
                            label = 'Edit';
                            confirm = false;
                            break;
                        case 'Edit':
                            label = 'Edit';
                            confirm = false;
                            break;
                        default:
                            label = action.actionName;
                            confirm = true;
                            break;
                    }
                    self.actions.push({
                        name: action.actionName,
                        label: label,
                        confirm: confirm
                    });
                });
            });
        };

        self.selectPlan = function (plan) {
            self.loadPlan(plan.planId);
        };

        self.selectVersion = function (version) {
            self.loadPlan(version.planId);
        };

        self.selectAction = function (action) {
            self.action(action);
            if (action.name == "Edit") {
                router.navigate('#edit/' + self.planId());
            } else if (action.confirm) {
                $('#perform-action-confirm').modal();
            } else {
                self.performAction();
            }
        };

        self.performAction = function () {
            ajax.post('plans/{0}/actions'.format(self.planId()), {
                actionName: self.action().name
            }).done(function (response) {
                self.planId(response.planId);
                if (self.action().name == 'Check Out') {
                    router.navigate('#edit/' + self.planId());
                } else {
                    self.refresh();
                }
            });
        };

        self.showWorkflowHelp = function () {
            $('#workflow-help').modal();
        };

        self.showVersioningHelp = function () {
            $('#versioning-help').modal();
        };

        self.toggleHistory = function () {
            self.historyEnabled(!self.historyEnabled());
        };

        self.showAll.subscribe(function (flag) {
            self.refresh();
        });

        self.activate = function () {
            self.fiscalYear(globals.fiscalYear);
            self.refresh();
            self.fySubscription = app.on('fiscalYear:change', function (fiscalYear) {
                self.fiscalYear(fiscalYear);
                self.refresh();
            });
        };

        self.deactivate = function () {
            self.fySubscription.off();
        };

        self.refresh = function () {
            self.loadSummaries().done(function () {
                self.loadPlan(self.planId());
            });
        };
    }
    return new PlanListViewModel();
});
