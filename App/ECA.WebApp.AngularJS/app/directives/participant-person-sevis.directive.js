(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log'];
    
    function participantPersonSevis ($log) {
        // Usage:
        //     <participant_person_sevis participantId={{id}} active=activevariable></participant_person_sevis>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'E',
            scope: {
                participantid: '@',
                sevisinfo: '=',
                active: '=',
                update: '&'
            },
            templateUrl: 'app/directives/participant-person-sevis.directive.html'
        };
        return directive;

        function link(scope, element, attrs) {
        };

    }

})();

