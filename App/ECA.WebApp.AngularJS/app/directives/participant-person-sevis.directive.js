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
                        $scope.sevisInfo.sevisValidationResult = valErrors;
                        // log participant sevis validation attempt
                        ParticipantPersonsSevisService.createParticipantSevisCommStatus($scope.participantid, response.data);
                        // update participant sevis validation results
                        updateSevisInfo($scope.participantid, response.data);
                        $scope.edit.isValidationLoading = false;
                    })
                    .catch(function () {
                        var message = "Unable to validate participant create SEVIS info.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
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
                        $scope.sevisInfo.sevisValidationResult = valErrors;
                        // log participant sevis validation attempt
                        ParticipantPersonsSevisService.createParticipantSevisCommStatus($scope.participantid, response.data);
                        // update participant sevis validation results
                        updateSevisInfo($scope.participantid, response.data);
                        $scope.edit.isValidationLoading = false;
                    })
                    .catch(function () {
                        var message = "Unable to validate participant create SEVIS info.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                        $scope.edit.isValidationLoading = false;
                    });
                };

                // get participant record and attach validation results
                function updateSevisInfo(participantId, validationResults) {
                    return ParticipantPersonsSevisService.getParticipantPersonsSevisById(participantId)
                    .then(function (data) {
                        var sevisInfo = data.data;
                        sevisInfo.sevisValidationResult = JSON.stringify(validationResults);
                        saveSevisInfo(participantId, sevisInfo);
                    })
                    .catch(function () {
                        $log.error('Unable to load participant SEVIS info.');
                        NotificationService.showErrorMessage('Unable to load participant SEVIS info.');
                    });
                };

                // update participant sevis results
                function saveSevisInfo(participantId, updatedSevisInfo) {
                    return ParticipantPersonsSevisService.updateParticipantPersonsSevis(updatedSevisInfo)
                    .then(function (data) {
                        NotificationService.showSuccessMessage('Participant SEVIS info saved successfully.');
                    })
                    .catch(function () {
                        $log.error('Unable to save participant SEVIS info for participant');
                        NotificationService.showErrorMessage('Unable to save participant SEVIS info for participant');
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

