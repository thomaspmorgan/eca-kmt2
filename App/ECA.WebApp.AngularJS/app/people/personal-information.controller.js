'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
  .run(['$anchorScroll', function ($anchorScroll) {
       $anchorScroll.yOffset = -200;   // always scroll by 100 less pixels
  }])
  .controller('PersonInformationCtrl', function ($scope, $stateParams, $location, $timeout, $anchorScroll) {

      $scope.showEvalNotes = true;
      $scope.showEduEmp = true;
      $scope.showGeneral = true;
      $scope.showPii = false;
      $scope.showContact = true;
      $scope.edit = {};
      $scope.edit.General = false;
      $scope.edit.Pii = false;
      $scope.edit.Contact = false;
      $scope.edit.EduEmp = false;
      
      // SEVIS validation: expand section and set active tab where error is located.
      $scope.$on('$viewContentLoaded', function () {

          var section = $stateParams.section;

          if (section)
          {
              switch (section) {
                  case "general":
                      $scope.showGeneral = true;
                      break;
                  case "pii":
                      $scope.showPii = true;
                      break;
                  case "contact":
                      $scope.showContact = true;
                      break;
                  case "eduemp":
                      $scope.showEduEmp = true;
                      break;
              }

              $timeout(function () {
                    $location.hash(section);
                    $anchorScroll();
              });
          }
      });

  });
