'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramEditCtrl
 * @description
 * # ProgramEditCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllProgramsCtrl', function (
        $scope,
        $stateParams,
        $state,
        $log,
        $q,
        BrowserService
      ) {

      
      BrowserService.setAllProgramsDocumentTitle();
  });
