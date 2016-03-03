(function () {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = [
        '$log',
        'LookupService',
        'FilterService',
        'NotificationService',
        'ParticipantPersonsSevisService',
        'StateService',
        'ConstantsService',
        'smoothScroll',
        '$state'];

    function participantPersonSevis(
        $log,
        LookupService,
        FilterService,
        NotificationService,
        ParticipantPersonsSevisService,
        StateService,
        ConstantsService,
        smoothScroll,
        $state) {
        // Usage:
        //     <participant_person_sevis participantId={{id}} active=activevariable, update=updatefunction></participant_person_sevis>
        // Creates:
        // 
        var directive = {
            restrict: 'E',
            scope: {
                participantid: '@',
                personid: '@',
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

                $scope.getSevisStartDateDivId = function (participantId) {
                    return 'sevisStartDate' + participantId;
                }

                $scope.onErrorClick = function (error) {
                    var personId = $scope.personid;
                    var participantId = $scope.participantid;
                    var errorTypeId = error.customState.sevisErrorTypeId;
                    if (errorTypeId == ConstantsService.sevisErrorType.email.id
                        || errorTypeId == ConstantsService.sevisErrorType.phoneNumber.id) {
                        return StateService.goToPersonContactInformationState(personId);
                    }
                    else if (errorTypeId == ConstantsService.sevisErrorType.fullName.id
                        || errorTypeId == ConstantsService.sevisErrorType.gender.id
                        || errorTypeId == ConstantsService.sevisErrorType.cityOfBirth.id
                        || errorTypeId == ConstantsService.sevisErrorType.countryOfBirth.id
                        || errorTypeId == ConstantsService.sevisErrorType.birthDate.id
                        || errorTypeId == ConstantsService.sevisErrorType.citizenship.id
                        || errorTypeId == ConstantsService.sevisErrorType.address.id
                        || errorTypeId == ConstantsService.sevisErrorType.permanentCountryOfResidence.id
                        ) {
                        return StateService.goToPiiState(personId);
                    }
                    else if (errorTypeId == ConstantsService.sevisErrorType.startDate.id
                        || errorTypeId == ConstantsService.sevisErrorType.endDate.id
                        ) {
                        scrollToSevisTabElement($scope.getSevisStartDateDivId($scope.participantid));
                    }
                    else if (errorTypeId == ConstantsService.sevisErrorType.position.id
                        || errorTypeId == ConstantsService.sevisErrorType.programCategory.id
                        || errorTypeId == ConstantsService.sevisErrorType.fieldOfStudy.id
                        ) {
                        return null;
                    }
                    else if (errorTypeId == ConstantsService.sevisErrorType.funding.id) {
                        return null;
                    }
                    else {
                        throw Error('The error type id ' + errorTypeId + ' is not supported.');
                    }
                };

                function scrollToSevisTabElement(id, callbackBefore, callbackAfter) {
                    callbackBefore = callbackBefore || function () { };
                    callbackAfter = callbackAfter || function () { };
                    var section = document.getElementById(id);
                    var options = {
                        duration: 500,
                        easing: 'easeIn',
                        offset: 165,
                        callbackBefore: callbackBefore,
                        callbackAfter: callbackAfter
                    }
                    smoothScroll(section, options);
                }
            }
        };

        return directive;
    }
})();

