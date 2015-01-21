'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOfficesCtrl', function ($scope, DragonBreath, $stateParams) {
    

    $scope.offices = [];
    $scope.currentpage = $stateParams.page || 1;
    $scope.limit = 200;

    var filterParams = {
        limit: $scope.limit,
        offset: (($scope.currentpage - 1) * 20)
    };



    DragonBreath.get(filterParams,'organizations')
        .success(function (data) {
            console.log(data);
            if(angular.isArray(data.results)){
                $scope.offices = data.results.sort(
                    function(office1, office2){
                        if (office1.name < office2.name) {
                            return -1;
                        }
                        if (office1.name > office2.name) {
                            return 1;
                        }
                        if (office1.abbreviation < office2.abbreviation){
                            return -1;
                        }
                        if (office1.abbreviation > office2.abbreviation) {
                            return 1;
                        }
                        return 0;
                    });
            }
            
        });




    $scope.branches = [
    	{
    		name: 'Africa & Europe',
    		ticked: false
    	},
    	{
    		name: 'WHA, EAP, NEA, SCA',
    		ticked: false
    	}
    ];
  });
