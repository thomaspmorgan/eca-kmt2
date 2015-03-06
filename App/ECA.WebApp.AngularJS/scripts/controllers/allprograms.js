/// <reference path="D:\Tom\Source\Repos\ECA-KMT\App\ECA.WebApp.AngularJS\views/program/moneyflow.html.BASE.41042.html" />
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
      
      $scope.errorMessage = 'Unknown Error';
      $scope.validations = [];

      $scope.today = function () {
          $scope.startDate = new Date();
      };
      $scope.today();

      $scope.alerts = [];

      $scope.themes = [];
      $scope.goals = [];
      $scope.regions = [];
      $scope.pointsOfContact = [];
      $scope.foci = [];

      // initialize new Program record
      $scope.newProgram = {
          name: '',
          description: '',
          parentProgramId: null,
          ownerOrganizationId: 1,
          startDate: new Date(),
          themes: [],
          goals: [],
          regions: [],
          focusId: 0,
          contacts: [],
          website: 'http://'
      };

      $scope.outThemes = [];
      $scope.outRegions = [];
      $scope.outGoals = [];
      $scope.outContacts = [];

    $scope.programs = [];

    $scope.programsLoading = false;

    $scope.lookupParams = {
        start: null,
        limit: 100,
        sort: null,
        filter: null
    };

    $scope.regionsLookupParams = {
        start: null,
        limit: 10,
        sort: null,
        filter: [{ property: 'locationtypeid', comparison: 'eq', value: 2 }]
    };

    LookupService.getAllThemes($scope.lookupParams)
        .then(function (data) {
            $scope.themes = data.results;
        });

    LookupService.getAllGoals($scope.lookupParams)
      .then(function (data) {
          $scope.goals = data.results;
      });

    LookupService.getAllContacts($scope.lookupParams)
        .then(function (data) {
            $scope.pointsOfContact = data.results;
        });

    LookupService.getAllRegions($scope.regionsLookupParams)
        .then(function (data) {
            $scope.regions = data.results;
        });

    LookupService.getAllFocusAreas($scope.lookupParams)
        .then(function (data) {
             $scope.foci = data.results;
        });
      // don't know why, but I need to access the scope variable for the data to load correctly
    var x = $scope.themes[1];
    x = $scope.goals[1];
    x = $scope.pointsOfContact[1];
    x = $scope.regions[1];
    x = $scope.foci[1];

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
        else {
            $scope.modal.createProgram = false;
            return true;
        }
    };
      // Need to have more added to function
    function unsavedChanges() {
        var unsavedChanges = false;
        if ($scope.newProgram.$dirty) {
            unsavedChanges = true;
        }
        return unsavedChanges;
    };

    $scope.modalClear = function () {
        angular.forEach($scope.newProgram, function (value, key) {
            $scope.newProgram[key] = ''
        });
        $scope.calClear();
        $scope.newProgram.ownerOrganizationId = 1;
        $scope.newProgram.startDate = new Date();
    };

    //$scope.updateProgram = function () {
    //    saveProgram();
    //};


    //function saveProgram() {
    //    ProgramService.update($scope.program, $stateParams.programId)
    //        .then(function (program) {
    //            $scope.program = program;
    //        });
    //};
    
    function getIds(element) {
        return element.id;
    };
 
    function cleanUpNewProgram() {
        //themes
        $scope.newProgram.themes = $scope.outThemes.map(getIds);
        $scope.newProgram.goals = $scope.outGoals.map(getIds);
        $scope.newProgram.contacts = $scope.outContacts.map(getIds);
        $scope.newProgram.regions = $scope.outRegions.map(getIds);
    };


    $scope.createProgram = function () {
        cleanUpNewProgram();
        ProgramService.create($scope.newProgram)
            .then(function (program) {
                if (Array.isArray(program)) {
                    $scope.errorMessage = "There were one or more errors:";
                    $scope.validations = program;
                    $scope.modal.confirmFail = true;
                }
                else if (program.hasOwnProperty('Message')) {
                    $scope.errorMessage = program.Message;
                    $scope.validations = program.ValidationErrors;
                    $scope.modal.confirmFail = true;
                }
                else if (program.hasOwnProperty('ErrorMessage')) {
                    $scope.errorMessage = program.ErrorMessage;
                    $scope.validations.push(program.Property);
                    $scope.validations.confirmFail = true;
                }
                else if (Array.isArray(program)) {
                    $scope.errorMessage = "There were one or more errors:";
                    $scope.validations = programs;
                    $scope.validations.confirmFail = true;
                }
                else {
                    $scope.program = program; //perhaps not, this is to get the id
                    $scope.modal.confirmSave = true;
                }
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
    };
    $scope.toggleMax();

    $scope.confirmCloseYes = function () {
        $scope.modal.confirmClose = false;
        $scope.modal.createProgram = false;
    };

    $scope.confirmCloseNo = function () {
        $scope.modal.createProgram = true;
        $scope.modal.confirmClose = false;
    };

    $scope.confirmSaveYes = function () {
        $scope.modal.confirmSave = false;
        $scope.modal.createProgram = false;
    };

    $scope.confirmFailOk = function () {
        $scope.modal.confirmFail = false;
    };

  });
