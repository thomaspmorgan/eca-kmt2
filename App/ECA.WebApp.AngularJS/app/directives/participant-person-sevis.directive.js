(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService', 'ParticipantPersonsSevisService'];
    
    function participantPersonSevis($log, LookupService, FilterService, NotificationService, ParticipantPersonsSevisService) {
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
                $scope.validateSevisInfo = function () {
                    return ParticipantPersonsSevisService.validateParticipantPersonsSevis($scope.participantid)
                    .then(function (response) {
                        $log.error('Validated participant SEVIS info');
                        var valErrors = [];
                        for (var i = 0; i < response.data.length; i++) {                            
                            valErrors.push(response.data[i].errorMessage);
                        }
                        $scope.validationResults = valErrors;
                    }, function (error) {
                        NotificationService.showErrorMessage(error.data);
                    });
                };
            }
        };

        return directive;
    }
})();

