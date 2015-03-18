'use strict';

/**
 * Factory for paging, sorting, and filtering
 */
angular.module('staticApp')
    .factory('TableService', function () {

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
            var limit = pagination.limit || 25;
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
                    if (key !== '$') { //The $ key is used for the search form fields
                        filter.push({
                            property: key.replace(/'/g, ""),
                            value: predicateObject[key],
                            comparison: 'like'
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
                return predicateObject.$.split(' ');
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