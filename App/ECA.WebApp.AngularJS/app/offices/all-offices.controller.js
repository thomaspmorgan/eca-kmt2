'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:AllOfficesCtrl
 * @description
 * # AllOfficesCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('AllOfficesCtrl', function (
      $scope,
      $log,
      $stateParams,
      $q,
      $timeout,
      NotificationService,
      FilterService,
      smoothScroll,
      ConstantsService,
      OfficeService,
      TableService) {

      var limit = 300;

      $scope.view = {};
      $scope.view.offices = [];
      $scope.view.totalNumberOfOffices = -1;
      $scope.view.skippedNumberOfOffices = -1;
      $scope.view.numberOfOffices = -1;
      $scope.view.officeFilter = '';
      $scope.view.hierarchyKey = "hierarchy";
      $scope.view.alphabeticalKey = "alpha";
      $scope.view.listType = $scope.view.hierarchyKey;
      $scope.view.officesLoading = false;

      function updatePagingDetails(total, start, count) {
          $scope.view.totalNumberOfOffices = total;
          $scope.view.skippedNumberOfOffices = start;
          $scope.view.numberOfOffices = count;
      };

      $scope.view.onSearchChange = function () {
          $scope.view.listType = $scope.view.alphabeticalKey;
      };

      $scope.view.onOfficeFiltersChange = function () {
          console.assert($scope.getAllOfficesTableState, "The table state function must exist.");
          $scope.view.officeFilter = '';
          var tableState = $scope.getAllOfficesTableState();
          return $scope.view.getOffices(tableState);
      }

      $scope.view.onExpandClick = function (office) {
          office.isExpanded = true;
          loadChildrenOffices(office)
          .then(function (childOffices) {
              if (childOffices.length > 0) {
                  scrollToOffice(office);
              }
              else {
                  $log.info('Office has no children.');
              }
          });
      }

      $scope.view.onCollapseClick = function (office) {
          office.isExpanded = false;
          removeChildrenOffices(office);
          scrollToOffice(office);
      }

      $scope.view.onScrollToParentClick = function (office) {
          if (office.parent) {
              scrollToOffice(office.parent);
          }
      }

      $scope.view.getOffices = function (tableState) {
          if ($scope.view.listType === $scope.view.hierarchyKey && tableState.search && tableState.search.predicateObject) {
              delete tableState.search.predicateObject.$;
          }
          $scope.view.officesLoading = true;
          TableService.setTableState(tableState);
          var params = {
              start: TableService.getStart(),
              limit: TableService.getLimit(),
              sort: TableService.getSort(),
              filter: TableService.getFilter(),
              keyword: TableService.getKeywords()
          };

          if ($scope.view.listType === $scope.view.alphabeticalKey) {
              return loadOfficesAlphabetically(params, tableState);
          }
          else {
              return loadOfficesInHierarchy(params, tableState);
          }
      };

      function loadOfficesAlphabetically(params, tableState) {
          params.sort = [
              {
                  property: 'name',
                  direction: ConstantsService.ascending
              }
          ];
          return OfficeService.getAll(params)
            .then(function (response) {
                angular.forEach(response.data.results, function (office, officeIndex) {
                    office.isRoot = true;
                    office.officeLevel = 0;
                });
                processData(response, params, tableState)
            })
          .catch(function (response) {
              $scope.view.officesLoading = false;
              var message = "Unable to load offices.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      var childrenOfficesFilter = FilterService.add('offices_directive_childrenfilter');
      function loadChildrenOffices(parentOffice) {
          childrenOfficesFilter.reset();
          childrenOfficesFilter = childrenOfficesFilter
              .skip(0)
              .take(limit)
              .equal("parentOrganization_OrganizationId", parentOffice.organizationId)
              .sortBy("name");
          var params = childrenOfficesFilter.toParams();
          parentOffice.loadingChildrenOffices = true;
          return OfficeService.getAll(params)
          .then(function (response) {
              parentOffice.loadingChildrenOffices = false;
              return processChildOfficeData(parentOffice, response, params);
          });
      }

      function processChildOfficeData(parentOffice, response, params) {
          var parentOfficeIndex = $scope.view.offices.indexOf(parentOffice);
          var childOffices = [];
          var totalChildOffices = 0;
          if (response.data) {
              childOffices = response.data.results;
              totalChildOffices = response.data.total;
          }
          else {
              childOffices = response.results;
              totalChildOffices = response.total;
          }
          if (totalChildOffices > limit) {
              var message = "Unable to load all child offices.  Child offices count exceeds max.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          }
          angular.forEach(childOffices, function (childOffice, childOfficeIndex) {
              setOfficeDivId(childOffice);
              $scope.view.offices.splice(parentOfficeIndex + 1 + childOfficeIndex, 0, childOffice);
              childOffice.parent = parentOffice;
          });

          parentOffice.children = childOffices;
          return childOffices;
      }

      function loadOfficesInHierarchy(params, tableState) {
          params.filter.push({
              property: 'parentOrganization_OrganizationId',
              comparison: ConstantsService.isNullComparisonType
          });
          return OfficeService.getAll(params)
            .then(function (response) {
                processData(response, params, tableState)
            })
          .catch(function (response) {
              $scope.view.officesLoading = false;
              var message = "Unable to load offices.";
              NotificationService.showErrorMessage(message);
              $log.error(message);
          });
      }

      function processData(response, params, tableState) {
          var offices = response.data.results;
          var total = response.data.total;
          var start = 0;
          if (offices.length > 0) {
              start = params.start + 1;
          };
          var count = params.start + offices.length;
          updatePagingDetails(total, start, count);
          angular.forEach(offices, function (office, officeIndex) {
              setOfficeDivId(office);
          });

          $scope.view.offices = offices;
          var limit = TableService.getLimit();
          tableState.pagination.numberOfPages = Math.ceil(total / limit);
          $scope.view.officesLoading = false;
          
      }

      function setOfficeDivId(office) {
          office.divId = 'office' + office.organizationId;
      }

      function scrollToOffice(office) {
          var id = office.divId;
          var options = {
              duration: 500,
              easing: 'easeIn',
              offset: 70,
              callbackBefore: function (element) { },
              callbackAfter: function (element) { }
          }
          var element = document.getElementById(id)
          smoothScroll(element, options);
      }

      function removeChildrenOffices(office) {
          if (office.children) {
              for (var i = 0; i < office.children.length; i++) {
                  var childOffice = office.children[i];
                  var childOfficeIndex = $scope.view.offices.indexOf(childOffice);
                  $scope.view.offices.splice(childOfficeIndex, 1);
                  removeChildrenOffices(childOffice);
              }
              delete office.children;
          }
      }

  });
