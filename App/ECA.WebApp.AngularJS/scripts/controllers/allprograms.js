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
  .controller('AllProgramsCtrl', function ($scope, $stateParams, $state, ProgramService, LookupService, TableService, $timeout) {
      
      $scope.errorMessage = 'Unknown Error';
      $scope.validations = [];

      $scope.totalNumberOfPrograms = 0;
      $scope.skippedNumberOfPrograms = 0;
      $scope.numberOfPrograms = 0;

      // set the office to 1 for now
      $scope.currentOffice = 1;
      $scope.categoryLabel = 'Focus Categories';
      $scope.objectiveLabel = 'Objectives';
      
      $scope.programFilter = '';

      $scope.totalRecords = 0;

      $scope.today = function () {
          $scope.startDate = new Date();
      };
      $scope.today();

      $scope.calOpened = false;

      $scope.modalForm = {};

      $scope.currentForm = null;

      $scope.editProgramLoading = false;
      $scope.currentpage = $stateParams.page || 1;

      $scope.editExisting = false;
      $scope.dropDownDirty = false;

      $scope.initialTableState = null;

      $scope.alerts = [];

      $scope.themes = [];
      $scope.categories = [];
      $scope.objectives = [];
      $scope.goals = [];
      $scope.regions = [];
      $scope.pointsOfContact = [];
      $scope.foci = [];

      $scope.programList = { type: 'hierarchy' };

      // initialize new Program record
      $scope.newProgram = {
          name: '',
          description: '',
          parentProgramId: null,
          ownerOrganizationId: 1,
          startDate: new Date(),
          themes: [],
          categories: [],
          objectives: [],
          goals: [],
          regions: [],
          focusId: null,
          contacts: [],
          website: null
      };

      $scope.out = {
          Themes: [],
          Regions: [],
          Goals: [],
          Contacts: [],
          Categories: [],
          Objectives: []
      };
    $scope.programs = [];

    $scope.programsLoading = false;

    $scope.lookupParams = {
        start: null,
        limit: 100,
        sort: null,
        filter: null
    };
    
    $scope.parentLookupParams = {
        start: null,
        limit: 25,
        sort: null,
        filter: null
    };

    $scope.regionsLookupParams = {
        start: null,
        limit: 10,
        sort: null,
        filter: [{ property: 'locationtypeid', comparison: 'eq', value: 2 }]
    };

    $scope.officeSpecificLookupParams = {
        start: null,
        limit: 10,
        sort: null,
        filter: null
        /*filter: [{ property: 'officeId', comparison: 'eq', value: $scope.currentOffice }]*/
    };

     // #region Lookup Services

    LookupService.getAllThemes($scope.lookupParams)
        .then(function (data) {
            $scope.themes = data.results;
            angular.forEach($scope.themes, function (value, key) {
                $scope.themes[key].ticked = false;
            });
        });

    LookupService.getAllGoals($scope.lookupParams)
      .then(function (data) {
          $scope.goals = data.results;
          angular.forEach($scope.goals, function (value, key) {
              $scope.goals[key].ticked = false;
          })
      });

    LookupService.getAllContacts($scope.lookupParams)
        .then(function (data) {
            $scope.pointsOfContact = data.results;
            angular.forEach($scope.pointsOfContact, function (value, key) {
                $scope.pointsOfContact[key].ticked = false;
            })
        });

    LookupService.getAllRegions($scope.regionsLookupParams)
        .then(function (data) {
            $scope.regions = data.results;
            angular.forEach($scope.regions, function (value, key) {
                $scope.regions[key].ticked = false;
            });
        });

    $scope.allCategoriesGrouped = [];

    LookupService.getAllCategories($scope.lookupParams)
        .then(function (data) {

            var focusName = '';
            $scope.categories = data.results;

            angular.forEach($scope.categories, function (value, key) {

                if (value.focusName != focusName) {
                    
                    focusName = value.focusName;
                    $scope.allCategoriesGrouped.push(
                      { name: '<strong>Focus: ' + value.focusName + '</strong>', focusGroup: true }
                    );
                }
                $scope.allCategoriesGrouped.push(
                    { name: value.name, ticked: false }
                );
                $scope.allCategoriesGrouped.push({ focusGroup: false });

            });
        });

    $scope.allObjectivesGrouped = [];

      LookupService.getAllObjectives($scope.lookupParams)
      .then(function (data) {

          var justificationName = '';
          $scope.objectives = data.results;

          angular.forEach($scope.objectives, function (value, key) {

              if (value.justificationName != justificationName) {

                  justificationName = value.justificationName;
                  $scope.allObjectivesGrouped.push(
                    { name: '<strong>Justification: ' + value.justificationName + '</strong>', justificationGroup: true }
                  );
              }
              $scope.allObjectivesGrouped.push(
                  { name: value.name, ticked: false }
              );
              $scope.allObjectivesGrouped.push({ justificationGroup: false });

          });

      });
      
      //#endregion

      // don't know why, but I need to access the scope variable for the data to load correctly
    var x = $scope.themes[1];
    x = $scope.goals[1];
    x = $scope.pointsOfContact[1];
    x = $scope.regions[1];
    x = $scope.foci[1];
    x = $scope.categories[1];
    x = $scope.objectives[1];

      //#region fill dropdown when editing

    $scope.tickSelectedItems = function () {

        // use javascript native foreach? 

        angular.forEach($scope.regions, function (value, key) {
            $scope.regions[key].ticked = ($.inArray(value.id, $scope.newProgram.regions) > -1);
        });
        angular.forEach($scope.pointsOfContact, function (value, key) {
            $scope.pointsOfContact[key].ticked = ($.inArray(value.id, $scope.newProgram.contacts) > -1);
        });
        angular.forEach($scope.goals, function (value, key) {
            $scope.goals[key].ticked = ($.inArray(value.id, $scope.newProgram.goals) > -1);
        });
        angular.forEach($scope.themes, function (value, key) {
            $scope.themes[key].ticked = ($.inArray(value.id, $scope.newProgram.themes) > -1);
        });
        angular.forEach($scope.categories, function (value, key) {
            $scope.categories[key].ticked = ($.inArray(value.id, $scope.newProgram.categories) > -1);
        });
        angular.forEach($scope.objectives, function (value, key) {
            $scope.objectives[key].ticked = ($.inArray(value.id, $scope.newProgram.objectives) > -1);
        });
    };
      
      //#endregion

    $scope.getOfficeLabels = function () {
        // read the office settings for category and justification
        // if no labels, do not show the field
        // for now show them regardless
        $scope.categoryLabel = 'Focus Categories';
        $scope.objectiveLabel = 'Objectives';
    };


    $scope.getParentPrograms = function (val) {
        $scope.parentLookupParams = {
            start: null,
            limit: 25,
            sort: null,
            filter: [{ property: 'name', comparison: 'like', value: val },
                    { property: 'programstatusid', comparison: 'eq', value: 1 }]
        };
        return ProgramService.getAllPrograms($scope.parentLookupParams)
            .then(function (data) {
                return data.results;
            });
    }

    $scope.changeProgramList = function () {
        var tableState = $scope.initialTableState;
        $scope.getPrograms(tableState);
    };

    $scope.getPrograms = function (tableState) {
        $scope.initialTableState = tableState;

        TableService.setTableState(tableState);
        var params = {
            start: TableService.getStart(),
            limit: TableService.getLimit(),
            sort: TableService.getSort(),
            filter: TableService.getFilter(),
            keyword: TableService.getKeywords()
        };

        $scope.programFilter = params.keyword;

      if ($scope.programList.type == "alpha") {
          params.filter = [{ property: 'programstatusID', comparison: 'eq', value: 1 }];
          $scope.refreshProgramsAlpha(params, tableState);
        }
        else {
            $scope.refreshProgramsHierarchy(params, tableState);
        };
    };

    $scope.refreshProgramsAlpha = function (params, tableState) {
        $scope.programsLoading = true;

        ProgramService.getAllProgramsAlpha(params)
        .then(function (data) {
            processData(data, tableState, params);
        });
    };

    $scope.refreshProgramsHierarchy = function (params, tableState) {
        $scope.programsLoading = true;

        ProgramService.getAllProgramsHierarchy(params)
        .then(function (data) {
            processData(data, tableState, params);
        });
    };

    function processData(data, tableState, params) {
        var programs = data.results;
        var total = data.total;
        var start = 0;
        if (programs.length > 0) {
            start = params.start + 1;
        };
        updatePagingDetails(total, start, programs.length);

        var limit = TableService.getLimit();
        tableState.pagination.numberOfPages = Math.ceil(total / limit);

        $scope.programs = programs;
        $scope.programsLoading = false;
    };

    function updatePagingDetails(total, start, count) {
        $scope.totalNumberOfPrograms = total;
        $scope.skippedNumberOfPrograms = start;
        $scope.numberOfPrograms = count;
    };

    $scope.getParentProgramName = function (programId) {
        ProgramService.get(programId)
            .then(function (parentProgram) {
                return parentProgram.name;
            });
    };

    $scope.createModalCancel = function () {

        $scope.currentForm = $scope.modalForm.programForm;
        $scope.checkFormStatus();
    };

    $scope.editModalCancel = function () {
        $scope.currentForm = $scope.modalForm.editProgramForm;
        $scope.checkFormStatus();
    };

    $scope.checkFormStatus = function() {
        if ($scope.currentFormIsDirty()) {
            $scope.showConfirmClose = true;
        }
        else {
            $scope.showCreateProgram = false;
            $scope.showEditProgram = false;
            return true;
        }
    };

    $scope.currentFormIsDirty = function () {
        return ($scope.currentForm.$dirty || $scope.dropDownDirty);
    };
      
    $scope.createProgramForm = function () {

        $scope.editExisting = false;
        $scope.showCreateProgram = true;
        $scope.dropDownDirty = false;
    };

    $scope.editProgram = function (programId) {

        var curDate = new Date();
        // to fix date-disabled error preventing form validation when 
        // a start date is from an existing program
        $scope.minDate = curDate.setFullYear(curDate.getFullYear() - 5);

        $scope.programId = programId;

        $('#loadingEditLabel' + programId).css("display", "inline-block");

        $scope.editProgramLoading = true;

        $scope.editExisting = true;
        $scope.dropDownDirty = false;

        $scope.programId = programId;
        ProgramService.get(programId)
            .then(function (newProgram) {

                $scope.newProgram = newProgram;
                $scope.newProgram.parentProgram = $scope.getParentProgramName(newProgram.parentProgramId);
                $scope.newProgram.themes = newProgram.themes.map(getIds);
                $scope.newProgram.goals = newProgram.goals.map(getIds);
                $scope.newProgram.contacts = newProgram.contacts.map(getIds);
                $scope.newProgram.regions = newProgram.regionIsos.map(getIds);
                $scope.newProgram.categories = newProgram.categories.map(getIds);
                $scope.newProgram.objectives = newProgram.objectives.map(getIds);
                
                $scope.tickSelectedItems();

                $scope.editProgramLoading = false;

                $scope.showEditProgram = true;

                $('#loadingEditLabel' + programId).css("display", "none");
            });
    };

    $scope.updateProgram = function () {
        saveProgram();
    };

    function getIds(element) {
        return element.id;
    };

    $scope.saveEditedProgram = function () {

        var editProgramForm = $scope.modalForm.editProgramForm;

        var programId = $scope.programId;

        if (editProgramForm.$valid) {
            cleanUpNewProgram();
            ProgramService.update($scope.newProgram, $scope.programId)
                .then(function (program) {
                    if (Array.isArray(program)) {
                        $scope.errorMessage = "There were one or more errors:";
                        $scope.validations = program;
                        $scope.confirmFail = true;
                    }
                    else if (program.hasOwnProperty('Message')) {

                        $scope.errorMessage = program.Message;
                        $scope.validations = program.ValidationErrors;
                        $scope.confirmFail = true;

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
                        $scope.confirmSave = true;
                    }
                });
        }
        else
        {
            alert('Please complete all required fields');
        }
    };
 
    $scope.saveCreatedProgram = function () {

        var programForm = $scope.modalForm.programForm;

        if (programForm.$valid) {
            cleanUpNewProgram();
            ProgramService.create($scope.newProgram)
                .then(function (program) {
                    if (Array.isArray(program)) {
                        $scope.errorMessage = "There were one or more errors:";
                        $scope.validations = program;
                        $scope.confirmFail = true;
                    }
                    else if (program.hasOwnProperty('Message')) {
                        $scope.errorMessage = program.Message;
                        $scope.validations = program.ValidationErrors;
                        $scope.confirmFail = true;
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
                        $scope.confirmSave = true;
                        $scope.modalClear();
                    }
                });
        }
    };

    function cleanUpNewProgram() {
        if ($scope.newProgram.parentProgram !== undefined) {
            $scope.newProgram.parentProgramId = $scope.newProgram.parentProgram.programId;
        }
        $scope.newProgram.themes = $scope.out.Themes.map(getIds);
        $scope.newProgram.goals = $scope.out.Goals.map(getIds);
        $scope.newProgram.contacts = $scope.out.Contacts.map(getIds);
        $scope.newProgram.regions = $scope.out.Regions.map(getIds);
        $scope.newProgram.categories = $scope.out.Categories.map(getIds);
        $scope.newProgram.objectives = $scope.out.Objectives.map(getIds);

    };

      // calendar popup for startDate
    $scope.calOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

       this.calOpened = true;

    };

      // #region clear modal form

    $scope.modalClear = function () {
        angular.forEach($scope.newProgram, function (value, key) {
            $scope.newProgram[key] = ''
        });
        $scope.calClear();
        $scope.newProgram.ownerOrganizationId = 1;
        $scope.newProgram.startDate = new Date();
        $scope.newProgram.parentProgramId = null;

        $scope.currentForm.$setPristine();

        var elements = angular.element(document.querySelectorAll('.multiSelect .reset'));
        angular.forEach(elements, function (value, key) {
            $timeout(function () {
                elements[key].click();
            })
        });

    };
    $scope.calClear = function () {
        $scope.startDate = null;
    };

      //#endregion

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


      // #region Confirmation dialogs

    $scope.closeEditingModal = function () {
        $scope.showEditProgram = false;
        $scope.showCreateProgram = false;
        $scope.modalClear();
    };

    $scope.confirmCloseYes = function () {
        $scope.showConfirmClose = false;
        $scope.closeEditingModal();
    };

    $scope.confirmCloseNo = function () {
        $scope.showConfirmClose = false;
    };

    $scope.confirmSaveYes = function () {
        $scope.confirmSave = false;
        $scope.closeEditingModal();
    };

    $scope.confirmFailOk = function () {
        $scope.confirmFail = false;
    };

      // #endregion

    $scope.setDropDownDirty = function () {
        $scope.dropDownDirty = true;
    };

  });

