'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:formElement
 * @description
 * # formElement
 */

//from here:  http://stackoverflow.com/questions/24198669/angular-bootsrap-datepicker-date-format-does-not-format-ng-model-value
//the angularjs datepicker has a bug in it that does not format a date from the server before being shown in the control
angular.module('staticApp').directive('datepickerPopup', function () {
    return {
        restrict: 'EAC',
        require: 'ngModel',
        link: function (scope, element, attr, controller) {
            //remove the default formatter from the input directive to prevent conflict
            controller.$formatters.shift();
        }
    }
});