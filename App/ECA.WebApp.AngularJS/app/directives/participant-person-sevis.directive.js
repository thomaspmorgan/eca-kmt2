﻿(function () {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = [
        '$log',
        '$q',
        '$filter',
        'LookupService',
        'FilterService',
        'NotificationService',
        'ParticipantPersonsSevisService',
        'StateService',
        'MessageBox',
        'ConstantsService',
        'smoothScroll',
        '$state'];

    function participantPersonSevis(
        $log,
        $q,
        $filter,
        LookupService,
        FilterService,
        NotificationService,
        ParticipantPersonsSevisService,
        StateService,
        MessageBox,
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
                exchangevisitorinfo: '=',
                active: '=',
                updatesevisinfo: '&',
                updateexchangevisitorinfo: '&'
            },
            templateUrl: 'app/directives/participant-person-sevis.directive.html',
            controller: function ($scope, $attrs) {
                var limit = 300;
                $scope.edit = [];
                $scope.view = {};
                $scope.view.PositionAndField = false;
                $scope.view.PositionAndFieldEdit = false;
                $scope.edit.isStartDatePickerOpen = false;
                $scope.edit.isEndDatePickerOpen = false;
                $scope.view.Funding = false;
                $scope.view.FundingEdit = false;
                $scope.positionAndFieldElementId = 'positionAndField' + $scope.participantid;
                $scope.fundingElementId = 'funding' + $scope.participantid;
                $scope.editMode = false;
                                
                var notifyStatuses = ConstantsService.sevisStatuses;
                
                $scope.edit.onDosStatusChange = function ($event, checkboxId, checked) {
                    return CreateMessageBox(checked)
                    .then(function (response) {
                        if (response === checked) {
                            $scope.updatesevisinfo({ participantId: $scope.participantid });
                        } else {
                            $scope.sevisinfo[checkboxId] = response;
                        }
                    });
                    
                    $event.preventDefault();
                    $event.stopPropagation();
                }
                
                $scope.edit.onStartDateChange = function () {
                    if (!$scope.editMode) {
                        return CreateMessageBox(false)
                        .then(function (response) {
                            if (response === false) {
                                $scope.sevisinfo.startDate = $scope.$parent.oldStartDate;
                            } else {
                                if (!isNaN(Date.parse($scope.sevisinfo.startDate))) {
                                    $scope.updatesevisinfo({ participantId: $scope.participantid });
                                }
                            }
                        });
                    } else {
                        if (!isNaN(Date.parse($scope.sevisinfo.startDate))) {
                            $scope.updatesevisinfo({ participantId: $scope.participantid });
                        }
                    }
                }

                $scope.edit.openStartDatePicker = function ($event) {
                    return CreateMessageBox($scope.edit.isStartDatePickerOpen)
                    .then(function (response) {
                        $scope.edit.isStartDatePickerOpen = response;
                    });

                    $event.preventDefault();
                    $event.stopPropagation();
                }

                $scope.edit.onEndDateChange = function () {
                    if (!$scope.editMode) {
                        return CreateMessageBox(false)
                        .then(function (response) {
                            if (response === false) {
                                $scope.sevisinfo.endDate = $scope.$parent.oldEndDate;
                            } else {
                                if (!isNaN(Date.parse($scope.sevisinfo.endDate))) {
                                    $scope.updatesevisinfo({ participantId: $scope.participantid });
                                }
                            }
                        });
                    } else {
                        if (!isNaN(Date.parse($scope.sevisinfo.endDate))) {
                            $scope.updatesevisinfo({ participantId: $scope.participantid });
                        }
                    }
                }

                $scope.edit.openEndDatePicker = function ($event) {
                    return CreateMessageBox($scope.edit.isEndDatePickerOpen)
                    .then(function (response) {
                        $scope.edit.isEndDatePickerOpen = response;
                    });

                    $event.preventDefault();
                    $event.stopPropagation();
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
                        scrollToPositionAndField();
                    }
                    else if (errorTypeId == ConstantsService.sevisErrorType.funding.id) {
                        scrollToFunding();
                    }
                    else {
                        throw Error('The error type id ' + errorTypeId + ' is not supported.');
                    }
                };

                $scope.saveFunding = function () {
                    $scope.updateexchangevisitorinfo({ participantId: $scope.participantid });
                    $scope.view.FundingEdit = false;
                };

                $scope.savePositionAndField = function () {
                    $scope.updateexchangevisitorinfo({ participantId: $scope.participantid });
                    $scope.view.PositionAndFieldEdit = false;
                };

                $scope.edit.searchFieldOfStudies = function (search) {
                    return loadFieldOfStudies(search);
                };

                $scope.edit.onFundingEditChange = function () {
                    return CreateMessageBox($scope.view.PositionAndFieldEdit)
                    .then(function (response) {
                        $scope.view.FundingEdit = response;
                    });

                    if ($scope.view.FundingEdit) {
                        $scope.view.GovtAgency1Other = ($scope.exchangevisitorinfo.govtAgency1Id == 22);
                        $scope.view.GovtAgency2Other = ($scope.exchangevisitorinfo.govtAgency2Id == 22);
                        $scope.view.IntlOrg1Other = ($scope.exchangevisitorinfo.intlOrg1Id == 18);
                        $scope.view.IntlOrg2Other = ($scope.exchangevisitorinfo.intlOrg2Id == 18);
                    }
                };

                $scope.edit.onPositionAndFieldEditChange = function () {
                    return CreateMessageBox($scope.view.PositionAndFieldEdit)
                    .then(function (response) {
                        $scope.view.PositionAndFieldEdit = response;
                    });
                    if ($scope.view.PositionAndFieldEdit)
                        loadFieldOfStudies($scope.exchangevisitorinfo.fieldOfStudy);
                }
                
                function CreateMessageBox(userSection) {
                    var defer = $q.defer();
                    if (notifyStatuses.indexOf($scope.sevisinfo.sevisStatusId) !== -1) {
                        MessageBox.confirm({
                            title: 'Confirm Edit',
                            message: 'The SEVIS participant status of this person is ' + $scope.sevisinfo.sevisStatus + '. Are you sure you want to edit?',
                            okText: 'Yes',
                            cancelText: 'No',
                            okCallback: function () {
                                userSection = true;
                                $scope.editMode = true;
                                defer.resolve(userSection);
                            },
                            cancelCallback: function () {
                                userSection = !userSection
                                $scope.editMode = false;
                                defer.resolve(userSection);
                            }
                        });
                    } else {
                        userSection = !userSection;
                        $scope.editMode = userSection;
                        defer.resolve(userSection);
                    }

                    return defer.promise;
                }

                $scope.onGovtAgency1Select = function (item) {
                    if (item.description != null)
                        if (item.description == "OTHER")
                            $scope.view.GovtAgency1Other = true;
                        else {
                            $scope.view.GovtAgency1Other = false;
                            $scope.exchangevisitorinfo.govtAgency1OtherName = '';
                        }
                };

                $scope.onGovtAgency2Select = function (item) {
                    if (item.description != null)
                        if (item.description == "OTHER")
                            $scope.view.GovtAgency2Other = true;
                        else {
                            $scope.view.GovtAgency2Other = false;
                            $scope.exchangevisitorinfo.govtAgency2OtherName = '';
                        }
                };

                $scope.onIntlOrg1Select = function (item) {
                    if (item.description != null)
                        if (item.description == "OTHER")
                            $scope.view.IntlOrg1Other = true;
                        else {
                            $scope.view.IntlOrg1Other = false;
                            $scope.exchangevisitorinfo.intlOrg1OtherName = '';
                        }
                };

                $scope.onIntlOrg2Select = function (item) {
                    if (item.description != null)
                        if (item.description == "OTHER")
                            $scope.view.IntlOrg2Other = true;
                        else {
                            $scope.view.IntlOrg2Other = false;
                            $scope.exchangevisitorinfo.intlOrg2OtherName = '';
                        }
                };

                function scrollToFunding() {
                    $scope.view.Funding = true;
                    scrollToSevisTabElement(
                        $scope.fundingElementId,
                        function () { },
                        function () { });
                }

                function scrollToPositionAndField() {
                    $scope.view.PositionAndField = true;
                    scrollToSevisTabElement(
                        $scope.positionAndFieldElementId,
                        function () { },
                        function () { });
                }

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

                function loadUSGovernmentAgencies() {
                    var usGovernmentAgenciesFilter = FilterService.add('project-participant-editSevis-usGovernmentAgencies');
                    usGovernmentAgenciesFilter = usGovernmentAgenciesFilter.skip(0).take(limit);
                    return LookupService.getSevisUSGovernmentAgencies(usGovernmentAgenciesFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of USGovernmentAgencies loaded is less than the total number.  Some USGovernmentAgencies may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        $scope.edit.usGovernmentAgencies = response.data.results;
                        return $scope.edit.usGovernmentAgencies;
                    })
                    .catch(function (response) {
                        var message = "Unable to load USGovernmentAgencies.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                function loadInternationalOrganizations() {
                    var internationalOrganizationsFilter = FilterService.add('project-participant-editSevis-internationalOrganizations');
                    internationalOrganizationsFilter = internationalOrganizationsFilter.skip(0).take(limit);
                    return LookupService.getSevisInternationalOrganizations(internationalOrganizationsFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of InternationalOrganizations loaded is less than the total number.  Some InternationalOrganizations may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        $scope.edit.internationalOrganizations = response.data.results;
                        return $scope.edit.internationalOrganizations;
                    })
                    .catch(function (response) {
                        var message = "Unable to load InternationalOrganizations.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                loadPositions();
                loadProgramCategories();
                loadUSGovernmentAgencies();
                loadInternationalOrganizations();
            }
        };

        return directive;
    }
})();

