'use strict';


angular.module('staticApp')
    .filter('highlight', function () {
        return function (text, search, caseSensitive) {
            var searchTerms = [];
            if (!angular.isArray(search)) {
                searchTerms = [search];
            }
            else {
                searchTerms = search;
            }
            text = text.toString();
            for (var i = 0; i < searchTerms.length; i++) {

                var searchTerm = searchTerms[i];
                if (text && (searchTerm || angular.isNumber(searchTerm))) {
                    
                    searchTerm = searchTerm.toString();
                    if (caseSensitive) {
                        text = text.split(searchTerm).join('<strong>' + searchTerm + '</strong>');
                    }
                    else {
                        text = text.replace(new RegExp(searchTerm, 'gi'), '<strong>$&</strong>');
                    }
                }
            }
            return text;
        };
    });