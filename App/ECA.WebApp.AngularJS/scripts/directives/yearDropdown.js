'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:yearDropdown
 * @description
 * # lists years in a dropdown according to parameters
 * 
 * Usage: <div year-dropdown ng-model="fiscalYear" offset="-10" range="2"></div>
 * 
 */
angular.module('staticApp')
    .directive('yearDropdown', function () {
        var currentYear = new Date().getFullYear();
        return {
            restrict: 'E',
            scope: {
                ngModel: "="
            },
            template: function (element, attrs) {
                return '<select ng-model="' + attrs.ngModel + '" class="form-control">' +
                '<option ng-selected="{{ngModel == year}}" ng-repeat="year in years" value="{{year}}">{{year}}</option>' +
                '</select>';
            },
            link: function (scope, element, attrs) {
                scope.years = [];
                for (var i = +attrs.offset; i < +attrs.range + 1; i++) {
                    scope.years.push(currentYear + i);
                }
            }
        }
    });
