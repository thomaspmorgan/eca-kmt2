'use strict';

angular.module('staticApp').filter('groupBy',
    function () {
        var uniqueItems = function (data, key) {
            var result = [];
            for (var i = 0; i < data.length; i++) {
                var value = data[i][key];
                if (result.indexOf(value) == -1) {
                    result.push(value);
                }
            }
            return result;
        };

        return function (collection, key) {
            if (collection === null) return;
            return uniqueItems(collection, key);
        };
    });