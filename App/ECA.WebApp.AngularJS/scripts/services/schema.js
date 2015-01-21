'use strict';

/**
 * @ngdoc service
 * @name staticApp.schema
 * @description
 * # schema
 * Service in the staticApp.
 */
angular.module('staticApp')
  .factory('schemaservice', ['$http',function($http){
  	var schemaHolder = {};

  	var getEntity = function(entityName){
  		return schemaHolder[entityName];

  	};

  	var getField = function(entityName,fieldName) {
  		console.log(schemaHolder[entityName].fieldMap[fieldName]);
  		return schemaHolder[entityName].fieldMap[fieldName];
  	};

	$http.get('data/schema.json')
       .then(function(res){
       		// translate the entity array to a map so that we don't have to do
			var parseSchemaMap = function(element) {
				var e = {};
				e.fieldMap = {};
				e.description = element.description;
				e.name = element.name;
				e.type = element.type;
				var fieldmap = function(field){
					e.fieldMap[field.field.toLowerCase()] = field;
				};

				element.fields.forEach(fieldmap);

				schemaHolder[e.name.toLowerCase()] = e;
			};


			var entities = res.data; 

			entities.forEach(parseSchemaMap);

        });

  	return {
  		getEntity: getEntity,
  		getField: getField
  	};
  }]);
