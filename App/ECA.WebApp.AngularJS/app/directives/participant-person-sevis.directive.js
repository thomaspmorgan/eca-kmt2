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
                
                // pre-sevis validation result update
                $scope.validateSevisInfo = function (sevisInfo) {
                    $scope.edit.isValidationLoading = true;
                    return ParticipantPersonsSevisService.verifyExchangeVisitor(sevisInfo.projectId, sevisInfo.participantId)
                    .then(function (response) {
                        return ParticipantPersonsSevisService.getParticipantPersonsSevisById(sevisInfo.projectId, sevisInfo.participantId)
                        .then(function (participantPersonSevisResponse) {
                            $scope.sevisinfo = participantPersonSevisResponse.data;
                            $scope.edit.isValidationLoading = false;
                        });
                    })
                    .catch(function (response) {
                        $scope.edit.isValidationLoading = false;
                        var message = "Unable to verify participant sevis info.";
                        NotificationService.showErrorMessage(message);
                        $log.error(message);
                    });
                }

                // Navigate to a section where the validation error can be resolved
                $scope.goToErrorSection = function (customState) {                    
                    if (customState)
                    {
                        $state.go(customState.category + '.' + customState.categorySub, { 'section': customState.section, 'tab': customState.tab, 'personId': $scope.personid, 'participantId': $scope.participantid }, { reload: true });
                    }
                };
            }
        };

        return directive;
    }
})();

