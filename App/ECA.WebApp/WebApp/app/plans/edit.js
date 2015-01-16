define(function (require) {

    var $ = require('jquery');
    var ko = require('knockout');
    var app = require('durandal/app');
    var router = require('plugins/router');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    function compareCodes(a, b) {
        if (a.code < b.code) {
            return -1;
        }
        if (a.code > b.code) {
            return 1;
        }
        return 0;
    }

    function categoriesFromDatabase(categories) {
        categories = categories || [];
        var results = [];
        for (var i = 0; i < categories.length; i++) {
            var category = {
                name: categories[i].category,
                items: []
            };
            for (var j = 0; j < categories[i].items.length; j++) {
                category.items.push({
                    value: categories[i].items[j].name,
                    selected: ko.observable(false)
                });
            }
            results.push(category);
        }
        return results;
    }

    function categoriesFromPlan(categories) {
        categories = categories || [];
        var results = [];
        for (var i = 0; i < categories.length; i++) {
            var category = {
                name: categories[i].name,
                items: []
            };
            for (var j = 0; j < categories[i].values.length; j++) {
                category.items.push({
                    value: categories[i].values[j],
                    selected: ko.observable(true)
                });
            }
            results.push(category);
        }
        return results;
    }

    // Given two arrays of items, return an array of items such that it
    // contains all items in b and those items in a that are not in b.
    function mergeItems(a, b) {
        var items = [];
        for (var i = 0; i < b.length; i++) {
            items.push(b[i]);
        }
        for (var i = 0; i < a.length; i++) {
            var found = false;
            for (var j = 0; j < b.length; j++) {
                if (a[i].value == b[j].value) {
                    found = true;
                }
            }
            if (!found) {
                items.push(a[i]);
            }
        }
        return items;
    }

    function mergeCategories(fromDatabase, fromPlan) {
        var target = categoriesFromDatabase(fromDatabase);
        var source = categoriesFromPlan(fromPlan);
        for (var i = 0; i < source.length; i++) {
            var pc = source[i];
            var found = false;
            for (var j = 0; j < target.length; j++) {
                var dc = target[j];
                if (pc.name == dc.name) {
                    dc.items = mergeItems(dc.items, pc.items);
                    found = true;
                    break;
                }
            }
            if (!found) {
                target.push(pc);
            }
        }
        for (var i = 0; i < target.length; i++) {
            var category = target[i];
            category.hasSelections = ko.computed(function () {
                for (var j = 0; j < this.items.length; j++) {
                    if (this.items[j].selected()) {
                        return true;
                    }
                }
                return false;
            }, category);
        }
        return target;
    }

    function ViewModel() {

        var self = this;

        self.user = globals.user;
        self.autoSaveNotification = ko.observable('');

        self.autoSaveNotification.subscribe(function (message) {
            if (message) {
                setTimeout(function () {
                    self.autoSaveNotification('');
                }, 3000);
            }
        });

        self.importPlan = function (p) {
            self.plan = {
                planId: p.planId,
                fiscalYear: p.fiscalYear,
                missionCode: p.missionCode,
                missionName: p.missionName,
                countryName: p.countryName,
                regionName: p.regionName,
                majorVersion: p.majorVersion,
                minorVersion: p.minorVersion,
                currentStatus: p.currentStatus,
                updatedBy: p.updatedBy,
                updatedAt: p.updatedAt,
                internalNote: ko.observable(p.internalNote),
                goals: ko.observableArray()
            };
            ko.utils.arrayForEach(p.goals || [], function (g) {
                var goal = {
                    text: ko.observable(g.text),
                    objectives: ko.observableArray()
                };
                ko.utils.arrayForEach(g.objectives || [], function (o) {
                    var objective = {
                        text: ko.observable(o.text),
                        program: ko.observable({ text: ko.observable() }),
                        result: ko.observable({ text: ko.observable() }),
                        subobjectives: ko.observableArray(),
                        activities: ko.observableArray(),
                        audienceCategories: ko.observableArray(),
                        themeCategories: ko.observableArray()
                    };
                    if (o.program) {
                        objective.program().text(o.program.text);
                    }
                    if (o.result) {
                        objective.result().text(o.result.text);
                    }
                    ko.utils.arrayForEach(o.subobjectives || [], function (s) {
                        self.addSubobjective(objective, s);
                    });
                    ko.utils.arrayForEach(o.activities || [], function (a) {
                        var activity = {
                            text: ko.observable(a.text),
                            fiscalQuarters: ko.observableArray(ko.utils.arrayMap(a.fiscalQuarters, function (q) {
                                return q.toString();
                            })),
                            internalCollaborators: ko.observableArray(),
                            externalCollaborator: ko.observable(a.externalCollaborator)
                        };
                        ko.utils.arrayForEach(a.internalCollaborators || [], function (ic) {
                            for (var i = 0, arr = self.internalCollaborators, len = arr.length; i < len; i++) {
                                if (ic.code == arr[i].code) {
                                    activity.internalCollaborators.push(arr[i]);
                                    break;
                                }
                            }
                        });
                        activity.internalCollaborators.subscribe(function (collaborators) {
                            collaborators.sort(compareCodes);
                        });
                        objective.activities.push(activity);
                    });
                    objective.audienceCategories(mergeCategories(self.audiences, o.audienceCategories));
                    objective.themeCategories(mergeCategories(self.themes, o.themeCategories));
                    goal.objectives.push(objective);
                });
                self.plan.goals.push(goal);
            });
        };

        self.addGoal = function (plan) {
            var goal = {
                text: ko.observable('New goal (click to edit)'),
                objectives: ko.observableArray()
            };
            plan.goals.push(goal);
        };

        self.addObjective = function (goal) {
            var objective = {
                text: ko.observable('New objective (click to edit)'),
                program: ko.observable({ text: ko.observable() }),
                result: ko.observable({ text: ko.observable() }),
                subobjectives: ko.observableArray(),
                activities: ko.observableArray(),
                audienceCategories: ko.observableArray(),
                themeCategories: ko.observableArray()
            };
            objective.audienceCategories(mergeCategories(self.audiences));
            objective.themeCategories(mergeCategories(self.themes));
            goal.objectives.push(objective);
        };

        self.addSubobjective = function (objective, s) {
            s = s.type ? null : s; // type is non-null when clicking the add button
            s = s || {
                text: ''
            };
            var subobjective = {
                text: ko.observable(s.text)
            };
            objective.subobjectives.push(subobjective);
        };

        self.deleteGoal = function (plan, goal) {
            if (confirm('Are you sure you want to delete this goal?')) {
                plan.goals.remove(goal);
            }
        };

        self.deleteObjective = function (goal, objective) {
            if (confirm('Are you sure you want to delete this objective?')) {
                goal.objectives.remove(objective);
            }
        };

        self.deleteSubobjective = function (objective, subobjective) {
            objective.subobjectives.remove(subobjective);
        };

        self.addActivity = function (objective) {
            var activity = {
                text: ko.observable(''),
                fiscalQuarters: ko.observableArray(),
                internalCollaborators: ko.observableArray([]),
                externalCollaborator: ko.observable('')
            };
            activity.internalCollaborators.subscribe(function (collaborators) {
                collaborators.sort(compareCodes);
            });
            objective.activities.push(activity);
        };

        self.deleteActivity = function (objective, activity) {
            objective.activities.remove(activity);
        };

        self.activate = function (planId) {
            self.planId = planId;
            return ajax.get('plans/' + planId).then(function (plan) {
                var a = ajax.get('audiences?fiscalYear=' + plan.fiscalYear);
                var t = ajax.get('themes?fiscalYear=' + plan.fiscalYear);
                var c = ajax.get('collaborators/internal');
                return $.when(a, t, c).done(function (a, t, c) {
                    var audiences = a[0],
                        themes = t[0],
                        internalCollaborators = c[0];
                    self.audiences = audiences[0] ? audiences[0].categories : []; // only one year
                    self.themes = themes[0] ? themes[0].categories : []; // only one year
                    self.internalCollaborators = [];
                    ko.utils.arrayForEach(internalCollaborators, function (ic) {
                        self.internalCollaborators.push({
                            code: ic.collaboratorCode,
                            name: ic.collaboratorName
                        });
                    });

                    self.importPlan(plan);
                    self.savedJson = self.getCurrentJson();
                    self.autoSaveTimerSuspended = false;

                    self.autoSaveTimer = setInterval(function () {
                        if (!self.autoSaveTimerSuspended) {
                            var currentJson = self.getCurrentJson();
                            if (self.savedJson != currentJson) {
                                self.savePlan(currentJson).done(function () {
                                    self.autoSaveNotification('Plan has been automatically saved.');
                                });
                            }
                        }
                    }, 2 * 60 * 1000); // two minutes
                });
            });
        };

        /**
         * Given a the current plan consisting of properties and
         * KO observables, returns a JSON representation
         * suitable for sending to the API as a PUT request.
         *
         * The purpose of this function is taking the checkbox
         * arrays for audiences and themes and, based on the
         * selected flag, add the items as values to the
         * array in the returned object.
         */
        self.getCurrentJson = function () {
            var plan = ko.toJS(self.plan);
            ko.utils.arrayForEach(plan.goals, function (goal) {
                ko.utils.arrayForEach(goal.objectives, function (objective) {
                    ko.utils.arrayForEach(objective.audienceCategories, function (category) {
                        category.values = [];
                        ko.utils.arrayForEach(category.items, function (item) {
                            if (item.selected) {
                                category.values.push(item.value);
                            }
                        });
                        delete category.hasSelections;
                        delete category.items;
                    });
                    ko.utils.arrayForEach(objective.themeCategories, function (category) {
                        category.values = [];
                        ko.utils.arrayForEach(category.items, function (item) {
                            if (item.selected) {
                                category.values.push(item.value);
                            }
                        });
                        delete category.hasSelections;
                        delete category.items;
                    });
                });
            });
            return ko.toJSON(plan);
        };

        self.handlersAdded = false;

        self.attached = function () {

            $('.plan-edit-objective').children('.plan-edit-section').hide();

            if (!self.handlersAdded) {
                $(document).on('click', '.plan-edit-objective-heading', function (ev) {
                    var parent = $(this).closest('.plan-edit-objective');
                    parent.children('.plan-edit-section').toggle();
                });

                function isEditInplaceVisible(target) {
                    while (!target.is('body') && target.length > 0) {
                        if (target.hasClass('plan-edit-inplace')) {
                            break;
                        }
                        if (target.hasClass('plan-edit-inplace-visible')) {
                            return true;
                        }
                        target = target.parent();
                    }
                    return false;
                }

                function isEditInplaceInput(target) {
                    while (!target.is('body') && target.length > 0) {
                        if (target.hasClass('plan-edit-inplace')) {
                            break;
                        }
                        if (target.hasClass('plan-edit-inplace-input')) {
                            return true;
                        }
                        target = target.parent();
                    }
                    return false;
                }

                $('body').click(function (ev) {
                    var target = $(ev.target);
                    if (isEditInplaceVisible(target)) {
                        // Restore
                        $('.plan-edit-inplace-hidden').hide();
                        $('.plan-edit-inplace-visible').show();
                        var parent = target.closest('.plan-edit-inplace');
                        parent.find('.plan-edit-inplace-hidden').show();
                        parent.find('textarea.plan-edit-inplace-input').focus();
                        parent.find('.plan-edit-inplace-visible').hide();
                    } else if (target.parent().length && !isEditInplaceInput(target)) {
                        $('.plan-edit-inplace-hidden').hide();
                        $('.plan-edit-inplace-visible').show();
                    }
                });
                self.handlersAdded = true;
            }
        };

        // Called if the user navigates away from the page.
        // Saves the plan if it has changed.
        self.deactivate = function () {
            clearInterval(self.autoSaveTimer);
            var currentJson = self.getCurrentJson();
            if (self.savedJson != currentJson) {
                return self.savePlan(currentJson);
            }
        };

        // Saves the plan and sets the savedJson property.
        self.savePlan = function (json) {
            if (typeof (json) !== 'string') {
                throw new Error('Expected a string as the json argument.')
            }
            return ajax.put('plans/' + self.plan.planId, json).then(function () {
                self.savedJson = json;
                return true;
            });
        };

        // Called when one of the two Save buttons is clicked.
        self.doSave = function () {
            self.autoSaveTimerSuspended = true;
            return self.savePlan(self.getCurrentJson()).then(function () {
                self.autoSaveNotification('Your plan has been saved.');
                self.autoSaveTimerSuspended = false;
            });
        };

        self.doSaveAndCheckIn = function () {
            self.doSave().done(function () {
                ajax.post('plans/{0}/actions'.format(self.plan.planId), {
                    actionName: 'Check In'
                }).done(function () {
                    router.navigate('#list');
                });
            });
        };
    }
    return new ViewModel();
});
