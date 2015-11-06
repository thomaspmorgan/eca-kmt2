(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService'];
    
    function participantPersonSevis ($log, LookupService, FilterService, NotificationService) {
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

                $scope.saveFunding = function () {
                    $scope.update({ participantId: $scope.participantid });
                    $scope.view.FundingEdit = false;
                };

                $scope.savePositionAndField = function () {
                    $scope.update({ participantId: $scope.participantid });
                    $scope.view.PositionAndFieldEdit = false;
                };

                $scope.edit.searchFieldOfStudies= function (search) {
                    return loadFieldOfStudies(search);
                }


                function loadPositions() {
                    var positionsFilter = FilterService.add('project-participant-editSevis-positions');
                    positionsFilter = positionsFilter.skip(0).take(limit);
                    return LookupService.getSevisPositions(positionsFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of positions loaded is less than the total number.  Some positinons may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        $scope.edit.positions = response.data.results;
                        return $scope.edit.positions;
                    })
                    .catch(function (response) {
                        var message = "Unable to load positions.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }


                function loadFieldOfStudies(search) {
                    var fieldOfStudiesFilter = FilterService.add('project-participant-editSevis-fieldOfStudies');
                    fieldOfStudiesFilter = fieldOfStudiesFilter.skip(0).take(limit);
                    if (search) {
                        fieldOfStudiesFilter = fieldOfStudiesFilter.like('description', search);
                    }
                    return LookupService.getSevisFieldOfStudies(fieldOfStudiesFilter.toParams())
                    .then(function (response) {
                        //if (response.data.total > limit) {
                        //    var message = "The number of fieldOfStudies loaded is less than the total number.  Some fieldOfStudies may not be shown."
                        //    NotificationService.showErrorMessage(message);
                        //    $log.error(message);
                        //}
                        $scope.edit.fieldOfStudies = response.data.results;
                        return $scope.edit.fieldOfStudies;
                    })
                    .catch(function (response) {
                        var message = "Unable to load fieldOfStudies.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                function loadProgramCategories() {
                    var programCategoriesFilter = FilterService.add('project-participant-editSevis-programCategories');
                    programCategoriesFilter = programCategoriesFilter.skip(0).take(limit);
                    return LookupService.getSevisProgramCategories(programCategoriesFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of programCategories loaded is less than the total number.  Some programCategories may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        $scope.edit.programCategories = response.data.results;
                        return $scope.edit.programCategories;
                    })
                    .catch(function (response) {
                        var message = "Unable to load programCategories.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                loadPositions();
                loadProgramCategories();
                //loadFieldOfStudies();
            }
        };
        return directive;

    }

})();

