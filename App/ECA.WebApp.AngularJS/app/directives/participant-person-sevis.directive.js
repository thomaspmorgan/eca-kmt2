(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService', 'SevisResultService', '$state'];
    
    function participantPersonSevis($log, LookupService, FilterService, NotificationService, SevisResultService, $state) {
        // Usage:
        //     <participant_person_sevis participantId={{id}} active=activevariable, update=updatefunction></participant_person_sevis>
        // Creates:
        // 
        var directive = {
            restrict: 'E',
            scope: {
                participantid: '@',
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
                
                // pre-sevis validation
                $scope.validateSevisInfo = function (sevisId) {
                    if (sevisId) {
                        $scope.sevisinfo.sevisValidationResult = SevisResultService.validateUpdateSevisInfo($scope.participantid);
                    } else {
                        $scope.sevisinfo.sevisValidationResult = SevisResultService.validateCreateSevisInfo($scope.participantid);
                    }
                }

                // Navigate to a section where the validation error can be resolved
                $scope.goToErrorSection = function (customState) {                    
                    if (customState)
                    {
                        $state.go(customState.category + '.' + customState.categorySub, { 'section': customState.section, 'tab': customState.tab, 'personId': $scope.participantid, 'participantId': $scope.participantid }, { reload: true });
                    }
                };
            }
        };

        return directive;
    }
})();

