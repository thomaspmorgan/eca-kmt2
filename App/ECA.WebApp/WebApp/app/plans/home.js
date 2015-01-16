define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var router = require('plugins/router');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    // Creates a search criteria object from a query parameters object.
    function SearchCriteria(params) {
        if (params) {
            this.textQuery = params.q;
            this.countryCode = params.c;
            this.audienceIds = params.a; // csv
            this.themeIds = params.t; // csv
            this.specified = true;
        } else {
            this.specified = false;
        }
    }

    SearchCriteria.prototype.toString = function () {
        var query = [];
        if (this.textQuery) query.push('q=' + this.textQuery);
        if (this.countryCode) query.push('c=' + this.countryCode);
        if (this.audienceIds) query.push('a=' + this.audienceIds);
        if (this.themeIds) query.push('t=' + this.themeIds);
        if (globals.fiscalYear) query.push('y=' + globals.fiscalYear);
        return query.length > 0 ? '?' + query.join('&') : '';
    };

    function PlanListViewModel() {
        var self = this;

        self.loadingMessage = ko.observable('');
        self.publishedPlans = ko.observableArray();
        self.selectedPlan = ko.observable();
        self.html = ko.observable();
        self.planLoaded = ko.observable(false);
        self.fiscalYear = ko.observable(globals.fiscalYear);
        self.rssFeedLink = ajax.apiUrl('rss?ticket=' + globals.ticket);

        self.viewLink = ko.computed(function () {
            var plan = self.selectedPlan();
            if (plan) {
                var url = ajax.apiUrl('plans/formatted/{planId}'.format(plan));
                return url + '?ticket=' + globals.ticket;
            } else {
                return '';
            }
        }, self);

        self.loadSummaries = function () {
            self.publishedPlans.removeAll();
            return ajax.get('plans/published' + self.criteria).done(function (plans) {
                for (var i = 0, n = plans.length; i < n; i++) {
                    var plan = plans[i];
                    self.publishedPlans.push(plan);
                }
            });
        };

        self.loadPlan = function () {
            var planId = self.selectedPlan().planId;
            self.planLoaded(false);
            self.html('Loading plan...');
            ajax.getHtml('plans/published/' + planId + self.criteria)
                .done(function (html) {
                    self.html(html);
                    // Expand plan if criteria contains items that
                    // can be found in the plan.
                    var c = self.criteria;
                    if (c && (c.textQuery || c.audienceIds || c.themeIds)) {
                        $('.section-hidden').show();
                        $('.section-open').hide();
                        $('.section-close').show();
                    }
                    self.planLoaded(true);
                })
                .fail(function () {
                    self.html('Error loading plan');
                });
        };

        self.selectPlan = function (plan) {
            self.selectedPlan(plan);
            self.loadPlan();
        };

        self.analysisResults = ko.observableArray();

        self.analyzePlan = function () {
            var planId = self.selectedPlan().planId;
            ajax.get('analysis/' + planId).done(function (results) {
                self.analysisResults(results);
                $('#context-analysis-dialog').modal();
            });
        };

        self.refresh = function () {
            self.planLoaded(false);
            self.html('');
            self.loadingMessage('Loading plan summaries...');
            self.loadSummaries().done(function () {
                self.loadingMessage('');
            });
        };

        self.activate = function (params) {
            self.criteria = new SearchCriteria(params);
            self.refresh();
            self.fySubscription = app.on('fiscalYear:change', function (fiscalYear) {
                self.fiscalYear(fiscalYear);
                self.refresh();
            });
        };

        self.deactivate = function () {
            self.fySubscription.off();
        };
    }

    return new PlanListViewModel();
});
