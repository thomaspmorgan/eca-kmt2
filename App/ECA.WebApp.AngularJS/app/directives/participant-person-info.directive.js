(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonInfo', participantPersonInfo);

    participantPersonInfo.$inject = ['$log', 'ConstantsService', 'ParticipantPersonsService'];
    
    function participantPersonInfo ($log, ConstantsService, ParticipantPersonsService) {
        // Usage:
        //     <participant_person_info participantId={{id}} active=activevariable></participant_person_info>
        // Creates:
        // 
        var directive = {
            restrict: 'E',
            scope: {
                participantid: '@',
                personid: '@',
                onparticipantupdated: '&'
            },
            templateUrl: 'app/directives/participant-person-info.directive.html'
        };

        return directive;
    }

})();

