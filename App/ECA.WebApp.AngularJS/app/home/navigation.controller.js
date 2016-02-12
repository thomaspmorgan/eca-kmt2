'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:NavigationCtrl
 * @description
 * # NavigationCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('NavigationCtrl', function (
      $rootScope,
      $scope,
      $state,
      $log,
      StateService,
      OrganizationService,
      PersonService,
      OfficeService,
      ProjectService,
      ProgramService,
      ConstantsService) {

      $scope.view = {};
      $scope.view.breadcrumbs = [];
      $scope.view.isLoadingBreadcrumbs = false;
      $scope.view.maxBreadcrumbs = 4;
      $scope.view.maxTruncatedBreadcrumbNameLength = 0;

      var currentStatePrefix = null;
      var currentStateParams = null;

      $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
          handleStateChangeSuccess(toState, toParams);
      });

      function handleStateChangeSuccess(toState, toParams) {
          var dotIndex = toState.name.indexOf('.');
          var prefix = toState.name;
          if (dotIndex !== -1) {
              prefix = toState.name.substring(0, dotIndex);
          }
          if (prefix !== currentStatePrefix || !areStateParamsEqual(toParams, currentStateParams)) {
              currentStatePrefix = prefix;
              currentStateParams = toParams;
              clearBreadcrumbs();
              setBreadcrumbsByState(currentStatePrefix, currentStateParams);
          }
      }

      function setBreadcrumbsByState(stateName, stateParams) {
          $scope.view.isLoadingBreadcrumbs = true;
          if (StateService.isProjectState(stateName)) {
              return ProjectService.getById(stateParams.projectId)
              .then(function (response) {
                  setBreadcrumbsByEntity(stateName, response.data);
                  $scope.view.isLoadingBreadcrumbs = false;
              })
          }
          else if (StateService.isProgramState(stateName)) {
              return ProgramService.get(stateParams.programId)
              .then(function (program) {
                  setBreadcrumbsByEntity(stateName, program);
                  $scope.view.isLoadingBreadcrumbs = false;
              })
          }
          else if (StateService.isOfficeState(stateName)) {
              return OfficeService.get(stateParams.officeId)
              .then(function (response) {
                  setBreadcrumbsByEntity(stateName, response.data);
                  $scope.view.isLoadingBreadcrumbs = false;
              });
          }
          else if (StateService.isOrganizationState(stateName)) {
              return OrganizationService.getById(stateParams.organizationId)
              .then(function (response) {
                  setBreadcrumbsByEntity(stateName, response.data);
                  $scope.view.isLoadingBreadcrumbs = false;
              });
          }
          else if (StateService.isPersonState(stateName)) {
              return PersonService.getPersonById(stateParams.personId)
              .then(function (person) {
                  setBreadcrumbsByEntity(stateName, person);
                  $scope.view.isLoadingBreadcrumbs = false;
              });
          }
          else {
              $log.info('No entity to set breadcrumbs to.');
          }
      }

      function areStateParamsEqual(stateParams, testStateParams) {
          if (!stateParams && testStateParams) {
              return false;
          }
          if (stateParams && !testStateParams) {
              return false;
          }
          if (stateParams.projectId && testStateParams.projectId) {
              return stateParams.projectId == testStateParams.projectId;
          }
          if (stateParams.programId && testStateParams.programId) {
              return stateParams.programId == testStateParams.programId;
          }
          if (stateParams.officeId && testStateParams.officeId) {
              return stateParams.officeId == testStateParams.officeId;
          }
          if (stateParams.organizationId && testStateParams.organizationId) {
              return stateParams.organizationId == testStateParams.organizationId;
          }
          if (stateParams.programId && testStateParams.programId) {
              return stateParams.personId == testStateParams.personId;
          }
          return false;
      }

      function clearBreadcrumbs() {
          $scope.view.breadcrumbs = [];
      }

      function setBreadcrumbsByEntity(stateName, entity) {
          if (StateService.isProjectState(stateName)) {
              setProjectBreadcrumbs(entity);
          }
          else if (StateService.isProgramState(stateName)) {
              setProgramBreadcrumbs(entity);
          }
          else if (StateService.isOfficeState(stateName)) {
              setOfficeBreadcrumbs(entity);
          }
          else if (StateService.isOrganizationState(stateName)) {
              setOrganizationBreadcrumbs(entity);
          }
          else if (StateService.isPersonState(stateName)) {
              setPersonBreadcrumbs(entity);
          }
          else {
              $log.info('No entity to set breadcrumbs to.');
          }
      }

      function setProgramBreadcrumbs(program) {
          addBreadcrumb(program.ownerName, StateService.getOfficeState(program.ownerOrganizationId));
          addBreadcrumb(program.name, StateService.getProgramState(program.id));
      }

      function setProjectBreadcrumbs(project) {
          addBreadcrumb(project.ownerName, StateService.getOfficeState(project.ownerId));
          addBreadcrumb(project.programName, StateService.getProgramState(project.programId));
          addBreadcrumb(project.name, StateService.getProjectState(project.id));
      }

      function setOrganizationBreadcrumbs(organization) {
          addBreadcrumb(organization.name, StateService.getOrganizationState(organization.id));
      }

      function setPersonBreadcrumbs(person) {
          addBreadcrumb(person.fullName, StateService.getPersonState(person.id));
      }

      function setOfficeBreadcrumbs(office) {
          addBreadcrumb(office.name, StateService.getOfficeState(office.id));
      }

      function addBreadcrumb(name, href) {
          $scope.view.breadcrumbs.push({
              name: name,
              href: href
          });
          $scope.view.maxTruncatedBreadcrumbNameLength = 160;
          var codeTruncatedNames = $scope.view.breadcrumbs.length - 1;
          if (codeTruncatedNames > 0) {
              $scope.view.maxTruncatedBreadcrumbNameLength = Math.floor($scope.view.maxTruncatedBreadcrumbNameLength / codeTruncatedNames);
          }
      }
  });
