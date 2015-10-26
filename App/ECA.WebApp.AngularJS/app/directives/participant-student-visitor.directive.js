(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantStudentVisitor', participantStudentVisitor);

    participantStudentVisitor.$inject = ['$log'];
    
    function participantStudentVisitor($log) {
        // Usage:
        //     <participant_student_visitor participantId={{id}} studentInfo=studentInfovariable active=activevariable></participant_student_visitor>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'E',
            scope: {
                participantid: '@',
                studentvisitorinfo: '=',
                active: '='
            },
            templateUrl: 'app/directives/participant-student-visitor.directive.html'
        };
        return directive;

        function link(scope, element, attrs) {
        };

    }

})();

