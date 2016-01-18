﻿(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService', 'ParticipantPersonsSevisService', '$location'];
    
    function participantPersonSevis($log, LookupService, FilterService, NotificationService, ParticipantPersonsSevisService, $location) {
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
                $scope.validateSevisInfo = function (sevisId) {
                    if (sevisId) {
                        validateUpdateSevisInfo();
                    } else {
                        validateCreateSevisInfo();
                    }
                };
                
                // pre-sevis create validation
                function validateCreateSevisInfo() {
                    $scope.edit.isValidationLoading = true;
                    return ParticipantPersonsSevisService.validateParticipantPersonsCreateSevis($scope.participantid)
                    .then(function (response) {
                        $log.info('Validated participant create SEVIS info');
                        var valErrors = [];
                        for (var i = 0; i < response.data.errors.length; i++) {
                            valErrors.push({ msg: response.data.errors[i].errorMessage, path: getErrorPath(response.data.errors[i].customState) });
                        }
                        $scope.validationResults = valErrors;
                        // update participant sevis status
                        var params = {
                            errors: response.data.errors.length,
                            isvalid: response.data.isValid
                        };
                        ParticipantPersonsSevisService.updateParticipantSevisCommStatus($scope.participantid, params);
                        $scope.edit.isValidationLoading = false;
                    }, function (error) {
                        NotificationService.showErrorMessage(error.data);
                        $scope.edit.isValidationLoading = false;
                    });
                };

                // pre-sevis update validation
                function validateUpdateSevisInfo() {
                    $scope.edit.isValidationLoading = true;
                    return ParticipantPersonsSevisService.validateParticipantPersonsUpdateSevis($scope.participantid)
                    .then(function (response) {
                        $log.info('Validated participant update SEVIS info');
                        var valErrors = [];
                        for (var i = 0; i < response.data.errors.length; i++) {
                            valErrors.push(response.data.errors[i].errorMessage);
                        }
                        $scope.validationResults = valErrors;
                        // update participant sevis status
                        var params = {
                            errors: response.data.errors.length,
                            isvalid: response.data.isValid
                        };
                        ParticipantPersonsSevisService.updateParticipantSevisCommStatus($scope.participantid, params);
                        $scope.edit.isValidationLoading = false;
                    }, function (error) {
                        NotificationService.showErrorMessage(error.data);
                        $scope.edit.isValidationLoading = false;
                    });
                };

                function getErrorPath(val) {
                    var path = "";
                    if (val)
                    {
                        path = $location.path() + "?section=" + val.section + "?tab=" + val.tab;
                    }                    
                    return path;
                };
            }
        };

        return directive;
    }
})();

