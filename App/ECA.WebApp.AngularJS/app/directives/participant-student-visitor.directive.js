(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantStudentVisitor', participantStudentVisitor);

    participantStudentVisitor.$inject = ['$q', '$log', 'FilterService', 'LookupService', 'NotificationService'];
    
    function participantStudentVisitor($q, $log, FilterService, LookupService, NotificationService) {
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
                    $scope.view.FundingEdit = false;
                };

                $scope.saveEducationInfo = function () {
                    $scope.update({ participantId: $scope.participantid });
                    $scope.view.EducationalInformationEdit = false;
                };

                $scope.edit.searchFieldOfStudiesMajor = function (search) {
                    if (search && search != '') {
                        loadFieldOfStudies(search)
                        .then(function (result) {
                            $scope.edit.fieldOfStudiesMajor = result.data.results;
                        });
                    }
                };

                $scope.edit.searchFieldOfStudiesSecondary = function (search) {
                    if (search && search != '') {
                        loadFieldOfStudies(search)
                        .then(function (result) {
                            $scope.edit.fieldOfStudiesSecondary = result.data.results;
                        });
                    }
                };

                $scope.edit.searchFieldOfStudiesMinor = function (search) {
                    if (search && search != '') {
                        loadFieldOfStudies(search)
                        .then(function (result) {
                            $scope.edit.fieldOfStudiesMinor = result.data.results;
                        });
                    }
                };

                $scope.edit.searchFieldOfStudiesMajorId = function (id) {
                    if (id) {
                        loadFieldOfStudiesById(id)
                        .then(function (result) {
                            $scope.edit.fieldOfStudiesMajor = result.data.results;
                        });
                    }
                };

                $scope.edit.searchFieldOfStudiesSecondaryId = function (id) {
                    if (id) {
                        loadFieldOfStudiesById(id)
                        .then(function (result) {
                            $scope.edit.fieldOfStudiesSecondary = result.data.results;
                        });
                    }
                };

                $scope.edit.searchFieldOfStudiesMinorId = function (id) {
                    if (id) {
                        loadFieldOfStudiesById(id)
                        .then(function (result) {
                            $scope.edit.fieldOfStudiesMinor = result.data.results;
                        });
                    }
                };

                $scope.edit.onEducationalInformationEditChange = function () {
                    $scope.view.EducationalInformationEdit = !$scope.view.EducationalInformationEdit;
                    if ($scope.view.EducationalInformationEdit) {
                        if ($scope.studentvisitorinfo.primaryMajor)
                            $scope.edit.searchFieldOfStudiesMajorId($scope.studentvisitorinfo.primaryMajorId);
                        if ($scope.studentvisitorinfo.secondaryMajor)
                            $scope.edit.searchFieldOfStudiesSecondaryId($scope.studentvisitorinfo.secondaryMajorId);
                        if ($scope.studentvisitorinfo.minor)
                            $scope.edit.searchFieldOfStudiesMinorId($scope.studentvisitorinfo.minorId);
                    };
                };

                $scope.edit.onFundingEditChange = function () {
                    $scope.view.FundingEdit = !$scope.view.FundingEdit;
                    if ($scope.view.FundingEdit) {

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
                    if (search && search != '') {
                        var fieldOfStudiesFilter = FilterService.add('project-participant-editSevis-fieldOfStudies');
                        fieldOfStudiesFilter = fieldOfStudiesFilter.like('description', search).skip(0).take(limit);
                        return LookupService.getSevisFieldOfStudies(fieldOfStudiesFilter.toParams())
                    }
                }

                function loadFieldOfStudiesById(id) {
                    if (id) {
                        var fieldOfStudiesFilter = FilterService.add('project-participant-editSevis-fieldOfStudiesById');
                        fieldOfStudiesFilter = fieldOfStudiesFilter.equal('id', id).skip(0).take(1);
                        return LookupService.getSevisFieldOfStudies(fieldOfStudiesFilter.toParams())
                    }
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
            }
        };

        return directive;
    }

})();

