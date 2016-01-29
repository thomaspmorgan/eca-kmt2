'use strict';

/**
 * Controller for the person information page
 */
angular.module('staticApp')
  .controller('PersonInformationCtrl', function ($scope, $stateParams, $timeout, smoothScroll) {

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
                  var options = {
                      duration: 500,
                      easing: 'easeIn',
                      offset: 150,
                      callbackBefore: function (element) { },
                      callbackAfter: function (element) { }
                  }
                  smoothScroll(section, options);
              });
          }
      });

  });
