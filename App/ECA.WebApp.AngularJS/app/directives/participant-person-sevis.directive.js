(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService', 'ParticipantPersonsSevisService', '$state'];
    
    function participantPersonSevis($log, LookupService, FilterService, NotificationService, ParticipantPersonsSevisService, $state) {
        // Usage:
        //     <participant_person_sevis participantId={{id}} active=activevariable, update=updatefunction></participant_person_sevis>
        // Creates:
        // 
        var directive = {
            restrict: 'E',
            scope: {
                participantid: '@',
                personid: '@',
                sevisinfo: '=',
                active: '=',
                update: '&'
            },
            templateUrl: 'app/directives/participant-person-sevis.directive.html',
            controller: function ($scope, $attrs) {

                var limit = 300;
                $scope.edit = [];

                $scope.edit.openStartDatePicker = function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();
                    $scope.edit.isStartDatePickerOpen = true
                }

                $scope.edit.openEndDatePicker = function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();
                    $scope.edit.isEndDatePickerOpen = true
                }

                // Navigate to a section where the validation error can be resolved
                $scope.goToErrorSection = function (error) {
                    if (error && error.customState)
                    {
                        $state.go(error.customState.category + '.' + error.customState.categorySub,
                            {
                                'section': error.customState.section,
                                'tab': error.customState.tab,
                                'personId': $scope.personid,
                                'participantId': $scope.participantid
                            }, { reload: true });
                    }
                };
            }
        };

        return directive;
    }
})();

