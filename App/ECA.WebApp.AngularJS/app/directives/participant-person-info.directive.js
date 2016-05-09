(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonInfo', participantPersonInfo);

    participantPersonInfo.$inject = ['$log', 'ConstantsService'];
    
    function participantPersonInfo ($log, ConstantsService) {
        // Usage:
        //     <participant_person_info participantId={{id}} active=activevariable></participant_person_info>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'E',
            scope: {
                participantid: '@',
                sevisinfo: '=',
                active: '=',
                onparticipantupdated: '&'
            },
            templateUrl: 'app/directives/participant-person-info.directive.html',
            controller: function ($scope, $attrs) {
                var sevisInfoCopy = null;
                var notifyStatuses = ConstantsService.sevisStatusIds.split(',');
                var projectId = 0;
                var participantId = $scope.participantid;
                $scope.view = {};

                $scope.$watch(function () {
                    return $scope.sevisinfo;
                }, function (newValue, oldValue) {
                    if (newValue && !sevisInfoCopy) {
                        sevisInfoCopy = angular.copy(newValue);
                        projectId = newValue.projectId;
                    }
                });                
            }
        };

        return directive;

        function link(scope, element, attrs) {
        };
    }

})();

