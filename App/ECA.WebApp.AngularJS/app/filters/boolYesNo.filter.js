// Filter for a boolean value that returns Yes for true and No for any other value or undefined

'use strict';

angular.module('staticApp').filter('boolYesNo', function () {
    return function (boolValue) {
        if (boolValue == true)
            return 'Yes';
        else
            return 'No';
    }
});

