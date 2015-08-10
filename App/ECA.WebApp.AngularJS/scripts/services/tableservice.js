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
            var filters = [];
            var predicateObject = tableState.search.predicateObject;
            if (predicateObject !== undefined) {
                for (var key in predicateObject) {
                    if (Array.isArray(predicateObject[key])) {
                        var property = key;
                        angular.forEach(predicateObject[key], function (predicateObjectFilter, index) {
                            var newFilter = getSingleFilter(predicateObjectFilter, property);
                            if (newFilter.isValid) {
                                filters.push(newFilter);
                            }
                        });
                    }
                    else if (predicateObject[key].comparison) {
                        var property = key;
                        var newFilter = getSingleFilter(predicateObject[key], property);
                        if (newFilter.isValid) {
                            filters.push(newFilter);
                        }
                    }
                    else if (key !== '$') { //The $ key is used for the search form fields
                        filters.push({
                            property: key.replace(/'/g, ""),
                            value: predicateObject[key],
                            comparison: ConstantsService.likeComparisonType
                        });
                    }
                }
            }
            angular.forEach(filters, function (filter, index) {
                delete filter.isValid;
            });
            return filters;
        }

        function getSingleFilter(predicateObjectFilter, property) {
            if (!predicateObjectFilter.comparison) {
                throw Error('The comparison must be defined.');
            }
            if (!property) {
                throw Error('The property is not defined.');
            }
            var comparisonType = predicateObjectFilter.comparison;
            var newFilter = {
                comparison: comparisonType,
                property: property.replace(/'/g, ""),
                isValid: false
            };
            if (comparisonType === ConstantsService.containsAnyComparisonType) {
                if (!predicateObjectFilter.ids) {
                    throw Error("The contains any comparison type requires a property named 'ids' with an array of numeric values.");
                }
                var ids = predicateObjectFilter.ids;
                if (ids.length > 0) {
                    newFilter.value = ids;
                    newFilter.isValid = true;
                }
            }
            else if (comparisonType === ConstantsService.inComparisonType) {                
                if (!predicateObjectFilter.ids) {
                    throw Error("The contains any comparison type requires a property named 'ids' with an array of numeric values.");
                }
                var ids = predicateObjectFilter.ids;
                if (ids.length > 0) {
                    newFilter.value = ids;
                    newFilter.isValid = true;
                }
            }
            else if (comparisonType === ConstantsService.lessThanComparisonType
                || comparisonType === ConstantsService.greaterThanComparisonType
                || comparisonType === ConstantsService.equalComparisonType) {
                if (!predicateObjectFilter.hasOwnProperty('value')) {
                    throw Error("The equality filters require a property named 'value' with a single numeric value.");
                }
                var value = predicateObjectFilter.value;
                newFilter.value = value;
                newFilter.isValid = true;
            }
            else {
                throw Error('The comparison type [' + comparisonType + '] is not yet supported.');
            }
            return newFilter;
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