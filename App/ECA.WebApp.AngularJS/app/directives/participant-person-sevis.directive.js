﻿(function () {
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
        'uiGridConstants',
        'ParticipantPersonsService'];

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
        uiGridConstants,
        ParticipantPersonsService) {
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
                $scope.positionAndFieldElementId = 'positionAndField' + $scope.participantid;
                $scope.pageTimeout = null;
                $scope.editLocked = true;

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
                        ParticipantPersonsService.getIsParticipantPersonLocked($scope.personid)
                         .then(function (response) {
                             $scope.editLocked = response.data;
                         });
                        $scope.view.PositionAndFieldEdit = false;
                    }
                });

                $scope.$watch(function () {
                    return $scope.view.DHSStatus;
                }, function (newValue, oldValue) {
                    if (newValue && newValue != oldValue) {
                        getSevisCommStatusesPage();
                    }
                });

                $scope.$watch(function () {
                    return $scope.exchangevisitorinfo;
                }, function (newValue, oldValue) {
                    if (newValue && newValue != oldValue) {
                        if (!$scope.edit.fieldOfStudies) {
                            $scope.edit.fieldOfStudies = [];
                        }

                        if (newValue.fieldOfStudyId) {
                            var ids = $scope.edit.fieldOfStudies.map(function (fos) { return fos.id; });
                            var idIndex = ids.indexOf(newValue.fieldOfStudyId);
                            if (idIndex == -1) {
                                $scope.edit.fieldOfStudies.push({
                                    description: newValue.fieldOfStudy,
                                    id: newValue.fieldOfStudyId
                                });
                            }
                        }
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

                $scope.savePositionAndField = function () {
                    $scope.updateexchangevisitorinfo({ participantId: $scope.participantid });
                    $scope.view.PositionAndFieldEdit = false;
                };

                $scope.edit.searchFieldOfStudies = function (search) {
                    return loadFieldOfStudies(search);
                };

                $scope.edit.onPositionAndFieldEditChange = function () {
                    if (!$scope.editLocked) {
                        $scope.view.PositionAndFieldEdit = true;
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

                // IE and Edge do not support printing
                $scope.browserSupportsMsSaveOrOpenBlob = function () {
                    var msSaveOrOpenBlob = false;
                    if (window.navigator.msSaveOrOpenBlob) {
                        msSaveOrOpenBlob = true;
                    }
                    return msSaveOrOpenBlob;
                };

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
                loadSevisCommStatuses();
            }
        };

        return directive;
    }
})();

