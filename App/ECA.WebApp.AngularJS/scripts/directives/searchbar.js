'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:searchbar
 * @description
 * # searchbar
 */
angular.module('staticApp')
  .directive('searchbar', ['solr', function (solr) {
    return {
      templateUrl: 'views/partials/searchbar.html',
      restrict: 'E',
      require: 'ngModel',
      scope: {},
      link: function postLink(scope, element, attrs, ngModelController) {
      	if (!ngModelController){ return;}
       scope.results = false;
       scope.text='';
       scope.show=true;
       scope.tophit = false;
       scope.firstrun = true;

       scope.autocomplete = function(){
       	scope.firstrun = true;
       	var res = solr.get(scope.text+'*', null, null,200);

       	var parseresults = function(results){
       		var formatElement = function(element){
       			return {
       				'id':element.Fields.docId,
       				'name':element.Fields.name,
       				'officeCode':element.Fields.officeCode,
       				'type':element.Fields.doctype
       			};
       		};
       		var res = results.data.Results.Collection;
       		if ((res.length === 0)&&(scope.firstrun === true)){
       			scope.firstrun = false;
       			var r2 = solr.get(scope.text,null,null,200);
       			r2.then(parseresults);
       			return;
       		} else if (res.length === 0) {
       			scope.results = false;
       			scope.tophit = false;
       		} 

       		var out = {};
       		scope.tophit = formatElement(res.shift());

       		var sorter = function(element){
       			out[element.Fields.doctype] = out[element.Fields.doctype] || [];
       			if (out[element.Fields.doctype].length < 10) {
       				out[element.Fields.doctype].push(formatElement(element));
       			}
       		};
       		res.forEach(sorter);

       		scope.results = out;
       	};

       	res.then(parseresults);
       };
      }
    };
  }]);
