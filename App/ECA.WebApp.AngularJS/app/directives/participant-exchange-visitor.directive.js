(function() {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantExchangeVisitor', participantExchangeVisitor);

    participantExchangeVisitor.$inject = ['$log', 'LookupService', 'FilterService', 'NotificationService', 'ParticipantExchangeVisitorService'];
    
    function participantExchangeVisitor($log, LookupService, FilterService, NotificationService, ParticipantExchangeVisitorService) {
        // Usage:
        //     <participant_exchange_visitor participantId={{id}} active=activevariable, update=updatefunction></participant_person_sevis>
        // Creates:
        // 
        var directive = {
            restrict: 'E',
            scope: {
                participantid: '@',
                exchangevisitorinfo: '=',
                active: '=',
                update: '&'
            },
            templateUrl: 'app/directives/participant-exchange-visitor.directive.html',
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

                $scope.edit.searchFieldOfStudies = function (search) {
                    return loadFieldOfStudies(search);
                };

                $scope.edit.onFundingEditChange = function () {
                    $scope.view.FundingEdit = !$scope.view.FundingEdit;
                    if ($scope.view.FundingEdit) {
                        $scope.view.GovtAgency1Other = ($scope.exchangevisitorinfo.govtAgency1Id == 22);
                        $scope.view.GovtAgency2Other = ($scope.exchangevisitorinfo.govtAgency2Id == 22);
                        $scope.view.IntlOrg1Other = ($scope.exchangevisitorinfo.intlOrg1Id == 18);
                        $scope.view.IntlOrg2Other = ($scope.exchangevisitorinfo.intlOrg2Id == 18);
                    }
                };

                $scope.edit.onPositionAndFieldEditChange = function () {
                    $scope.view.PositionAndFieldEdit = !$scope.view.PositionAndFieldEdit;
                    if ($scope.view.PositionAndFieldEdit)
                        loadFieldOfStudies($scope.exchangevisitorinfo.fieldOfStudy);
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
                //loadFieldOfStudies();
            }
        };

        return directive;
    }
})();

