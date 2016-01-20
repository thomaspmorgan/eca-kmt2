'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
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
              $timeout(function () {
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

                    $location.hash(section);
                    $anchorScroll();
              }, 500);
          }
      });

  });
