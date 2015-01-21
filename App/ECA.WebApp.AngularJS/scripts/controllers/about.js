'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AboutCtrl
 * @description
 * # AboutCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AboutCtrl', function ($scope,schemaservice) {

  	$scope.searchEntity = '';
  	$scope.searchField = '';

  	$scope.findEntity = function(){
  		$scope.field = {};
  		console.log(schemaservice);
  		$scope.entity = schemaservice.getEntity($scope.searchEntity);
  	};

  	$scope.findField = function(){
  		$scope.entity = schemaservice.getEntity($scope.searchEntity);
  		$scope.field = schemaservice.getField($scope.searchEntity,$scope.searchField);
  	};  	

  	$scope.entity = {};
  	$scope.field = {};

    $scope.inputs = [
      {
        label: 'label',
        placeholder: 'placehold',
        guidance: 'guidance'
      },
      {
        label: 'label',
        placeholder: 'placehold',
        guidance: 'guidance'
      }
    ];
  });
