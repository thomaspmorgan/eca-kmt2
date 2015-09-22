'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:yearDropdown
 * @description
 * # lists years in a dropdown according to parameters
 * 
 * Usage: <div year-dropdown model="fiscalYear" offset="-2" range="10"></div>
 * 
 */
angular.module('staticApp', []).directive('yearDropdown', function () {
    var currentYear = new Date().getFullYear();
    return {
        scope: {
            model: "=ngModel"
        },
        require: 'ngModel',
        replace: true,
        link: function (scope, element, attrs) {
            scope.years = [];
            for (var i = +attrs.offset; i < +attrs.range + 1; i++) {
                scope.years.push(currentYear + i);
            }
            scope.bar = currentYear;
        },
        template: '<select ng-model="bar" ng-options="y for y in years"></select>'
    }
});
