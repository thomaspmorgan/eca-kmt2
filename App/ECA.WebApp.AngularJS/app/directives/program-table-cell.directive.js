(function () {
    'use strict';

    angular
        .module('staticApp')
        .directive('programTableCell', programTableCell);

    programTableCell.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService', 'ParticipantExchangeVisitorService', 'smoothScroll'];

    function programTableCell($log, LookupService, FilterService, NotificationService, ParticipantExchangeVisitorService, smoothScroll) {
        // Usage:
        //     <participant_exchange_visitor participantId={{id}} active=activevariable, update=updatefunction></participant_person_sevis>
        // Creates:
        // 
        var directive = {
            restrict: 'E',
            scope: {
                //participantid: '@',
                program: '=',
                programfilter: '='
                //active: '=',
                //update: '&'
            },
            templateUrl: 'app/directives/program-table-cell.directive.html',
            controller: function ($scope, $attrs) {

                var limit = 300;
                $scope.view = {};
                $scope.view.onExpandClick = function (program) {
                    program.isExpanded = true;
                    showChildren(program);
                    scrollToProgram(program);
                }

                $scope.view.onCollapseClick = function (program) {
                    program.isExpanded = false;
                    hideChildren(program);
                    scrollToProgram(program);
                }

                function scrollToProgram(program) {
                    var id = program.divId;
                    var options = {
                        duration: 500,
                        easing: 'easeIn',
                        offset: 115,
                        callbackBefore: function (element) { },
                        callbackAfter: function (element) { }
                    }
                    var element = document.getElementById(id)
                    smoothScroll(element, options);
                }

                function showChildren(program) {
                    angular.forEach(program.children, function (child, index) {
                        child.isHidden = false;
                        child.isExpanded = false;
                    });
                }

                function hideChildren(program) {
                    for (var i = 0; i < program.children.length; i++) {
                        var child = program.children[i];
                        if (child.children && child.children.length > 0) {
                            hideChildren(child);
                        }
                        child.isHidden = true;
                        child.isExpanded = false;
                    }
                }
            }
        };

        return directive;
    }
})();
