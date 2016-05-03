// Filter for truncating long text values and appending an ellipsis (...)

'use strict';

angular.module('staticApp').filter('truncateLongText', function () {
    return function (value) {
        if (value && value.length > 35) {
            value = value.substring(0, 35);

            var lastspace = value.lastIndexOf(' ');
            if (lastspace !== -1) {
                value = value.substr(0, lastspace);
            }

            return value + '…';
        }
        return value;
    };
});