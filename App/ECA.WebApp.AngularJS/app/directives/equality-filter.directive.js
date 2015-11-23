angular.module('staticApp')
  .directive('equalityFilter', ['stConfig', '$timeout', '$filter', '$parse', 'ConstantsService', function (stConfig, $timeout, $filter, $parse, ConstantsService) {
      return {
          require: '^stTable',
          templateUrl: 'app/directives/equality-filter.directive.html',
          scope: {
              property: '=property',
              propertyType: '=propertytype'
          },
          link: function (scope, element, attr, ctrl) {
              var tableCtrl = ctrl;
              var promise = null;
              var throttle = attr.stDelay || stConfig.search.delay;
              var event = attr.stInputEvent || stConfig.search.inputEvent;

              scope.view = {};
              scope.view.isLessThanDatePickerOpen = false;
              scope.view.isGreaterThanDatePickerOpen = false;

              scope.view.lessThanValue = null;
              scope.view.greaterThanValue = null;

              scope.view.stopEvents = function ($event) {
                  $event.preventDefault();
                  $event.stopPropagation();
              };

              scope.view.onLessThanDatePickerOpen = function ($event) {
                  scope.view.stopEvents($event);
                  scope.view.isLessThanDatePickerOpen = true;
              }

              scope.view.onGreaterThanDatePickerOpen = function ($event) {
                  scope.view.stopEvents($event);
                  scope.view.isGreaterThanDatePickerOpen = true;
              }

              scope.view.onValueChange = function () {
                  doFilter(scope.view.lessThanValue, scope.view.greaterThanValue, scope.property);
              }

              function doFilter(lessThanValue, greaterThanValue, property) {
                  var predicateObjects = [];
                  if (lessThanValue !== null) {
                      predicateObjects.push({
                          comparison: ConstantsService.lessThanComparisonType,
                          value: lessThanValue

                      });
                  }
                  if (greaterThanValue !== null) {
                      predicateObjects.push({
                          comparison: ConstantsService.greaterThanComparisonType,
                          value: greaterThanValue
                      });
                  }

                  promise = $timeout(function () {
                      tableCtrl.search(predicateObjects, property);
                      promise = null;
                  }, throttle);

                  // view -> table state
                  element.bind(event, function (evt) {
                      evt = evt.originalEvent || evt;
                      if (promise !== null) {
                          $timeout.cancel(promise);
                      }
                  });
              }
          }
      };
  }]);