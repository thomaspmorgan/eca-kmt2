(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonInfo', participantPersonInfo);

    participantPersonInfo.$inject = ['$log'];
    
    function participantPersonInfo ($log) {
        // Usage:
        //     <participant_person_info participantId={{id}} active=activevariable></participant_person_info>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'E',
            scope: {
                participantid: '@',
                active: '='
            },
            templateUrl: 'app/directives/participant-person-info.directive.html'
        };
        return directive;

        function link(scope, element, attrs) {
        };

    }

})();

