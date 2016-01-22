(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService', 'ParticipantPersonsSevisService', '$state'];
    
    function participantPersonSevis($log, LookupService, FilterService, NotificationService, ParticipantPersonsSevisService, $state) {
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
                $scope.sevisInfo = {};

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
                            valErrors.push({ msg: response.data.errors[i].errorMessage, path: response.data.errors[i].customState });
                        }
                        $scope.validationResults = valErrors;
                        // log participant sevis validation attempt
                        ParticipantPersonsSevisService.createParticipantSevisCommStatus($scope.participantid, response.data);
                        // update participant sevis validation results
                        loadSevisInfo($scope.participantid);
                        saveSevisInfoById($scope.participantid, response.data);
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
                            valErrors.push({ msg: response.data.errors[i].errorMessage, path: response.data.errors[i].customState });
                        }
                        $scope.validationResults = valErrors;
                        // log participant sevis validation attempt
                        ParticipantPersonsSevisService.createParticipantSevisCommStatus($scope.participantid, response.data);
                        // update participant sevis validation results
                        loadSevisInfo($scope.participantid);
                        saveSevisInfoById($scope.participantid, response.data);
                        $scope.edit.isValidationLoading = false;
                    }, function (error) {
                        NotificationService.showErrorMessage(error.data);
                        $scope.edit.isValidationLoading = false;
                    });
                };

                // get participant record
                function loadSevisInfo(participantId) {
                    return ParticipantPersonsSevisService.getParticipantPersonsSevisById(participantId)
                    .then(function (data) {
                        $scope.sevisInfo = data.data;
                    }, function (error) {
                        if (error.status === 404) {
                            $scope.sevisInfo = {};
                        } else {
                            $log.error('Unable to load participant SEVIS info for ' + participantId + '.');
                            NotificationService.showErrorMessage('Unable to load participant SEVIS info for ' + participantId + '.');
                        }
                    });
                };

                // update participant sevis results
                function saveSevisInfoById(participantId, results) {
                    var sevisInfo = $scope.sevisInfo;
                    sevisInfo.sevisResults = results;
                    return ParticipantPersonsSevisService.updateParticipantPersonsSevis(sevisInfo)
                    .then(function (data) {
                        NotificationService.showSuccessMessage('Participant SEVIS info saved successfully.');
                        $scope.sevisInfo = data.data;
                    }, function (error) {
                        $log.error('Unable to save participant SEVIS info for participantId: ' + participantId);
                        NotificationService.showErrorMessage('Unable to save participant SEVIS info for participant: ' + participantId + '.');
                    });
                };

                // Navigate to a section where the validation error can be resolved
                $scope.goToErrorSection = function (customState) {
                    
                    if (customState)
                    {
                        $state.go(customState.category + '.' + customState.categorySub, { 'section': customState.section, 'tab': customState.tab, 'personId': $scope.participantid, 'participantId': $scope.participantid }, { reload: true });
                    }

                };
            }
        };

        return directive;
    }
})();

