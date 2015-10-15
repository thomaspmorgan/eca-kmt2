'use strict';


angular.module('staticApp')
    .filter('newline', function ($sce) {
        return function (text) {
            return text ? $sce.trustAsHtml(text.toString().replace(/(?:\\[rn]|[\r\n]+)+/g, '<br/>')) : '';
        }
    });