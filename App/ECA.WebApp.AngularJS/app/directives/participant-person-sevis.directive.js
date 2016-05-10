(function () {
    'use strict';

    angular
        .module('staticApp')
        .directive('participantPersonSevis', participantPersonSevis);

    participantPersonSevis.$inject = [
        '$log',
        '$q',
        '$filter',
        '$modal',
        'LookupService',
        'FilterService',
        'NotificationService',
        'ParticipantPersonsSevisService',
        'StateService',
        'MessageBox',
        'ConstantsService',
        'UiGridFilterService',
        'smoothScroll',
        '$state',
        '$timeout',
        'DownloadService',
        'uiGridConstants'];

    function participantPersonSevis(
        $log,
        $q,
        $filter,
        $modal,
        LookupService,
        FilterService,
        NotificationService,
        ParticipantPersonsSevisService,
        StateService,
        MessageBox,
        ConstantsService,
        UiGridFilterService,
        smoothScroll,
        $state,
        $timeout,
        DownloadService,
        uiGridConstants) {
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
                var defaultPageSize = 10;
                UiGridFilterService.setPageSize(defaultPageSize);
                $scope.edit = {};
                $scope.view = {};
                $scope.view.PositionAndField = false;
                $scope.view.PositionAndFieldEdit = false;
                $scope.edit.sevisCommStatuses = [];
                $scope.edit.isStartDatePickerOpen = false;
                $scope.edit.isEndDatePickerOpen = false;
                $scope.view.Funding = false;
                $scope.view.FundingEdit = false;
                $scope.positionAndFieldElementId = 'positionAndField' + $scope.participantid;
                $scope.fundingElementId = 'funding' + $scope.participantid;
                $scope.pageTimeout = null;

                $scope.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                    if (col.filters[0].term) {
                        return 'header-filtered';
                    } else {
                        return '';
                    }
                };

                $scope.columns = [
                    { name: 'addedOn', displayName: 'Date', type: 'date', cellFilter: 'date:\'MMM dd, yyyy hh:mm a\'', enableFiltering: false },
                    {
                        name: 'sevisCommStatusName',
                        displayName: 'Status',
                        filter: {
                            noTerm: true,
                            type: uiGridConstants.filter.SELECT,
                            selectOptions: [],
                            field: 'sevisCommStatusId'
                        },
                        headerCellClass: $scope.highlightFilteredHeader                        
                    },
                    { name: 'displayName', displayName: 'User' },
                    {
                        name: 'batchId',
                        cellTemplate:
                        '<div class="ui-grid-cell-contents">'
                        + '<a ng-click="grid.appScope.onBatchIdClick(row.entity)" ng-bind="row.entity.batchId"></a>'
                        + '</div>'
                    }
                ];

                $scope.view.gridOptions = {
                    paginationPageSizes: [defaultPageSize, 25, 50, 75],
                    paginationPageSize: defaultPageSize,
                    useExternalPagination: true,
                    useExternalSorting: true,
                    useExternalFiltering: true,
                    enableFiltering: true,
                    multiSelect: false,
                    enableGridMenu: true,
                    gridMenuCustomItems: [
                      {
                          title: 'Refresh',
                          action: function ($event) {
                              getSevisCommStatusesPage();
                          },
                          order: 210
                      }
                    ],
                    columnDefs: $scope.columns,
                    onRegisterApi: function (gridApi) {
                        $scope.gridApi = gridApi;
                        $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                            UiGridFilterService.setSort(grid, sortColumns);
                            getSevisCommStatusesPage();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                            UiGridFilterService.setPagination(newPage, pageSize);
                            getSevisCommStatusesPage();
                        });
                        $scope.gridApi.core.on.filterChanged($scope, function () {
                            var grid = this.grid;
                            UiGridFilterService.setFilters(grid);
                            getSevisCommStatusesPage();
                        });
                    }
                };

                var sevisInfoCopy = null;
                var notifyStatuses = ConstantsService.sevisStatusIds.split(',');
                var projectId = 0;
                var participantId = $scope.participantid;

                $scope.$watch(function () {
                    return $scope.sevisinfo;
                }, function (newValue, oldValue) {
                    if (newValue && !sevisInfoCopy) {
                        sevisInfoCopy = angular.copy(newValue);
                        projectId = newValue.projectId;
                    } if (newValue != oldValue) {
                        getSevisCommStatusesPage();
                    }
                });

                $scope.$watch(function () {
                    return $scope.view.DHSStatus;
                }, function (newValue, oldValue) {
                    if (newValue && newValue != oldValue) {
                        getSevisCommStatusesPage();
                    }
                });

                $scope.$on("$destroy", function (event) {
                    if (angular.isDefined($scope.pageTimeout)) {
                        $timeout.cancel($scope.pageTimeout);
                    }
                });

                $scope.downloadDS2019 = function () {
                    var url = 'Project/' + projectId + '/ParticipantPersonSevis/' + $scope.sevisinfo.participantId + '/DS2019File';
                    DownloadService.get(url, 'application/pdf')
                    .then(function () {

                    }, function () {
                        NotificationService.showErrorMessage('Unable to download file.');
                    });
                }

                $scope.printDS2019 = function () {
                    var url = 'Project/' + projectId + '/ParticipantPersonSevis/' + $scope.sevisinfo.participantId + '/DS2019File';
                    DownloadService.print(url, 'application/pdf')
                    .then(function () {

                    }, function () {
                        NotificationService.showErrorMessage('Unable to print file.');
                    });
                }

                $scope.view.showNextSevisCommStatuses = function () {
                    $scope.view.maxNumberOfSevisCommStatusesToShow += $scope.view.sevisCommStatusesPageSize;
                }

                $scope.edit.onDosStatusChange = function ($event, checkboxId, checked) {
                    if (!$scope.sevisinfo.blockEdit) {
                        $scope.sevisinfo[checkboxId] = checked;
                        $scope.updatesevisinfo({ participantId: $scope.participantid });
                        sevisInfoCopy = angular.copy($scope.sevisinfo);
                    } else {
                        return false;
                    }
                }

                $scope.edit.onStartDateChange = function () {
                    var form = $scope.form.startDate;
                    onFormDateChange(form, "startDate");
                }

                $scope.edit.openStartDatePicker = function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();
                    $scope.edit.isStartDatePickerOpen = true;
                }

                $scope.edit.onEndDateChange = function () {
                    var form = $scope.form.endDate;
                    onFormDateChange(form, "endDate");
                }

                $scope.edit.openEndDatePicker = function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();
                    $scope.edit.isEndDatePickerOpen = true;
                }

                $scope.getSevisStartDateDivId = function (participantId) {
                    return 'sevisStartDate' + participantId;
                }

                $scope.onBatchIdClick = function (commStatus) {
                    var modal = $modal.open({
                        animation: true,
                        templateUrl: 'app/projects/sevis-batch-processing-info-modal.html',
                        controller: 'SevisBatchProcessingInfoModalCtrl',
                        size: 'lg',
                        resolve: {
                            projectId: function () {
                                return projectId;
                            },
                            sevisCommStatus: function () {
                                return commStatus;
                            }
                        }
                    });
                    modal.result.then(function (addedLocation) {
                        $log.info('Finished viewing sevis batch info.');
                        modal.close();

                    }, function () {
                        $log.info('Modal dismissed at: ' + new Date());
                    });
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
                    else if (errorTypeId == ConstantsService.sevisErrorType.dependent.id) {
                        var dependentPersonId = error.customState.personDependentId;
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
                    if (!$scope.sevisinfo.blockEdit) {
                        $scope.view.FundingEdit = true;
                        $scope.view.GovtAgency1Other = ($scope.exchangevisitorinfo.govtAgency1Id == ConstantsService.otherUSGovernmentAgencyId);
                        $scope.view.GovtAgency2Other = ($scope.exchangevisitorinfo.govtAgency2Id == ConstantsService.otherUSGovernmentAgencyId);
                        $scope.view.IntlOrg1Other = ($scope.exchangevisitorinfo.intlOrg1Id == ConstantsService.otherInternationalOrganizationId);
                        $scope.view.IntlOrg2Other = ($scope.exchangevisitorinfo.intlOrg2Id == ConstantsService.otherInternationalOrganizationId);
                    } else {
                        return false;
                    }
                };

                $scope.edit.onPositionAndFieldEditChange = function () {
                    if (!$scope.sevisinfo.blockEdit) {
                        $scope.view.PositionAndFieldEdit = true;
                        loadFieldOfStudies($scope.exchangevisitorinfo.fieldOfStudy);
                    } else {
                        return false;
                    }
                }

                function getSevisCommStatusesPage() {
                    if (angular.isDefined($scope.pageTimeout)) {
                        $timeout.cancel($scope.pageTimeout);
                    }

                    $scope.pageTimeout = $timeout(function () {
                        var params = UiGridFilterService.getParams();
                        return ParticipantPersonsSevisService.getSevisCommStatuses(projectId, $scope.participantid, params)
                        .then(function (response) {
                            $scope.view.gridOptions.totalItems = response.data.total;
                            $scope.view.gridOptions.data = response.data.results;
                            return response.data.results;
                        })
                        .catch(function (response) {
                            var message = "Unable to load sevis comm statuses for the participant.";
                            $log.error(message);
                            NotificationService.showErrorMessage(message);
                        })
                    }, 500);
                }

                function onFormDateChange(form, sevisInfoPropertyName) {
                    if (form.$valid) {
                        if (!$scope.sevisinfo.blockEdit) {
                            $scope.updatesevisinfo({ participantId: $scope.participantid });
                            sevisInfoCopy = angular.copy($scope.sevisinfo);
                        } else {
                            return false;
                        }
                    }
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

                // IE and Edge do not support printing
                $scope.browserSupportsMsSaveOrOpenBlob = function () {
                    var msSaveOrOpenBlob = false;
                    if (window.navigator.msSaveOrOpenBlob) {
                        msSaveOrOpenBlob = true;
                    }
                    return msSaveOrOpenBlob;
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
                        fieldOfStudiesFilter = fieldOfStudiesFilter.keywords(search);
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

 /**
 * AngularJS default filter with the following expression:
 * "person in people | filter: {name: $select.search, age: $select.search}"
 * performs an AND between 'name: $select.search' and 'age: $select.search'.
 * We want to perform an OR.
 added by efren zamora
 */
                function propsFilter()
                {
                    return function (items, props) {
                        var out = [];

                        if (angular.isArray(items)) {
                            var keys = Object.keys(props);

                            items.forEach(function (item) {
                                var itemMatches = false;

                                for (var i = 0; i < keys.length; i++) {
                                    var prop = keys[i];
                                    var text = props[prop].toLowerCase();
                                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                                        itemMatches = true;
                                        break;
                                    }
                                }

                                if (itemMatches) {
                                    out.push(item);
                                }
                            });
                        } else {
                            // Let the output be the input untouched
                            out = items;
                        }

                        return out;
                    };
                }

                function loadSevisCommStatuses() {
                    var commStatusFilter = FilterService.add('project-participant-editSevis-allseviscommstatuses');
                    commStatusFilter = commStatusFilter.skip(0).take(limit);
                    return LookupService.getSevisCommStatuses(commStatusFilter.toParams())
                    .then(function (response) {
                        if (response.data.total > limit) {
                            var message = "The number of sevis comm statuses loaded is less than the total number.  Some sevis comm statuses may not be shown."
                            NotificationService.showErrorMessage(message);
                            $log.error(message);
                        }
                        angular.forEach(response.data.results, function (result, index) {
                            result.value = result.id;
                            result.label = result.name;
                        });
                        $scope.edit.sevisCommStatuses = response.data.results;
                        $scope.columns[1].filter.selectOptions = $scope.edit.sevisCommStatuses;
                        return $scope.edit.sevisCommStatuses;
                    })
                    .catch(function (response) {
                        var message = "Unable to load sevis comm statuses.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                }

                loadPositions();
                loadProgramCategories();
                loadUSGovernmentAgencies();
                loadInternationalOrganizations();
                loadSevisCommStatuses();
            }
        };

        return directive;
    }
})();

