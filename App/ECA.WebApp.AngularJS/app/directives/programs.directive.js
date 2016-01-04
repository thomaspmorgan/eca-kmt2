(function () {
    'use strict';

    angular
        .module('staticApp')
        .directive('programs', programs);

    programs.$inject = [
        'smoothScroll',
        '$stateParams',
        '$log',
        '$q',
        'AuthService',
        'TableService',
        'FilterService',
        'ConstantsService',
        'ProgramService',
        'NotificationService'];

    function programs(smoothScroll, $stateParams, $log, $q, AuthService, TableService, FilterService, ConstantsService, ProgramService, NotificationService) {

        var directive = {
            restrict: 'E',
            replace: false,
            scope: {
                officeid: '=',
                showdraftsfilter: '='
            },
            templateUrl: 'app/directives/programs.directive.html',
            controller: function ($scope, $attrs) {
                var limit = 300;
                $scope.view = {};
                $scope.view.programs = [];
                $scope.view.loadingPrograms = false;
                $scope.view.hierarchyKey = "hierarchy";
                $scope.view.alphabeticalKey = "alpha";
                $scope.view.listType = $scope.view.hierarchyKey;

                $scope.view.onSearchChange = function () {
                    $scope.view.listType = $scope.view.alphabeticalKey;
                };

                $scope.view.onProgramFiltersChange = function () {
                    console.assert($scope.getAllProgramsTableState, "The table state function must exist.");
                    $scope.view.programFilter = '';
                    var tableState = $scope.getAllProgramsTableState();
                    return $scope.view.getPrograms(tableState);
                }

                $scope.view.onExpandClick = function (program) {
                    program.isExpanded = true;
                    loadChildrenPrograms(program)
                    .then(function (childPrograms) {
                        if (childPrograms.length > 0) {
                            scrollToProgram(program);
                        }
                        else {
                            $log.info('Program has no children.');
                        }
                    });
                }

                $scope.view.onCollapseClick = function (program) {
                    program.isExpanded = false;
                    removeChildrenPrograms(program);
                    scrollToProgram(program);
                }

                $scope.view.onScrollToParentClick = function (program) {
                    if (program.parent) {
                        scrollToProgram(program.parent);
                    }
                }

                $scope.view.getPrograms = function (tableState) {
                    //remove keyword search parameter if viewing programs in hiearchy
                    if ($scope.view.listType === $scope.view.hierarchyKey && tableState.search && tableState.search.predicateObject) {
                        delete tableState.search.predicateObject.$;
                    }

                    TableService.setTableState(tableState);
                    var params = {
                        start: TableService.getStart(),
                        limit: TableService.getLimit(),
                        sort: TableService.getSort(),
                        filter: TableService.getFilter(),
                        keyword: TableService.getKeywords()
                    };

                    if (!params.filter) {
                        params.filter = [];
                    }

                    if ($scope.view.showDraftsOnly) {
                        params.filter.push({ property: 'programStatusId', comparison: ConstantsService.equalComparisonType, value: ConstantsService.programStatus.draft.id });
                        params.filter.push({ property: 'createdByUserId', comparison: ConstantsService.equalComparisonType, value: $scope.view.ecaUserId });
                    }
                    else {
                        params.filter.push({ property: 'programStatusId', comparison: ConstantsService.notEqualComparisonType, value: ConstantsService.programStatus.draft.id });
                    }
                    var officeId = getOfficeId();
                    if (officeId !== null) {
                        params.filter.push({ property: 'owner_OrganizationId', comparison: ConstantsService.equalComparisonType, value: officeId });
                    }

                    if ($scope.view.listType === $scope.view.alphabeticalKey) {
                        return loadProgramsAlphabetically(params, tableState);
                    }
                    else {
                        return loadProgramsInHierarchy(params, tableState);
                    }
                };

                function removeChildrenPrograms(program) {
                    if (program.children) {
                        for (var i = 0; i < program.children.length; i++) {
                            var childProgram = program.children[i];
                            var childProgramIndex = $scope.view.programs.indexOf(childProgram);
                            $scope.view.programs.splice(childProgramIndex, 1);
                            removeChildrenPrograms(childProgram);
                        }
                        delete program.children;
                    }
                }

                function getOfficeId() {
                    var officeIdValue = $scope.officeid;
                    if (angular.isNumber(officeIdValue)) {
                        return officeIdValue;
                    }
                    else if (angular.isString(officeIdValue)) {
                        var stateParamsOfficeId = $stateParams[officeIdValue];
                        if (stateParamsOfficeId) {
                            return parseInt(stateParamsOfficeId, 10);
                        }
                        else {
                            return parseInt(officeIdValue, 10);
                        }
                        
                    }
                    else {
                        return null;
                    }
                }

                function loadProgramsAlphabetically(params, tableState) {
                    $scope.view.loadingPrograms = true;
                    return ProgramService.getAllProgramsAlpha(params)
                    .then(function (data) {
                        angular.forEach(data.results, function (program, index) {
                            program.isRoot = true;
                        });
                        processData(data, tableState, params);
                    })
                    .catch(function (response) {
                        var message = "Unable to load programs.";
                        $log.error(message);
                        NotificationService.showErrorMessage(message);
                    });
                };

                function loadProgramsInHierarchy(params, tableState) {
                    $scope.view.loadingPrograms = true;
                    return ProgramService.getAllProgramsHierarchy(params)
                    .then(function (response) {
                        processData(response, tableState, params);
                    });
                };

                var childrenProgramsFilter = FilterService.add('programs_directive_childrenfilter');
                function loadChildrenPrograms(parentProgram) {
                    childrenProgramsFilter.reset();
                    childrenProgramsFilter = childrenProgramsFilter.skip(0).take(limit).sortBy("name");
                    var params = childrenProgramsFilter.toParams();
                    parentProgram.loadingChildrenPrograms = true;
                    return ProgramService.getAllProgramsHierarchy(params, parentProgram.programId)
                    .then(function (response) {
                        parentProgram.loadingChildrenPrograms = false;
                        return processChildProgramData(parentProgram, response, params);
                    });
                }

                function processChildProgramData(parentProgram, response, params) {
                    var parentProgramIndex = $scope.view.programs.indexOf(parentProgram);
                    var childPrograms = [];
                    var totalChildPrograms = 0;
                    if (response.data) {
                        childPrograms = response.data.results;
                        totalChildPrograms = response.data.total;
                    }
                    else {
                        childPrograms = response.results;
                        totalChildPrograms = response.total;
                    }
                    if (totalChildPrograms > limit) {
                        var message = "Unable to load all child programs.  Child programs count exceeds max.";
                        NotificationService.showErrorMessage(message);
                        $log.error(message);
                    }
                    angular.forEach(childPrograms, function (childProgram, childProgramIndex) {
                        setProgramDivId(childProgram);
                        $scope.view.programs.splice(parentProgramIndex + 1 + childProgramIndex, 0, childProgram);
                        childProgram.parent = parentProgram;
                    });
                    
                    parentProgram.children = childPrograms;
                    return childPrograms;
                }

                function processData(response, tableState, params) {
                    var programs = null;
                    var total = null;
                    if (response.data) {
                        programs = response.data.results;
                        total = response.data.total;
                    }
                    else {
                        programs = response.results;
                        total = response.total;
                    }

                    var start = 0;
                    if (programs.length > 0) {
                        start = params.start + 1;
                    };
                    var count = params.start + programs.length;
                    updatePagingDetails(total, start, count);
                    var limit = TableService.getLimit();
                    tableState.pagination.numberOfPages = Math.ceil(total / limit);

                    angular.forEach(programs, function (program, index) {
                        setProgramDivId(program);
                    });

                    $scope.view.programs = programs;
                    $scope.view.loadingPrograms = false;
                    return programs;
                };

                function setProgramDivId(program) {
                    program.divId = 'program' + program.programId;
                }

                function updatePagingDetails(total, start, count) {
                    $scope.view.totalNumberOfPrograms = total;
                    $scope.view.skippedNumberOfPrograms = start;
                    $scope.view.numberOfPrograms = count;
                };

                function scrollToProgram(program) {
                    var id = program.divId;
                    var toolbarDivs = document.getElementsByClassName('toolbar');
                    var additionalOffset = 0;
                    if (toolbarDivs.length > 0) {
                        angular.forEach(toolbarDivs, function (toolbarDiv, index) {
                            var angularElement = angular.element(toolbarDiv)[0];
                            additionalOffset += angularElement.offsetHeight;
                        });
                    }
                    var options = {
                        duration: 500,
                        easing: 'easeIn',
                        offset: 70 + additionalOffset,
                        callbackBefore: function (element) { },
                        callbackAfter: function (element) { }
                    }
                    var element = document.getElementById(id)
                    smoothScroll(element, options);
                }         

                function loadUserId() {
                    return AuthService.getUserInfo()
                        .then(function (response) {
                            $scope.view.ecaUserId = response.data.ecaUserId;
                            return response.data.ecaUserId;
                        })
                        .catch(function () {
                            $log.error('Unable to load user info.');
                        });
                }

                $q.all([loadUserId()])
                  .then(function (results) {

                  })
                  .catch(function () {

                  });
            }
        };

        return directive;
    }
})();
