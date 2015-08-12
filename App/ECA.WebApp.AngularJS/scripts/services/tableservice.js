'use strict';

/**
 * Factory for paging, sorting, and filtering
 */
angular.module('staticApp')
    .factory('TableService', function (ConstantsService) {

        var tableState;
        
        function setTableState(state) {
            tableState = state;
        }

        function getStart() {
            var pagination = tableState.pagination;
            var start = pagination.start || 0;
            return start;
        }

        function getLimit() {
            var pagination = tableState.pagination;
            var limit = pagination.number || 25;
            return limit;
        }

        function getSort() {
            var sort = [];
            var predicate = tableState.sort.predicate;
            var reverse = tableState.sort.reverse;
            if (predicate !== null && reverse !== undefined) {
                sort.push({
                    property: predicate.replace(/'/g, ""),
                    direction: reverse === false ? "asc" : "desc"
                })
            }
            return sort;
        }

        function getFilter() {
            var filter = [];
            var predicateObject = tableState.search.predicateObject;
            if (predicateObject !== undefined) {
                for (var key in predicateObject) {
                    if (predicateObject[key].comparison) {
                        var comparisonType = predicateObject[key].comparison;
                        var newFilter = {
                            comparison: comparisonType,
                            property: key.replace(/'/g, ""),
                        };                        
                        if (comparisonType === ConstantsService.containsAnyComparisonType) {
                            var ids = predicateObject[key].ids;
                            if (ids.length > 0) {
                                newFilter.value = ids;
                                filter.push(newFilter);
                            }
                        }
                        else if (comparisonType === ConstantsService.inComparisonType) {
                            var ids = predicateObject[key].ids;
                            if (ids.length > 0) {
                                newFilter.value = ids;
                                filter.push(newFilter);
                            }
                        }
                    }
                    else if (key !== '$') { //The $ key is used for the search form fields
                        filter.push({
                            property: key.replace(/'/g, ""),
                            value: predicateObject[key],
                            comparison: ConstantsService.likeComparisonType
                        });
                    }
                }
            }
            return filter;
        }

        function getKeywords() {
            var keywords = [];
            var predicateObject = tableState.search.predicateObject;
            if (predicateObject && predicateObject.$)
            {
                var searchDelimiter = ConstantsService.searchDelimiter;
                return predicateObject.$.split(searchDelimiter);
            }
            
            else {
                return null;
            }
        }

        return {
            setTableState: setTableState,
            getStart: getStart,
            getLimit: getLimit,
            getSort: getSort,
            getFilter: getFilter,
            getKeywords: getKeywords
        };
    });