'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:yearDropdown
 * @description
 * # lists years in a dropdown according to parameters
 * 
 * Usage: <year-dropdown name="fiscalYear" id="fiscalYear" ng-model="moneyFlow.fiscalYear" offset="-10" range="2" class="yearDropdown" required></year-dropdown>
 * offset: years in the past to include.
 * range: years in the future to include.
 */
angular.module('staticApp')
    .directive('yearDropdown', function () {
        var currentYear = new Date().getFullYear();
        return {
            restrict: 'E',
            scope: {
                model: "="
            },
            template: function (element, attrs) {
                return '<select name="' + attrs.name + '" id="' + attrs.id + '" ng-model="' + attrs.model + '" ng-change="handleChange(' + attrs.model + ')" class="form-control">' +
                '<option ng-selected="{{model == year}}" ng-repeat="year in years" value="{{year}}">{{year}}</option>' +
                '</select>';
            },
            link: function (scope, element, attrs) {
                scope.years = [];
                for (var i = +attrs.offset; i < +attrs.range + 1; i++) {
                    scope.years.push(currentYear + i);
                }
                
                element.val(scope.model);
                element.data('old-value', scope.model);

                scope.$watch('model', function () {
                    element.val(scope.model);
                });
                
                scope.handleChange = function (newyear) {
                    if (element.data('old-value') !== element.val()) {
                        scope.model = newyear;
                        element.data('old-value', element.val());
                        attrs.ngSelected = newyear;
                    }
                }

            }
        }
    });
