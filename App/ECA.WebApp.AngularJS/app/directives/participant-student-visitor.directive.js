(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantStudentVisitor', participantStudentVisitor);

    participantStudentVisitor.$inject = ['$log','FilterService', 'LookupService', 'NotificationService'];
    
    function participantStudentVisitor($log, FilterService, LookupService, NotificationService) {
        // Usage:
        //     <participant_student_visitor participantId={{id}} studentInfo=studentInfovariable active=activevariable></participant_student_visitor>
        // Creates:
        // 
        var directive = {
            restrict: 'E',
            scope: {
                participantid: '@',
                studentvisitorinfo: '=',
                active: '=',
                update: '&'
            },
            templateUrl: 'app/directives/participant-student-visitor.directive.html',
            controller: function ($scope, $attrs) {

                var limit = 300;
                $scope.edit = [];

                $scope.saveFinancialInfo = function () {
                    $scope.update({ participantId: $scope.participantid });
                    $scope.view.FinancialInfoEdit = false;
                };

                $scope.saveEducationInfo = function () {
                    $scope.update({ participantId: $scope.participantid });
                    $scope.view.EducationInfoEdit = false;
                };

                $scope.edit.searchFieldOfStudies = function (search) {
                    return loadFieldOfStudies(search);
                };

                $scope.edit.onFundingEditChange = function () {
                    $scope.view.FundingEdit = !$scope.view.FundingEdit;
                    if ($scope.view.FundingEdit) {
                        //$scope.view.GovtAgency1Other = ($scope.sevisinfo.govtAgency1Id == 22);
                        //$scope.view.GovtAgency2Other = ($scope.sevisinfo.govtAgency2Id == 22);
                        //$scope.view.IntlOrg1Other = ($scope.sevisinfo.intlOrg1Id == 18);
                        //$scope.view.IntlOrg2Other = ($scope.sevisinfo.intlOrg2Id == 18);
                    }
                };

                $scope.onGovtAgency1Select = function (item) {
                    if (item.description != null)
                        if (item.description == "OTHER")
                            $scope.view.GovtAgency1Other = true;
                        else {
                            $scope.view.GovtAgency1Other = false;
                            $scope.sevisinfo.govtAgency1OtherName = '';
                        }
                };

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

                function loadStudentCreations() {
                    var studentCreationsFilter = FilterService.add('project-participant-editSevis-studentCreations');
                    studentCreationsFilter = studentCreationsFilter.skip(0).take(limit);
                    return LookupService.getSevisStudentCreations(studentCreationsFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of StudentCreations loaded is less than the total number.  Some StudentCreations may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        $scope.edit.studentCreations = response.data.results;
                        return $scope.edit.studentCreations;
                    })
                    .catch(function (response) {
                        var message = "Unable to load StudentCreations.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                function loadEducationLevels() {
                    var educationLevelsFilter = FilterService.add('project-participant-editSevis-educationLevels');
                    educationLevelsFilter = educationLevelsFilter.skip(0).take(limit);
                    return LookupService.getSevisEducationLevels(educationLevelsFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of EducationLevels loaded is less than the total number.  Some EducationLevels may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        $scope.edit.educationLevels = response.data.results;
                        return $scope.edit.educationLevels;
                    })
                    .catch(function (response) {
                        var message = "Unable to load EducationLevels.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                loadStudentCreations();
                loadEducationLevels();
                //loadFieldOfStudies();
            }
        };
        return directive;

    }

})();

