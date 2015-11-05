(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log', 'LookupService', 'FilterService'];
    
    function participantPersonSevis ($log) {
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

                $scope.save = function () {
                    $scope.update({ participantId: $scope.participantid });
                    $scope.view.FundingEdit = false;
                }

                var limit = 300;
                var positionsFilter = FilterService.add('project-participant-sevis-positions');
                positionsFilter = positionsFilter.skip(0).take(limit);

                function loadPositions() {
                    return LookupService.getPositions(positionsFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of SEVIS positions loaded is less than the total number.  Some positions may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        $scope.view.positions = response.data.results;
                        return $scope.view.positions;
                    })
                    .catch(function (response) {
                        var message = "Unable to load sevis positions.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                loadPositions();

            }
        };
        return directive;

    }

})();

