'use strict';
/*global Datamap */
/**
 * @ngdoc directive
 * @name staticApp.directive:map
 * @description
 * # map
 */
angular.module('staticApp')
  .directive('map', function () {
    return {
      template: '<div></div>',
      restrict: 'E',
      scope: {
      	countries: '='
      },
      link: function postLink(scope, element, attrs) {
        var activecolor = attrs.activecolor || '3f4d8f';

      	// style the div container
      	element.children().first().css({
      		width: attrs.width + 'px',
      		height: attrs.height + 'px',
      		      		//position: 'relative'
      	});

      	// add a map to the div inside the <map> tag
        var map = new Datamap({
        	element: element[0].firstChild,
        	fills: {
            	defaultFill: 'rgb(200,200,200)',
            	active: '#'+activecolor //any hex, color name or rgb/rgba value
        	},
	        geographyConfig: {
	            highlightOnHover: false,
	            popupOnHover: false
	        },
	        data: {}
	        
        });

        // this populates the map with active countries
      	var populateCountries = function(countrylist) {
      		var countries = {};
	      	var builder = function(ele){
	      		countries[ele.abbreviation] = {fillKey: 'active'};
	      	};
      		countrylist.forEach(builder);

      		map.updateChoropleth(countries);
      	};

      	// initialize the map
        if (scope.countries) {
      		populateCountries(scope.countries);
        }

  		// when new countries come in, repopulate the map
  		scope.$watch('countries',function(newVal, oldVal){
        if (newVal !== oldVal) {
    			populateCountries(scope.countries);
        }
  		});
      }
    };
  });
