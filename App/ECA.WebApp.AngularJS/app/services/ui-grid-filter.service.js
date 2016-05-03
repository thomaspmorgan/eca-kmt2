'use strict';

/**
 * Factory for paging, sorting, and filtering
 */
angular.module('staticApp')
    .factory('UiGridFilterService', function (ConstantsService, uiGridConstants) {

        var _paginationOptions = {
            pageSize: 25,
            page: 1,
            filter: [],
            keyword: null,
            sort: null
        };

        function setPageSize(pageSize) {
            _paginationOptions.pageSize = pageSize
        }

        function setPage(page) {
            _paginationOptions.page = page;
        }

        function setSort(grid, sortColumns) {
            if (sortColumns.length == 0) {
                _paginationOptions.sort = null;
            }
            else {
                _paginationOptions.sort = { property: sortColumns[0].name, direction: sortColumns[0].sort.direction };
            }
        }

        function setPagination(newPage, pageSize) {
            _paginationOptions.page = newPage;
            _paginationOptions.pageSize = pageSize;
        }

        function setFilters(grid) {
            _paginationOptions.filter = [];
            angular.forEach(grid.columns, function (c, cIndex) {
                angular.forEach(c.filters, function (f, fIndex) {
                    var filterValue = f.term;
                    var property = c.name;
                    var comparison = ConstantsService.likeComparisonType;
                    
                    if (c.filter.type && filterValue) {
                        if (c.filter.type === uiGridConstants.filter.SELECT) {
                            comparison = ConstantsService.equalComparisonType;
                            _paginationOptions.filter.push({
                                property: c.filter.field || c.property,
                                comparison: comparison,
                                value: filterValue
                            });
                        }
                    }
                    else { // a like filter
                        if (filterValue && filterValue.length > 0) {
                            _paginationOptions.filter.push({
                                property: property,
                                comparison: comparison,
                                value: filterValue
                            });
                        }
                    }
                });
            });
        }

        function setKeyword(keyword) {
            if (keyword && keyword.length > 0) {
                _paginationOptions.keyword = keyword;
            }
            else {
                _paginationOptions.keyword = null;
            }
        }

        function getParams() {
            return {
                start: (_paginationOptions.page - 1) * _paginationOptions.pageSize,
                limit: ((_paginationOptions.page - 1) * _paginationOptions.pageSize) + _paginationOptions.pageSize,
                sort: _paginationOptions.sort,
                keyword: _paginationOptions.keyword,
                filter: _paginationOptions.filter
            }
        }

        return {
            getParams: getParams,
            setSort: setSort,
            setPageSize: setPageSize,
            setPagination: setPagination,
            setFilters: setFilters,
            setKeyword: setKeyword
        };
    });