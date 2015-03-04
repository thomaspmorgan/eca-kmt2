'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllProgramsCtrl', function ($scope, $stateParams, ProgramService, LookupService, TableService) {
      
      $scope.today = function () {
          $scope.startDate = new Date();
      };
      $scope.today();

      $scope.alerts = [];

      $scope.themes = [];

      $scope.goals = [];

      $scope.regions = [
          {
             regionId: 1,
             regionName: 'Africa'
          },
          {
              regionId: 2,
              regionName: 'East Asia and the Pacific'
          },
          {
              regionId: 3,
              regionName: 'Europe and Eurasia'
          },
          {
              regionId: 4,
              regionName: 'Near East'
          },
          {
              regionId: 5,
              regionName: 'South and Central Asia'
          },
          {
              regionId: 6,
              regionName: 'Western Hemisphere'
          }
      ];

      $scope.foci = [
          {
              focusId: 1,
              focusName: 'Focus One'
          },
          {
              focusId: 2,
              focusName: 'Focus Two'
          }
      ];

      $scope.pointsOfContact = [
	        {
	            contactId: 1,
	            contactName: 'Jack P Diddly'
	        },
	        {
	            contactId: 2,
	            contactName: 'Steven Cobert'
	        }
      ];

      // initialize new Program record
      $scope.newProgram = {
          name: '',
          description: '',
          parentProgram: '',
          startDate: new Date(),
          themes: [],
          goals: [],
          regions: [],
          focus: '',
          pointsOfContact: [],
          website: 'http://'
      };

    $scope.programs = [];

    $scope.programsLoading = false;

    $scope.lookupParams = {
        start: null,
        limit: 100,
        sort: null,
        filter: null
    };

    LookupService.getAllThemes($scope.lookupParams)
        .then(function (data) {
            $scope.themes = data.results;
        });

    LookupService.getAllGoals($scope.lookupParams)
      .then(function (data) {
          $scope.goals = data.results;
      })
      // don't know why, but I need to access the scope variable for the data to load correctly
    var x = $scope.themes[1];
    x = $scope.goals[1];

    $scope.getPrograms = function (tableState) {

        $scope.programsLoading = true;

        TableService.setTableState(tableState);

        var params = {
            start: TableService.getStart(),
            limit: TableService.getLimit(),
            sort: TableService.getSort(),
            filter: TableService.getFilter()

        };

        ProgramService.getAllPrograms(params)
        .then(function (data) {
            $scope.programs = data.results;
            var limit = TableService.getLimit();
            tableState.pagination.numberOfPages = Math.floor(data.total / limit);
            $scope.programsLoading = false;
        });
    };

      // modal form
    $scope.modalClose = function () {
        if (unsavedChanges()) {
           $scope.modal.confirmClose = true;
        }
    };
      // Need to have more added to function
    function unsavedChanges() {
        var unsavedChanges = false;
        if ($scope.newProgram.name.length > 0 || $scope.newProgram.description.length > 0) {
            unsavedChanges = true;
        }
        return unsavedChanges;
    };

    $scope.modalClear = function () {
        angular.forEach($scope.newProgram, function (value, key) {
            $scope.newProgram[key] = ''
        });
        $scope.calClear()
    };

    $scope.updateProgram = function () {
        saveProgram();
    };


    function saveProgram() {
        ProgramService.update($scope.program, $stateParams.programId)
            .then(function (program) {
                $scope.program = program;
            });
    };

      // calendar popup for startDate
    $scope.calOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.calOpened = true;

    };

    $scope.calClear = function () {
        $scope.startDate = null;
    };

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    $scope.toggleMax = function () {
        var curDate = new Date();
        curDate = curDate.setFullYear(curDate.getFullYear() + 1);
        $scope.maxDate = curDate;
    }
    $scope.toggleMax();

    $scope.confirmCloseYes = function ()
    {
        $scope.modal.confirmClose = false;
        $scope.modal.createProgram = false;
    }

    $scope.confirmCloseNo = function ()
    {
        $scope.modal.createProgram = true;
        $scope.modal.confirmClose = false;
    }

  });
