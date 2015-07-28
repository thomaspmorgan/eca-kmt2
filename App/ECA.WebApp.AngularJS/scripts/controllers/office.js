'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('OfficeCtrl', function ($scope,
      $stateParams,
      $q,
      $log,
      DragonBreath,
      OfficeService,
      TableService,
      LookupService,
      ProgramService) {

      var officeId = $stateParams.officeId;
      loadOfficeSpecificData(officeId);

      $scope.tabs = {
          overview: {
              title: 'Overview',
              path: 'overview',
              active: true,
              order: 1
          },
          partners: {
              title: 'Branches & Programs',
              path: 'branches',
              active: true,
              order: 2
          },
          participants: {
              title: 'Timeline',
              path: 'activity',
              active: true,
              order: 3
          },
          artifacts: {
              title: 'Attachments',
              path: 'artifacts',
              active: true,
              order: 4
          },
          moneyflows: {
              title: 'Funding',
              path: 'moneyflows',
              active: true,
              order: 5
          }
      };

      $scope.header = 'Branches & Programs';
      $scope.office = {};
      $scope.programs = [];
      $scope.branches = [];
      $scope.totalNumberOfPrograms = 0;
      $scope.skippedNumberOfPrograms = 0;
      $scope.numberOfPrograms = 0;
      $scope.programFilter = '';

      $scope.isLoadingOfficeById = true;
      $scope.isLoadingPrograms = false;
      $scope.isLoadingBranches = true;

      $scope.officeExists = true;
      $scope.showLoadingOfficeByIdError = false;
      $scope.loadingProgramsErrorOccurred = false;
      $scope.loadingBranchesErrorOccurred = false;

      // variables for program creation and editing **************************

      $scope.modalForm = {};
        $scope.validations =[];
        $scope.categoryLabel = 'Focus Categories';
        $scope.objectiveLabel = 'Objectives';

        $scope.showObjectiveJustification = false;
        $scope.showCategoryFocus = false;

        $scope.isFormBusy = false;

        $scope.today = function () {
            $scope.startDate = new Date();
        };
        $scope.today();

        $scope.calOpened = false;

        $scope.modalForm = {};
        $scope.currentForm = null;

        $scope.editProgramLoading = false;
        $scope.editExisting = false;
        $scope.dropDownDirty = false;

        $scope.themes = [];
        $scope.categories = [];
        $scope.objectives = [];
        $scope.goals = [];
        $scope.regions = [];
        $scope.pointsOfContact = [];

      // initialize new Program record
            $scope.newProgram = {
                name: '',
                description : '',
                parentProgramId: null,
                ownerOrganizationId: officeId,
                programStatusId: null,
                startDate : new Date(),
                themes: [],
                categories: [],
                objectives: [],
                goals : [],
                regions: [],
                contacts: [],
                website: null
            };

            $scope.out = {
                Themes: [],
                Regions: [],
                Goals: [],
                Contacts: [],
                Categories: [],
                Objectives: [],
                OwnerOrganizationId : []
            };

      // lookup params for services
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

      // end editing and creation variables **********************************



      function reset() {
          $scope.officeExists = true;
          $scope.showLoadingOfficeByIdError = false;
          $scope.loadingProgramsErrorOccurred = false;
          $scope.loadingBranchesErrorOccurred = false;
      }

      function showLoadingBranches() {
          $scope.isLoadingBranches = true;
      }

      function hideLoadingBranches() {
          $scope.isLoadingBranches = false;
      }

      function showLoadingOfficeById() {
          $scope.isLoadingOfficeById = true;
      }

      function hideLoadingOfficeById() {
          $scope.isLoadingOfficeById = false;
      }

      function showNotFound() {
          $scope.officeExists = false;
      }

      function showLoadingOfficeByIdError() {
          $scope.showLoadingOfficeByIdError = true;
      }

      function showLoadingProgramsError() {
          $scope.loadingProgramsErrorOccurred = true;
      }

      function showLoadingBranchesError() {
          $scope.loadingBranchesErrorOccurred = true;
      }

      function updateHeader() {
          if ($scope.branches.length === 0) {
              $scope.header = "Programs";
          }
      }

      function getOfficeById(id) {
          var dfd = $q.defer();
          reset();
          OfficeService.get(id)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);

              });
          return dfd.promise;
      }

      function updatePagingDetails(total, start, count) {
          $scope.totalNumberOfPrograms = total;
          $scope.skippedNumberOfPrograms = start;
          $scope.numberOfPrograms = count;
      }

      function getChildOfficesById(officeId) {
          var dfd = $q.defer();

          OfficeService.getChildOffices(officeId)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);

              });
          return dfd.promise;
      }

      function getProgramsByOfficeId(officeId, params) {
          var dfd = $q.defer();

          OfficeService.getPrograms(params, officeId)
              .then(function (data, status, headers, config) {
                  dfd.resolve(data.data);
              },
              function (data, status, headers, config) {
                  var errorCode = data.status;
                  dfd.reject(errorCode);

              });
          return dfd.promise;
      }

      $scope.getPrograms = function (tableState) {
          reset();
          $scope.isLoadingPrograms = true;
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter(),
              keyword: TableService.getKeywords()
          };
          $scope.programFilter = params.keyword;
          getProgramsByOfficeId(officeId, params)
            .then(function (data) {
                processData(data, tableState, params);
            }, function (errorCode) {
                showLoadingProgramsError();
            })
            .then(function () {
                $scope.isLoadingPrograms = false;
            });
      }

      function processData(data, tableState, params) {
          var programs = data.results;
          var total = data.total;
          var start = 0;
          if (programs.length > 0) {
              start = params.start + 1;
          };
          var count = params.start + programs.length;

          updatePagingDetails(total, start, count);

          var limit = TableService.getLimit();
          tableState.pagination.numberOfPages = Math.ceil(total / limit);

          $scope.programs = programs;
          $scope.programsLoading = false;
      };

      showLoadingOfficeById();
      getOfficeById(officeId)
          .then(function (data) {
              $scope.office = data;

          }, function (errorCode) {
              if (errorCode === 404) {
                  showNotFound();
              }
              else {
                  showLoadingOfficeByIdError();
              }
          })
        .then(function () {
            hideLoadingOfficeById();
        });

      showLoadingBranches();
      getChildOfficesById(officeId)
          .then(function (data) {
              var childOffices = data;
              $scope.branches = childOffices;
          }, function (errorCode) {
              showLoadingBranchesError();
          })
          .then(function(){
              hideLoadingBranches();
              updateHeader();
          });
      
      function loadOfficeSpecificData(officeId) {
          var params = {
              start: 0,
              limit: 100
          };
          return $q.all([loadOfficeSettings(officeId), loadCategories(officeId, params), loadObjectives(officeId, params)])
          .then(function () {
              $log.info('Loaded office specific data.');
          });
      }

      function loadOfficeSettings(officeId) {
          $scope.isFormBusy = true;
          return OfficeService.getSettings(officeId)
              .then(function (response) {
                  var objectiveLabel = response.data.objectiveLabel;
                  var categoryLabel = response.data.categoryLabel;
                  var focusLabel = response.data.focusLabel;
                  var justificationLabel = response.data.justificationLabel;
                  var isCategoryRequired = response.data.isCategoryRequired;
                  var isObjectiveRequired = response.data.isObjectiveRequired;

                  $scope.categoryLabel = categoryLabel + '/' + focusLabel;
                  $scope.objectiveLabel = objectiveLabel + '/' + justificationLabel;

                  $scope.showCategoryFocus = isCategoryRequired;
                  $scope.showObjectiveJustification = isObjectiveRequired;

              })
              .then(function () {
                  $scope.isFormBusy = false;
              })
          .catch(function () {
              $log.error('Unable to load office settings.');
              NotificationService.showErrorMessage('Unable to load office settings.');
          });
      }

      // program creation and editing routines *************************************************
      $scope.createModalCancel = function () {

          $scope.currentForm = $scope.modalForm.programForm;
          $scope.checkFormStatus();
      };

      $scope.editModalCancel = function () {
          $scope.currentForm = $scope.modalForm.editProgramForm;
          $scope.checkFormStatus();
      };

      $scope.checkFormStatus = function () {
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
          return ($scope.modalForm.editProgramForm.$dirty || $scope.modalForm.programForm.$dirty || $scope.dropDownDirty);
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
          $scope.isFormBusy = true;
          $scope.programId = programId;
          ProgramService.get(programId)
              .then(function (newProgram) {
                      $scope.newProgram = newProgram;
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
                  }).then(function () {
                      $scope.isFormBusy = false;
                  })
              .catch(function () {
                  $log.error('Unable to load program.');
                  NotificationService.showErrorMessage('Unable to load program.');
              });
      };

      $scope.saveEditedProgram = function () {

          var editProgramForm = $scope.modalForm.editProgramForm;

          var programId = $scope.programId;
          $scope.isFormBusy = true;
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
                  })
                  .then(function () {
                      $scope.isFormBusy = false;
                  })
                  .catch(function () {
                      NotificationService.showErrorMessage('Unable to save program.');
                  });
          }
          else {
              alert('Please complete all required fields');
          }

      };

      $scope.saveCreatedProgram = function () {

          $scope.isFormBusy = true;
          var programForm = $scope.modalForm.programForm;
          if (programForm.$valid) {
              cleanUpNewProgram();

              if ($scope.out.OwnerOrganization.length > 0) {
                  $scope.newProgram.ownerOrganizationId = $scope.out.OwnerOrganization[0].organizationId;
              }
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
                  })
              .then(function () {
                  $scope.isFormBusy = false;
              })
              .catch(function () {
                  NotificationService.showErrorMessage('Unable to save program.');
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

          $scope.modalForm.editProgramForm.$setPristine();
          $scope.modalForm.programForm.$setPristine();

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
          $scope.changeProgramList();

      };

      $scope.confirmFailOk = function () {
          $scope.confirmFail = false;
      };

      // #endregion

      $scope.setDropDownDirty = function () {
          $scope.dropDownDirty = true;
      };

      $scope.getParentPrograms = function (val) {
          $scope.parentLookupParams = {
              start: null,
              limit: 25,
              sort: null,
              filter: [{ property: 'name', comparison: 'like', value: val },
                      { property: 'programstatusid', comparison: 'eq', value: 1 }]
          };

          return ProgramService.getAllProgramsAlpha($scope.parentLookupParams)
              .then(function (data) {
                  return data.results;
              });
      };
      
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
          angular.forEach($scope.allCategoriesGrouped, function (value, key) {
              $scope.allCategoriesGrouped[key].ticked = ($.inArray(value.id, $scope.newProgram.categories) > -1);
          });
          angular.forEach($scope.allObjectivesGrouped, function (value, key) {
                $scope.allObjectivesGrouped[key].ticked = ($.inArray(value.id, $scope.newProgram.objectives) > -1);
            });
        };


      $scope.allCategoriesGrouped = [];
      function loadCategories(officeId, params) {
          return OfficeService.getCategories(officeId, params)
            .then(function (response) {
                var focusName = '';
                $scope.categories = response.data.results;

                angular.forEach($scope.categories, function (value, key) {

                    if (value.focusName != focusName) {

                        $scope.allCategoriesGrouped.push({ focusGroup: false });

                        focusName = value.focusName;
                        $scope.allCategoriesGrouped.push(
                          { name: '<strong>Focus: ' + value.focusName + '</strong>', focusGroup: true }
                        );
                    }
                    $scope.allCategoriesGrouped.push(
                        { id: value.id, name: value.name, ticked: false }
                    );
                });
            });
      }


      $scope.allObjectivesGrouped = [];

      function loadObjectives(officeId, params) {
          return OfficeService.getObjectives(officeId, params)
            .then(function (response) {

                var justificationName = '';
                $scope.objectives = response.data.results;

                angular.forEach($scope.objectives, function (value, key) {

                    if (value.justificationName != justificationName) {

                        $scope.allObjectivesGrouped.push({ justificationGroup: false });

                        justificationName = value.justificationName;
                        $scope.allObjectivesGrouped.push(
                          { name: '<strong>Justification: ' + value.justificationName + '</strong>', justificationGroup: true }
                        );
                    }
                    $scope.allObjectivesGrouped.push(
                        { id: value.id, name: value.name, ticked: false }
                    );
                });

            });
      }

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

      $scope.createProgramForm = function () {

          $scope.editExisting = false;
          $scope.showCreateProgram = true;
          $scope.dropDownDirty = false;

          // set office-specific settings

      };

      // END OF SECTION

  });
