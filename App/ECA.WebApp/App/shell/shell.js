define(function (require) {

    var router = require('plugins/router');
    var ko = require('knockout');
    var ajax = require('pdtracker/ajax');
    var globals = require('pdtracker/globals');

    return {
        user: globals.user,

        router: router,

        activate: function () {
            //kendo.bind($("#row"));
            ko.bindingHandlers.editableText = {
                init: function (element, valueAccessor) {
                    var $element = $(element);
                    var initialValue = ko.utils.unwrapObservable(valueAccessor());
                    $element.html(initialValue);
                    $element.on('keyup', function () {
                        observable = valueAccessor();
                        observable($element.html());
                    });
                }
            };

            ko.bindingHandlers.contenteditable = {
                init: function (element, valueAccessor, allBindingsAccessor) {
                    var value = ko.utils.unwrapObservable(valueAccessor());
                    console.log('init: ', value);
                    $(element).html(value)
                },

                update: function (element, valueAccessor) {
                    var value = ko.utils.unwrapObservable(valueAccessor());
                    console.log('update: ', value);
                    if ((value === null) || (value === undefined)) {
                        value = "";
                    }
                }
            };

            router.map([{
                route: ['','Home'],
                title: 'Home',
                moduleId: 'home/home',
            }, {
                route: 'Programs',
                title: 'Programs',
                moduleId: 'programs/programs',
            }, {
                route: 'Program/:id*details',
                title: 'Program',
                moduleId: 'program/program',
            }, {
                route: 'colors',
                title: 'Color Palette',
                moduleId: 'help/color-palette'
            }, {
                route: '508',
                title: 'Section 508 Checklist',
                moduleId: 'help/508'
            }, {
                route: 'Training',
                title: 'Home',
                moduleId: 'help/training',
            }]);

            return router.activate();
        },

        attached: function () {
            $(document).on('click', '.disclose-trigger', function (ev) {
                ev.preventDefault();
                var parent = $(this).closest('.disclose');
                if (parent.hasClass('disclose-disclosed')) {
                    parent.find('.disclose-hidden').hide();
                    parent.find('.disclose-visible').show();
                    parent.removeClass('disclose-disclosed');
                } else {
                    parent.find('.disclose-hidden').show();
                    parent.find('.disclose-visible').hide();
                    parent.addClass('disclose-disclosed');
                }
            });
            $(".leftNavClose").on("click", function () {
                $("#sidebar").toggleClass("active");
                $("#wrapper").toggleClass("active");
            });
        }
    };
});
