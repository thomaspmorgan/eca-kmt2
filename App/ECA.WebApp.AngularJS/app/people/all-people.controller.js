'use strict';

/**
 * @ngdoc function
* @name staticApp.controller:AllPersonsCtrl
 * @description
 * # AllPersonsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllPeopleCtrl', function ($scope, $stateParams, $state, $log, PersonService, TableService, BrowserService) {

      BrowserService.setAllPeopleDocumentTitle();
      $scope.persons = [];
      $scope.start = 0;
      $scope.end = 0;
      $scope.total = 0;

      $scope.personsLoading = false;
      $scope.selectedPersonType = {};

      $scope.onEditIconClick = function (person) {
          $state.go('participants.edit', { personId: person.personId });
      }

      $scope.onPersonClick = function (person) {

          if (person.personId) {
              $log.info('Navigating the individual state.');
              $state.go('participants.personalinformation', { personId: person.personId });
          }
          else {
              NotificationService.showErrorMessage('The person does not have an ID to navigate.');
          }
      };

      $scope.getPersons = function (tableState) {

          $scope.personsLoading = true;

          TableService.setTableState(tableState);

          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter()
          };

          PersonService.getPeople(params)
            .then(function (people) {
                $scope.persons = people.data.results;
                var limit = TableService.getLimit();
                var start = TableService.getStart();
                tableState.pagination.numberOfPages = Math.ceil(people.data.total / limit);
                $scope.start = start + 1;
                $scope.end = start + people.data.results.length;
                $scope.total = people.data.total;
                $scope.personsLoading = false;
            });
      };
  });