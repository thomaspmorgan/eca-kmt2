'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AllpeopleCtrl
 * @description
 * # AllpeopleCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllPeopleCtrl', function ($scope, DragonBreath, $stateParams) {
    $scope.people = [];
    $scope.currentpage = $stateParams.page || 1;
    $scope.limit = 200;

    var filterParams = {
        limit: $scope.limit,
        offset: (($scope.currentpage - 1) * 20)
    };



    DragonBreath.get(filterParams,'people')
        .success(function (data) {
            console.log(data);
            if(angular.isArray(data.results)){
                $scope.people = data.results;
            }
            
        });
  });
