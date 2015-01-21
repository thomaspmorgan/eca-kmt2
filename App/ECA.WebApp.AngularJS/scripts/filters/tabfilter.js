'use strict';

/**
 * @ngdoc filter
 * @name staticApp.filter:tabFilter
 * @function
 * @description
 * # tabFilter
 * Filter in the staticApp.
 */
angular.module('staticApp')
  .filter('tabFilter', function () {
    return function (input) {
    	var output = [];
    	angular.forEach(input, function (val, key) {
    		if (val.active) {
    			output.push(val);
    		}
    	});
      return output;
    };
  });
