﻿'use strict';


angular.module('staticApp')
    .filter('highlight', function () {
        return function (text, search, caseSensitive) {
            var searchTerms = [];
            search = search + '';

            if (search && search.length > 0) {
                searchTerms = search.split(" ");
            }

            if (text && text.length > 0) {
                text = text.toString();
            }
            
            if (!angular.isArray(searchTerms)) {
                searchTerms = search;
            }

            for (var i = 0; i < searchTerms.length; i++) {

                var searchTerm = searchTerms[i];
                if (text && (searchTerm || angular.isNumber(searchTerm))) {
                    
                    searchTerm = searchTerm.toString();
                    if (searchTerm.length > 1) {
                        if (caseSensitive) {
                            text = text.split(searchTerm).join('<strong>' + searchTerm + '</strong>');
                        }
                        else {
                            text = text.replace(new RegExp(searchTerm, 'gi'), '<strong>$&</strong>');
                        }
                    }                    
                }
            }
            return text;
        };
    });